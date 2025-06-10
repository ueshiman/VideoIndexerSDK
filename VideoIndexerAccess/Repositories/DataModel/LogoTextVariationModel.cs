using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class LogoTextVariationModel
    {
        public string Text { get; set; } = "";
        public bool CaseSensitive { get; set; }
        public DateTimeOffset CreationTime { get; set; }
        public string? CreatedBy { get; set; } = "";
    }
}
