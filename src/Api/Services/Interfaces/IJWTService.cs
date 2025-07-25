namespace Api.Services.Interfaces
{
    public interface IJWTService
    {
        string GenerateJWTAsync(string userId);
    }
}
