using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ParentBookingAPI.Model;
using ParentBookingAPI.Repository.Interfaces;
using ParentBookingAPI.Service.UserService;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Security.Cryptography;


namespace ParentBookingAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IUserService _userService;

        private readonly IAuthRepository _authRepository;

        public static UserInfo userInfo = new UserInfo();
        public AuthController(IConfiguration config, IUserService userService, IAuthRepository authRepository)
        {
            _config = config;
            _userService = userService;
            _authRepository = authRepository;
        }

        //V 2
        [HttpPost("registerUser")]
        public async Task<IActionResult> InsertUserInfo([FromBody] UserInfoDTO userInfo)
        {
            try
            {
                int userId = await _authRepository.InsertUserInfoAsync(userInfo);
                return Ok(userId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserInfoDTO request)
        {

            var user = await _authRepository.GetUserInfoByUsernameAsync(request.Username);

            if (user == null)
            {
                return BadRequest("User Not Found!");
            }

            if (!VerifyPasswordHash(request.Password, request.Username))
            {
                return BadRequest("Wrong Password");
            }

            string token = _authRepository.CreateToken(user);

            var refreshToken = await GenerateRefreshTokenAsync(user.UserID);
            SetRefreshToken(refreshToken);

            return Ok(new { token = token , message ="Login Success"});
        }


        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            // Retrieve the refresh token from the request cookie
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token");
            }

            // Retrieve user data from the database based on the refresh token
            var user = await _authRepository.GetUserInfoByTokenAsync(refreshToken);

            if (user == null)
            {
                return Unauthorized("User not found or invalid Refresh Token");
            }

            if (user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token Expired");
            }

            // Generate a new access token
            string token = _authRepository.CreateToken(user);

            // Generate a new refresh token
            var newRefreshToken = await GenerateRefreshTokenAsync(user.UserID);
            await _authRepository.StoreRefreshTokenAsync(newRefreshToken, user.UserID);

            SetRefreshToken(newRefreshToken);

            //return Ok(token);
            return Ok(new { newAccessToken = token, message = "Login Success" });
        }

        private async Task<RefreshToken> GenerateRefreshTokenAsync(int userId)
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };
            // Save the generated refresh token to the database
            await _authRepository.StoreRefreshTokenAsync(refreshToken, userId);

            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = newRefreshToken.Expires,
                  SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            userInfo.RefreshToken = newRefreshToken.Token;
            userInfo.TokenCreated = newRefreshToken.Created;
            userInfo.TokenExpires = newRefreshToken.Expires;
        }

        private bool VerifyPasswordHash(string password, string username)
        {
            // Fetch the user's information from the database based on the provided username
            var user = _authRepository.GetUserInfoByUsernameAsync(username).Result;

            if (user == null)
            {
                return false; // User not found
            }

            using (var hmac = new HMACSHA512(user.PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(user.PasswordHash);
            }
        }
    }
}
