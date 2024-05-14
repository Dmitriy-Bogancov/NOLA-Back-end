using MediatR;
using NOLA_API.Application.Core;
using NOLA_API.DataModels;
using Microsoft.EntityFrameworkCore;
using NOLA_API.DTOs;
using NOLA_API.Extensions;

namespace NOLA_API.Application.Advertisements
{
    public class Show
    {
        public class Query : IRequest<Result<List<AdvertisementDto>>>
        {
        }

        public class Handler : IRequestHandler<Query, Result<List<AdvertisementDto>>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<List<AdvertisementDto>>> Handle(Query request, CancellationToken cancellationToken)
            {

                var ads = await _context.Ads
                    .ToListAsync(cancellationToken);
                    var adsDto = new List<AdvertisementDto>();
                    ads.ForEach(a=> {
                        var adDto = new AdvertisementDto
                        {
                            Id = a.Id,
                            Title = a.Title,
                            Description = a.Description,
                            Banners = a.Banners,
                            Links = a.Links,
                            Visitors = a.Visitors.Select(v => v.ToProfile()).ToList()
                        };
                        adsDto.Add(adDto);
                    
                    });
                return Result<List<AdvertisementDto>>.Success(adsDto);
            }
        }
    }
}