namespace LearnAPI.DTOs
{
    // What frontend SENDS to get new access token
    public class RefreshRequestDto
    {
        public string RefreshToken { get; set; } = string.Empty;
    }

    // What you SEND BACK after login / refresh
    public class TokenResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
}