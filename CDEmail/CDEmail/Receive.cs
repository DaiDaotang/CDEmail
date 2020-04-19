using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.IO;

namespace CDEmail
{
    public partial class Receive : Form
    {
        #region 窗口切换
        private static Receive formInstance;
        public static Receive GetIntance
        {
            get
            {
                if (formInstance != null)
                {
                    return formInstance;
                }
                else
                {
                    formInstance = new Receive();
                    return formInstance;
                }
            }
        }
        public Receive()
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
        public NewMailMessage mailmsg;

        public String pop3server;
        public int pop3port;
        public String user;
        public String pwd;

        private TcpClient tcp;
        private NetworkStream ns;
        private StreamReader sr;
        private bool isTooLong = false;
        private bool login = false;
        private String order = "";
        private String recv = "";

        private String enlousure = "";
        #endregion

        #region 展示邮件
        public void ShowMailMessage()
        {
            tBody.Text = "";
            tFrom.Text = mailmsg.MailInfo.From.ToString();
            tSubject.Text = mailmsg.MailInfo.Subject;

            String rawmsg = GetRawMessage();
            ShowMailText(rawmsg);
        }
        #endregion

        #region 获取邮件全文
        public String GetRawMessage()
        {
            try
            {
                Login();
                // 登录失败
                if (!login)
                {
                    return null;
                }

                order = "retr " + mailmsg.MailInfo.Num.ToString() + "\r\n";
                if (SendOrder(order))
                {
                    btnRcvClousure.Enabled = false;
                    String rm = "";
                    String tmp = "";
                    PrintRecv(recv = sr.ReadLine());
                    if (recv.Substring(0, 4) == "-ERR")
                    {
                        WarningMessage("获取邮件失败");
                        return null;
                    }
                    mailmsg.Size = Convert.ToInt32(recv.Split(' ')[1]);
                    PrintRecv(mailmsg.Size.ToString());

                    if(mailmsg.Size > 400000)
                    {
                        isTooLong = true;
                        while((recv = sr.ReadLine()) != ".")
                        {
                            if (recv.StartsWith("Content-Type:"))
                            {
                                tmp = recv.Substring(13, recv.Length - 13).Trim();
                                if(!tmp.StartsWith("text") && !tmp.StartsWith("multipart"))
                                {
                                    btnRcvClousure.Enabled = true;
                                    break;
                                }
                            }
                            rm += recv + "\r\n";
                        }
                    }
                    else
                    {
                        // 正常
                        byte[] rmb = new byte[mailmsg.Size];    // raw message bytes
                        int count = 0;
                        Thread.Sleep(500);
                        while ((count = ns.Read(rmb, count, mailmsg.Size - count)) != mailmsg.Size) ;
                        rm = Encoding.GetEncoding(936).GetString(rmb, 0, count);
                    }

                    rm += "\r\n.\r\n";
                    PrintRecv(rm);
                    return rm;
                }
                else
                {
                    WarningMessage("获取邮件失败");
                    return null;
                }
            }
            catch (Exception ex)
            {
                PrintRecv(ex.StackTrace);
                PrintRecv(ex.Message);
                return null;
            }
            finally
            {
                Disconnect();
            }
        }
        #endregion

