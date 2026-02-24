using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SIMAPI.Data.Models.Login;
using System.Security.Claims;

namespace SIMAPI.Controllers
{
    [ApiController]
    [Authorize]
    public class BaseController : Controller
    {
        public int GetUserId
        {
            get
            {
                var claim = User.FindFirst("userId");
                if (claim == null)
                    throw new UnauthorizedAccessException("UserId claim not found");
                return claim != null ? int.Parse(claim.Value) : 0;
            }
        }
        public LoggedInUserDto GetUser
        {

            get
            {
                //return new User()
                //{

                //    UserId = 1,
                //    UserRoleId = 2,
                //    UserRole = new UserRole() { RoleName = "Admin"}
                //};
                if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
                {
                    ClaimsIdentity claimIdentity = User.Identity as ClaimsIdentity;
                    LoggedInUserDto userObj = JsonConvert.DeserializeObject<LoggedInUserDto>(claimIdentity.FindFirst("userDetails").Value);
                    
                    return userObj;
                }
                return null;
            }
        }
    }
}