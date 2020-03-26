using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using test;

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
            int port = int.Parse(textBox2.Text);
            string email = textBox3.Text;
            string password = textBox4.Text;
            string target = textBox5.Text;
            string title = textBox6.Text;
            string body = textBox8.Text;

            
            CSendMail * csm = new CSendMail();
            csm->SetSMTP(address, port);
            csm->LoginSMTP(email, password);
            csm->SetEnclPath(filenames);
            csm->SetTargetEmail(target, title, body, true);
            //csm->SetTargetEmail(target, title, body);
            csm->Send();
        }

        private void Filebutton_Click(object sender, EventArgs e)
        {

        }

        private void Deletebutton_Click(object sender, EventArgs e)
        {

        }
    }
}
