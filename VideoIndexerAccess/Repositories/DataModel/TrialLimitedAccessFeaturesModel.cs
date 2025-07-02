using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class TrialLimitedAccessFeaturesModel
    {
        public bool IsFaceIdentificationEnabled { get; set; }
        public bool IsCelebrityRecognitionEnabled { get; set; }
        public bool IsFaceDetectionEnabled { get; set; }
    }
}
