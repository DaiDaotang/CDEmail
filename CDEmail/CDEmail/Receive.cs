using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            // 连接调用了查看信息数量的函数
            ReceiveMail receiveMail = new ReceiveMail(tPop3Server.Text, tUsername.Text, tPassword.Text);
            MessageBox.Show(receiveMail.GetNumberOfNewMessages().ToString());
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
