using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Model;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Business.Utility
{
    public enum TokenResult
    {
        VALID_TOKEN,
        INVALID_TOKEN,
        EXPIRED_TOKEN
    }

    public class Token
    {
        private readonly Context _context;

        public Token(Context context)
        {
            _context = context;
        
        }
        public string CreateJWT(User user, string tokenSecret)
        {
            var userRoles = _context.UserRoles.Include(x => x.User)
                    .Include(x => x.Role).Where(x => x.UserId == user.Id).Select(x => x.Role.Name).ToList();
            List<Claim> claims = new List<Claim>
            {
                
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.UserName)
            };
            userRoles.ForEach(role =>
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            });

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(tokenSecret));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);
           
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            var checkToken = _context.UserTokens.Where(x => x.UserId == user.Id && x.IsDelete == false).ToList();
            foreach (var x in checkToken)
            {
                x.IsDelete = true;
                _context.UserTokens.Update(x);
            }
           
            
            var userToken = new UserToken()
            {
                IsDelete = false,
                expiresTime = DateTime.Now.AddDays(1),
                Token = jwt,
                UserId=user.Id,
            };
            _context.UserTokens.Add(userToken);

            _context.SaveChanges();
            return jwt;
        }


        public static TokenResult VerifyToken(string token)
        {
            return TokenResult.VALID_TOKEN;
        }
    }
}
