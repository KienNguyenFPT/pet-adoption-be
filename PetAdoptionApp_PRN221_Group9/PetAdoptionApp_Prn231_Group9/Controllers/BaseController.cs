using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace PetAdoptionApp_Prn231_Group9.Controllers
{
    [EnableCors("MyAllowSpecificOrigins")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    
    public class BaseController : ControllerBase // controller without view support
    {

    }
}
