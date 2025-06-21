namespace Auth.API.Model
{
    public class UserDto
    {
        public int userId { get; set; }
        public string emailId { get; set; }
        public DateTime createdDate { get; set; }
        public string fullName { get; set; }
        public string mobileNo { get; set; }
        public string role { get; set; } = "user";
    }
}
