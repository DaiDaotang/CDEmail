using System.Net.Sockets;
using System.Collections;
using System.IO;
using System.Net;
using System;
using System.Net.Mail;

namespace CDEmail
{
    class ReceiveMail
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
        private void Connect()
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

                //Console.WriteLine(sr.ReadLine() );

            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("用户名或密码错误");
            }
        }


        //为了读到数据流中的正确信息，重新建的一个方法（只是在读邮件详细信息是用到《即GetNewMessages（）方法中用到，这样就可以正常显示邮件正文的中英文》）
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
                input = "user " + user + "rn";
                outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
                ns.Write(outbytes, 0, outbytes.Length);

                readuser = sr.ReadLine();

                input = "pass " + pwd + "rn";
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
                if (thisResponse.Substring(0, 4) == "-ERR")
                {
                    return -1;
                }
                string[] tmpArray;
                //将从服务器取到的数据以“”分成字符数组
                tmpArray = thisResponse.Split(' ');
                //断开与服务器的连接
                Disconnect();
                //取到的表示邮件数目
                countmail = Convert.ToInt32(tmpArray[1]);
                count = countmail;
                return countmail;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Could not connect to mail server");
                return -1;//表示读邮件时 出错，将接收邮件的线程 阻塞5分钟
            }
        }
        #endregion


        //获取邮件
        #region
        /// <summary>
        /// 获取所有新邮件
        /// </summary>
        /// <returns> 返回 ArrayList</returns>
        public ArrayList GetNewMessages()   //public ArrayList GetNewMessages(string subj)
        {

            bool blag = false;
            int newcount = GetNumberOfNewMessages();
            ArrayList newmsgs = new ArrayList();
            try
            {
                TcpClient tcpc = new TcpClient(POPServer, 110);
                Connecttest(tcpc);
                //    newcount = GetNumberOfNewMessages();

                for (int n = 1; n < newcount + 1; n++)
                {
                    ArrayList msglines = GetRawMessage(tcpc, n);
                    string msgsubj = GetMessageSubject(msglines).Trim();
                    MailMessage msg = new MailMessage();
                    //首先判断Substring是什么编码（"=?gb2312?B?"或者"=?gb2312?Q?"），然后转到相应的解码方法，实现解码
                    //如果是"=?gb2312?B?"，转到DecodeBase64（）进行解码
                    if (msgsubj.Length > 11)
                    {
                        //base64编码
                        if (msgsubj.Substring(0, 11) == "=?gb2312?B?")
                        {
                            blag = true;
                            msgsubj = msgsubj.Substring(11, msgsubj.Length - 13);
                            msg.Subject = DecodeBase64(msgsubj);
                        }
                        //如果是"=?gb2312?Q?"编码，先得取到被编码的部分，字符如果没编码就不转到解码方法
                        if (msgsubj.Length > 11)
                        {

                            if (msgsubj.Substring(0, 11) == "=?gb2312?Q?")
                            {
                                blag = true;
                                msgsubj = msgsubj.Substring(11, msgsubj.Length - 13);
                                string text = msgsubj;
                                string str = string.Empty;
                                string decodeq = string.Empty;
                                while (text.Length > 0)
                                {
                                    //判断编码部分是否开始
                                    if (text.Substring(0, 1) == "=")
                                    {
                                        decodeq = text.Substring(0, 3);
                                        text = text.Substring(3, text.Length - 3);
                                        //当出现编码部分时，则取出连续的编码部分
                                        while (text.Length > 0)
                                        {
                                            if (text.Substring(0, 1) == "=")
                                            {
                                                decodeq += text.Substring(0, 3);
                                                text = text.Substring(3, text.Length - 3);
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        //将连续的编码进行解码
                                        str += DecodeQ(decodeq);
                                    }
                                    //如果该字节没编码，则不用处理
                                    else
                                    {
                                        str += text.Substring(0, 1);
                                        text = text.Substring(1, text.Length - 1);
                                    }

                                }
                                //用空格代替subject中的“0”,以便能取道全部的内容
                                msg.Subject = str.Replace("0", " ");
                            }
                        }
                        if (blag == false)
                        {
                            msg.Subject = msgsubj.Replace("0", " ");
                        }


                    }
                    else
                    {
                        msg.Subject = msgsubj.Replace("0", " ");
                    }
                    blag = false;
                    //取发邮件者的邮件地址
                    msg.From = new MailAddress(GetMessageFrom(msglines));
                    //取邮件正文
                    string msgbody = GetMessageBody(msglines);
                    msg.Body = msgbody;
                    newmsgs.Add(msg);

                    //将收到的邮件保存到本地，调用另一个类的保存邮件方法，不使用此功能
                    //     Filesr.Savefile("subject:"+msg.Subject+"rn"+"sender:"+msg.From+"rn"+"context:"+msg.Body,"mail"+n+".txt");
                    //删除邮件，不使用
                    //      DeleteMessage(n);
                }
                //断开与服务器的连接
                Disconnect();
                return newmsgs;
            }
            catch
            {
                //     System.Windows.Forms.MessageBox.Show("读取邮件出错，请重试");
                return newmsgs;
            }
        }
        #endregion


        //从服务器读邮件信息
        private ArrayList GetRawMessage(TcpClient tcpc, int messagenumber)
        {
            Byte[] outbytes;
            string input;
            string line = "";
            input = "retr " + messagenumber.ToString() + "rn";
            outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
            ns.Write(outbytes, 0, outbytes.Length);
            ArrayList msglines = new ArrayList();
            StreamReader srtext;
            srtext = new StreamReader(tcpc.GetStream(), System.Text.Encoding.Default);
            //每份邮件以英文“.”结束
            do
            {
                line = srtext.ReadLine();
                msglines.Add(line);
            } while (line != ".");
            msglines.RemoveAt(msglines.Count - 1);
            return msglines;
        }


        //获取邮件标题
        private string GetMessageSubject(ArrayList msglines)
        {
            IEnumerator msgenum = msglines.GetEnumerator();
            while (msgenum.MoveNext())
            {
                string line = (string)msgenum.Current;
                if (line.StartsWith("Subject:"))
                {
                    return line.Substring(8, line.Length - 8);
                }
            }
            return "None";
        }


        //获取邮件的发送人地址
        private string GetMessageFrom(ArrayList msglines)
        {
            string[] tokens;
            IEnumerator msgenum = msglines.GetEnumerator();
            while (msgenum.MoveNext())
            {
                string line = (string)msgenum.Current;
                if (line.StartsWith("From"))
                {
                    tokens = line.Split(new Char[] { ':' });
                    return tokens[1].Trim(new Char[] { '<', '>', ' ' });
                }
            }
            return "None";
        }


        //邮件正文
        private string GetMessageBody(ArrayList msglines)
        {
            string body = "";
            string line = " ";
            IEnumerator msgenum = msglines.GetEnumerator();
            while (line.CompareTo("") != 0)
            {
                msgenum.MoveNext();
                line = (string)msgenum.Current;
            }
            while (msgenum.MoveNext())
            {
                body = body + (string)msgenum.Current + "rn";
            }
            return body;
        }


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
                input = "dele " + messagenumber.ToString() + "rn";
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


    }
}
