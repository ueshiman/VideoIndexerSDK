using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    // Accountクラス（全プロパティを定義）
    public class ApiTrialAccountModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public string accountType { get; set; }
        public string url { get; set; }
        public string accessToken { get; set; }
        public bool isInMoveToArm { get; set; }
        public bool isArmOnly { get; set; }
        public DateTime? moveToArmStartedDate { get; set; }
        public string cName { get; set; }
        public string description { get; set; }
        public DateTime? createTime { get; set; }
        public List<ApiTrialAccountUserModel> owners { get; set; }
        public List<ApiTrialAccountUserModel> contributors { get; set; }
        public List<string> invitedContributors { get; set; }
        public List<ApiTrialAccountUserModel> readers { get; set; }
        public List<string> invitedReaders { get; set; }
        public ApiTrialAccountQuotaUsageModel quotaUsage { get; set; }
        public ApiTrialAccountStatisticsModel statistics { get; set; }
        public ApiTrialLimitedAccessFeaturesModel limitedAccessFeatures { get; set; }
        public string state { get; set; }
        public bool isPaid { get; set; }
    }
}
