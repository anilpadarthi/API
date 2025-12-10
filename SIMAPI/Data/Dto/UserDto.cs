using SIMAPI.Data.Entities;

namespace SIMAPI.Data.Dto
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Gender { get; set; }
        public string? DOB { get; set; }
        public string? Password { get; set; }
        public int UserRoleId { get; set; }
        public string? Address { get; set; }
        public string? Locality { get; set; }       
        public string? DOJ { get; set; }
        public string? Designation { get; set; }
        public short Status { get; set; }
        public bool IsMcomAccess { get; set; }
        public bool IsLeapAccess { get; set; }
        public IFormFile? UserImageFile { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public UserSalarySetting? userSalarySettings { get; set; }
        public List<UserDocumentDto>? UserDocuments { get; set; }
    }
}
