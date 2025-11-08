using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SIMAPI.Data.Entities;
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

                //return 1;
                if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
                {
                    ClaimsIdentity claimIdentity = User.Identity as ClaimsIdentity;
                    LoggedInUserDto userObj = JsonConvert.DeserializeObject<LoggedInUserDto>(claimIdentity.FindFirst("userDetails").Value);
                    return userObj.userId;
                }
                return new int();
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