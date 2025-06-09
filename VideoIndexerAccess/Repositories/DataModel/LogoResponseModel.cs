using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class LogoResponseModel
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string CreatedBy { get; set; } = "";
        public string WikipediaSearchTerm { get; set; } = "";
    }
}
