namespace APIGastroLink.Services.Interface {
    public interface ITokenService {
        string GenerateToken(string userId, string userName, IEnumerable<string> roles = null);
    }
}
