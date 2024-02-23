using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityService.Services
{
    public class CustomProfileService(UserManager<ApplicationUser> userManager) : IProfileService
    {
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await userManager.GetUserAsync(context.Subject);
            if (user is null || user.UserName is null) { return; }

            var existingClaims = await userManager.GetClaimsAsync(user);
            if (existingClaims is null) { return; }

            var claims = new List<Claim> { new Claim("username", user.UserName) };
            context.IssuedClaims.AddRange(claims);

            var nameClaim = existingClaims.FirstOrDefault(x => x.Type == JwtClaimTypes.Name);
            if (nameClaim is null) { return; }

            context.IssuedClaims.Add(nameClaim);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.CompletedTask;
        }
    }
}
