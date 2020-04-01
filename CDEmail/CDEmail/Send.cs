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
            string from = textBox3.Text;
            string AuthorizationCode = textBox4.Text;
            string to = textBox5.Text;
            string title = textBox6.Text;
            string body = textBox8.Text;
            string File_Path = textBox7.Text;


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
            //se.AddFile(@"C:\Users\66\Desktop\demo.docx");
            //se.AddFile(@"C:\Users\66\Desktop\demo2.docx");
            se.Send(); 

            //MailMessage msg = new MailMessage();

            //msg.To.Add(to);//收件人地址 
            ////msg.CC.Add("cc@qq.com");//抄送人地址 

            //msg.From = new MailAddress(from);//发件人邮箱
            //msg.Subject = title;//邮件标题 
            //msg.SubjectEncoding = Encoding.UTF8;//标题格式为UTF8 

            //msg.Body = body;//邮件内容 
            //msg.BodyEncoding = Encoding.UTF8;//内容格式为UTF8 

            ////设置邮件的附件，将在客户端选择的附件先上传到服务器保存一个，然后加入到mail中  
            //if (File_Path != "" && File_Path != null)
            //{
            //    //将附件添加到邮件
            //    msg.Attachments.Add(new Attachment(File_Path));
            //    //获取或设置此电子邮件的发送通知。
            //    msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
            //}

            //SmtpClient client = new SmtpClient();

            //if (address.Length != 0)
            //{
            //    //根据发件人的邮件地址判断发件服务器地址   默认端口一般是25
            //    string[] addressor = from.Trim().Split(new Char[] { '@', '.' });
            //    switch (addressor[1])
            //    {
            //        case "163":
            //            if(address== "smtp.163.com")
            //            {
            //                client.Host = address;
            //            }
            //            else
            //            {
            //                Console.WriteLine("邮件地址与服务器不符！");
            //            }                      
            //            break;
            //        case "126":
            //            if (address == "smtp.126.com")
            //            {
            //                client.Host = address;
            //            }
            //            else
            //            {
            //                Console.WriteLine("邮件地址与服务器不符！");
            //            }
            //            break;
            //        case "qq":
            //            client.Host = "smtp.qq.com";
            //            break;
            //        case "gmail":
            //            client.Host = "smtp.gmail.com";
            //            break;
            //        case "hotmail":
            //            client.Host = "smtp.live.com";//outlook邮箱
            //                                          //client.Port = 587;
            //            break;
            //        case "foxmail":
            //            client.Host = "smtp.foxmail.com";
            //            break;
            //        case "sina":
            //            client.Host = "smtp.sina.com.cn";
            //            break;
            //        default:
            //            client.Host = "smtp.exmail.qq.com";//qq企业邮箱
            //            break;
            //    }
            //}

            //client.Port = port;//SMTP端口，QQ邮箱填写587 

            //client.EnableSsl = true;//启用SSL加密 
            //                        //发件人邮箱账号，授权码(注意此处，是授权码你需要到qq邮箱里点设置开启Smtp服务，然后会提示你第三方登录时密码处填写授权码)
            //client.Credentials = new System.Net.NetworkCredential(from, AuthorizationCode);

            ////如果发送失败，SMTP 服务器将发送 失败邮件告诉我  
            //msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            //try
            //{
            //    client.Send(msg);//发送邮件
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, @"异常错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

    

        private void Filebutton_Click(object sender, EventArgs e)
        {

        }

        private void Deletebutton_Click(object sender, EventArgs e)
        {

        }
    }
}
