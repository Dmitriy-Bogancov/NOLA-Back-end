using NOLA_API.Application.Core;
using NOLA_API.Interfaces;
using NOLA_API.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NOLA_API.DataModels;

namespace NOLA_API.Application.Drafts
{
        public class Create
        {
            public class Command : IRequest<Result<Unit>>
            {
                public Draft Draft { get; set; }
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

                    if (user == null) return Result<Unit>.Failure("Looks like you are not logged in.");
                    if (user.EmailConfirmed == false) return Result<Unit>.Failure("Please confirm your email address.");
                    if(string.IsNullOrEmpty(user.UserName) || user.Links.Count == 0) return Result<Unit>.Failure("Please update your profile first.");

                    request.Draft.UserId = user.Id;

                    request.Draft.Id = new Guid();
                    _context.Drafts.Add(request.Draft);

                    var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                    if (!result) return Result<Unit>.Failure("Failed to create advertisement.");
                    return Result<Unit>.Success(Unit.Value);
                }
            }
        }
    }