        #region 获取邮件正文 和 附件
        public void ShowMailText(String p_Mail)
        {
            String _ConvertType = GetTextType(p_Mail, "Content-Type:", ";").Trim();       // 获取邮件类型
            if (_ConvertType.Length == 0)
            {
                _ConvertType = GetTextType(p_Mail, "Content-Type:", "\r").Trim();
            }

            int _StarIndex = -1;
            int _EndIndex = -1;
            String _ReturnText = "";
            String _Transfer = "";
            String _Boundary = "";
            String _EncodingName = GetTextType(p_Mail, "charset=\"", "\"").Replace("\"", "");   // 获取邮件字体类别
            System.Text.Encoding _Encoding =
                (_EncodingName == "") ?
                System.Text.Encoding.Default :
                System.Text.Encoding.GetEncoding(_EncodingName);

            // 根据邮件不同而分类
            switch (_ConvertType)
            {
                case "text/html;":
                    _Transfer = GetTextType(p_Mail, "Content-Transfer-Encoding: ", "\r\n").Trim();
                    _StarIndex = p_Mail.IndexOf("\r\n\r\n");
                    if (_StarIndex != -1)
                        _ReturnText = p_Mail.Substring(_StarIndex, p_Mail.Length - _StarIndex);
                    switch (_Transfer)
                    {
                        case "8bit":

                            break;
                        case "quoted-printable":
                            _ReturnText = DecodeQuotedPrintable(_ReturnText, _Encoding);
                            break;
                        case "base64":
                            _ReturnText = DecodeBase64(_ReturnText, _Encoding);
                            break;
                    }
                    tBody.Text += _ReturnText;
                    break;

                case "text/plain;":
                    _Transfer = GetTextType(p_Mail, "Content-Transfer-Encoding:", "\r\n").Trim();
                    _StarIndex = p_Mail.IndexOf("\r\n\r\n");
                    if (_StarIndex != -1) 
                        _ReturnText = p_Mail.Substring(_StarIndex, p_Mail.Length - _StarIndex);
                    switch (_Transfer)
                    {
                        case "8bit":

                            break;
                        case "quoted-printable":
                            _ReturnText = DecodeQuotedPrintable(_ReturnText, _Encoding);
                            break;
                        case "base64":
                            _ReturnText = DecodeBase64(_ReturnText, _Encoding);
                            break;
                        default:
                            break;
                    }
                    tBody.Text += _ReturnText;
                    break;

                case "multipart/alternative;":
                    _Boundary = GetTextType(p_Mail, "boundary=\"", "\"").Replace("\"", "");
                    _StarIndex = p_Mail.IndexOf("--" + _Boundary + "\r\n");
                    if (_StarIndex == -1)
                        return;
                    while (true)
                    {
                        _EndIndex = p_Mail.IndexOf("--" + _Boundary, _StarIndex + _Boundary.Length);
                        if (_EndIndex == -1) 
                            break;
                        ShowMailText(p_Mail.Substring(_StarIndex, _EndIndex - _StarIndex));
                        _StarIndex = _EndIndex;
                    }
                    break;
                case "multipart/mixed;":
                    _Boundary = GetTextType(p_Mail, "boundary=\"", "\"").Replace("\"", "");
                    if (_Boundary == "")
                        _Boundary = GetTextType(p_Mail, "boundary=", "\r\n").Replace("\r\n", "");
                    _StarIndex = p_Mail.IndexOf("--" + _Boundary + "\r\n");
                    if (_StarIndex == -1)
                        return;
                    while (true)
                    {
                        _EndIndex = p_Mail.IndexOf("--" + _Boundary, _StarIndex + _Boundary.Length);
                        if (_EndIndex == -1) 
                            break;
                        ShowMailText(p_Mail.Substring(_StarIndex, _EndIndex - _StarIndex));
                        _StarIndex = _EndIndex; 
                    }
                    break;
                default:
                    btnRcvClousure.Enabled = true;
                    enlousure = p_Mail.Substring(p_Mail.IndexOf("Content-Type:"));
                    PrintRecv("以下是附件\r\n" + enlousure);
                    break;
            }
        }
        #endregion

        #region 获取类型
        /// <summary>
        /// 获取类型（正则）
        /// </summary>
        /// <param name="p_Mail">原始文字</param>
        /// <param name="p_TypeText">前文字</param>
        /// <param name="p_End">结束文字</param>
        /// <returns>符合的记录</returns>
        public string GetTextType(string p_Mail, string p_TypeText, string p_End)
        {
            // 正则表达式
            Regex _Regex = new Regex(@"(?<=" + p_TypeText + ").*?(" + p_End + ")+");
            // 匹配集合
            MatchCollection _Collection = _Regex.Matches(p_Mail);
            if (_Collection.Count == 0)
                return "";
            // 返回第一个
            return _Collection[0].Value.Trim();
        }
        #endregion

