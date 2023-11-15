namespace template_webApi.Models
{
    public class AuthenticationResponseModel
    {
        public string Token { get; set; }
        
        public DateTime Expiration { get; set; }
    }
}
