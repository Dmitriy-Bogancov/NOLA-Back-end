using NOLA_API.DataModels;
using NOLA_API.Repositories;

namespace NOLA_API.Controllers;

public class AdvertisementsController(IRepository<Advertisement> repository) : BaseApiController<Advertisement>(repository);