        #region QuotedPrintable编码解码
        /// <summary>
        /// QuotedPrintable编码接码
        /// </summary>
        /// <param name="p_Text">原始文字</param>
        /// <param name="p_Encoding">编码方式</param>
        /// <returns>接码后信息</returns>
        public string DecodeQuotedPrintable(string p_Text, System.Text.Encoding p_Encoding)
        {
            System.IO.MemoryStream _Stream = new System.IO.MemoryStream();
            char[] _CharValue = p_Text.ToCharArray();
            for (int i = 0; i != _CharValue.Length; i++)
            {
                switch (_CharValue[i])
                {
                    case '=':
                        if (_CharValue[i + 1] == '\r' || _CharValue[i + 1] == '\n')
                        {
                            i += 2;
                        }
                        else
                        {
                            try
                            {
                                _Stream.WriteByte(Convert.ToByte(_CharValue[i + 1].ToString() + _CharValue[i + 2].ToString(), 16));
                                i += 2;
                            }
                            catch
                            {
                                _Stream.WriteByte(Convert.ToByte(_CharValue[i]));
                            }
                        }
                        break;
                    default:
                        _Stream.WriteByte(Convert.ToByte(_CharValue[i]));
                        break;
                }
            }
            return p_Encoding.GetString(_Stream.ToArray());
        }
        #endregion

        #region BASE64编码解码
        /// <summary>
        /// 解码BASE64
        /// </summary>
        /// <param name="p_Text"></param>
        /// <param name="p_Encoding"></param>
        /// <returns></returns>
        public string DecodeBase64(string p_Text, System.Text.Encoding p_Encoding)
        {
            if (p_Text.Trim().Length == 0) return "";
            byte[] _ValueBytes = Convert.FromBase64String(p_Text);
            return p_Encoding.GetString(_ValueBytes);
        }
        #endregion

        #region 转换文字里的字符集
        /// <summary>
        /// 转换文字里的字符集
        /// </summary>
        /// <param name="p_Text"></param>
        /// <returns></returns>
        public string GetReadText(string p_Text)
        {
            Regex _Regex = new Regex(@"(?<=\=\?).*?(\?\=)+");
            MatchCollection _Collection = _Regex.Matches(p_Text);
            string _Text = p_Text;
            foreach (System.Text.RegularExpressions.Match _Match in _Collection)
            {
                string _Value = "=?" + _Match.Value;
                if (_Value[0] == '=')
                {
                    string[] _BaseData = _Value.Split('?');
                    if (_BaseData.Length == 5)
                    {
                        System.Text.Encoding _Coding = System.Text.Encoding.GetEncoding(_BaseData[1]);
                        _Text = _Text.Replace(_Value, _Coding.GetString(Convert.FromBase64String(_BaseData[3])));
                    }
                }
                else
                {
                }
            }
            return _Text;
        }
        #endregion

