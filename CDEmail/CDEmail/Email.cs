using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CDEmail
{
    public partial class Email : Form
    {
        private static Email formInstance;
        public static Email GetIntance
        {
            get
            {
                if (formInstance != null)
                {
                    return formInstance;
                }
                else
                {
                    formInstance = new Email();
                    return formInstance;
                }
            }
        }


        public object lockObj = new object();
        public bool formSwitchFlag = false;

        /// <summary>
        /// 子窗体界面单例元素
        /// </summary>
        public static Send send = null;
        public static Receive receive = null;
        public static ReceiveList receiveList = null;

        /// <summary>
        /// 当前显示窗体
        /// </summary>
        private System.Windows.Forms.Form currentForm;
        public Email()
        {
            InitializeComponent();

            //主窗体容器打开
            this.IsMdiContainer = true;

            //实例化子窗体界面
            send = Send.GetIntance;
            receive = Receive.GetIntance;
            receiveList = ReceiveList.GetIntance;

            //设置子窗体的父窗体
            send.BaseForm = this;
            receive.baseform = this;
            receiveList.baseform = this;

            //初始化按钮
            this.initButton();
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

        private bool initButton()
        {
            try
            {
                this.button1.BackColor = Color.FromArgb(255, 239, 213);
                this.button2.BackColor = Color.FromArgb(255, 239, 213);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 显示窗体
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="frm"></param>
        public void ShowForm(System.Windows.Forms.Panel panel, System.Windows.Forms.Form frm)
        {
            lock (this)
            {
                try
                {
                    if (this.currentForm != null && this.currentForm == frm)
                    {
                        return;
                    }
                    else if (this.currentForm != null)
                    {
                        if (this.ActiveMdiChild != null)
                        {
                            this.ActiveMdiChild.Hide();
                        }
                    }
                    this.currentForm = frm;
                    frm.TopLevel = false;
                    frm.MdiParent = this;
                    panel.Controls.Clear();
                    panel.Controls.Add(frm);
                    frm.Show();
                    frm.Dock = System.Windows.Forms.DockStyle.Fill;
                    this.Refresh();
                    foreach (Control item in frm.Controls)
                    {
                        item.Focus();
                        break;
                    }
                }
                catch (System.Exception ex)
                {
                    //
                }
            }
        }

        public void button1_Click(object sender, EventArgs e)
        {
            ShowSend();
        }

        public void button2_Click(object sender, EventArgs e)
        {
            ShowReceiveList();
        }

        public void ShowSend()
        {
            try
            {
                this.initButton();
                this.button1.BackColor = Color.FromArgb(95, 129, 174);
                Monitor.Enter(this.lockObj);
                if (!formSwitchFlag)
                {
                    formSwitchFlag = true;
                    this.ShowForm(pnlCenter, send);
                    formSwitchFlag = false;
                }
                else
                {
                    return;
                }
            }
            catch (System.Exception ex)
            {
                //
            }
            finally
            {
                Monitor.Exit(this.lockObj);
            }
        }
        public void ShowReceiveList()
        {
            try
            {
                this.initButton();
                this.button2.BackColor = Color.FromArgb(95, 129, 174);
                Monitor.Enter(this.lockObj);
                if (!formSwitchFlag)
                {
                    formSwitchFlag = true;
                    //this.ShowForm(pnlCenter, receive);
                    this.ShowForm(pnlCenter, receiveList);
                    formSwitchFlag = false;
                }
                else
                {
                    return;
                }
            }
            catch (System.Exception ex)
            {
                //
            }
            finally
            {
                Monitor.Exit(this.lockObj);
            }
        }

        public void ShowMail(NewMailInfo mailinfo, String _server, int _port, String _user, String _pwd)
        {
            try
            {
                this.initButton();
                Monitor.Enter(this.lockObj);
                if (!formSwitchFlag)
                {
                    formSwitchFlag = true;

                    receive.mailmsg = new NewMailMessage();
                    receive.mailmsg.MailInfo = mailinfo;
                    receive.pop3server = _server;
                    receive.pop3port = _port;
                    receive.user = _user;
                    receive.pwd = _pwd;
                    receive.ShowMailMessage();

                    this.ShowForm(pnlCenter, receive);

                    formSwitchFlag = false;
                }
                else
                {
                    return;
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                Monitor.Exit(this.lockObj);
            }
        }

        public void ReplyMail(String username, String password, String to)
        {
            try
            {
                this.initButton();
                this.button1.BackColor = Color.FromArgb(95, 129, 174);
                Monitor.Enter(this.lockObj);
                if (!formSwitchFlag)
                {
                    formSwitchFlag = true;
                    send.InitTextBox(username, password, to);
                    this.ShowForm(pnlCenter, send);
                    formSwitchFlag = false;
                }
                else
                {
                    return;
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                Monitor.Exit(this.lockObj);
            }
        }

    }
}
