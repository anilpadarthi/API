namespace SIMAPI.Data.Models
{
    public class UserRoleOption
    {
        public string OptionName { get; set; }
        public int UserOptionAccessId { get; set; }
        public int UserOptionId { get; set; }
        public int UserRoleId { get; set; }
        public bool IsCreate { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsRead { get; set; }


    }
}
