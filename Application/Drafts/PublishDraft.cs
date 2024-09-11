// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using NOLA_API.Application.Core;
// using NOLA_API.DataModels;
// using NOLA_API.Domain;
// using NOLA_API.Interfaces;
//
// namespace NOLA_API.Application.Drafts;
//
// public class PublishDraft
// {
//     public class Command : IRequest<Result<Unit>>
//     {
//         public Guid Id { get; init; }
//
//         public Draft Draft { get; set; }
//     }
//
//
//     public class Handler(DataContext context, IUserAccessor userAccessor) : IRequestHandler<Command, Result<Unit>>
//     {
//         public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
//         {
//             var draft = await context.Drafts.FindAsync(request.Id, cancellationToken);
//
//             var user = await context.Users.FirstOrDefaultAsync(x =>
//                 x.UserName == userAccessor.GetUsername(), cancellationToken: cancellationToken);
//
//             if (user == null) return Result<Unit>.Failure("Looks like you are not logged in.");
//             if (user.EmailConfirmed == false) return Result<Unit>.Failure("Please confirm your email address.");
//             if (string.IsNullOrEmpty(user.UserName) || user.Links.Count == 0) return Result<Unit>.Failure("Please update your profile first.");
//                 
//             var ad = new Advertisement
//             {
//                 Id = new Guid(),
//                 Title = draft.Title,
//                 Description = draft.Description,
//                 Status = Status.Moderation,
//                 Banners = draft.Banners,
//                 CreatedAt = DateTime.Now,
//             };
//                     
//             var owner = new AdVisitor
//             {
//                 AppUser = user,
//                 Post = ad,
//                 IsOwner = true
//             };
//
//             ad.Visitors.Add(owner);
//             context.Ads.Add(ad);
//             var result = await context.SaveChangesAsync(cancellationToken) > 0;
//
//             context.Drafts.Remove(draft);
//             var delResult = await context.SaveChangesAsync(cancellationToken) > 0;
//
//             if (!result) return Result<Unit>.Failure("Failed to create advertisement.");
//             if (!delResult) return Result<Unit>.Failure("Failed to remove draft.");
//             return Result<Unit>.Success(Unit.Value);
//         }
//     }
// }