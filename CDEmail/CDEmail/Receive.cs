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

        public Email BaseForm
        {
            get; set;
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

        private void btnBack_Click(object sender, EventArgs e)
        {
            BaseForm.ShowReceiveList();
        }
    }
}
