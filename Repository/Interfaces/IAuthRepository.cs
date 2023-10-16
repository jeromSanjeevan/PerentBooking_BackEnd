using ParentBookingAPI.Model;
using System.Threading.Tasks;

namespace ParentBookingAPI.Repository.Interfaces
{
    public interface IAuthRepository
    {
        Task<int> InsertUserInfoAsync(UserInfoDTO userInfo);

        Task<UserInfo> GetUserInfoByUsernameAsync(string username);
        Task<UserInfo> GetUserInfoByTokenAsync(string token);
        Task<int> StoreRefreshTokenAsync(RefreshToken refreshToken, int userId);
        string CreateToken(UserInfo userInfo);

    }
}
