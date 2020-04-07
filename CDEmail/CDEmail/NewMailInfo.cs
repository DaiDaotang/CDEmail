using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CDEmail
{
    class NewMailInfo
    {
        public MailAddress From
        {
            get;set;
        }
        public MailAddress To 
        {
            get;set;
        }
        public DateTime Date
        {
            get; set;
        }
        public String Subject
        {
            get; set;
        }

        public NewMailInfo()
        {
            From = null;
            To = null;
            Subject = null;
            Date = new DateTime();
        }

        public override string ToString()
        {
            return "From: \t\t" + From.ToString() + "\r\n" +
                "To: \t\t" + To.ToString() + "\r\n" +
                "Subject: \t" + Subject + "\r\n" +
                "Date: \t\t" + Date.ToString();
        }
    }
}
