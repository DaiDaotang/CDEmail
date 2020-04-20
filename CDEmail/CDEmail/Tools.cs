using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CDEmail
{
    class Tools
    {
        #region 转换文字里的字符集
        /// <summary>
        /// 转换文字里的字符集
        /// </summary>
        /// <param name="p_Text"></param>
        /// <returns></returns>
        public static string GetReadText(string p_Text)
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
        #endregion

        #region QuotedPrintable编码解码
        /// <summary>
        /// QuotedPrintable编码接码
        /// </summary>
        /// <param name="p_Text">原始文字</param>
        /// <param name="p_Encoding">编码方式</param>
        /// <returns>接码后信息</returns>
        public static string DecodeQuotedPrintable(string p_Text, System.Text.Encoding p_Encoding)
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

        #region BASE64编码解码
        /// <summary>
        /// 解码BASE64
        /// </summary>
        /// <param name="p_Text"></param>
        /// <param name="p_Encoding"></param>
        /// <returns></returns>
        public static string DecodeBase64(string p_Text, System.Text.Encoding p_Encoding)
        {
            if (p_Text.Trim().Length == 0) return "";
            byte[] _ValueBytes = Convert.FromBase64String(p_Text);
            return p_Encoding.GetString(_ValueBytes);
        }
        #endregion
    }
}
