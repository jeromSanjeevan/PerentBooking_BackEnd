using Microsoft.AspNetCore.Mvc;

namespace ParentBookingAPI.Controllers
{
    public class DummyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //v1
        //[HttpPost("refresh-token")]
        //public async Task<ActionResult<string>> RefreshToken()
        //{

        //    var refreshToken = Request.Cookies["refreshToken"];

        //    if (!userInfo.RefreshToken.Equals(refreshToken))
        //    {
        //        return Unauthorized(" Invalid Refresh Token");
        //    }
        //    else if (!userInfo.TokenExpires < DateTime.Now)
        //    {
        //        return Unauthorized(" Token Expired");
        //    }

        //    string token = _authRepository.CreateToken(user);
        //    var newRefreshToken = GenerateRefreshToken();
        //    SetRefreshToken(newRefreshToken);


        //    return Ok(token);


        //}


        //v1

        //private asynRefreshToken GenerateRefreshToken()
        //{
        //    var refreshToken = new RefreshToken
        //    {
        //        Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
        //        Expires = DateTime.Now.AddDays(7),
        //        Created = DateTime.Now
        //    };

        //    return refreshToken;
        //}



        //private string CreateToken(UserInfo userInfo)
        //{
        //    List<Claim> claims = new List<Claim>()
        //    {
        //        new Claim(ClaimTypes.Name,userInfo.Username),
        //           new Claim(ClaimTypes.Role,"Admin")
        //    };

        //    var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));

        //    var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        //    var token = new JwtSecurityToken(
        //        claims: claims,
        //        expires: DateTime.Now.AddDays(1),
        //        signingCredentials: cred
        //        );

        //    var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        //    return jwt;
        //}

        //private void CreatePaasswordhash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        //{
        //    using (var hmsc = new HMACSHA512())
        //    {
        //        passwordSalt = hmsc.Key;
        //        passwordHash = hmsc.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        //    }
        //}



        //[HttpGet, Authorize]
        //public ActionResult<string> GetMe()
        //{

        //    var userName = _userService.GetMyName();

        //    //var userName = User?.Identity?.Name;
        //    //var userName2 = User.FindFirstValue(ClaimTypes.Name);
        //    //var role = User.FindFirstValue(ClaimTypes.Role);

        //    return Ok(userName);

        //}

        //V1
        //[HttpPost("register")]
        //public async Task<ActionResult<UserInfo>> Register(UserInfoDTO request)
        //{
        //    CreatePaasswordhash(request.Password, out byte[] passwordHas, out byte[] passwordSalt);

        //    userInfo.Username = request.Username;
        //    userInfo.PasswordHash = passwordHas;
        //    userInfo.PasswordSalt = passwordSalt;

        //    return Ok(userInfo);
        //}

        //V1
        //[HttpPost("login")]
        //public async Task<ActionResult<string>> Login(UserInfoDTO request)
        //{
        //    if (userInfo.Username != request.Username)
        //    {
        //        return BadRequest("User Not Found!");
        //    }

        //    if (!VerifyPasswordHash(request.Password, userInfo.PasswordHash, userInfo.PasswordSalt))
        //    {
        //        return BadRequest("Wrong Password");
        //    }

        //    string token = CreateToken(userInfo);


        //    var refreshToken = GenerateRefreshToken();
        //    SetRefreshToken(refreshToken);

        //    return Ok(token);
        //}

        //V1
        //private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        //{
        //    using (var hmac = new HMACSHA512(passwordSalt))
        //    {
        //        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        //        return computedHash.SequenceEqual(passwordHash);
        //    }
        //}


        //Old Authentication Method - It Works

        //[HttpPost]
        //public async Task<IActionResult> Post(User _userData)
        //{
        //    if (_userData != null && _userData.Email != null && _userData.Password != null)
        //    {
        //        var user = await GetUser(_userData.Email, _userData.Password);


        //        //create claims details based on the user information
        //        if (user != null)
        //        {
        //            var claims = new[]
        //            {
        //                         new Claim(JwtRegisteredClaimNames.Sub, _config["Jwt:Subject"]),
        //                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
        //                        new Claim("UserId", user.UserId.ToString()),
        //                        new Claim("DisplayName", user.DisplayName),
        //                        new Claim("UserName", user.UserName),
        //                        new Claim("Email", user.Email)
        //                    };

        //            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        //            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //            var token = new JwtSecurityToken(
        //                _config["Jwt:Issuer"],
        //                _config["Jwt:Audience"],
        //                claims,
        //                expires: DateTime.UtcNow.AddMinutes(1),
        //                signingCredentials: signIn);

        //            return Ok(new JwtSecurityTokenHandler().WriteToken(token));

        //        }
        //        else
        //        {
        //            return BadRequest("Invalid credentials");
        //        }
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}
        ////List<User> hardcodedUsers = new List<User>
        ////{
        ////    new User
        ////    {
        ////        UserId = 1,
        ////        DisplayName = "John Doe",
        ////        UserName = "johndoe",
        ////        Email = "john@example.com",
        ////        Password = "password123"

        ////    },
        ////    new User
        ////    {
        ////        UserId = 2,
        ////        DisplayName = "Alice Smith",
        ////        UserName = "alicesmith",
        ////        Email = "alice@example.com",
        ////        Password = "alicepassword"
        ////    },
        ////    // Add more users as needed
        ////};
        //private async Task<User> GetUser(string email, string password)
        //{
        //    // Assuming hardcodedUsers is the list containing your hardcoded data
        //    var user = hardcodedUsers.FirstOrDefault(u => u.Email == email && u.Password == password);
        //    return await Task.FromResult(user);
        //}
    }
}
