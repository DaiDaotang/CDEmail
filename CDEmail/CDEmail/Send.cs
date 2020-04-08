using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Configuration;


namespace CDEmail
{
    public partial class Send : Form
    {
        
        private static Send formInstance;
        public static Send GetIntance
        {
            get
            {
                if (formInstance != null)
                {
                    return formInstance;
                }
                else
                {
                    formInstance = new Send();
                    return formInstance;
                }
            }
        }

        public Email BaseForm
        {
            get; set;
        }

        public Send()
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

        private void Sendbutton_Click(object sender, EventArgs e)
        {
            string address = textBox1.Text;
            //int port = int.Parse(textBox2.Text);
            string from = textBox3.Text;
            string AuthorizationCode = textBox4.Text;
            string to = textBox5.Text;
            string title = textBox6.Text;
            string body = textBox8.Text;
            


            /*SendEmail se = new SendEmail("whuddt@126.com", "whuddt@126.com", "AHBZYBMCDUUBJQKM", "smtp.126.com");
            se.AddGoalAdress("1025563447@qq.com");
            se.Emailbody = "this  is a  testxvv sdfsfsdfsdf afaf afawdqwdeeeeeee";
            se.Emailhead = "test wq w email";
            //se.AddFile(@"C:\Users\66\Desktop\demo.docx");
            //se.AddFile(@"C:\Users\66\Desktop\demo2.docx");
            se.Send();*/
            SendEmail se = new SendEmail(from, from, AuthorizationCode, address);
            se.AddGoalAdress(to);
            se.Emailbody = body;
            se.Emailhead = title;
            for (int i = 0; i < comboBox1.Items.Count; i++)
            {
                se.AddFile(@comboBox1.GetItemText(comboBox1.Items[i]));
            }
            if (se.Send())
            {
                MessageBox.Show("邮件发送成功！");
            }
            else
            {
                MessageBox.Show("邮件发送失败！");
            }
        }

    

        private void Filebutton_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFDialogSFile = new OpenFileDialog();
            oFDialogSFile.InitialDirectory = "C:\\";//设置对话框的初始目录为C盘
            oFDialogSFile.Filter = "all files (*.*)|*.*";//筛选字符串为所有文件
            oFDialogSFile.RestoreDirectory = true;
            oFDialogSFile.ShowDialog();
            comboBox1.Items.Add(oFDialogSFile.FileName.Trim());//当选择好文件后将文件名赋值给下拉框
            comboBox1.Text = oFDialogSFile.FileName.Trim();
        }

        private void Deletebutton_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("没有附件可删！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                comboBox1.Items.Remove(comboBox1.Text.Trim());
                if (comboBox1.Items.Count == 0)
                {
                    comboBox1.Text = "";
                }
            }
        }
    }
}
