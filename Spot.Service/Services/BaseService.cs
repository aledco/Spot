using AutoMapper;

namespace Spot.Business.Services
{
    public abstract class BaseService
    {
        protected readonly IMapper _mapper;

        public BaseService(IMapper mapper)
        {
            this._mapper = mapper;
        }
    }
}
