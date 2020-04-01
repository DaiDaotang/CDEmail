using System;
using System.Net.Mail;
using System.Text;
using System.Net;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            SendEmail se = new SendEmail("1025563447@qq.com", "1025563447@qq.com", "iaizxqujhwipbfgf", "smtp.qq.com");
            se.AddGoalAdress("whuddt@126.com");
            se.Emailbody = "this  is a  testxvv sdfsfsdfsdf afaf afawdqwdeeeeeee";
            se.Emailhead = "test wq w email";
            //se.AddFile(@"C:\Users\66\Desktop\demo.docx");
            //se.AddFile(@"C:\Users\66\Desktop\demo2.docx");
            se.Send();
        }
    }
}
