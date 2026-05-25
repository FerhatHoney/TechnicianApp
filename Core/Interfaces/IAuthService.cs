using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TechnicianApp.Core.Models;

namespace TechnicianApp.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AppUser> LoginWithEmailAsync(string email, string password);
        Task<AppUser> RegisterWithEmailAsync(string fullName, string email, string password);
        Task<AppUser?> LoginWithGoogleAsync();
        Task LogoutAsync();
        Task<AppUser?> GetCurrentUserAsync();
    }
}