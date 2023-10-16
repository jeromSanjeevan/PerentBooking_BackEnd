using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using ParentBookingAPI.Helper;
using ParentBookingAPI.Model;
using ParentBookingAPI.Model.DTO;
using ParentBookingAPI.Repository.Interfaces;
using System.Data;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace ParentBookingAPI.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DatabaseHelper _dbHelper;
        private readonly IConfiguration _config;


        public AuthRepository(IConfiguration config, DatabaseHelper dbHelper)
        {

            _dbHelper = dbHelper;
            _config = config;
        }

        public async Task<int> InsertUserInfoAsync(UserInfoDTO request)
        {
            byte[] passwordSalt;
            byte[] passwordHash;

            CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);

            UserInfo userInfo = new UserInfo
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };

            SqlParameter[] parameters = new SqlParameter[]
            {
                 new SqlParameter("@Username", SqlDbType.NVarChar) { Value = userInfo.Username },
                 new SqlParameter("@PasswordHash", SqlDbType.VarBinary) { Value = userInfo.PasswordHash },
                new SqlParameter("@PasswordSalt", SqlDbType.VarBinary) { Value = userInfo.PasswordSalt },

            };

            return await _dbHelper.ExecuteInsertStoredProcedureAsync("SP_RegisterUser", parameters);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<UserInfo> GetUserInfoByUsernameAsync(string username)
        {
            var parameters = new SqlParameter[]
        {
            new SqlParameter("@Username", SqlDbType.NVarChar) { Value = username }
        };

            var userInfo = await _dbHelper.ExecuteStoredProcedureAsync("SP_GetLoginInfoByUsername", parameters, MapUserResponseDto);

            return userInfo.FirstOrDefault(); // Assuming the ExecuteStoredProcedureAsync returns a list
        }

        public async Task<UserInfo> GetUserInfoByTokenAsync(string token)
        {
            var parameters = new SqlParameter[]
        {
            new SqlParameter("@Token", SqlDbType.NVarChar) { Value = token }
        };

            var userInfo = await _dbHelper.ExecuteStoredProcedureAsync("SP_GetLoginInfoByToken", parameters, MapUserResponseDto);

            return userInfo.FirstOrDefault(); // Assuming the ExecuteStoredProcedureAsync returns a list
        }

        private UserInfo MapUserResponseDto(SqlDataReader reader)
        {
            UserInfo userInfo = new UserInfo();

            int userIdOrdinal = reader.GetOrdinal("UserId");
            if (!reader.IsDBNull(userIdOrdinal))
            {
                userInfo.UserID = reader.GetInt32(userIdOrdinal);
            }

            int usernameOrdinal = reader.GetOrdinal("Username");
            if (!reader.IsDBNull(usernameOrdinal))
            {
                userInfo.Username = reader.GetString(usernameOrdinal);
            }
            // Handle other properties in a similar way

            int roleOrdinal = reader.GetOrdinal("Role");
            if (!reader.IsDBNull(roleOrdinal))
            {
                userInfo.Role = reader.GetString(roleOrdinal);
            }

            int passwordHashOrdinal = reader.GetOrdinal("PasswordHash");
            if (!reader.IsDBNull(passwordHashOrdinal))
            {
                userInfo.PasswordHash = reader.GetFieldValue<byte[]>(passwordHashOrdinal);
            }

            int passwordSaltOrdinal = reader.GetOrdinal("PasswordSalt");
            if (!reader.IsDBNull(passwordSaltOrdinal))
            {
                userInfo.PasswordSalt = reader.GetFieldValue<byte[]>(passwordSaltOrdinal);
            }

            int refreshTokenOrdinal = reader.GetOrdinal("RefreshToken");
            if (!reader.IsDBNull(refreshTokenOrdinal))
            {
                userInfo.RefreshToken = reader.GetString(refreshTokenOrdinal);
            }

            int tokenCreatedOrdinal = reader.GetOrdinal("TokenCreated");
            if (!reader.IsDBNull(tokenCreatedOrdinal))
            {
                userInfo.TokenCreated = reader.GetDateTime(tokenCreatedOrdinal);
            }

            int tokenExpiresOrdinal = reader.GetOrdinal("TokenExpires");
            if (!reader.IsDBNull(tokenExpiresOrdinal))
            {
                userInfo.TokenExpires = reader.GetDateTime(tokenExpiresOrdinal);
            }

            return userInfo;
        }

        public string CreateToken(UserInfo userInfo)
        {

            List<Claim> claims = new List<Claim>()
            {
                new Claim("name",userInfo.Username),
                new Claim("role",userInfo.Role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);



            var token = new JwtSecurityToken(
                claims: claims,
                //expires: DateTime.Now.AddDays(1),
              expires : DateTime.Now.AddSeconds(30),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public async Task<int> StoreRefreshTokenAsync(RefreshToken refreshToken, int userId)
        {
            var parameters = new SqlParameter[]
            {
            new SqlParameter("@Token", SqlDbType.NVarChar, 255) { Value = refreshToken.Token },
            new SqlParameter("@Expires", SqlDbType.DateTime) { Value = refreshToken.Expires },
            new SqlParameter("@Created", SqlDbType.DateTime) { Value = refreshToken.Created },
            new SqlParameter("@UserId", SqlDbType.Int) { Value = userId }
            };

            return await _dbHelper.ExecuteUpdateStoredProcedureAsync("SP_UpdateRefreshToken", parameters);
        }
    }
}
