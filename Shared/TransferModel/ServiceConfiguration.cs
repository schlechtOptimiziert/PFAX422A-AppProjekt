namespace TransferModel;

public class ServiceConfiguration
{
    public ConnectionStringConfiguration ConnectionStrings { get; set; }
}

public class ConnectionStringConfiguration
{
    public string Db { get; set; }
}
