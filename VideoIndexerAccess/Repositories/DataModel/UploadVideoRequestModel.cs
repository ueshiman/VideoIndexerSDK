using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class UploadVideoRequestModel
    {
        public string VideoName { get; set; }
        public Stream VideoStream { get; set; }
        public string FileName { get; set; }
        public string? Privacy { get; set; }
        public string? Priority { get; set; }
        public string? Description { get; set; }
        public string? Partition { get; set; }
        public string? ExternalId { get; set; }
        public string? ExternalUrl { get; set; }
        public string? CallbackUrl { get; set; }
        public string? Metadata { get; set; }
        public string? Language { get; set; }
        public string? VideoUrl { get; set; }
        public string? IndexingPreset { get; set; }
        public string? StreamingPreset { get; set; }
        public string? PersonModelId { get; set; }
        public bool? SendSuccessEmail { get; set; }
    }
}
