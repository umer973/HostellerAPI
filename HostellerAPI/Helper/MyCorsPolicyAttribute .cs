using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Cors;
using System.Web.Http.Cors;

namespace HostellerAPI
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class MyCorsPolicyAttribute : Attribute, ICorsPolicyProvider
    {
        private CorsPolicy _policy;

        public MyCorsPolicyAttribute()
        {
            // Create a CORS policy.
            _policy = new CorsPolicy
            {
                AllowAnyMethod = true,
                AllowAnyHeader = true,
                AllowAnyOrigin=true
            };

            // Add allowed origins.
            _policy.Origins.Add("http://localhost:33155/api/test");
            _policy.Origins.Add("http://192.168.0.108:82/RISTest/");
            _policy.Origins.Add("http://localhost:4100/");
            _policy.Origins.Add("http://localhost:4200/");
            

            _policy.Origins.Add("http://localhost:33155/");
            _policy.Origins.Add("http://192.168.0.108:33155/");
            _policy.Origins.Add("http://192.168.0.29:86/api/");
            _policy.Origins.Add("http://192.168.0.29:86/");
            _policy.Origins.Add("http://124.124.89.244:82/api/");
            _policy.Origins.Add("http://124.124.89.244:82/");
            _policy.Origins.Add("http://192.168.0.29:90/");
            _policy.Origins.Add("http://192.168.0.185:55/");
            _policy.Origins.Add("http://192.168.0.185:55/PORTAL/");
            

            //_policy.Origins.Add("http://124.124.89.244:86/");
            //_policy.Origins.Add("http://ki108/ris_api/");


        }

        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request)
        {
            return Task.FromResult(_policy);
        }

        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_policy);
        }
    }
}