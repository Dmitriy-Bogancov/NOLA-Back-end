using MediatR;
using Microsoft.EntityFrameworkCore;
using NOLA_API.Application.Core;
using NOLA_API.DataModels;
using NOLA_API.DTOs;
using NOLA_API.Extensions;

namespace NOLA_API.Application.Advertisements
{
    public class GetOne
    {
        public class Query : IRequest<Result<AdvertisementDto>>
        {
            public Guid Id { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<AdvertisementDto>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<AdvertisementDto>> Handle(Query request, CancellationToken cancellationToken)
            {

                var ad = await _context.Ads
                    .FirstOrDefaultAsync(x => x.Id == request.Id);
                    var dto = new AdvertisementDto
                    {
                        Id = ad.Id,
                        Title = ad.Title,
                        Description = ad.Description,
                        Banners = ad.Banners,
                        Links = ad.Links,
                        Visitors = ad.Visitors.Select(v => v.ToProfile()).ToList()
                    };
                return Result<AdvertisementDto>.Success(dto);
            }
        }
    }
}