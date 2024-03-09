using classroomLibrary.Domain.Response;
using classroomLibrary.ViewModels.Account;
using System.Security.Claims;

namespace classroomLibrary.Data.Interfaces
{
    public interface IAccountService
    {
            public Task<BaseResponse<ClaimsIdentity>> Register(RegisterViewModel model);

            public Task<BaseResponse<ClaimsIdentity>> Login(LoginViewModel model);

            public Task<BaseResponse<bool>> ChangePassword(ChangePasswordViewModel model);
    }
}
