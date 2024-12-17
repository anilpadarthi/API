using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SIMAPI.Data.Entities;
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
                    User userObj = JsonConvert.DeserializeObject<User>(claimIdentity.FindFirst("userDetails").Value);
                    return userObj.UserId;
                }
                return new int();
            }
        }
        public User GetUser
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
                    User userObj = JsonConvert.DeserializeObject<User>(claimIdentity.FindFirst("userDetails").Value);
                    return userObj;
                }
                return null;
            }
        }
    }
}