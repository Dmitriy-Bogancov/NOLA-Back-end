using MediatR;
using NOLA_API.Application.Core;
using NOLA_API.DataModels;

namespace NOLA_API.Application.Drafts;

public class Edit
{
    public class Command : IRequest<Result<Unit>>
    {
        public Draft Draft { get; set; }
    }


    public class Handler(DataContext context) : IRequestHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            // ReSharper disable once HeapView.BoxingAllocation
                
            var ad = await context.Drafts.FindAsync(request.Draft.Id, cancellationToken);
            if (ad == null) return null;

            ad.Title = request.Draft.Title ?? ad.Title;
            ad.Description = request.Draft.Description;
            ad.Banners = request.Draft.Banners;

            var result = await context.SaveChangesAsync(cancellationToken) > 0;
            if (!result) return Result<Unit>.Failure("Failed to update activity!");
            return Result<Unit>.Success(Unit.Value);
        }
    }
}