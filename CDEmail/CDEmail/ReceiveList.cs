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

        private ReceiveMail ReceiveMailConnect
        {
            get;set;
        }

        // 父窗体
        public Email BaseForm
        {
            get;set;
        }
        // 邮件总数量
        public int MailCount
        {
            get;set;
        }
        // 邮件头部信息列表
        public ArrayList MailList
        {
            get;set;
        }
        // 当前页数
        public int CurrentPage
        {
            get;set;
        }
        // 每页邮件数量
        public int CountPerPage
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
        // 若能使用这一个函数，则一定是重新连接，所以Page=1
        private void connectToServer()
        {

            // 第1页，每页30封
            CurrentPage = 1;
            CountPerPage = 30;

            // 创建对象
            ReceiveMailConnect = new ReceiveMail(tPop3Server.Text, tUsername.Text, tPassword.Text);

            // 获取邮件数量
            MailCount = ReceiveMailConnect.GetNumberOfNewMessages();

            // 将信息标题等信息列入DataGridView中
            MailList = ReceiveMailConnect.GetNewMailInfo(MailCount, Math.Max(MailCount - 30, 0));
            ChangeDGVMail(MailList);
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

        // 读
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
                // WarningMessage(msg.ToString());
                BaseForm.ShowMail((NewMailInfo)MailList[n], ReceiveMailConnect);
            }                
         }

        // 删
        private void btnDeleteMail_Click(object sender, EventArgs e)
        {
            int n = dgvMails.SelectedCells.Count;

            if (n == 0)
            {
                WarningMessage("请选中邮件");
            }
            else
            {
                ArrayList list = new ArrayList();
                int tmp = 0;

                foreach (DataGridViewCell cell in dgvMails.SelectedCells)
                    if (!list.Contains((tmp = (int)dgvMails.Rows[cell.RowIndex].Cells[0].Value)))
                        list.Add(tmp);

                if (CheckMessage(String.Format("确认删除{0:D1}封邮件吗？", list.Count), "删除确认框"))
                {
                    foreach(int msg in list)
                    {
                        if (ReceiveMailConnect.DeleteMessage(msg))
                        {
                            continue;
                        }
                        else
                        {
                            WarningMessage("删除有错误，请稍后重试");
                            break;
                        }
                    }
                    WarningMessage("删除成功");
                    Connect();
                }
            }
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

        private bool CheckMessage(String text, String title)
        {
            return MessageBox.Show(text, title, MessageBoxButtons.OKCancel) == DialogResult.OK;
        }
    }
}
