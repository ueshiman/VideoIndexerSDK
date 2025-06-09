using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class CustomLanguageModelTrainingDataFileModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public bool? Enable { get; set; }
        public string? Creator { get; set; }
        public string? CreationTime { get; set; }
    }
}
