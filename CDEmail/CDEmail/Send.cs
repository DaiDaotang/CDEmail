using System;
using System.Data;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.Text;

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
        #region Private Variables

        private TcpClient Server;
        private NetworkStream StrmWtr;
        private StreamReader StrmRdr;
        private String cmdData;
        private byte[] szData;
        private const String CRLF = "\r\n";
        #endregion

        #region Private Functions

        private String getSatus()
        {
            String ret = StrmRdr.ReadLine();
            lsb_status.Items.Add(ret);
            lsb_status.SelectedIndex = lsb_status.Items.Count - 1;
            return ret;
        }
        #endregion

        private void AddFile(DataTable filelist, String path)
        {
            //根据路径读出文件流   
            FileStream fstr = new FileStream(path, FileMode.Open);//建立文件流对象   
            byte[] by = new byte[Convert.ToInt32(fstr.Length)];
            fstr.Read(by, 0, by.Length);//读取文件内容   
            fstr.Close();//关闭   
                         //格式转换   
            String fileinfo = Convert.ToBase64String(by);//转化为base64编码   
                                                         //增加到文件表中   
            DataRow dr = filelist.NewRow();
            dr[0] = Path.GetFileName(path);//获取文件名   
            dr[1] = fileinfo;//文件内容   
            filelist.Rows.Add(dr);//增加   
        }

        private void Sendbutton_Click(object sender, EventArgs e)
        {
            try
            {
                //Send Email
                cmdData = "MAIL FROM: <" + tb_from.Text + ">" + CRLF;
                szData = System.Text.Encoding.ASCII.GetBytes(cmdData.ToCharArray());
                StrmWtr.Write(szData, 0, szData.Length);
                this.getSatus();

                cmdData = "RCPT TO: <" + tb_to.Text + ">" + CRLF;
                szData = System.Text.Encoding.ASCII.GetBytes(cmdData.ToCharArray());
                StrmWtr.Write(szData, 0, szData.Length);
                this.getSatus();

                cmdData = "DATA" + CRLF;
                szData = System.Text.Encoding.ASCII.GetBytes(cmdData.ToCharArray());
                StrmWtr.Write(szData, 0, szData.Length);
                this.getSatus();

                cmdData = "from: " + tb_from.Text + CRLF
                            + "to: " + tb_to.Text + CRLF
                            + "subject: " + tb_subject.Text + CRLF
                            + "Content-Type:multipart/mixed;boundary=\"unique-boundary-1\"" + CRLF + CRLF + CRLF
                            + "--unique-boundary-1" + CRLF
                            + "Content-Type:   multipart/alternative;boundary=\"unique-boundary-2\"" + CRLF + CRLF
                            + "--unique-boundary-2" + CRLF
                            + "Content-Type:text/plain;charset=\"UTF-8\"" + CRLF + CRLF
                            + tb_content.Text + CRLF + CRLF
                            + "--unique-boundary-2--" + CRLF + CRLF;
                // szData = System.Text.Encoding.UTF8.GetBytes(cmdData.ToCharArray());
                szData = System.Text.Encoding.GetEncoding(936).GetBytes(cmdData.ToCharArray());
                StrmWtr.Write(szData, 0, szData.Length);
                

                DataTable filelist = new DataTable(); 
                filelist.Columns.Add(new DataColumn("filename", typeof(string)));//文件名   
                filelist.Columns.Add(new DataColumn("filecontent", typeof(string)));//文件内容   
                for (int i = 0; i < comboBox1.Items.Count; i++)
                {
                    AddFile(filelist, @comboBox1.GetItemText(comboBox1.Items[i]));
                }
                for (int i = 0; i < filelist.Rows.Count; i++)
                {
                    DataRow dr = filelist.Rows[i];
                    cmdData = "--unique-boundary-1" + CRLF
                        + "Content-Type: application/octet-stream;name=\"" + dr[0].ToString() + "\"" + CRLF
                        + "Content-Type: text/plain;charset=\"UTF-8\"" + CRLF
                        + "Content-Transfer-Encoding: base64" + CRLF
                        + "Content-Disposition: attachment; filename=\"" + dr[0].ToString() + "\"" + CRLF + CRLF
                        + dr[1].ToString() + CRLF + CRLF;
                    szData = System.Text.Encoding.UTF8.GetBytes(cmdData.ToCharArray());
                    StrmWtr.Write(szData, 0, szData.Length);
                }
                cmdData = "--unique-boundary-1--" + CRLF + "." + CRLF;
                szData = System.Text.Encoding.UTF8.GetBytes(cmdData.ToCharArray());
                StrmWtr.Write(szData, 0, szData.Length);
                string r = this.getSatus();
                if (r.IndexOf("250")!=-1)
                {
                    MessageBox.Show("邮件发送成功！");
                }
                else
                {
                    MessageBox.Show("邮件发送失败！");
                }
            }
            catch (InvalidOperationException err)
            {
                lsb_status.Items.Add("ERROR: " + err.ToString());
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

        private void btn_conn_Click(object sender, EventArgs e)
        {
            if (btn_conn.Text == "连接")
            {
                Cursor cr = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;
                Server = new TcpClient(tb_server.Text, 25);
                lsb_status.Items.Clear();
                try
                {
                    StrmWtr = Server.GetStream();
                    StrmRdr = new StreamReader(Server.GetStream());
                    this.getSatus();

                    //Login
                    cmdData = "HELO " + tb_server.Text + CRLF;
                    szData = System.Text.Encoding.ASCII.GetBytes(cmdData.ToCharArray());
                    StrmWtr.Write(szData, 0, szData.Length);
                    this.getSatus();

                    cmdData = "AUTH LOGIN" + CRLF;
                    szData = System.Text.Encoding.ASCII.GetBytes(cmdData.ToCharArray());
                    StrmWtr.Write(szData, 0, szData.Length);
                    this.getSatus();

                    cmdData = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(tb_username.Text)) + CRLF;
                    szData = System.Text.Encoding.ASCII.GetBytes(cmdData.ToCharArray());
                    StrmWtr.Write(szData, 0, szData.Length);
                    this.getSatus();

                    cmdData = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(tb_password.Text)) + CRLF;
                    szData = System.Text.Encoding.ASCII.GetBytes(cmdData.ToCharArray());
                    StrmWtr.Write(szData, 0, szData.Length);
                    this.getSatus();


                    btn_conn.Text = "断开";
                    btn_send.Enabled = true;

                }
                catch (InvalidOperationException err)
                {
                    lsb_status.Items.Add("ERROR: " + err.ToString());
                }
                finally
                {
                    Cursor.Current = cr;
                }
            }
            else
            {
                Cursor cr = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;

                //Logout
                cmdData = "QUIT" + CRLF;
                szData = System.Text.Encoding.ASCII.GetBytes(cmdData.ToCharArray());
                StrmWtr.Write(szData, 0, szData.Length);
                this.getSatus();

                StrmWtr.Close();
                StrmRdr.Close();


                btn_conn.Text = "连接";
                btn_send.Enabled = false;

                Cursor.Current = cr;
            }
        }
    }
}
