// using MediatR;
// using NOLA_API.Application.Core;
// using NOLA_API.DataModels;
// using Microsoft.EntityFrameworkCore;
//
// namespace NOLA_API.Application.Drafts;
//
// public class Show
// {
//     public class Query : IRequest<Result<List<Draft>>> { 
//     }
//
//     public class Handler(DataContext context) : IRequestHandler<Query, Result<List<Draft>>>
//     {
//         public async Task<Result<List<Draft>>> Handle(Query request, CancellationToken cancellationToken)
//         {
//           
//             var drafts = await context.Drafts
//                 .ToListAsync(cancellationToken);
//             return Result<List<Draft>>.Success(drafts);
//             
//         }
//     }
// }