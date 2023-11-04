namespace ART_AI_API.Models
{
    public class AuthenticationResponseModel
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }
        
        public DateTime Expiration { get; set; }
    }
}
