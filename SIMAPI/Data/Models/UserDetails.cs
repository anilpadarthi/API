using SIMAPI.Data.Entities;

namespace SIMAPI.Data.Models
{
    public class UserDetails
    {
        public User user { get; set; }
        public UserSalarySetting? userSalarySettings { get; set; }
        public IEnumerable<UserDocument> userDocuments { get; set; }
    }
}
