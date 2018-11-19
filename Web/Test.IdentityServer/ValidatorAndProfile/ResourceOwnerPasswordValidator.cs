using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Test.Service.Dto;
using Test.Service.Interface;

namespace Test.IdentityServer.ValidatorAndProfile
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private IUserSvc _userSvc { get; set; }

        public ResourceOwnerPasswordValidator(IUserSvc userSvc)
        {
            _userSvc = userSvc;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var loginDto = new LoginDto()
            {
                Password = context.Password,
                UserName = context.UserName,
                ClientId = context.Request.Client.ClientId
            };
            var result = await _userSvc.LoginAsync(loginDto);
            if (result.ActionResult)
            {
                var data = result.Data;
                var claims = new Claim[]
                {
                    new Claim("UserId",data.Id.ToString()),
                    new Claim(JwtClaimTypes.Name,data.Name),
                    new Claim(ClaimTypes.NameIdentifier,data.Id.ToString())
                };
                context.Result = new GrantValidationResult(subject: context.UserName, authenticationMethod: "custom", claims: claims);
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, result.Message);
            }
        }
    }
}
