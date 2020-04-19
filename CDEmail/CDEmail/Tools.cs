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
        public static String GBKToString(String gbk)
        {
            if (gbk.Trim().Length == 0)
            {
                return "";
            }

            String ans;

            byte[] bytes = Encoding.GetEncoding("GBK").GetBytes(gbk);
            // byte[] bytes = Encoding.Default.GetBytes(gbk);
            // byte[] bytes = Convert.FromBase64CharArray(gbk.ToCharArray(), 0, gbk.Length);

            // ans = Encoding.GetEncoding("GBK").GetString(bytes);
            ans = Encoding.UTF8.GetString(bytes);

            return ans;
        }

        public static String GB2312ToString(String gb2312)
        {
            return Encoding.GetEncoding(936).GetString(Encoding.Unicode.GetBytes(gb2312));
        }

        public String DecodeBase64(string p_Text, System.Text.Encoding p_Encoding)
        {
            if (p_Text.Trim().Length == 0) return "";
            byte[] _ValueBytes = Convert.FromBase64String(p_Text);
            return p_Encoding.GetString(_ValueBytes);
        }

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
    }
}
