using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class SecretController : ControllerBase
    {
        [Authorize]
        public string Index()
        {
            return "secret message";
        }
    }
}