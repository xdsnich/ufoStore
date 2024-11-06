using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ufoShopBack.Data.Authentication
{
    public static class AuthOptions
    {
        private static JwtSettings _jwtSettings;
        public static void Configure(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }
        public static string ISSUER => _jwtSettings.Issuer;
        public static string AUDIENCE => _jwtSettings.Audience;
        public static int Lifetime => _jwtSettings.Lifetime;
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
    }
}