using Microsoft.AspNetCore.Identity;

namespace week4.Data.Models
{
    public class UserApp : IdentityUser
    {
        public string City { get; set; }
    }
}