        #region 输出
        private void PrintRecv(String text)
        {
            Console.WriteLine(text);
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

        #region 登录与断线
        // 登录
        private void Login()
        {
            try
            {
                isTooLong = false;
                enlousure = "";
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
                order = "user " + user + "\r\n";
                if (SendOrder(order))
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
                order = "pass " + pwd + "\r\n";
                if (SendOrder(order))
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
            order = "quit\r\n";
            SendOrder(order);
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
            catch (Exception ex)
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

        #region 按钮  返回
        private void btnBack_Click(object sender, EventArgs e)
        {
            baseform.ShowReceiveList();
        }
        #endregion

        #region 按钮  回复
        private void btnReply_Click(object sender, EventArgs e)
        {
            String tmp = mailmsg.MailInfo.From.ToString().Trim();
            if(tmp.Contains("<") && tmp.Contains(">"))
            {
                int start = tmp.LastIndexOf("<") + 1;
                int len = tmp.LastIndexOf(">") - start;
                tmp = tmp.Substring(start, len).Trim();
            }
            baseform.ReplyMail(user, pwd, tmp);
        }
        #endregion

        #region 按钮  删除
        private void btnDelete_Click(object sender, EventArgs e)
        {
            WarningMessage("正在研发");

        }
        #endregion

        #region 按钮  翻页
        private void btnPrevMail_Click(object sender, EventArgs e)
        {
            int count = GetMsgCount();
            int n = mailmsg.MailInfo.Num + 1;
            if (n > count)
            {
                WarningMessage("这是最后一封");
                return;
            }
            SetNewMailInfo(n);
            mailmsg.Body = "";
            mailmsg.Enclousure = null;
            ShowMailMessage();
        }

        private void btnNextMail_Click(object sender, EventArgs e)
        {
            int n = mailmsg.MailInfo.Num - 1;
            if (n < 1)
            {
                WarningMessage("这是第一封");
                return;
            }
            SetNewMailInfo(n);
            mailmsg.Body = "";
            mailmsg.Enclousure = null;
            ShowMailMessage();
        }

        private void SetNewMailInfo(int n)
        {
            try
            {
                Login();
                if (!login)
                {
                    WarningMessage("请重试");
                    return;
                }
                // 获取uidl
                order = "uidl " + n.ToString() + "\r\n";
                String tmp;
                NewMailInfo mailinfo = null;
                if (SendOrder(order))
                {
                    tmp = recv.Split(' ')[2];
                    mailinfo = new NewMailInfo(n, tmp);
                }
                else
                {
                    WarningMessage("获取失败");
                    return;
                }
                // 获取头部信息
                order = "top " + n.ToString() + " 0\r\n";
                if (SendOrder(order))
                {
                    PrintRecv(recv = sr.ReadLine());
                    while ((recv = sr.ReadLine()) != ".")
                    {
                        if (recv.ToLower().StartsWith("from"))
                        {
                            tmp = recv.Substring(5);
                            mailinfo.From = new MailAddress(tmp);
                        }
                        else if (recv.ToLower().StartsWith("to"))
                        {
                            mailinfo.To = new MailAddress(recv.Substring(3));

                        }
                        else if (recv.ToLower().StartsWith("subject"))
                        {
                            tmp = recv.Substring(8);
                            mailinfo.Subject = tmp;
                        }
                        else if (recv.ToLower().StartsWith("date"))
                        {
                            mailinfo.Date = Convert.ToDateTime(recv.Substring(5, recv.IndexOf("+0800") - 5).Trim());
                        }
                    }
                }
                mailmsg.MailInfo = mailinfo;
            }
            catch(Exception e)
            {
                PrintRecv(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
        }
        #endregion

        #region 按钮  收取附件
        private void btnRcvClousure_Click(object sender, EventArgs e)
        {
            if (isTooLong)
            {
                WarningMessage("很长");
            }
            else
            {
                WarningMessage("不长");

                int start;
                String transfer = "";
                String content_type = GetTextType(enlousure, "Content-Type:", "\r\n").Replace(";", "").Trim();
                String filename = GetTextType(enlousure, "filename=\"", "\"").Replace("\"", "").Trim();
                String content_transfer = GetTextType(enlousure, "Content-Transfer-Encoding:", "\r\n").Trim();
                enlousure = enlousure.Substring(enlousure.IndexOf("\r\n\r\n"));
                byte[] filebytes;

                if (content_type.IndexOf("application/") == 0)
                {
                    switch (content_transfer)
                    {
                        case "base64":
                            filebytes = Convert.FromBase64String(enlousure);

                            String path = "G:\\Temp\\enclousure";
                            if (!Directory.Exists(path))
                                Directory.CreateDirectory(path);
                            // MemoryStream memory = new MemoryStream(filebytes);
                            FileStream stream = new FileStream(path + "\\" + filename, FileMode.OpenOrCreate, FileAccess.Write);
                            stream.Write(filebytes, 0, filebytes.Length);
                            stream.Close();
                            break;
                    }
                }
                else if(content_type.IndexOf("image/") == 0)
                {

                }

                //if (enlousure.IndexOf("application/") != -1)
                //{
                //    start = enlousure.IndexOf("\r\n\r\n");
                //    if (_StarIndex != -1)
                //        _ReturnText = p_Mail.Substring(_StarIndex, p_Mail.Length - _StarIndex);
                //    _Transfer = GetTextType(p_Mail, "Content-Transfer-Encoding: ", "\r\n").Trim();
                //    String _Name = GetTextType(p_Mail, "filename=\"", "\"").Replace("\"", "");
                //    _Name = GetReadText(_Name);
                //    byte[] _FileBytes = new byte[0];
                //    switch (_Transfer)
                //    {
                //        case "base64":
                //            _FileBytes = Convert.FromBase64String(_ReturnText);
                //            break;
                //    }
                //}
            }
        }
        #endregion

        #region 对话框 警告信息
        private void WarningMessage(String text)
        {
            MessageBox.Show(text);
        }
        #endregion

        #region 对话框 确认信息
        private bool CheckMessage(String text, String title)
        {
            return MessageBox.Show(text, title, MessageBoxButtons.OKCancel) == DialogResult.OK;
        }
        #endregion

        #region 测试
        // 测试按钮
        // 测试函数
        #endregion

    }
}
