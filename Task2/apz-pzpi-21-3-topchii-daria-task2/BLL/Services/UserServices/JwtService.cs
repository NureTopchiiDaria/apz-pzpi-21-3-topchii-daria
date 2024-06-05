using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BLL.Abstractions.Interfaces.UserInterfaces;
using Core.DataClasses;
using Core.Models.UserModels;
using Core.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BLL.Services.UserServices
{
    internal class JwtService : IJwtService
    {
        private readonly AppSettings appSettings;

        public JwtService(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
        }

        public string GenerateJwt(UserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.appSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(this.appSettings.JwtExpirationTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var jwt = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(jwt);
        }

        public OptionalResult<Guid> ValidateJwt(string jwt)
        {
            if (jwt is null)
            {
                return new OptionalResult<Guid>(false, "Token is empty");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.appSettings.SecretKey);

            try
            {
                tokenHandler.ValidateToken(
                    jwt,
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero,
                    },
                    out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                return new OptionalResult<Guid>(userId);
            }
            catch (Exception ex)
            {
                return new OptionalResult<Guid>(false, ex.Message);
            }
        }
    }
}
