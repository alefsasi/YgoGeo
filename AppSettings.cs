using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace yu_geo_api
{
    public class AppSettings
    {
        public string SecreteKey { get; set; }
        public string StorageConnectionString { get; set; }

    }
}
