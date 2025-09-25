using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class SpeechDatasetFileMapper : ISpeechDatasetFileMapper
    {
        private readonly IFilePropertiesMapper _filePropertiesMapper;

        public SpeechDatasetFileMapper(IFilePropertiesMapper filePropertiesMapper)
        {
            _filePropertiesMapper = filePropertiesMapper;
        }

        public SpeechDatasetFileModel MapFrom(VideoIndexerAccessCore.VideoIndexerClient.ApiModel.ApiSpeechDatasetFileModel apiModel)
        {
            return new SpeechDatasetFileModel
            {
                DatasetId = apiModel.datasetId,
                FileId = apiModel.fileId,
                Name = apiModel.name,
                ContentUrl = apiModel.contentUrl,
                Kind = apiModel.kind,
                CreatedDateTime = apiModel.createdDateTime,
                PropertiesModel = _filePropertiesMapper.MapFrom(apiModel.propertiesModel)
            };
        }

        public ApiSpeechDatasetFileModel MapFromApiSpeechDatasetFileModel(SpeechDatasetFileModel model)
        {
            return new ApiSpeechDatasetFileModel
            {
                datasetId = model.DatasetId,
                fileId = model.FileId,
                name = model.Name,
                contentUrl = model.ContentUrl,
                kind = model.Kind,
                createdDateTime = model.CreatedDateTime,
                propertiesModel = _filePropertiesMapper.MapFromApiFilePropertiesModel(model.PropertiesModel)
            };
        }
    }
}
