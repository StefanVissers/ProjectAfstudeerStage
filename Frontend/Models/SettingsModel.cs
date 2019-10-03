using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Models
{
    public class SettingsModel
    {

    }

    public class SecretSettings
    {
        public string SecretString { get; set; }
    }

    public class MongoDBAppSettings
    {
        public string MongoDatabaseName { get; set; }
        public string MongoUserDatabaseName { get; set; }
        public string MongoAuthDatabaseName { get; set; }
        public string MongoUsername { get; set; }
        public string MongoPassword { get; set; }
        public string MongoPort { get; set; }
        public string MongoHost { get; set; }
    }

    public class CouchDbSettings
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string BaseURL { get; set; }

        public string Port { get; set; }

        public string UserDatabaseName { get; set; }

        public string DocumentDatabaseName { get; set; }
    }
}
