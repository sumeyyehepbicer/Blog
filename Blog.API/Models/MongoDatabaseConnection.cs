using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.API.Models
{
    public class MongoDatabaseConnection
    {
        public string DefaultConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string LogCollectionName { get; set; }

    }
}
