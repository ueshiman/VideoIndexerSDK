using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class FilePropertiesMapper : IFilePropertiesMapper
    {
        public FilePropertiesModel MapFrom(ApiFilePropertiesModel apiModel)
        {
            return new FilePropertiesModel
            {
                Size = apiModel.size,
                Duration = apiModel.duration
            };
        }

        public ApiFilePropertiesModel MapFromApiFilePropertiesModel(FilePropertiesModel model)
        {
            return new ApiFilePropertiesModel
            {
                size = model.Size,
                duration = model.Duration
            };
        }
    }
}
