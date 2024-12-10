using System.ServiceModel;
using Dna.SoapEndpoint.Tests.DomainApi;

namespace Dna.SoapEndpoint.Tests;

public class BaseUt
{
    private const string _endpointAddress = "https://api.domainnameapi.com/DomainApi.svc";
    internal DomainApiClient DomainClient 
    {
        get
        {
            return new DomainApiClient(new CustomBinding("BasicHttpBinding_IDomainApi", _endpointAddress), new System.ServiceModel.EndpointAddress(_endpointAddress));;
        } 
    }
    
    internal const string Username = "ownername";
    internal const string Password = "ownerpassword";
}