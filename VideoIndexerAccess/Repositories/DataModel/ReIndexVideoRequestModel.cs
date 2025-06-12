using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class ReIndexVideoRequestModel
    {
        public required string VideoId { get; set; }
        public List<string>? ExcludedAi { get; set; }
        public bool? IsSearchable { get; set; }
        public string? IndexingPreset { get; set; }
        public string? StreamingPreset { get; set; }
        public string? CallbackUrl { get; set; }
        public string? SourceLanguage { get; set; }
        public bool? SendSuccessEmail { get; set; }
        public string? LinguisticModelId { get; set; }
        public string? PersonModelId { get; set; }
        public string? Priority { get; set; }
        public string? BrandsCategories { get; set; }
        public string? CustomLanguages { get; set; }
        public string? LogoGroupId { get; set; }
        public string? PunctuationMode { get; set; }
    }
}
