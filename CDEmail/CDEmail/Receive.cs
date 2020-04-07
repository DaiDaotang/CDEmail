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

namespace CDEmail
{
    public partial class Receive : Form
    {
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

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(connectToServer));
            thread.IsBackground = true;
            thread.Start();
        }

        private void connectToServer()
        {
            // 创建对象
            ReceiveMail receiveMail = new ReceiveMail(tPop3Server.Text, tUsername.Text, tPassword.Text);

            // 将信息标题列表收入列表中
            ArrayList msglist = receiveMail.GetNewMailInfo();
            //for (int i = 0; i < msglist.Count; i++)
            //{
            //    Console.WriteLine("Mail " + i + "\r\n" + ((NewMailInfo)msglist[i]).ToString());
            //}
            MailMessage mail = receiveMail.GetANewMail((NewMailInfo)msglist[4]);
            Console.WriteLine("From:");
            Console.WriteLine(mail.From);
            Console.WriteLine("To:");
            Console.WriteLine(mail.To);
            Console.WriteLine("Subject:");
            Console.WriteLine(mail.Subject);
            Console.WriteLine("Body:");
            Console.WriteLine(mail.Body);

            //receiveMail.Test(3);
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {

        }

        private void btnReadMail_Click(object sender, EventArgs e)
        {

        }

        private void btnRcvClousure_Click(object sender, EventArgs e)
        {

        }
    }
}
