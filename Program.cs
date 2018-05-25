using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tsNFDNormalize001
{
    class Program
    {
        static void Main(string[] args)
        {
            var log = new StringBuilder();
            foreach (var item in args)
            {
                ConvNFDNormalize(item, ref log);
            }
            if (log.Length != 0)
            {
                var res = MessageBox.Show(log.ToString(), @"失敗", MessageBoxButtons.OK);
            }
        }

        private static void ConvNFDNormalize(string path, ref StringBuilder log)
        {
            try
            {
                Action<string, string> func = null;
                if (System.IO.File.Exists(path))
                {
                    func = (n1, n2) => { System.IO.File.Move(n1, n2); };
                }
                else if (System.IO.Directory.Exists(path))
                {
                    func = (n1, n2) => { System.IO.Directory.Move(n1, n2); };
                }
                if (func != null)
                {
                    var dir_name = System.IO.Path.GetDirectoryName(path) + System.IO.Path.DirectorySeparatorChar;
                    var name1 = System.IO.Path.GetFileName(path);

                    var name2 = ConvSjisCode(name1);
                    var name3 = ConvDakuten(name2);
                    if (name1 != name3)
                    {
                        func(dir_name + name1, dir_name + name3);
                    }
                }
            }
            catch (Exception e)
            {
                log.AppendFormat(@"conv err {0} : {1}\r\n", path, e.Message);
            }
        }

        private static string ConvDakuten(string in_str)
        {
            // 全角の濁点・半濁点を、UNICODE二文字目の濁点・半濁点に置換後に、Normalize
            return in_str
                .Replace("\u309b", "\u3099")
                .Replace("\u309c", "\u309a")
                .Normalize();
        }

        private static string ConvSjisCode(string in_str)
        {
            // Shift_JISで表現できない文字列を削除する
            Encoding sjisEnc = Encoding.GetEncoding(@"Shift_JIS");
            byte[] sjis_byte = sjisEnc.GetBytes(in_str);
            var str1 = sjisEnc.GetString(sjis_byte);
            var str2 = str1.Replace(@"?", @"");
            return str2;
        }
    }
}
