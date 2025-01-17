using Newtonsoft.Json;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Repository.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace SIMAPI.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<CommonResponse> ValidateUser(string email, string password)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                User userDetails = await _userRepository.GetUserDetailsAsync(email, password);

                if (userDetails != null)
                {
                    var userOptions = await _userRepository.GetUserRoleOptionsAsync(userDetails.UserRoleId);
                    userDetails.UserImage = FileUtility.GetImagePath(FolderUtility.user, userDetails.UserImage);
                    var token = createToken(userDetails, userOptions);
                    response.data = new { userDetails, userOptions, token };
                    response.statusCode = HttpStatusCode.OK;
                    response.status = true;
                }
                else
                {
                    response.data = "Invalid username or password";
                    response.statusCode = HttpStatusCode.NoContent;
                    response.status = false;
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }


        private string createToken(User userDetails, IEnumerable<UserRoleOption> userOptions)
        {
            //Set issued at date
            DateTime issuedAt = DateTime.UtcNow;
            //set the time when it expires
            DateTime expires = DateTime.UtcNow.AddHours(1);

            var tokenHandler = new JwtSecurityTokenHandler();
            List<Claim> userClaims = new List<Claim>();
            userClaims.Add(new Claim(nameof(userDetails), JsonConvert.SerializeObject(userDetails)));
            userClaims.Add(new Claim(nameof(userOptions), JsonConvert.SerializeObject(userOptions)));

            //create a identity and add claims to the user which we want to log in
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userClaims);

            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(_configuration["Jwt:Key"]));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);


            //create the jwt
            var token =
                (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(issuer: _configuration["Jwt:Issuer"], audience: _configuration["Jwt:Issuer"],
                        subject: claimsIdentity, notBefore: issuedAt, expires: expires, signingCredentials: signingCredentials);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
