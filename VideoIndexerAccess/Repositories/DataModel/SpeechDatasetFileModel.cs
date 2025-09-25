using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class SpeechDatasetFileModel
    {
            public Guid DatasetId { get; set; }
            public Guid FileId { get; set; }
            public string Name { get; set; }
            public string ContentUrl { get; set; }
            public int Kind { get; set; }
            public string CreatedDateTime { get; set; }
            public FilePropertiesModel PropertiesModel { get; set; }
    }
}
