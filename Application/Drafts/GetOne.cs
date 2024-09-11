// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using NOLA_API.Application.Core;
// using NOLA_API.DataModels;
//
// namespace NOLA_API.Application.Drafts;
//
// public class GetOne
// {
//     public class Query : IRequest<Result<Draft>>
//     {
//         public Guid Id { get; set; }
//     }
//     public class Handler(DataContext context) : IRequestHandler<Query, Result<Draft>>
//     {
//         public async Task<Result<Draft>> Handle(Query request, CancellationToken cancellationToken)
//         {
//
//             var draft = await context.Drafts
//                 .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
//             return Result<Draft>.Success(draft);
//         }
//     }
// }