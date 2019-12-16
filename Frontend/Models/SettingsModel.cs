using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Models
{
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

    public class DockerSettings
    {
        public string DockerClientConfigURI { get; set; }

        public string ShellFile { get; set; }

        public string ShellArgs { get; set; }

        public string DockerContainerId { get; set; }
    }
}
