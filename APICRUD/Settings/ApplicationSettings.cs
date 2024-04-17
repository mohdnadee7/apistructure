namespace APICRUD.Settings
{
    public class ApplicationSettings
    {
       public ConnectionStrings ConnectionStrings { get; set; }
    }
    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
    }
}
