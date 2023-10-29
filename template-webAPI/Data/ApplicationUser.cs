using Microsoft.AspNetCore.Identity;

namespace template_webApi.Data
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
