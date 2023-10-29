using Microsoft.AspNetCore.Identity;

namespace ecommerce_webApi.Data
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
