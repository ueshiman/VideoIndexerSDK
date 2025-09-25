using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class SpeechDatasetMapper : ISpeechDatasetMapper
    {
        private readonly SpeechDatasetPropertiesMapper _speechDatasetPropertiesMapper;

        public SpeechDatasetMapper(SpeechDatasetPropertiesMapper speechDatasetPropertiesMapper)
        {
            _speechDatasetPropertiesMapper = speechDatasetPropertiesMapper;
        }

        public SpeechDatasetModel MapFrom(ApiSpeechDatasetModel apiModel)
        {
            return new SpeechDatasetModel
            {
                Id = apiModel.id,
                Properties = apiModel.properties is null ? null : _speechDatasetPropertiesMapper.MapFrom(apiModel.properties),
                DisplayName = apiModel.displayName,
                Description = apiModel.description,
                Locale = apiModel.locale,
                Kind = apiModel.kind,
                Status = apiModel.status,
                LastActionDateTime = apiModel.lastActionDateTime,
                CreatedDateTime = apiModel.createdDateTime,
                CustomProperties = apiModel.customProperties
            };
        }

        public ApiSpeechDatasetModel MapToApiSpeechDatasetModel(SpeechDatasetModel model)
        {
            return new ApiSpeechDatasetModel
            {
                id = model.Id,
                properties = model.Properties is null ? null : _speechDatasetPropertiesMapper.MapToApiSpeechDatasetPropertiesModel(model.Properties),
                displayName = model.DisplayName,
                description = model.Description,
                locale = model.Locale,
                kind = model.Kind,
                status = model.Status,
                lastActionDateTime = model.LastActionDateTime,
                createdDateTime = model.CreatedDateTime,
                customProperties = model.CustomProperties
            };
        }
    }
}
