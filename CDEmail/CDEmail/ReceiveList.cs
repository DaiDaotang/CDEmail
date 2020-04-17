using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CDEmail
{
    public partial class ReceiveList : Form
    {
        #region 窗口切换
        private static ReceiveList formInstance;

        public static ReceiveList GetIntance
        {
            get
            {
                if (formInstance != null)
                {
                    return formInstance;
                }
                else
                {
                    formInstance = new ReceiveList();
                    return formInstance;
                }
            }
        }
        public ReceiveList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 解决闪烁问题
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
        #endregion

        #region 变量
        public Email baseform;
        private String pop3server;
        private int pop3port;
        private String user;
        private String pwd;
        private int msgcount;
        private ArrayList msglist;
        private int curpage;
        private int cntperpage = 5;

        private TcpClient tcp; 
        private NetworkStream ns;
        private StreamReader sr;
        private bool login = false;
        #endregion

        #region 按钮  连接
        private void button1_Click(object sender, EventArgs e)
        {
            pop3server = tPop3Server.Text;
            pop3port = Convert.ToInt32(tPop3Port.Text);
            user = tUsername.Text;
            pwd = tPassword.Text;
            curpage = 1;

            Connect();
        }
        #endregion

        #region 按钮  读取
        private void btnReadMail_Click(object sender, EventArgs e)
        {
            // 获取邮件序号
            int n = GetSelectedMailIndexInList();
            PrintRecv("Start Read");
            if (n == -1)
            {
                WarningMessage("请选择一封邮件");
            }
            else
            {
                // WarningMessage("正在研发");
                baseform.ShowMail((NewMailInfo)msglist[n], pop3server, pop3port, user, pwd);
            }
        }
        #endregion

        #region 按钮  删除  需更改：若改序号不再是该邮件
        private void btnDeleteMail_Click(object sender, EventArgs e)
        {
            int n = dgvMails.SelectedCells.Count;
            if (n == 0)
            {
                WarningMessage("请选中邮件");
            }
            else
            {
                // 要删除的邮件序号
                ArrayList list = new ArrayList();
                int tmp = 0;
                foreach (DataGridViewCell cell in dgvMails.SelectedCells)
                    if (!list.Contains((tmp = (int)dgvMails.Rows[cell.RowIndex].Cells[0].Value)))
                        list.Add(tmp);

                if (CheckMessage(String.Format("确认删除{0:D1}封邮件吗？", list.Count), "删除确认框"))
                {
                    try
                    {
                        String input = "";
                        String recv = "";
                        Login();
                        if (!login)
                        {
                            return;
                        }
                        foreach (int msg in list)
                        {
                            input = "dele " + msg.ToString() + "\r\n";
                            if (SendOrder(input))
                            {
                                PrintRecv(recv = sr.ReadLine());
                                if (recv.Substring(0, 4) == "-ERR")
                                {
                                    WarningMessage("删除有错误，请稍后重试");
                                    return;
                                }

                            }
                            else
                            {
                                WarningMessage("删除有错误，请稍后重试");
                                return;
                            }
                        }
                        WarningMessage("删除成功");
                    }
                    catch (Exception ex)
                    {
                        PrintRecv(ex.StackTrace);
                    }
                    finally
                    {
                        Disconnect();
                        curpage = 1;
                        Connect();
                    }
                }
            }
        }
        #endregion

        #region 按钮  换页
        private void btnPrePage_Click(object sender, EventArgs e)
        {
            if (curpage == 1)
                WarningMessage("已经到达第一页");
            else
            {
                curpage--;
                GetMsgInfoList();
            }
        }
        private void btnNextPage_Click(object sender, EventArgs e)
        {
            curpage++;
            GetMsgInfoList();
        }
        #endregion

        #region 发送指令
        private bool SendOrder(String input)
        {
            try
            {
                Byte[] outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
                ns.Write(outbytes, 0, outbytes.Length);
            }
            catch (Exception ex)
            {
                PrintRecv(ex.StackTrace);
                return false;
            }
            return true;
        }
        #endregion

        #region 连接
        private void Connect()
        {
            Thread thread = new Thread(
                new ThreadStart(
                    delegate{
                        GetMsgInfoList();    // 第curpage页
                    }))
            {
                IsBackground = true
            };
            thread.Start();
        }
        #endregion

        #region 获取邮件头部分信息，并更新列表
        private void GetMsgInfoList()
        {
            // 邮件数量
            msgcount = GetMsgCount();

            int start = Math.Max(msgcount - (curpage - 1) * cntperpage, 0);
            if(start == 0)
            {
                WarningMessage("已是最后一页");
                curpage--;
                return;
            }
            int end = Math.Max(msgcount - curpage * cntperpage, 0);

            // 邮件头部信息列表
            msglist = new ArrayList();
            try
            {
                Login();
                if (!login)
                {
                    PrintRecv("登陆失败");
                    return;
                }
                String input;
                String recv;
                String uid;
                NewMailInfo mailinfo = null;
                for (int n = start; n > end; n--)
                {
                    // 获取uid
                    input = "uidl " + n.ToString() + "\r\n";
                    if (SendOrder(input))
                    {
                        PrintRecv(recv = sr.ReadLine());
                        uid = recv.Split(' ')[2];
                    }
                    else
                    {
                        WarningMessage("获取邮件失败");
                        return;
                    }

                    // 获取基本信息
                    input = "top " + n.ToString() + " 0\r\n";
                    if (SendOrder(input))
                    {
                        PrintRecv(recv = sr.ReadLine());
                        mailinfo = new NewMailInfo(n, uid);
                        String tmp;
                        while ((recv = sr.ReadLine()) != ".")
                        {
                            if (recv.ToLower().StartsWith("from"))
                            {
                                tmp = recv.Substring(5);
                                if(tmp.Contains("=?GBK?") || tmp.Contains("=?GBK?"))
                                {
                                    PrintRecv(tmp);
                                    PrintRecv(Encoding.GetEncoding(936).GetString(Encoding.Unicode.GetBytes(tmp)));
                                    tmp = tmp.Substring(8, tmp.Length - 10);
                                    tmp = Encoding.GetEncoding(936).GetString(Encoding.Unicode.GetBytes(tmp));
                                }
                                mailinfo.From = new MailAddress(tmp);
                            }
                            else if (recv.ToLower().StartsWith("to"))
                            {
                                mailinfo.To = new MailAddress(recv.Substring(3));

                            }
                            else if (recv.ToLower().StartsWith("subject"))
                            {
                                tmp = recv.Substring(8);
                                if (tmp.Contains("=?GBK?"))
                                {
                                    PrintRecv(tmp);
                                    PrintRecv(Encoding.GetEncoding("GBK").GetString(Encoding.Unicode.GetBytes(tmp)));
                                    PrintRecv(tmp.Substring(9, tmp.Length - 11));
                                    PrintRecv(Encoding.GetEncoding("GBK").GetString(Encoding.Unicode.GetBytes(tmp)));

                                    tmp = Encoding.GetEncoding(936).GetString(Encoding.Convert(Encoding.GetEncoding("GBK"), Encoding.GetEncoding(936), Encoding.Unicode.GetBytes(tmp.Substring(9, tmp.Length - 11))));

                                    //tmp = Encoding.GetEncoding("GBK").GetString(Encoding.Unicode.GetBytes(tmp));
                                    //tmp = tmp.Substring(9, tmp.Length - 11);
                                    //tmp = Encoding.GetEncoding("GBK").GetString(Encoding.Unicode.GetBytes(tmp));
                                }
                                mailinfo.Subject = tmp;
                            }
                            else if (recv.ToLower().StartsWith("date"))
                            {
                                mailinfo.Date = Convert.ToDateTime(recv.Substring(5, recv.IndexOf("+0800") - 5).Trim());
                            }
                        }
                        msglist.Add(mailinfo);
                    }
                    else
                    {
                        WarningMessage("获取邮件失败");
                        return;
                    }
                }
            }
            catch(Exception ex)
            {
                PrintRecv(ex.StackTrace);
            }
            finally
            {
                Disconnect();
            }

            // 更改DataGridView
            ChangeDGVMail(msglist);
        }
        delegate void ChangeDGVMailCallBack(ArrayList msglist);
        private void ChangeDGVMail(ArrayList msglist)
        {
            if (this.InvokeRequired)
            {
                ChangeDGVMailCallBack cdcb = new ChangeDGVMailCallBack(ChangeDGVMail);
                this.Invoke(cdcb, new object[] { msglist });
            }
            else
            {
                BindingSource binding = new BindingSource();
                binding.DataSource = msglist;
                binding.ResetBindings(true);
                dgvMails.DataSource = binding;
            }
        }
        #endregion

        #region 登录与断线
        // 登录
        private void Login()
        {
            try
            {
                String input;
                String recv;

                tcp = new TcpClient(pop3server, pop3port);
                ns = tcp.GetStream();
                ns.ReadTimeout = 10000;
                sr = new StreamReader(ns, Encoding.Default);

                // 若连接失败
                PrintRecv(recv = sr.ReadLine());
                if (recv.Substring(0, 4) == "-ERR")
                {
                    WarningMessage("与服务器连接有误");
                    return;
                }

                // 用户名
                input = "user " + user + "\r\n";
                if (SendOrder(input))
                {
                    PrintRecv(recv = sr.ReadLine());
                    if (recv.Substring(0, 4) == "-ERR")
                    {
                        WarningMessage("与服务器连接有误");
                        return;
                    }
                }
                else
                {
                    WarningMessage("发送指令失败");
                    return;
                }

                // 密码
                input = "pass " + pwd + "\r\n";
                if (SendOrder(input))
                {
                    PrintRecv(recv = sr.ReadLine());
                    if (recv.Substring(0, 4) == "-ERR")
                    {
                        WarningMessage("密码错误");
                        return;
                    }
                }
                else
                {
                    WarningMessage("发送指令失败");
                    return;
                }
            }
            catch (Exception ex)
            {
                PrintRecv(ex.StackTrace);
                return;
            }
            login = true;
        }

        // 断开连接
        private void Disconnect()
        {
            if (!login)
                return;
            login = false;
            String input = "quit\r\n";
            SendOrder(input);
            // PrintRecv(sr.ReadLine());
            //sr.Close();
            ns.Close();
        }
        #endregion

        #region 获取邮件数量
        private int GetMsgCount()
        {
            try
            {
                Login();
                // 登录失败
                if (!login)
                {
                    return -1;
                }
                String input = "stat\r\n";
                String recv;

                if (SendOrder(input))
                {
                    PrintRecv(recv = sr.ReadLine());
                    if (recv.Substring(0, 4) == "-ERR")
                    {
                        WarningMessage("与服务器连接有误");
                        return -1;
                    }
                    return Convert.ToInt32(recv.Split(' ')[1]);

                }
                else
                {
                    WarningMessage("获取邮件数量失败");
                    return -1;
                }
            }
            catch(Exception ex)
            {
                PrintRecv(ex.StackTrace);
                return -1;
            }
            finally
            {
                Disconnect();
            }
        }
        #endregion

        #region 获取选中的一封邮件的序号
        private int GetSelectedMailIndexInList()
        {
            int n = dgvMails.SelectedCells.Count;

            if (n == 0)
            {
                return -1;
            }
            else
            {
                int r = dgvMails.SelectedCells[0].RowIndex;
                if (n > 1)
                    foreach (DataGridViewCell cell in dgvMails.SelectedCells)
                        if (cell.RowIndex != r)
                            return -1;
                // 返回Index
                return r;
            }
        }
        #endregion

        #region 警告信息
        private void WarningMessage(String text)
        {
            MessageBox.Show(text);
        }
        #endregion

        #region 确认信息
        private bool CheckMessage(String text, String title)
        {
            return MessageBox.Show(text, title, MessageBoxButtons.OKCancel) == DialogResult.OK;
        }
        #endregion

        #region 输出
        private void PrintRecv(String text)
        {
            Console.WriteLine(text);
        }
        #endregion

        #region 测试
        private void Test()
        {
            Login();
            String input = "";
            String recv = "";
            for (int i = msgcount; i > 0; i--)
            {
                PrintRecv(i.ToString());
                input = "uidl " + i.ToString() + "\r\n";
                SendOrder(input);
                PrintRecv(recv = sr.ReadLine());
            }
            Disconnect();
        }
        private void btnTest_Click(object sender, EventArgs e)
        {
            Test();
        }
        #endregion
    }
}
