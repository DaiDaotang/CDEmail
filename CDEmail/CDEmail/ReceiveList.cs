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
        // 窗口切换
        #region
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

        // 变量
        #region
        public Email baseform;
        private String pop3server;
        private int pop3port;
        private String user;
        private String pwd;
        private int msgcount;
        private ArrayList msglist;
        private int curpage;
        private int cntperpage;

        private TcpClient tcp; 
        private NetworkStream ns;
        private StreamReader sr;
        private bool login = false;
        #endregion

        // 连接按钮
        #region
        private void button1_Click(object sender, EventArgs e)
        {
            pop3server = tPop3Server.Text;
            pop3port = Convert.ToInt32(tPop3Port.Text);
            user = tUsername.Text;
            pwd = tPassword.Text;
            FirstConnect();
        }
        #endregion

        // 连接
        #region
        private void FirstConnect()
        {
            Thread thread = new Thread(new ThreadStart(FirstConnectToServer));
            thread.IsBackground = true;
            thread.Start();
        }
        // 连接服务器，将邮件头文件ArrayList添加到DataGridView中。若能使用这一个函数，则一定是重新连接，所以curpage=1
        private void FirstConnectToServer()
        {
            // 第1页，每页30封
            //CurrentPage = 1;
            //CountPerPage = 30;
            curpage = 1;
            cntperpage = 30;

            // 邮件数量
            msgcount = GetMsgCount();

            // 邮件头部信息列表
            msglist = GetMsgInfoList(msgcount, Math.Max(msgcount - 30, 0));

            // 更改DataGridView
            ChangeDGVMail(msglist);


            //// 创建对象
            //ReceiveMailConnect = new ReceiveMail(tPop3Server.Text, tUsername.Text, tPassword.Text);

            //// 获取邮件数量
            //MailCount = ReceiveMailConnect.GetNumberOfNewMessages();

            //// 将信息标题等信息列入DataGridView中
            //MailList = ReceiveMailConnect.GetNewMailInfo(MailCount, Math.Max(MailCount - 30, 0));
            //ChangeDGVMail(MailList);
        }
        #endregion

        // 登录与断线
        #region
        // 登录
        private void Login()
        {
            try
            {
                String input;
                String recv;

                tcp = new TcpClient(pop3server, pop3port);
                ns = tcp.GetStream();
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

        // 获取邮件数量
        #region
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

        // 获取邮件头部分信息
        #region
        private ArrayList GetMsgInfoList(int start, int end)
        {
            ArrayList res = new ArrayList();
            NewMailInfo mailinfo;

            try
            {
                Login();
                if (!login)
                {
                    return null;
                }
                String input;
                String recv;
                String uid;
                for(int n = start; n > end; n--)
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
                        return null;
                    }

                    // 获取基本信息
                    input = "top " + n.ToString() + " 0\r\n";
                    if (SendOrder(input))
                    {
                        PrintRecv(recv = sr.ReadLine());
                        mailinfo = new NewMailInfo(n, uid);
                        while((recv = sr.ReadLine()) != ".")
                        {
                            if (recv.ToLower().StartsWith("from"))
                            {
                                mailinfo.From = new MailAddress(recv.Substring(5));
                            }
                            else if (recv.ToLower().StartsWith("to"))
                            {
                                mailinfo.To = new MailAddress(recv.Substring(3));

                            }
                            else if (recv.ToLower().StartsWith("subject"))
                            {
                                mailinfo.Subject = recv.Substring(8);
                            }
                            else if (recv.ToLower().StartsWith("date"))
                            {
                                mailinfo.Date = Convert.ToDateTime(recv.Substring(5, recv.IndexOf("+0800") - 5).Trim());
                            }
                        }
                        res.Add(mailinfo);
                    }
                    else
                    {
                        WarningMessage("获取邮件失败");
                        return null;
                    }
                }
                return res;
            }
            catch(Exception ex)
            {
                PrintRecv(ex.StackTrace);
                return null;
            }
            finally
            {
                Disconnect();
            }
        }
        #endregion

        // 保证线程安全，更新DataGridView
        #region
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

        // 获取选中的一封邮件
        #region
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

        // 读取按钮
        #region
        private void btnReadMail_Click(object sender, EventArgs e)
        {
            // 获取邮件序号
            int n = GetSelectedMailIndexInList();

            if(n == -1)
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

        // 删除按钮 需更改：如果这个序号的邮件不再是这一封邮件
        #region
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
                        FirstConnect();
                    }
                }
            }
        }
        #endregion

        // 换页按钮
        #region
        private void btnPrePage_Click(object sender, EventArgs e)
        {
            WarningMessage("正在研发");

        }
        private void btnNextPage_Click(object sender, EventArgs e)
        {

            WarningMessage("正在研发");
        }
        #endregion

        // 警告信息
        #region
        private void WarningMessage(String text)
        {
            MessageBox.Show(text);
        }
        #endregion

        // 确认信息
        #region
        private bool CheckMessage(String text, String title)
        {
            return MessageBox.Show(text, title, MessageBoxButtons.OKCancel) == DialogResult.OK;
        }
        #endregion

        // 输出
        #region
        private void PrintRecv(String text)
        {
            Console.WriteLine(text);
        }
        #endregion

        // 发送指令
        #region
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

        // 测试
        #region
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
        #endregion

        // 测试按钮
        #region
        private void btnTest_Click(object sender, EventArgs e)
        {
            Test();
        }
        #endregion
    }
}
