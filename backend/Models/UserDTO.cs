
using System;

namespace prid_tuto.Models
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Pseudo { get; set; }
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
    }

    public class UserWithPasswordDTO : UserDTO
    {
        public string Password { get; set; }
    }
}