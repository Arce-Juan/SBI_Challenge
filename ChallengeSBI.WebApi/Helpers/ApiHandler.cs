using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace ChallengeSBI.WebApi.Helpers
{
    public class ApiHandler : IRequestHandler<Api, string>
    {
        private string _urlApiPosts = $"https://jsonplaceholder.typicode.com/posts";
        private System.Lazy<Newtonsoft.Json.JsonSerializerSettings> _settings;
        public Task<string> Handle(Api request, CancellationToken cancellationToken)
        {
            var requestApi = (HttpWebRequest)WebRequest.Create(_urlApiPosts);
            requestApi.Method = "GET";
            requestApi.ContentType = "application/json";
            requestApi.Accept = "application/json";
            try
            {
                using (WebResponse response = requestApi.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null)
                            return null;

                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            if (responseBody == null)
                                return null;
                            
                            return Task.FromResult(responseBody);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Task.FromResult("Foult: " + ex.Message);
            }
        }
    }
}
