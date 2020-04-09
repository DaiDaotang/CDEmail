using System.Net.Sockets;
using System.Collections;
using System.IO;
using System.Net;
using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace CDEmail
{
    public class ReceiveEMail
    {
        string server;
        int port;
        string user;
        string pwd;

        TcpClient tc;
        NetworkStream ns;
        StreamReader sr;

        static int count;

        // 构造函数
        #region
        public ReceiveEMail(String _server, String _user, String _pwd)
        {
            server = _server;
            port = 110;
            user = _user;
            pwd = _pwd;
        }
        #endregion

        // 命令
        #region
        private bool SendOrder(String input)
        {
            try
            {
                Byte[] outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
                ns.Write(outbytes, 0, outbytes.Length);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return false;
            }
            finally
            {
                Disconnect();
            }
            return true;
        }
        #endregion

        // 连接登录
        #region
        private void Connect()
        {
            tc = new TcpClient(server, port);
            String input = "";
            String line = "";

            try
            {
                input = "user " + user + "\r\n";
                SendOrder(input);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
        #endregion

        // 断开连接
        #region
        private void Disconnect()
        {

        }
        #endregion

        // 获得邮件数目
        #region
        #endregion

        // 删除第n封邮件
        #region
        #endregion

        // 获取第start封-第end封邮件头部分
        #region
        #endregion

        // 获取邮件全文
        #region
        #endregion
    }
}
