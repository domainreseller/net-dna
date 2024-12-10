namespace Dna.SoapEndpoint.Tests;

public class ClientConsts
{
    /// <summary>
    /// Başvuru kategorisidir. 0 (kurumsal başvuru) ve 1 (kişisel başvuru) değerlerini alır.
    /// </summary>
    public const string TRABIS_DOMAIN_CATEGORY_KEY = "TRABISDOMAINCATEGORY";
    /// <summary>
    /// T.C. kimlik numarası. Kişisel başvurularda gönderilmesi zorunludur.
    /// <example>
    /// Kimlik bilgisi yoksa "11111111111" olarak gönderilmelidir
    /// </example>
    /// </summary>
    public const string TRABIS_CITIZIENID_KEY = "TRABISCITIZIENID";
    /// <summary>
    /// T.C. vergi numarasıdır. Kurumsal başvularda gönderilmesi zorunludur.
    /// <example>
    /// Vergi bilgisi yoksa "1111111111" olarak iletilmelidir
    /// </example>
    /// </summary>
    public const string TRABIS_TAX_NUMBER_KEY = "TRABISTAXNUMBER";
    /// <summary>
    /// T.C. vergi dairesi bilgisidir. Kurumsal başvurularda gönderilmesi zorunludur.
    /// <example>
    /// Vergi dairesine ait bilgi yoksa "1111111111" olarak iletilmelidir
    /// </example>
    /// </summary>
    public const string TRABIS_TAX_OFFICE_KEY = "TRABISTAXOFFICE";
    /// <summary>
    /// .tr domainleri WHOIS çıktılarında "web adresi" kısmını temsil eder. Opsiyoneldir
    /// </summary>
    public const string TRABIS_WEBADDRESS_KEY = "TRABISWEBADDRESS";
    /// <summary>
    /// Kayıt eden kişinin adı ve soyadıdır. Kişisel kayıtlarda gönderilmesi zorunludur
    /// </summary>
    public const string TRABIS_NAMESURNAME_KEY = "TRABISNAMESURNAME";
    /// <summary>
    /// Kayıt eden şirket / organizasyon bilgisi. Kurumsal kayıtlarda gönderilmesi zorunludur.
    /// </summary>
    public const string TRABIS_ORGANIZATION_KEY = "TRABISORGANIZATION";

    /// <summary>
    /// Kişisel başvurular için zorunlu parametreler
    /// </summary>
    public static List<string> TrabisOwnerRegistrationAttributes => new() { TRABIS_CITIZIENID_KEY, TRABIS_NAMESURNAME_KEY };
    /// <summary>
    /// Kurumsal başvurular için zorunlu parametreler
    /// </summary>
    public static List<string> TrabisDefaultRegistrationAttributes => new() { TRABIS_TAX_OFFICE_KEY, TRABIS_TAX_NUMBER_KEY, TRABIS_ORGANIZATION_KEY };
    
}

