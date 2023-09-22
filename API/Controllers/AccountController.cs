using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseAPIController
    {
        public UserManager<AppUser> userManager { get; }
        public SignInManager<AppUser> signInManager { get;}
        public ITokenService tokenService { get; }
        public IMapper mapper { get; set; }
        public AppIdentityDbContext identityContext { get; set; }

        public AccountController(UserManager<AppUser> _userManager, SignInManager<AppUser> 
            _signInManager, ITokenService _tokenService, IMapper _mapper, 
            AppIdentityDbContext _identityContext)
        {
            this.tokenService = _tokenService;
            this.userManager = _userManager;
            this.signInManager = _signInManager;
            this.mapper = _mapper;
            this.identityContext = _identityContext;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var user = await userManager.FindByEmailFromClaimsPrincipal(User);

            return new UserDTO
            {
                Email = user.Email,
                Token = tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };
        }

        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await userManager.FindByEmailAsync(email) != null;
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDTO>> GetUserAddress()
        {
            var user = await userManager.FindUserByClaimsPrincipalWithAddress(HttpContext.User);

            return mapper.Map<Address, AddressDTO>(user.Address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDTO>> UpdateUserAddress(AddressDTO address)
        {
            var user = await userManager.FindUserByClaimsPrincipalWithAddress(HttpContext.User);

            // Update the user's address
            user.Address = mapper.Map<AddressDTO, Address>(address);

            var result = await userManager.UpdateAsync(user);

            if(result.Succeeded) return Ok(mapper.Map<Address, AddressDTO>(user.Address));

            return BadRequest("Problem updating the user");

            // try
            // {
            //     // Save changes to the database using identity context
            //     await identityContext.SaveChangesAsync();
            // }
            // catch (Exception ex)
            // {
            //     return StatusCode(500, "An error occurred while saving the address.");
            // }
            
            // return mapper.Map<Address, AddressDTO>(user.Address);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO )
        {
            var user = await userManager.FindByEmailAsync(loginDTO.Email);

            if(user == null) return Unauthorized(new ApiResponse(401));

            //Para evitar brute force é settar para true
            var result = await signInManager.CheckPasswordSignInAsync(user, 
                loginDTO.Password, false);
                
            if(!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return new UserDTO
            {
                Email = loginDTO.Email,
                Token = tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };
        } 

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {            
            //Check if email already exists, if it does, return bad request
            if(CheckEmailExistsAsync(registerDTO.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse{
                    Errors = new []{"Email already exists"}
                });
            }

            var user = new AppUser
            {
                DisplayName = registerDTO.DisplayName,
                Email = registerDTO.Email,
                UserName = registerDTO.Email
            };

            var result = await userManager.CreateAsync(user, registerDTO.Password);

            //Check if user was created, if not return bad request
            if(!result.Succeeded) return BadRequest(new ApiResponse(400));

            return new UserDTO
            {
                DisplayName = user.DisplayName,
                Token = tokenService.CreateToken(user),
                Email = user.Email
            };
        }
    }
}