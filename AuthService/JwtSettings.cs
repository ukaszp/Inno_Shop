namespace AuthService
{
    public class JwtSettings
    {
        public required string Key { get; set; }        
        public required string Issuer { get; set; }   
        public string? Audience { get; set; }
        public int ExpiresInMinutes { get; set; } = 60;
    }

}
