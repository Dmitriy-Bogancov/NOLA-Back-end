using NOLA_API.DataModels;
using NOLA_API.Repositories;

namespace NOLA_API.Controllers;

// [Authorize(Policy = "IsOwner")]
public class DraftsController(IRepository<Draft> repository) : BaseApiController<Draft>(repository);