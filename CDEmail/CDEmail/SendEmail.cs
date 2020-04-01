using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace CDEmail
{
    public class SendEmail
    {
        //NetworkCredential evidence;//发件邮箱的登陆凭证
        SmtpClient smpt;//SMTP 事务的主机的名称或 IP 地址
        MailAddress sendadress; //发件地址对象
        MailMessage sendmessage;//邮件对象


        string emailStr = @"([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,5})+"; //邮箱正则表达式对象
        string fileStr = @"^[a-zA-Z]:(((\\(?! )[^/:*?<>\""|\\]+)+\\?)|(\\)?)\s*$";//文件路径正则表达式对象
        /// <summary>
        /// 邮件标题
        /// </summary>
        public string Emailhead { get; set; }
        /// <summary>
        /// 邮件主体信息
        /// </summary>
        public string Emailbody { get; set; }
        /// <summary>
        /// 设置发件邮箱的相关信息
        /// </summary>
        /// <param name="Sendadress">发件地址</param>
        /// <param name="id">SMTP服务登陆账号</param>
        /// <param name="pwd">授权码</param>
        /// <param name="smpt">SMTP 事务的主机的名称或 IP 地址</param>
        public SendEmail(string Sendadress, string id, string pwd, string smptstr)
        {
            if (!CheckEmailAdress(Sendadress))
                throw new Exception("错误的邮箱地址");
            this.sendadress = new MailAddress(Sendadress);//根据地址字符串生成地址对象
            this.sendmessage = new MailMessage();
            this.sendmessage.From = sendadress;//设置邮件对象的发送地址

            smpt = new SmtpClient(smptstr);
            smpt.UseDefaultCredentials = true;//使用默认凭据
            smpt.Credentials = new NetworkCredential(id, pwd);
            smpt.EnableSsl = true; //启用ssl,也就是安全发送
        }
        /// <summary>
        /// 添加收件人
        /// </summary>
        /// <param name="goaladress">收件地址</param>
        /// <returns></returns>
        public bool AddGoalAdress(string goaladress)
        {
            //验证字符串是否是有效的邮箱地址
            if (!CheckEmailAdress(goaladress))
                return false;
            sendmessage.To.Add(goaladress);
            return true;
        }
        public bool AddFile(string filepath)
        {
            Regex fileReg = new Regex(fileStr);
            //验证字符串是否是有效的文件地址
            if (!fileReg.IsMatch(filepath) || !File.Exists(filepath))
            {
                throw new Exception("错误的文件地址格式或者文件不存在");
            }

            sendmessage.Attachments.Add(new Attachment(filepath.Replace('\\', '/')));
            return true;
        }
        public bool Send()
        {
            if (sendmessage.To.Count == 0)
                return false;
            try
            {
                sendmessage.Subject = Emailhead;
                sendmessage.Body = Emailbody;
                smpt.Send(sendmessage);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 验证字符串是否是有效的邮箱地址
        /// </summary>
        /// <param name="address">地址字符串</param>
        /// <returns></returns>
        public bool CheckEmailAdress(string address)
        {
            Regex emailReg = new Regex(emailStr);
            //验证字符串是否是有效的邮箱地址
            return emailReg.IsMatch(address);
        }
    }
}
