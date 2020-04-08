using System.Net.Sockets;
using System.Collections;
using System.IO;
using System.Net;
using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace CDEmail
{
    public class ReceiveMail
    {
        string POPServer;
        string user;
        string pwd;
        NetworkStream ns;
        StreamReader sr;
        static int count;


        //调用Srfile类，写入文件内容和文件名,文件格式：.txt 读邮件信息
        // Srfile Filesr = new Srfile();
        //对邮件标题 base64 解码


        /// <summary>
        /// base64 解码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private string DecodeBase64(string code) //string code_type,
        {
            string decode = "";
            string st = code + "000";//
            string strcode = st.Substring(0, (st.Length / 4) * 4);
            byte[] bytes = Convert.FromBase64String(strcode);
            try
            {
                decode = System.Text.Encoding.GetEncoding("GB2312").GetString(bytes);
            }
            catch
            {
                decode = code;
            }
            return decode;
        }


        //对邮件标题解码 quoted-printable
        /// <summary>
        /// quoted-printable 解码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private string DecodeQ(string code)
        {

            string[] textArray1 = code.Split(new char[] { '=' });
            byte[] buf = new byte[textArray1.Length];
            try
            {

                for (int i = 0; i < textArray1.Length; i++)
                {
                    if (textArray1[i].Trim() != string.Empty)
                    {

                        byte[] buftest = new byte[2];

                        buf[i] = (byte)int.Parse(textArray1[i].Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    }
                }
            }
            catch
            {
                return null;
            }
            return System.Text.Encoding.Default.GetString(buf);
        }


        public ReceiveMail() { }


        #region
        /// <summary>
        /// 接收邮件服务器相关信息
        /// </summary>
        /// <param name="server">参数 pop邮件服务器地址 </param>
        /// <param name="user">参数 登录到pop邮件服务器的用户名 </param>
        /// <param name="pwd">参数 登录到pop邮件服务器的密码</param>
        /// <returns>无返回</returns>
        public ReceiveMail(string server, string _user, string _pwd)
        {
            POPServer = server;
            user = _user;
            pwd = _pwd;
        }
        #endregion


        //登陆服务器
        private TcpClient Connect()
        {
            TcpClient sender = new TcpClient(POPServer, 110);
            Byte[] outbytes;
            string input;
            string str = string.Empty;
            try
            {
                ns = sender.GetStream();
                sr = new StreamReader(ns);
                str = sr.ReadLine();

                Console.WriteLine(str);

                //检查密码
                input = "user " + user + "\r\n";
                outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
                ns.Write(outbytes, 0, outbytes.Length);
                str = sr.ReadLine();

                Console.WriteLine(str);

                input = "pass " + pwd + "\r\n";
                outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
                ns.Write(outbytes, 0, outbytes.Length);
                str = sr.ReadLine();

                Console.WriteLine(str);

                return sender;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("用户名或密码错误");
                return null;
            }
        }

        //为了读到数据流中的正确信息，重新建的一个方法（只是在读邮件详细信息是用到，即GetNewMessages()方法中用到，这样就可以正常显示邮件正文的中英文）
        private void Connecttest(TcpClient tcpc)
        {
            Byte[] outbytes;
            string input;
            string readuser = string.Empty;
            string readpwd = string.Empty;
            try
            {
                ns = tcpc.GetStream();
                sr = new StreamReader(ns);
                sr.ReadLine();
                //Console.WriteLine(sr.ReadLine() );
                input = "user " + user + "\r\n";
                outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
                ns.Write(outbytes, 0, outbytes.Length);

                readuser = sr.ReadLine();

                input = "pass " + pwd + "\r\n";
                outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
                ns.Write(outbytes, 0, outbytes.Length);
                readpwd = sr.ReadLine();
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("用户名或密码错误");
            }
        }

        //断开与服务器的连接
        private void Disconnect()
        {
            //"quit" 即表示断开连接
            string input = "quit" + "\r\n";
            Byte[] outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
            ns.Write(outbytes, 0, outbytes.Length);
            //关闭数据流
            ns.Close();
        }

        //获得新邮件数目
        #region
        /// <summary>
        /// 获取邮件数目
        /// </summary>
        /// <returns>返回 int 邮件数目</returns>
        public int GetNumberOfNewMessages()
        {
            Byte[] outbytes;
            string input;
            int countmail;
            try
            {
                Connect();
                //"stat"向邮件服务器 表示要取邮件数目
                input = "stat" + "\r\n";
                //将string input转化为7位的字符，以便可以在网络上传输
                outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
                ns.Write(outbytes, 0, outbytes.Length);
                string thisResponse = sr.ReadLine();
                //断开与服务器的连接
                Disconnect();

                if (thisResponse.Substring(0, 4) == "-ERR")
                {
                    return -1;
                }
                string[] tmpArray;
                //将从服务器取到的数据以' '分成字符数组
                tmpArray = thisResponse.Split(' ');
                //取到的表示邮件数目
                countmail = Convert.ToInt32(tmpArray[1]);
                count = countmail;
                return countmail;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Could not connect to mail server");
                return -1;//表示读邮件时 出错
            }
        }
        #endregion

        //删除第几封邮件
        #region
        /// <summary>
        ///根据输入的数字，删除相应编号的邮件
        /// </summary>
        /// <param name="messagenumber">参数 删除第几封邮件 </param>
        /// <returns>返回 bool true成功；false 失败</returns>
        public bool DeleteMessage(int messagenumber)
        {
            Connect();
            Byte[] outbytes;
            string input;
            byte[] backmsg = new byte[25];
            string msg = string.Empty;

            try
            {
                input = "dele " + messagenumber.ToString() + "\r\n";
                outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
                ns.Write(outbytes, 0, outbytes.Length);
                ns.Read(backmsg, 0, 25);
                msg = System.Text.Encoding.Default.GetString(backmsg, 0, backmsg.Length);
                Disconnect();
                if (msg.Substring(0, 3) == "+OK")
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion


        // 获取邮件的头部分
        public ArrayList GetNewMailInfo(int start, int end)
        {
            ArrayList res = new ArrayList();

            try
            {
                int newcount = GetNumberOfNewMessages();    // 邮件数量
                TcpClient tcpc = Connect(); // 连接 TCP

                ArrayList msglines = new ArrayList();
                NewMailInfo mailinfo;

                Byte[] outbytes;
                string input;
                string line = "";

                for (int n = start; n > end; n--)
                {
                    input = "top " + n.ToString() + " 0\r\n";
                    outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
                    ns.Write(outbytes, 0, outbytes.Length);
                    sr = new StreamReader(tcpc.GetStream(), System.Text.Encoding.Default);

                    mailinfo = new NewMailInfo(n);

                    Console.WriteLine("---------------------------------------------------" + n);
                    while ((line = sr.ReadLine()) != ".")
                    {
                        if (line.ToLower().StartsWith("from"))
                        {
                            mailinfo.From = new MailAddress(line.Substring(5));
                        }
                        else if (line.ToLower().StartsWith("to"))
                        {
                            mailinfo.To = new MailAddress(line.Substring(3));
                        }
                        else if (line.ToLower().StartsWith("subject"))
                        {
                            mailinfo.Subject = line.Substring(8);
                        }
                        else if (line.ToLower().StartsWith("date"))
                        {
                            mailinfo.Date = Convert.ToDateTime(line.Substring(5, line.IndexOf("+0800") - 5).Trim());
                        }
                    }
                    res.Add(mailinfo);
                }

                return res;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }

        // 获取邮件
        public MailMessage GetANewMail(NewMailInfo mailinfo)
        {
            String rawmessage = sr.ReadToEnd();
            try
            {
                // 连接 TCP
                TcpClient tcpc = Connect();

                Byte[] outbytes;
                string input;
                string line = "";

                input = "retr " + mailinfo.Num.ToString() + "\r\n";
                outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
                ns.Write(outbytes, 0, outbytes.Length);
                sr = new StreamReader(tcpc.GetStream(), System.Text.Encoding.Default);

                ArrayList rawmsg = new ArrayList();     // 原始邮件
                while((line = sr.ReadLine()) != ".")
                {
                    rawmsg.Add(line);
                    Console.WriteLine(line);
                }

                // 断开连接
                Disconnect();   

                MailMessage mail = new MailMessage();
                mail.From = mailinfo.From;
                mail.Subject = mailinfo.Subject;

                // 寻找定义的边界
                String boundary = null;
                int i = 0;
                foreach(String str in rawmsg)
                {
                    Console.WriteLine(line);
                    i++;
                    if (line.StartsWith("Content-Type: multipart/"))
                    {
                        if (!line.Contains("boundary="))
                        {
                            line = sr.ReadLine();
                        }
                        line = line.Trim(' ');
                        line = line.Replace("Content-Type: multipart/", "");
                        line = line.Replace("alternative", "");
                        line = line.Replace("mixed", "");
                        line = line.Replace("boundary=", "");
                        boundary = line.Trim('"', ' ', ';');
                        break;
                    }
                }

                // 邮件主体
                String body = "";

                // 若未寻找到边界
                if(boundary == null)
                {
                    i = 0;
                    foreach(String str in rawmsg)
                    {

                    }
                }
                // 若有边界
                else
                {
                    while(i < rawmsg.Count)
                    {
                        line = (String)rawmsg[i];
                        // 若可能是边界
                        if (line.StartsWith("--"))
                        {
                            // 若确实是边界
                            // 隐约觉得这样会提高速度？因为一个是比较前两个字符，另一个是全部比较
                            if (line == "--" + boundary)
                            {
                                break;
                            }
                        }
                    }
                    // 找到正文
                    while(i < rawmsg.Count)
                    {

                    }

                    for(; i < rawmsg.Count; i++)
                    {
                        line = (String)rawmsg[i];
                        // 若是边界的起始，则可能是边界
                        if (line.StartsWith("--"))
                        {
                            // 若确实是边界
                            if (line.Contains(boundary))
                            {
                                // 第一次边界后就是正文
                                for(int j = i + 1; j < rawmsg.Count; j++)
                                {
                                    line = (String)rawmsg[i];
                                    // 又是边界，则退出
                                    if(line.StartsWith("--") && line.Contains(boundary))
                                    {
                                        break;
                                    }
                                    body += line;
                                }
                                // 正文搜索完毕
                                break;
                            }
                        }
                    }
                }
                mail.Body = body;
                return mail;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }

        // 获取邮件全文
        public String GetANewMailMessage(NewMailInfo mailinfo)
        {
            try
            {
                // 连接 TCP
                TcpClient tcpc = Connect();
                Byte[] outbytes;
                string input;
                String line;
                input = "retr " + mailinfo.Num.ToString() + "\r\n";
                outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
                ns.Write(outbytes, 0, outbytes.Length);
                sr = new StreamReader(tcpc.GetStream(), System.Text.Encoding.Default);
                String rawmessage = ""; // 原信息
                while ((line = sr.ReadLine()) != ".")
                {
                    rawmessage += line+"\r\n";
                    Console.WriteLine(line);
                }
                Disconnect();
                return rawmessage;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return null;
            }
        }
        
        // 测试
        public void Test(int n)
        {
            ShowNMailInfo(n);
            ShowNMail(n);
        }

        private void ShowNMailInfo(int n)
        {
            try
            {
                TcpClient tcpc = Connect(); // 连接 TCP

                Byte[] outbytes;
                string input;
                string line = "";

                input = "top " + n.ToString() + " 0\r\n";
                outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
                ns.Write(outbytes, 0, outbytes.Length);
                sr = new StreamReader(tcpc.GetStream(), System.Text.Encoding.Default);

                Console.WriteLine("---------------------------------------------------");
                while ((line = sr.ReadLine()) != ".")
                {
                    Console.WriteLine(line);
                }

                Console.WriteLine("---------------------------------------------------");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private void ShowNMail(int n)
        {
            try
            {
                TcpClient tcpc = Connect(); // 连接 TCP

                Byte[] outbytes;
                string input;
                string line = "";

                input = "retr " + n.ToString() + "\r\n";
                outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
                ns.Write(outbytes, 0, outbytes.Length);
                sr = new StreamReader(tcpc.GetStream(), System.Text.Encoding.Default);

                Console.WriteLine("---------------------------------------------------");
                while ((line = sr.ReadLine()) != ".")
                {
                    Console.WriteLine(line);
                }

                Console.WriteLine("---------------------------------------------------");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
