using System;
using System.Collections.Generic;

namespace ReflectiveAttributeStore
{
    public class Configuration
    {
        private static Configuration instance;

        public string ReflectiveUrl { get; set; }
        public string Context { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Debug { get; set; }

        private Configuration()
        {
            ;
        }

        public void Init(Dictionary<string, string> config)
        {
            if (config.ContainsKey("ReflectiveUrl"))
            {
                ReflectiveUrl = config["ReflectiveUrl"];
            }
            else
            {
                throw new Exception("ReflectiveUrl not configured");
            }

            if (config.ContainsKey("Context"))
            {
                Context = config["Context"];
            }
            else
            {
                throw new Exception("Context not configured");
            }

            if (config.ContainsKey("Username"))
            {
                Username = config["Username"];
            }
            else
            {
                throw new Exception("Username not configured");
            }

            if (config.ContainsKey("Password"))
            {
                Password = config["Password"];
            }
            else
            {
                throw new Exception("Password not configured");
            }

            if (config.ContainsKey("Debug"))
            {
                Debug = "true".Equals(config["Debug"].ToLower());
            }
            else
            {
                Debug = false;
            }
        }

        public static Configuration GetInstance()
        {
            if (instance != null)
            {
                return instance;
            }

            return (instance = new Configuration());
        }

        public override string ToString()
        {
            return "Configuration: URL=" + ReflectiveUrl + ", Debug=" + Debug + ", Username=" + Username;
        }
    }
}
