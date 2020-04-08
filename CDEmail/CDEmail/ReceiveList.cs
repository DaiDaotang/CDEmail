using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CDEmail
{
    public partial class ReceiveList : Form
    {
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

        // 父窗体
        public Email BaseForm
        {
            get;set;
        }

        // 邮件数量
        public int MailCount
        {
            get;set;
        }
        
        // 当前正在浏览的页数
        public int Page
        {
            get;set;
        }

        // 连接按钮
        private void button1_Click(object sender, EventArgs e)
        {
            Connect();
        }

        // 连接客服端，获取邮件头部
        private void Connect()
        {
            Thread thread = new Thread(new ThreadStart(connectToServer));
            thread.IsBackground = true;
            thread.Start();
        }

        // 连接服务器，将邮件头文件ArrayList添加到DataGridView中
        private void connectToServer()
        {
            // 创建对象
            ReceiveMail receiveMail = new ReceiveMail(tPop3Server.Text, tUsername.Text, tPassword.Text);

            // 获取邮件数量
            MailCount = receiveMail.GetNumberOfNewMessages();

            // 将信息标题等信息列入DataGridView中
            ArrayList msglist = receiveMail.GetNewMailInfo();
            ChangeDGVMail(msglist);
        }

        // 保证线程安全更改DataGridView
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

        private void btnReadMail_Click(object sender, EventArgs e)
        {
            if(dgvMails.SelectedRows.Count != 1)
            {
                WarningMessage("请选择一封邮件以读取");
                return;
            }

            BaseForm.ShowMail();
        }

        private void btnDeleteMail_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection collection = dgvMails.SelectedRows;
        }

        private void btnPrePage_Click(object sender, EventArgs e)
        {

        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {

        }

        private void WarningMessage(String text)
        {
            MessageBox.Show(text);
        }
    }
}
