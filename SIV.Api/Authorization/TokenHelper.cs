using SqlSugar;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SIV.Api.Authorization
{
    /// <summary>
    /// 令牌
    /// </summary>
    public class TokenModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }


    }

    public class TokenHelper
    {
        /// <summary>
        /// 颁发Token
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="permissionRequirement"></param>
        /// <returns></returns>
        public static object IssueToken(Claim[] claims, SSORequirement permissionRequirement)
        {
            var now = DateTime.Now;
            //实例化JwtSecurityToken
            var jwt = new JwtSecurityToken(
                issuer: permissionRequirement.Issuer,
                audience: permissionRequirement.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(permissionRequirement.Expiration),
                signingCredentials: permissionRequirement.SigningCredentials
            );
            // 生成 Token
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new
            {
                token_type = "Bearer",
                token = encodedJwt,
                expires_in = permissionRequirement.Expiration.TotalSeconds,
                user_id = claims[0].Value
            };
        }

        /// <summary>
        /// 解析JWTToken
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static TokenModel ResolveToken(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);

            var tm = new TokenModel
            {
                UserID = 0,
            };

            object? userID;
            try
            {
                if (jwtToken.Payload.TryGetValue(ClaimTypes.Sid, out userID))
                {
                    if (int.TryParse(userID?.ToString(), out var uid))
                    {
                        tm.UserID = uid;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return tm;
        }
    }
}
