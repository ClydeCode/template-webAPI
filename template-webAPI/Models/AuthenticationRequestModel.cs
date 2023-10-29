using Microsoft.Build.Framework;

namespace template_webApi.Models
{
    public class AuthenticationRequestModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
