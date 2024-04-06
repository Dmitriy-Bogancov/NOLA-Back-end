using NOLA_API.Application.Core;
using NOLA_API.Interfaces;
using NOLA_API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NOLA_API.Persistence;
using NOLA_API.DataModels;

namespace NOLA_API.Application.Advertisements
{
 

        public class Create
        {
            public class Command : IRequest<Result<Unit>>
            {
                public Advertisement Advertisement { get; set; }
            }

            public class Handler : IRequestHandler<Command, Result<Unit>>
            {
                private readonly DataContext _context;
                private readonly IUserAccessor _userAccessor;

                public Handler(DataContext context, IUserAccessor userAccessor)
                {
                    _context = context;
                    _userAccessor = userAccessor;
                }

                public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
                {
                    var user = await _context.Users.FirstOrDefaultAsync(x =>
                        x.UserName == _userAccessor.GetUsername());

                    var owner = new AdVisitor
                    {
                        AppUser = user,
                        Post = request.Advertisement,
                        IsOwner = true
                    };

                    request.Advertisement.Visitors.Add(owner);

                    request.Advertisement.Id = new Guid();
                    _context.Ads.Add(request.Advertisement);

                    var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                    if (!result) return Result<Unit>.Failure("Failed to create advertisement.");
                    return Result<Unit>.Success(Unit.Value);
                }
            }
        }
    }
