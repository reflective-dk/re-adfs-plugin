using System;
using System.Collections.Generic;
using System.IdentityModel;
using System.Net;
using Microsoft.IdentityServer.ClaimsPolicy.Engine.AttributeStore;

namespace ReflectiveAttributeStore
{
    public class MainClass : IAttributeStore
    {
        public ReflectiveClient client;

        public IAsyncResult BeginExecuteQuery(string query, string[] parameters, AsyncCallback callback, object state)
        {
            ValidateQuery(query, parameters);

            string[][] outputValues = new string[1][];
            switch (query)
            {
                case "getCprNumber":
                    AttributeStoreLogger.Debug("looking up cprNumber for user '" + parameters[0] + "'");

                    Object cprNo = this.client.Profile(parameters[0], null)["cprNr"];
                    if (cprNo == null)
                    {
                        AttributeStoreLogger.Debug("CPRNumber, not found");
                        outputValues[0] = new string[1] { null };
                    } else
                    {
                        AttributeStoreLogger.Debug("CPRNumber, found");
                        outputValues[0] = new string[1] { cprNo.ToString() };
                    }

                    break;
                default:
                    throw new AttributeStoreQueryFormatException("The query string is not supported:" + query);
            }

            TypedAsyncResult<string[][]> asyncResult = new TypedAsyncResult<string[][]>(callback, state);
            asyncResult.Complete(outputValues, true);

            return asyncResult;
        }

        public string[][] EndExecuteQuery(IAsyncResult result)
        {
            return TypedAsyncResult<string[][]>.End(result);
        }

        public void Initialize(Dictionary<string, string> config)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                Configuration.GetInstance().Init(config);
                AttributeStoreLogger.Info("Reflective AttributeStore loaded. details :" + Configuration.GetInstance().ToString());

                this.client = new ReflectiveClient();
                AttributeStoreLogger.Debug("Client instantiated:" + this.client);

                this.client.Login();
                AttributeStoreLogger.Info("Reflective AttributeStore authenticated");
            }
            catch (Exception ex)
            {
                throw new AttributeStoreInvalidConfigurationException("Failed to load configuration: " + ex.Message + ". Details: " + ex.StackTrace.ToString());
            }
        }

        private void ValidateQuery(string query, string[] parameters)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new AttributeStoreQueryFormatException("No query string.");
            }

            if (parameters == null)
            {
                throw new AttributeStoreQueryFormatException("No query parameter.");
            }

            if ("getCprNumber".Equals(query) && parameters.Length != 1)
            {
                throw new AttributeStoreQueryFormatException("One query parameter required for getting cprNumber (username)");
            }
        }
    }
}
