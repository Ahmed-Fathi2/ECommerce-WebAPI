namespace ECommerce.Application.Common.Settings;

public class BlobStorageSettings
{
    public static string SectionName { get; set; } = "BlobStorage";
    public string ConnectionString { get; set; }
    public string ContainerName { get; set; }
    public string AccountKey { get; set; }

}



