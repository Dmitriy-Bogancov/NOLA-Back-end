using MediatR;
using NOLA_API.Application.Core;
using NOLA_API.DataModels;

namespace NOLA_API.Application.Advertisements
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Advertisement Advertisement { get; set; }
        }


        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // ReSharper disable once HeapView.BoxingAllocation

                var ad = await _context.Ads.FindAsync(request.Advertisement.Id);
                if (ad == null) return null;

                Process(request, ad);

                var result = await _context.SaveChangesAsync() > 0;
                if (!result) return Result<Unit>.Failure("Failed to update activity!");
                return Result<Unit>.Success(Unit.Value);
            }

            static void Process(Command request, Advertisement? ad)
            {
                ad.Title = request.Advertisement.Title ?? ad.Title;
                ad.Description = request.Advertisement.Description;
                ad.Banners = request.Advertisement.Banners;
                ad.Status = request.Advertisement.Status;
            }
        }
    }
}
