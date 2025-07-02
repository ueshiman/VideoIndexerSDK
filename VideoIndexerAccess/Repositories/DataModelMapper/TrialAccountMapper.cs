using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class TrialAccountMapper : ITrialAccountMapper
    {
        private readonly ITrialAccountUserMapper _trialAccountUserMapper;
        private readonly ITrialAccountQuotaUsageMapper _accountQuotaUsageMapper;
        private readonly ITrialAccountStatisticsMapper _trialAccountStatisticsMapper;
        private readonly ITrialLimitedAccessFeaturesMapper _limitedAccessFeaturesMapper;

        public TrialAccountMapper(ITrialAccountUserMapper trialAccountUserMapper, ITrialAccountQuotaUsageMapper accountQuotaUsageMapper, ITrialAccountStatisticsMapper trialAccountStatisticsMapper, ITrialLimitedAccessFeaturesMapper limitedAccessFeaturesMapper)
        {
            _trialAccountUserMapper = trialAccountUserMapper;
            _accountQuotaUsageMapper = accountQuotaUsageMapper;
            _trialAccountStatisticsMapper = trialAccountStatisticsMapper;
            _limitedAccessFeaturesMapper = limitedAccessFeaturesMapper;
        }

        public TrialAccountModel MapFrom(ApiTrialAccountModel model)
        {
            return new TrialAccountModel
            {
                Id = model.id,
                Name = model.name,
                Location = model.location,
                AccountType = model.accountType,
                Url = model.url,
                AccessToken = model.accessToken,
                IsInMoveToArm = model.isInMoveToArm,
                IsArmOnly = model.isArmOnly,
                MoveToArmStartedDate = model.moveToArmStartedDate,
                CName = model.cName,
                Description = model.description,
                CreateTime = model.createTime,
                Owners = model.owners?.Select(_trialAccountUserMapper.MapFrom)?.ToList() ?? new List<TrialAccountUserModel>(),
                Contributors = model.contributors?.Select(_trialAccountUserMapper.MapFrom)?.ToList() ?? new List<TrialAccountUserModel>(),
                InvitedContributors = model.invitedContributors?.ToList() ?? new List<string>(),
                Readers = model.readers?.Select(_trialAccountUserMapper.MapFrom)?.ToList() ?? new List<TrialAccountUserModel>(),
                InvitedReaders = model.invitedReaders?.ToList() ?? new List<string>(),
                QuotaUsage = _accountQuotaUsageMapper.MapFrom(model.quotaUsage),
                Statistics = _trialAccountStatisticsMapper.MapFrom(model.statistics),
                LimitedAccessFeatures = _limitedAccessFeaturesMapper.MapFrom(model.limitedAccessFeatures),
                State = model.state,
                IsPaid = model.isPaid
            };
        }
    }
}
