using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                var adv = await _context.Ads.FindAsync(request.Advertisement.Id);

                if (adv == null) return null;

                adv.Title = request.Advertisement.Title ?? adv.Title;
                adv.Description = request.Advertisement.Description;
                adv.Banner = request.Advertisement.Banner;
                adv.Status = request.Advertisement.Status;

                var result = await _context.SaveChangesAsync() > 0;
                if (!result) return Result<Unit>.Failure("Failed to update activity!");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
