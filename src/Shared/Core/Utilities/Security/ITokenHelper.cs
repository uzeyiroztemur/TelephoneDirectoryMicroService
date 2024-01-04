namespace Core.Utilities.Security
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(UserClaims userClaims);
        bool ValidateToken(string accessToken);
    }
}
