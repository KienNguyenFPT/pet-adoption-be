using BusinessLogicLayer.IServices;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BusinessLogicLayer
{
    public class ClaimServices : IClaimServices
    {
        public ClaimServices(IHttpContextAccessor httpContextAccessor)
        {
            var Id = httpContextAccessor.HttpContext.User?.FindFirstValue("Id");
            GetCurrentUserId = string.IsNullOrEmpty(Id) ? Guid.Empty : Guid.Parse(Id);
        }
        public Guid GetCurrentUserId { get; }
    }
}
