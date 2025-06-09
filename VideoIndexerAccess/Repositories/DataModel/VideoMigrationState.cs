using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public enum VideoMigrationState
    {
        NotStarted = 0,
        Pending = 1,
        InProgress = 2,
        Success = 3,
        Failed = 4,
        NotApplicable = 5,
        PendingUserAction = 6
    }
}
