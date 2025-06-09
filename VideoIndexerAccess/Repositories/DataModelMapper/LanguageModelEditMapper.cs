using VideoIndexerAccess.Repositories.DataModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class LanguageModelEditMapper : ILanguageModelEditMapper
    {
        public LanguageModelEditModel MapFrom(ApiLanguageModelEditModel model)
        {
            return new LanguageModelEditModel
            {
                Id = model.id,
                VideoId = model.videoId,
                LineId = model.lineId,
                CreateDate = model.createDate,
                OriginalValue = model.originalValue,
                CurrentValue = model.currentValue,
                LinguisticTrainingDataGroupsId = model.linguisticTrainingDataGroupsId,
                VideoName = model.videoName
            };
        }

        public ApiLanguageModelEditModel MapToApiLanguageModelEditModel(LanguageModelEditModel model)
        {
            return new ApiLanguageModelEditModel
            {
                id = model.Id,
                videoId = model.VideoId,
                lineId = model.LineId,
                createDate = model.CreateDate,
                originalValue = model.OriginalValue,
                currentValue = model.CurrentValue,
                linguisticTrainingDataGroupsId = model.LinguisticTrainingDataGroupsId,
                videoName = model.VideoName
            };
        }
    }
}
