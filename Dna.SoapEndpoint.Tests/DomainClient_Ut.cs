using System.Diagnostics;
using Dna.SoapEndpoint.Tests.DomainApi;

namespace Dna.SoapEndpoint.Tests;

public class DomainClient_Ut : BaseUt
{
    [SetUp]
    public void Setup()
    {
    }
    
    /// <summary>
    /// Domain müsaitlik durumunu kontrol eder
    /// </summary>
    [Test]
    public async Task Should_Check_Avaibility_Async()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
    
        DateTime startDate = DateTime.Now;
        var request = new CheckAvailabilityRequest()
        {
            DomainNameList = new string[] { "dnatest1239183" }, 
            TldList = new string[] { "com", "com.tr" },
            Commad = "create",
            UserName = Username,
            Password = Password,
            Period = 1
        };
        CheckAvailabilityResponse response = new();

        try
        {
            response = await DomainClient.CheckAvailabilityAsync(request);
            if (response.OperationResult == ExecutionStatus.ERROR)
                Debug.WriteLine($"Failed: {response.OperationMessage}");
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
        }
    
        stopwatch.Stop();
        DateTime completedDate = DateTime.Now;
        Assert.Pass();
    }
    
    /// <summary>
    /// Sistemin desteklediği uzantı listesini ve detaylarını getirir
    /// </summary>
    [Test]
    public async Task Should_Get_Tld_List_Async()
    {
        int lastestNumber = 10;
        for (int i = 1; i < lastestNumber; i++)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
        
            DateTime startDate = DateTime.Now;
            var request = new GetTldListRequest()
            {
                PageNumber = i,
                PageSize = 100,
                UserName = Username,
                Password = Password,
            };
            
            Console.WriteLine($"{i}nd page requested");

            var response = await DomainClient.GetTldListAsync(request);
            if (response.OperationResult == ExecutionStatus.ERROR)
                Console.WriteLine($"Failed: {response.OperationMessage}");

            // Example
            if (response.TldInfoList.Any(a=> a.Name == "org"))
                Console.WriteLine($".org domain checked");
            
            stopwatch.Stop();
            DateTime completedDate = DateTime.Now;
        }
      
    }
    
    /// <summary>
    /// Hesabınızda tanımlı olan bir domaine ait detayları getirir
    /// </summary>
    [Test]
    public async Task Should_Get_Domain_Detail_Async()
    {
        DomainClient.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 20);
        List<string> list = new List<string>()
        {
            "apiname11.com"
        };

        foreach (var domain in list)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
        
            DateTime startDate = DateTime.Now;
            var request = new GetDetailsRequest()
            {
                DomainName = domain,
                UserName = Username,
                Password = Password,
            };
            
            GetDetailsResponse response = new();
            try
            {
                response = await DomainClient.GetDetailsAsync(request);
                if (response.OperationResult == ExecutionStatus.ERROR)
                    Console.WriteLine($"Failed: {response.OperationMessage}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            } 
        
            stopwatch.Stop();
            DateTime completedDate = DateTime.Now;
        }
    }
    
    /// <summary>
    /// İstekte bulunulan domainlerin bilgilerini senkronize ederek güncel halde tutar, opsiyonel kullanılır
    /// </summary>
    [Test]
    public async Task Should_Sync_Domain_Async()
    {
        DomainClient.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 20);

        List<string> list = new List<string>()
        {
            "dnatest.tr", "dnatest.net"
        };

        foreach (var domain in list)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
        
            DateTime startDate = DateTime.Now;
            var request = new SyncFromRegistryRequest()
            {
                DomainName = domain,
                UserName = Username,
                Password = Password,
            };

            SyncFromRegistryResponse response = new();
            try
            {
                response = await DomainClient.SyncFromRegistryAsync(request);
                if (response.OperationResult == ExecutionStatus.ERROR)
                    Console.WriteLine($"Failed: {response.OperationMessage}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            } 
        
            stopwatch.Stop();
            DateTime completedDate = DateTime.Now;
        }
    }
    
    /// <summary>
    /// Domain kayıt işlemi
    /// </summary>
    [Test]
    public async Task Should_Register_Domain_Async()
    {
        DomainClient.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 20);

        string domainName = "dnatest111.com";
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        // Registrant contact info
        ContactInfo contactInfo = new ContactInfo()
        {
            FirstName = "John",
            LastName = "Doe",
            EMail = "info@apiname.com",
            AddressLine1 = "72 Chase Side, Suite 2, London, United Kingdom, N14 5PH",
            Country = "US",
            City = "London",
            State = "Washington",
            ZipCode = "0913131",
            Company = "APINAME",
            Phone = "5551234567",
            PhoneCountryCode = "90",
        };
    
        DateTime startDate = DateTime.Now;
        var request = new RegisterWithContactInfoRequest()
        {
            DomainName = domainName,
            UserName = Username,
            Password = Password,
            Period = 1, 
            NameServerList = new []{ "tr.apiname.com", "eu.apiname.com"},
            AdministrativeContact = contactInfo,
            BillingContact = contactInfo,
            TechnicalContact = contactInfo,
            RegistrantContact = contactInfo,
            PrivacyProtectionStatus = false
        };

        RegisterResponse response = new();
        try
        { 
            response = await DomainClient.RegisterWithContactInfoAsync(request);
            if (response.OperationResult == ExecutionStatus.ERROR)
                Console.WriteLine($"Registration failed: {response.OperationMessage}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
        
        stopwatch.Stop();
        DateTime completedDate = DateTime.Now;
    }
    
    /// <summary>
    /// Ek parametreler kullanılarak yapılan domain kayıt işlemidir. Ek parametreler şu an için ".tr" domainlerinde zorunludur. Diğer domainler için henüz zorunlu değil.
    /// <example>
    /// .tr domainleri için gerekli olan parametreler <see cref="ClientConsts"/> içerisinde tanımlı. Kişisel ve kurumsal başvurular birbirinden ayrıdır.
    /// </example>
    /// </summary>
    [Test]
    public async Task Should_Register_Domain_With_AdditionalParams_Async()
    {
        DomainClient.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 20);

        // Ek parametre bilgisi
        Dictionary<string, string> additionalParams = new();
        additionalParams.Add(ClientConsts.TRABIS_DOMAIN_CATEGORY_KEY, "0");
        additionalParams.Add(ClientConsts.TRABIS_TAX_NUMBER_KEY, "1111111111");
        additionalParams.Add(ClientConsts.TRABIS_TAX_OFFICE_KEY, "Londra");
        additionalParams.Add(ClientConsts.TRABIS_ORGANIZATION_KEY, "APINAME");

        string domainName = "apinametest111.com.tr";
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        // Registrant contact info
        ContactInfo contactInfo = new ContactInfo()
        {
            FirstName = "John",
            LastName = "Doe",
            EMail = "info@apiname.com",
            AddressLine1 = "72 Chase Side, Suite 2, London, United Kingdom, N14 5PH",
            Country = "US",
            City = "London",
            State = "Washington",
            ZipCode = "0913131",
            Company = "APINAME",
            Phone = "5551234567",
            PhoneCountryCode = "90"
        };
    
        DateTime startDate = DateTime.Now;
        var request = new RegisterWithContactInfoRequest()
        {
            DomainName = domainName,
            UserName = Username,
            Password = Password,
            Period = 1, 
            NameServerList = new []{ "tr.apiname.com", "eu.apiname.com"},
            AdministrativeContact = contactInfo,
            BillingContact = contactInfo,
            TechnicalContact = contactInfo,
            RegistrantContact = contactInfo,
            PrivacyProtectionStatus = false,
            AdditionalAttributes = additionalParams
        };

        RegisterResponse response = new();
        try
        { 
            response = await DomainClient.RegisterWithContactInfoAsync(request);
            if (response.OperationResult == ExecutionStatus.ERROR)
                Console.WriteLine($"Registration failed: {response.OperationMessage}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
        
        stopwatch.Stop();
        DateTime completedDate = DateTime.Now;
    }
    
    /// <summary>
    /// Domain yenileme işlemini yapar
    /// </summary>
    [Test]
    public async Task Should_Renew_Domain_Async()
    {
        DomainClient.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 20);

        string domainName = "apinametest11.com";
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        DateTime startDate = DateTime.Now;
        var request = new RenewRequest()
        {
            DomainName = domainName,
            UserName = Username,
            Password = Password,
            Period = 1,
        };

        RenewResponse response = new();
        try
        { 
            response = await DomainClient.RenewAsync(request);
            if (response.OperationResult == ExecutionStatus.ERROR)
                Console.WriteLine($"Failed: {response.OperationMessage}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
        
        stopwatch.Stop();
        DateTime completedDate = DateTime.Now;
    }
    
    /// <summary>
    /// WHOIS koruması sağlar ve bilgilerinizi saklar
    /// </summary>
    [Test]
    public async Task Should_Modify_Privacy_Protection_Async()
    {
        DomainClient.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 20);

        string domainName = "apiname12313.net";
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        DateTime startDate = DateTime.Now;
        var request = new ModifyPrivacyProtectionStatusRequest()
        {
            DomainName = domainName,
            UserName = Username,
            Password = Password,
            ProtectPrivacy = true
        };

        ModifyPrivacyProtectionStatusResponse response = new();
        try
        { 
            response = await DomainClient.ModifyPrivacyProtectionStatusAsync(request);
            if (response.OperationResult == ExecutionStatus.ERROR)
                Console.WriteLine($"Failed: {response.OperationMessage}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
        
        stopwatch.Stop();
        DateTime completedDate = DateTime.Now;
    }
      
    /// <summary>
    /// Domain transfer kilidini kaldırır
    /// </summary>
    [Test]
    public async Task Should_Unlock_Domain_Async()
    {
        DomainClient.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 20);

        string domainName = "apiname12313.net";
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        DateTime startDate = DateTime.Now;
        var request = new DisableTheftProtectionLockRequest()
        {
            DomainName = domainName,
            UserName = Username,
            Password = Password,
        };

        DisableTheftProtectionLockResponse response = new();
        try
        { 
            response = await DomainClient.DisableTheftProtectionLockAsync(request);
            if (response.OperationResult == ExecutionStatus.ERROR)
                Console.WriteLine($"Failed: {response.OperationMessage}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
        
        stopwatch.Stop();
        DateTime completedDate = DateTime.Now;
    }
    
    /// <summary>
    /// Domain transfer kilidini etkinleştirir
    /// </summary>
    [Test]
    public async Task Should_Lock_Domain_Async()
    {
        DomainClient.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 20);

        string domainName = "apiname12313.net";
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        DateTime startDate = DateTime.Now;
        var request = new EnableTheftProtectionLockRequest()
        {
            DomainName = domainName,
            UserName = Username,
            Password = Password,
        };

        EnableTheftProtectionLockResponse response = new();
        try
        { 
            response = await DomainClient.EnableTheftProtectionLockAsync(request);
            if (response.OperationResult == ExecutionStatus.ERROR)
                Console.WriteLine($"Failed: {response.OperationMessage}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
        
        stopwatch.Stop();
        DateTime completedDate = DateTime.Now;
    }
    
    /// <summary>
    /// Domain ait contact bilgilerini günceller
    /// </summary>
    [Test]
    public async Task Should_Save_Contact_Domain_Async()
    {
        DomainClient.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 20);
        
        string domainName = "apiname113.com";
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        
        // Değiştirilen contact bilgileri
        ContactInfo contactInfo = new ContactInfo()
        {
            FirstName = "John",
            LastName = "Doe",
            EMail = "info@apiname.com",
            AddressLine1 = "72 Chase Side, Suite 2, London, United Kingdom, N14 5PH",
            Country = "US",
            City = "London",
            State = "Washington",
            ZipCode = "0913131",
            Company = "APINAME",
            Phone = "5551234567",
            PhoneCountryCode = "90",
        };

        var request = new SaveContactsRequest()
        {
            DomainName = domainName,
            UserName = Username,
            Password = Password,
            RegistrantContact = contactInfo,
            AdministrativeContact = contactInfo,
            BillingContact = contactInfo,
            TechnicalContact = contactInfo
        };
        
        
        DateTime startDate = DateTime.Now;
        SaveContactsResponse response = new();
        try
        { 
            response = await DomainClient.SaveContactsAsync(request);
            if (response.OperationResult == ExecutionStatus.ERROR)
                Console.WriteLine($"Failed: {response.OperationMessage}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
        
        stopwatch.Stop();
        DateTime completedDate = DateTime.Now;
    }
     
    /// <summary>
    /// İstenilen host (child name) kaydını ekler
    /// </summary>
    [Test]
    public async Task Should_Add_Child_Name_Async()
    {
        DomainClient.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 20);

        string hostName = "ns21.atakdomaintest3.com";
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
    
        DateTime startDate = DateTime.Now;
        var request = new AddChildNameServerRequest()
        {
            ChildNameServer = hostName,
            UserName = Username,
            Password = Password,
            IpAddressList = new string[] { "185.46.40.240" }
        };

        AddChildNameServerResponse response = new();
        try
        { 
            response = await DomainClient.AddChildNameServerAsync(request);
            if (response.OperationResult == ExecutionStatus.ERROR)
                Console.WriteLine($"Uygulama hata verdi: {response.OperationMessage}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
        
        stopwatch.Stop();
        DateTime completedDate = DateTime.Now;
    }
    
    /// <summary>
    /// İstenilen host (child name) kaydını siler
    /// </summary>
    [Test]
    public async Task Should_Delete_Child_Name_Async()
    {
        DomainClient.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 20);

        string hostName = "ns10.apiname11.com";
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
    
        DateTime startDate = DateTime.Now;
        var request = new DeleteChildNameServerRequest()
        {
            ChildNameServer = hostName,
            UserName = Username,
            Password = Password
        };

        DeleteChildNameServerResponse response = new();
        try
        { 
            response = await DomainClient.DeleteChildNameServerAsync(request);
            if (response.OperationResult == ExecutionStatus.ERROR)
                Console.WriteLine($"Failed: {response.OperationMessage}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
        
        stopwatch.Stop();
        DateTime completedDate = DateTime.Now;
    }
    
    /// <summary>
    /// İstenilen host (child name) kaydına ait bilgileri getirir
    /// </summary>
    [Test]
    public async Task Should_Get_Child_Name_Async()
    {
        DomainClient.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 20);

        string childName = "ns1.apiname11.com";
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
    
        DateTime startDate = DateTime.Now;
        var request = new GetChildNameServerRequest()
        {
            ChildNameServer = childName,
            UserName = Username,
            Password = Password
        };

        GetChildNameServerResponse response = new();
        try
        { 
            response = await DomainClient.GetChildNameServerAsync(request);
            if (response.OperationResult == ExecutionStatus.ERROR)
                Console.WriteLine($"Failed: {response.OperationMessage}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
        
        stopwatch.Stop();
        DateTime completedDate = DateTime.Now;
    }
    
    /// <summary>
    /// İstekte bulunulan domaine ait host kayıtlarını listeler
    /// </summary>
    [Test]
    public async Task Should_Get_Child_Name_Servers_Async()
    {
        DomainClient.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 20);

        string domainName = "apiname11.com";
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
    
        DateTime startDate = DateTime.Now;
        var request = new GetChildNameServersRequest()
        {
            DomainName = domainName,
            UserName = Username,
            Password = Password
        };

        GetChildNameServersResponse response = new();
        try
        { 
            response = await DomainClient.GetChildNameServersAsync(request);
            if (response.OperationResult == ExecutionStatus.ERROR)
                Console.WriteLine($"Failed: {response.OperationMessage}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
        
        stopwatch.Stop();
        DateTime completedDate = DateTime.Now;
    }
    
    /// <summary>
    /// İstekte bulunulan domaine ait host kayıtlarını listeler
    /// </summary>
    [Test]
    public async Task Should_Modify_Child_Name_Async()
    {
        DomainClient.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 20);

        string childName = "ns1.apiname11.com";
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
    
        DateTime startDate = DateTime.Now;
        var request = new ModifyChildNameServerRequest()
        {
            ChildNameServer = childName,
            UserName = Username,
            Password = Password
        };

        ModifyChildNameServerResponse response = new();
        try
        { 
            response = await DomainClient.ModifyChildNameServerAsync(request);
            if (response.OperationResult == ExecutionStatus.ERROR)
                Console.WriteLine($"Failed: {response.OperationMessage}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
        
        stopwatch.Stop();
        DateTime completedDate = DateTime.Now;
    }
    
    /// <summary>
    /// İstekte bulunulan domaine ait yönlendirme kaydı oluşturur. Yönlendirme işlemi yapıldığında NS adresleriniz sistemin yönlendirme adreslerine yönlendilir ve buna uygun DNS kayıtları oluşturulur
    /// </summary>
    [Test]
    public async Task Should_Create_Forward_Async()
    {
        DomainClient.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 20);

        string domainName = "apiname11.com";
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
    
        DateTime startDate = DateTime.Now;
        var request = new ForwardRequest()
        {
            DomainName = domainName,
            UrlAction = "https://www.domainnameapi.com",
            UrlForwardType = UrlForwardType.STANDART,
            UserName = Username,
            Password = Password
        };

        ForwardResponse response = new();
        try
        { 
            response = await DomainClient.CreateForwardAsync(request);
            if (response.OperationResult == ExecutionStatus.ERROR)
                Console.WriteLine($"Failed: {response.OperationMessage}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
        
        stopwatch.Stop();
        DateTime completedDate = DateTime.Now;
    }
    
    /// <summary>
    /// Varolan yönlendirme kaydını düzenler
    /// </summary>
    [Test]
    public async Task Should_Modify_Forward_Async()
    {
        DomainClient.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 20);

        string domainName = "apiname11.com";
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
    
        DateTime startDate = DateTime.Now;
        var request = new ForwardRequest()
        {
            DomainName = domainName,
            UrlAction = "https://www.apiname.com",
            UrlForwardType = UrlForwardType.STANDART,
            UserName = Username,
            Password = Password
        };

        ForwardResponse response = new();
        try
        { 
            response = await DomainClient.ModifyForwardAsync(request);
            if (response.OperationResult == ExecutionStatus.ERROR)
                Console.WriteLine($"Failed: {response.OperationMessage}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
        
        stopwatch.Stop();
        DateTime completedDate = DateTime.Now;
    }
    
    /// <summary>
    /// Varolan yönlendirme kaydını siler
    /// </summary>
    [Test]
    public async Task Should_Delete_Forward_Async()
    {
        DomainClient.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 20);

        string domainName = "apiname11.com";
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
    
        DateTime startDate = DateTime.Now;
        var request = new ForwardRequest()
        {
            DomainName = domainName,
            UserName = Username,
            Password = Password
        };

        ForwardResponse response = new();
        try
        { 
            response = await DomainClient.DeleteForwardAsync(request);
            if (response.OperationResult == ExecutionStatus.ERROR)
                Console.WriteLine($"Failed: {response.OperationMessage}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
        
        stopwatch.Stop();
        DateTime completedDate = DateTime.Now;
    }
    
    /// <summary>
    /// Varolan yönlendirme kaydını getirir
    /// </summary>
    [Test]
    public async Task Should_Get_Forward_Async()
    {
        DomainClient.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 20);

        string domainName = "apiname11.com";
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
    
        DateTime startDate = DateTime.Now;
        var request = new ForwardRequest()
        {
            DomainName = domainName,
            UserName = Username,
            Password = Password
        };

        ForwardResponse response = new();
        try
        { 
            response = await DomainClient.GetForwardAsync(request);
            if (response.OperationResult == ExecutionStatus.ERROR)
                Console.WriteLine($"Failed: {response.OperationMessage}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
        
        stopwatch.Stop();
        DateTime completedDate = DateTime.Now;
    }
    
    /// <summary>
    /// Domain NS bilgilerini değiştirir
    /// </summary>
    [Test]
    public async Task Should_Change_Name_Server_Async()
    {
        DomainClient.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 20);

        string domainName = "apiname11.com";
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
    
        DateTime startDate = DateTime.Now;
        var request = new ModifyNameServerRequest()
        {
            DomainName = domainName,
            UserName = Username,
            Password = Password, 
            NameServerList = new []{ "tr.apiname.com", "eu.apiname.com" }
        };

        ModifyNameServerResponse response = new();
        try
        { 
            response = await DomainClient.ModifyNameServerAsync(request);
            if (response.OperationResult == ExecutionStatus.ERROR)
                Console.WriteLine($"Failed: {response.OperationMessage}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
        
        stopwatch.Stop();
        DateTime completedDate = DateTime.Now;
    }
    
    /// <summary>
    /// Transfer işlemini kontrol eder
    /// </summary>
    [Test]
    public async Task Should_Transfer_Check_Async()
    {
        DomainClient.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 20);

        string domainName = "apiname11.com";
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
    
        DateTime startDate = DateTime.Now;
        var request = new CheckTransferRequest()
        {
            DomainName = domainName,
            UserName = Username,
            Password = Password, 
            AuthCode = "11aabbcc"
        };

        CheckTransferResponse response = new();
        try
        { 
            response = await DomainClient.CheckTransferAsync(request);
            if (response.OperationResult == ExecutionStatus.ERROR)
                Console.WriteLine($"Failed: {response.OperationMessage}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
        
        stopwatch.Stop();
        DateTime completedDate = DateTime.Now;
    }
    
    /// <summary>
    /// Transfer işlemini başlatır
    /// </summary>
    [Test]
    public async Task Should_Transfer_Request_Async()
    {
        DomainClient.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 20);

        string domainName = "apiname11.com";
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
    
        DateTime startDate = DateTime.Now;
        var request = new TransferRequest()
        {
            DomainName = domainName,
            UserName = Username,
            Password = Password, 
            AuthCode = "11aabbcc"
        };

        TransferResponse response = new();
        try
        { 
            response = await DomainClient.TransferAsync(request);
            if (response.OperationResult == ExecutionStatus.ERROR)
                Console.WriteLine($"Failed: {response.OperationMessage}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
        
        stopwatch.Stop();
        DateTime completedDate = DateTime.Now;
    }
    
    private async Task WriteLogAsync(string message)
    {
        string fileName = $"endpoint_test_log_{DateTime.Now.ToString("yyyyMMdd")}.txt";
        string fullPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\logs\\" + fileName;
        try
        {
            if (!File.Exists(fullPath))
            {
                var stream = File.Create(fullPath);
                stream.Close();
            }

            using (StreamWriter writer = File.AppendText(fullPath))
                writer.WriteLine("{0} - {1} {2}", message, DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
        }
        catch (Exception ex)
        {
        }
    }
}