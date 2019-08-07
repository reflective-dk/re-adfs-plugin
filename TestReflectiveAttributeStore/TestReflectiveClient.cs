using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReflectiveAttributeStore;

namespace TestReflectiveAttributeStore
{
    [TestClass]
    public class TestReflectiveClient
    {
        public Dictionary<string, string> readCredentials(string name)
        {
            Dictionary<string, string> _werFileContent = new Dictionary<string, string>();
            using (StreamReader sr = new StreamReader(name))
            {

                string _line;
                while ((_line = sr.ReadLine()) != null)
                {
                    string[] keyvalue = _line.Split('=');
                    if (keyvalue.Length == 2)
                    {
                        _werFileContent.Add(keyvalue[0], keyvalue[1]);
                    }
                }
            }
            return _werFileContent;
        }

        [TestMethod]
        public void callLogin()
        {
            Dictionary<string, string> dict = readCredentials("../../base.txt");
            Configuration.GetInstance().Init(dict);

            ReflectiveClient client = new ReflectiveClient();
            string token = client.Login();
            Assert.IsTrue(token.Length > 0, "should return some token");
        }

        [TestMethod]
        public void failedLogin()
        {
            var failed = false;
            try
            {
                string token = ReflectiveClient.Login("https://test.reflective.dk", "what is this?", "non", "{\"domain\": \"bogus\"}");
            } catch (Exception e) {
                failed = true;
            }

            Assert.IsTrue(failed, "should fail login");
        }

        [TestMethod]
        public void profile()
        {
            Dictionary<string, string> dict = readCredentials("../../base.txt");
            Configuration.GetInstance().Init(dict);

            ReflectiveClient client = new ReflectiveClient();
            client.Login();

            Dictionary<string, Object> profileResult = client.Profile("anders.hansen@hjertekoebing.dk", null);
            Console.WriteLine(profileResult);
            Assert.AreEqual(profileResult["cprNr"], "1111111111");
        }
        
    }
}
