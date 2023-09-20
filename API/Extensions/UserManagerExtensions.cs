using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindUserByClaimsPrincipalWithAddress(this 
            UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);

            return await userManager.Users.Include(u=>u.Address)
                .SingleOrDefaultAsync(u => u.Email == email);
        }

        public static async Task<AppUser> FindByEmailFromClaimsPrincipal(this 
            UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            return await userManager.Users.SingleOrDefaultAsync(u=>u.Email == user
                .FindFirstValue(ClaimTypes.Email));
        }
    }
}