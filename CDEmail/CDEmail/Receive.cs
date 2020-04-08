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
using System.Text.RegularExpressions;

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

        // 父窗体
        public Email BaseForm
        {
            get; set;
        }

        // 邮件头部信息
        public NewMailInfo MailInfo
        {
            get; set;
        }

        // 连接服务器的对象
        public ReceiveMail ReceiveMailConnect
        {
            get;set;
        }

        // 展示邮件
        public void ShowMailMessage()
        {
            tFrom.Text = MailInfo.From.ToString();
            tSubject.Text = MailInfo.Subject;

            String rawmessage = ReceiveMailConnect.GetANewMailMessage(MailInfo);
            ShowMailText(rawmessage);
            //MailMessage msg = ReceiveMailConnect.GetANewMail(MailInfo);
            //tFrom.Text = msg.From.ToString();
            //tSubject.Text = msg.Subject;
            //tBody.Text = msg.Body;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            BaseForm.ShowReceiveList();
        }

        // 获取邮件正文 和 附件
        #region 
        /// <summary>
        /// 获取文字主体
        /// </summary>
        /// <param name="p_Mail"></param>
        /// <returns></returns>
        public void ShowMailText(String p_Mail)
        {
            String _ConvertType = GetTextType(p_Mail, "Content-Type: ", ";");       // 获取邮件类型
            if (_ConvertType.Length == 0)
            {
                _ConvertType = GetTextType(p_Mail, "Content-Type: ", "\r");
            }

            int _StarIndex = -1;
            int _EndIndex = -1;
            String _ReturnText = "";
            String _Transfer = "";
            String _Boundary = "";
            String _EncodingName = GetTextType(p_Mail, "charset=\"", "\"").Replace("\"", "");   // 获取邮件字体类别
            System.Text.Encoding _Encoding =
                (_EncodingName == "") ?
                System.Text.Encoding.Default :
                System.Text.Encoding.GetEncoding(_EncodingName);

            // 根据邮件不同而分类
            switch (_ConvertType)
            {
                case "text/html;":
                    _Transfer = GetTextType(p_Mail, "Content-Transfer-Encoding: ", "\r\n").Trim();
                    _StarIndex = p_Mail.IndexOf("\r\n\r\n");
                    if (_StarIndex != -1)
                        _ReturnText = p_Mail.Substring(_StarIndex, p_Mail.Length - _StarIndex);
                    switch (_Transfer)
                    {
                        case "8bit":

                            break;
                        case "quoted-printable":
                            _ReturnText = DecodeQuotedPrintable(_ReturnText, _Encoding);
                            break;
                        case "base64":
                            _ReturnText = DecodeBase64(_ReturnText, _Encoding);
                            break;
                    }
                    tBody.Text += _ReturnText;
                    break;

                case "text/plain;":
                    tBody.Text = "";
                    _Transfer = GetTextType(p_Mail, "Content-Transfer-Encoding: ", "\r\n").Trim();
                    _StarIndex = p_Mail.IndexOf("\r\n\r\n");
                    if (_StarIndex != -1) _ReturnText = p_Mail.Substring(_StarIndex, p_Mail.Length - _StarIndex);
                    switch (_Transfer)
                    {
                        case "8bit":

                            break;
                        case "quoted-printable":
                            _ReturnText = DecodeQuotedPrintable(_ReturnText, _Encoding);
                            break;
                        case "base64":
                            _ReturnText = DecodeBase64(_ReturnText, _Encoding);
                            break;
                    }
                    tBody.Text += _ReturnText;
                    break;

                case "multipart/alternative;":
                    _Boundary = GetTextType(p_Mail, "boundary=\"", "\"").Replace("\"", "");
                    _StarIndex = p_Mail.IndexOf("--" + _Boundary + "\r\n");
                    if (_StarIndex == -1)
                        return;
                    while (true)
                    {
                        _EndIndex = p_Mail.IndexOf("--" + _Boundary, _StarIndex + _Boundary.Length);
                        if (_EndIndex == -1) 
                            break;
                        ShowMailText(p_Mail.Substring(_StarIndex, _EndIndex - _StarIndex));
                        _StarIndex = _EndIndex;
                    }
                    break;
                case "multipart/mixed;":
                    _Boundary = GetTextType(p_Mail, "boundary=\"", "\"").Replace("\"", "");
                    if (_Boundary == "")
                        _Boundary = GetTextType(p_Mail, "boundary=", "\r\n").Replace("\r\n", "");
                    _StarIndex = p_Mail.IndexOf("--" + _Boundary + "\r\n");
                    if (_StarIndex == -1)
                        return;
                    while (true)
                    {
                        _EndIndex = p_Mail.IndexOf("--" + _Boundary, _StarIndex + _Boundary.Length);
                        if (_EndIndex == -1) 
                            break;
                        ShowMailText(p_Mail.Substring(_StarIndex, _EndIndex - _StarIndex));
                        _StarIndex = _EndIndex;
                    }
                    break;
                default:
                    break;
                    //if (_ConvertType.IndexOf("application/") == 0)
                    //{
                    //    _StarIndex = p_Mail.IndexOf("\r\n\r\n");
                    //    if (_StarIndex != -1)
                    //        _ReturnText = p_Mail.Substring(_StarIndex, p_Mail.Length - _StarIndex);
                    //    _Transfer = GetTextType(p_Mail, "Content-Transfer-Encoding: ", "\r\n").Trim();
                    //    String _Name = GetTextType(p_Mail, "filename=\"", "\"").Replace("\"", "");
                    //    _Name = GetReadText(_Name);
                    //    byte[] _FileBytes = new byte[0];
                    //    switch (_Transfer)
                    //    {
                    //        case "base64":
                    //            _FileBytes = Convert.FromBase64String(_ReturnText);
                    //            break;
                    //    }
                    //    // MailTable.Rows.Add(new object[] { "application/octet-stream", _FileBytes, _Name });

                    //}
                    //break;
            }
        }
        #endregion

        // 获取类型
        #region
        /// <summary>
        /// 获取类型（正则）
        /// </summary>
        /// <param name="p_Mail">原始文字</param>
        /// <param name="p_TypeText">前文字</param>
        /// <param name="p_End">结束文字</param>
        /// <returns>符合的记录</returns>
        public string GetTextType(string p_Mail, string p_TypeText, string p_End)
        {
            // 正则表达式
            Regex _Regex = new Regex(@"(?<=" + p_TypeText + ").*?(" + p_End + ")+");
            // 匹配集合
            MatchCollection _Collection = _Regex.Matches(p_Mail);
            if (_Collection.Count == 0)
                return "";
            // 返回第一个
            return _Collection[0].Value;
        }
        #endregion

        #region
        /// <summary>
        /// QuotedPrintable编码接码
        /// </summary>
        /// <param name="p_Text">原始文字</param>
        /// <param name="p_Encoding">编码方式</param>
        /// <returns>接码后信息</returns>
        public string DecodeQuotedPrintable(string p_Text, System.Text.Encoding p_Encoding)
        {
            System.IO.MemoryStream _Stream = new System.IO.MemoryStream();
            char[] _CharValue = p_Text.ToCharArray();
            for (int i = 0; i != _CharValue.Length; i++)
            {
                switch (_CharValue[i])
                {
                    case '=':
                        if (_CharValue[i + 1] == '\r' || _CharValue[i + 1] == '\n')
                        {
                            i += 2;
                        }
                        else
                        {
                            try
                            {
                                _Stream.WriteByte(Convert.ToByte(_CharValue[i + 1].ToString() + _CharValue[i + 2].ToString(), 16));
                                i += 2;
                            }
                            catch
                            {
                                _Stream.WriteByte(Convert.ToByte(_CharValue[i]));
                            }
                        }
                        break;
                    default:
                        _Stream.WriteByte(Convert.ToByte(_CharValue[i]));
                        break;
                }
            }
            return p_Encoding.GetString(_Stream.ToArray());
        }
        #endregion

        #region
        /// <summary>
        /// 解码BASE64
        /// </summary>
        /// <param name="p_Text"></param>
        /// <param name="p_Encoding"></param>
        /// <returns></returns>
        public string DecodeBase64(string p_Text, System.Text.Encoding p_Encoding)
        {
            if (p_Text.Trim().Length == 0) return "";
            byte[] _ValueBytes = Convert.FromBase64String(p_Text);
            return p_Encoding.GetString(_ValueBytes);
        }
        #endregion

        /// <summary>
        /// 转换文字里的字符集
        /// </summary>
        /// <param name="p_Text"></param>
        /// <returns></returns>
        public string GetReadText(string p_Text)
        {
            Regex _Regex = new Regex(@"(?<=\=\?).*?(\?\=)+");
            MatchCollection _Collection = _Regex.Matches(p_Text);
            string _Text = p_Text;
            foreach (System.Text.RegularExpressions.Match _Match in _Collection)
            {
                string _Value = "=?" + _Match.Value;
                if (_Value[0] == '=')
                {
                    string[] _BaseData = _Value.Split('?');
                    if (_BaseData.Length == 5)
                    {
                        System.Text.Encoding _Coding = System.Text.Encoding.GetEncoding(_BaseData[1]);
                        _Text = _Text.Replace(_Value, _Coding.GetString(Convert.FromBase64String(_BaseData[3])));
                    }
                }
                else
                {
                }
            }
            return _Text;
        }
    }
}
