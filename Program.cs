using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tsNFDNormalize001
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var item in args)
            {
                ConvNFDNormalize(item);
            }
        }

        private static void ConvNFDNormalize(string path)
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
                    // 全角の濁点・半濁点を、UNICODE二文字目の濁点・半濁点に置換後に、Normalize
                    var name2 = name1
                        .Replace("\u309b", "\u3099")
                        .Replace("\u309c", "\u309a")
                        .Normalize();
                    if (name1 != name2)
                    {
                        func(dir_name + name1, dir_name + name2);
                    }
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(@"conv err {0} : {1}", path, e.Message);
            }
        }
    }
}
