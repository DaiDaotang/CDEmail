using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CDEmail
{
    public class NewMailMessage
    {
        public NewMailInfo MailInfo
        {
            get; set;
        }

        public int Size
        {
            get; set;
        }

        public String Body
        {
            get; set;
        }

        public Object Enclousure
        {
            get; set;
        }
    }
}