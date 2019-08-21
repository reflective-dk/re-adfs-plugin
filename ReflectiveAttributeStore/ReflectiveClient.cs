using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using RestSharp;

namespace ReflectiveAttributeStore
{
    public class ReflectiveClient
    {
        public static string Login(string url, string username, string password, string context)
        {
            string client_id = "reflective";
            string client_secret = "reflective";
            //request token
            var restclient = new RestClient(url);

            AttributeStoreLogger.Debug("Logging in, url=" + url);
            AttributeStoreLogger.Debug("Logging in, username=" + username);
            AttributeStoreLogger.Debug("Logging in, password=" + password);
            AttributeStoreLogger.Debug("Logging in, context=" + context);

            RestRequest request = new RestRequest("api/oauth/token") { Method = Method.POST };
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("context", context);

            request.AddParameter("client_id", client_id);
            request.AddParameter("client_secret", client_secret);
            request.AddParameter("grant_type", "password");
            request.AddParameter("username", username);
            request.AddParameter("password", password);
            var theResponse = restclient.Execute(request);
            var responseJson = theResponse.Content;

            if((int)theResponse.StatusCode != 200) {
              AttributeStoreLogger.Debug("Login failed, statusCode:" + theResponse.StatusCode);
              AttributeStoreLogger.Debug(responseJson);
            }
            var token = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson)["access_token"].ToString();
            return token.Length > 0 ? token : null;
        }

        public string token;
        public string url;
        public string context;

        public string Login()
        {
            AttributeStoreLogger.Debug("Logging in, Configuration=" + Configuration.GetInstance().ToString());
            this.url = Configuration.GetInstance().ReflectiveUrl;
            this.context = Configuration.GetInstance().Context;

            string username = Configuration.GetInstance().Username;
            string password = Configuration.GetInstance().Password;
            this.token = ReflectiveClient.Login(this.url, username, password, this.context);
            return this.token;
        }

        public Dictionary<string, Object> Profile(string username, string cprNo, int retry = 1)
        {
            string body = "";
            if (username != null)
            {
                body = "{ \"username\":\"" + username + "\" }";
            } else if (cprNo != null)
            {
                body = "{ \"cprNo\":\"" + cprNo + "\" }";
            } else
            {
                throw new Exception("provide either username or cprNo");
            }

            //request token
            var restclient = new RestClient(this.url);
            RestRequest request = new RestRequest("/api/intermediary/profile") { Method = Method.POST };

            request.AddHeader("Authorization", "Bearer " + this.token);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("context", context);

            request.AddParameter("application/json", body, ParameterType.RequestBody);

            var theResponse = restclient.Execute(request);
            if ((int) theResponse.StatusCode == 401)
            {
                if (retry < 5)
                {
                    AttributeStoreLogger.Warn("not authenticated, trying login, retry attempts: " + retry);
                    this.Login();
                    return this.Profile(username, cprNo, retry + 1);
                } else
                {
                    AttributeStoreLogger.Warn("retried 5 times, to re-login, not successful");
                    return null;
                }
            } else
            {
                var responseJson = theResponse.Content;
                var profile = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson);
                return profile;
            }
        }

        public override string ToString()
        {
            return "ReflectiveCLient: url=" + url + ", context=" + context;
        }
    }
}
