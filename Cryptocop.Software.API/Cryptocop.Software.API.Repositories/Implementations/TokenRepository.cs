using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Repositories.Interfaces;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class TokenRepository : ITokenRepository
    {
        public JwtTokenDto CreateNewToken()
        {
            throw new System.NotImplementedException();
        }

        public bool IsTokenBlacklisted(int tokenId)
        {
            throw new System.NotImplementedException();
        }

        public void VoidToken(int tokenId)
        {
            throw new System.NotImplementedException();
        }
    }
}