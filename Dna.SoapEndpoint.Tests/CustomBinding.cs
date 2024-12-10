using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;

namespace Dna.SoapEndpoint.Tests;

    public class CustomBinding : BasicHttpBinding
    {
        public CustomBinding(string configName, string baseUrl)
        { 
            this.Name = configName;
            
            this.AllowCookies = false;
            this.BypassProxyOnLocal = false;
            this.UseDefaultWebProxy = true;
            
            this.CloseTimeout = new TimeSpan(0,3,0);
            this.OpenTimeout = new TimeSpan(0,3,0);
            this.ReceiveTimeout = new TimeSpan(0,10,0);
            this.SendTimeout = new TimeSpan(0,3,0);

            this.MaxReceivedMessageSize = int.MaxValue;
            this.MaxBufferPoolSize = int.MaxValue;
            this.MaxBufferSize = int.MaxValue;
            this.MessageEncoding = WSMessageEncoding.Text;
            this.TextEncoding = Encoding.UTF8;
            this.TransferMode = System.ServiceModel.TransferMode.Buffered;


            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            this.Security = new BasicHttpSecurity()
            {
                Mode = GetSecurityMode(baseUrl),
                Transport = new HttpTransportSecurity() 
                {
                      ClientCredentialType =  HttpClientCredentialType.None,
                      ProxyCredentialType = HttpProxyCredentialType.None,
                },
                Message = new  BasicHttpMessageSecurity()
                {
                   ClientCredentialType = BasicHttpMessageCredentialType.UserName,
                   AlgorithmSuite = SecurityAlgorithmSuite.Default
                }
            };

            this.ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas() 
            {
                MaxDepth = 32,
                MaxStringContentLength = int.MaxValue,
                MaxArrayLength = int.MaxValue,
                MaxBytesPerRead = int.MaxValue,
                MaxNameTableCharCount = int.MaxValue,
            };
        }

        System.ServiceModel.BasicHttpSecurityMode GetSecurityMode(string baseUrl)
        {
            if (!baseUrl.Contains("localhost"))
                return System.ServiceModel.BasicHttpSecurityMode.Transport;
            else return System.ServiceModel.BasicHttpSecurityMode.TransportCredentialOnly;
        }
    }
