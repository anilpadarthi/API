//using Newtonsoft.Json;
//using SIMAPI.Data.Entities;
//using System.Security.Claims;

//namespace SIMAPI.Business
//{
//    public class UserSession
//    {


//        public int UserID
//        {
//            get
//            {

//                //return 1;
//                if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
//                {
//                    ClaimsIdentity claimIdentity = User.Identity as ClaimsIdentity;
//                    User userObj = JsonConvert.DeserializeObject<User>(claimIdentity.FindFirst("userDetails").Value);
//                    return userObj.UserId;
//                }
//                return new int();
//            }
//        }
//        public User GetUser
//        {

//            get
//            {
               
//                if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
//                {
//                    ClaimsIdentity claimIdentity = User.Identity as ClaimsIdentity;
//                    User userObj = JsonConvert.DeserializeObject<User>(claimIdentity.FindFirst("userDetails").Value);
//                    return userObj;
//                }
//                return null;
//            }
//        }
//    }
//}
