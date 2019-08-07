

using System;
using System.IO;

namespace ReflectiveAttributeStore
{
    class AttributeStoreLogger
    {
        public static void Warn(string message)
        {
            Log("WARN", message);
        }

        public static void Info(string message)
        {
            Log("INFO", message);
        }

        public static void Debug(string message)
        {
            if (Configuration.GetInstance().Debug)
            {
                Log("DEBUG", message);
            }
        }

        private static void Log(string level, string message)
        {
            try
            {
                // try to ensure that folder exists
                Directory.CreateDirectory("c:\\logs\\reflective");

                using (StreamWriter w = File.AppendText("c:\\logs\\reflective\\adfs-plugin.log"))
                {
                    w.WriteLine("{0} - {1} - {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), level, message);
                }
            }
            catch (Exception)
            {
                ; // ignore
            }
        }
    }
}
