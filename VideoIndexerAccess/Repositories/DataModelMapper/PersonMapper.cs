using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class PersonMapper : IPersonMapper
    {
        private IFaceModelMapper _faceModelMapper;

        public PersonMapper(IFaceModelMapper faceModelMapper)
        {
            _faceModelMapper = faceModelMapper;
        }

        public PersonModel MapFrom(ApiPersonModel model)
        {
            return new PersonModel
            {
                Id = model.id,
                Name = model.name,
                Description = model.description,
                SampleFace = model.sampleFace == null ? null : _faceModelMapper.MapFrom(model.sampleFace),
                ImageCount = model.imageCount,
                Score = model.score,
                LastModified = model.lastModified,
                LastModifierName = model.lastModifierName
            };
        }

        public ApiPersonModel MapToApiPersonModel(PersonModel model)
        {
            return new ApiPersonModel
            {
                id = model.Id,
                name = model.Name,
                description = model.Description,
                sampleFace = model.SampleFace == null ? null : _faceModelMapper.MapToApiFaceModel(model.SampleFace),
                imageCount = model.ImageCount,
                score = model.Score,
                lastModified = model.LastModified,
                lastModifierName = model.LastModifierName
            };
        }   
    }
}
