using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class BrandModel
    {
        public string? ReferenceUrl { get; set; }
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? AccountId { get; set; }
        public string? LastModifierUserName { get; set; }
        public DateTime? Create { get; set; }
        public DateTime? LastModified { get; set; }
        public bool? Enabled { get; set; }
        public string? Description { get; set; }
        public string[]? Tags { get; set; }
    }
}
