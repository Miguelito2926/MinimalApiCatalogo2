using MinimalApiCatalogo2.Models;

namespace MinimalApiCatalogo2.Services
{
    public interface ITokenService 
    {
        string GerarToken(string key, string issuer, string audience, UserModel user);
     
    }
}
