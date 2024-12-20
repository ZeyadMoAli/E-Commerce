﻿using System.Security.Claims;
using API.Dtos;
using API.Extensions;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AccountController:BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,ITokenService tokenService , IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if(user==null)
            return Unauthorized();
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if(!result.Succeeded)
            return Unauthorized();

        return new UserDto
        {
            Email = user.Email,
            Token = _tokenService.CreateToken(user) ,
            DisplayName = user.UserName,
        };

    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (EmailExists(registerDto.Email).Result.Value)
        {
            return BadRequest("Email already exists");
        }
        var user = new AppUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            UserName = registerDto.Email,
        };
        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if(!result.Succeeded)
            return BadRequest();
        return new UserDto
        {
            Email = user.Email,
            Token = _tokenService.CreateToken(user),
            DisplayName = user.UserName,
        };
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        
        var user = await _userManager.FindByEmailFromClaimsPrincipalAsync(User);

        return new UserDto
        {
            Email = user.Email,
            Token = _tokenService.CreateToken(user),
            DisplayName = user.UserName,
        };
    }
    [HttpGet("emailExists")]
    public async Task<ActionResult<bool>> EmailExists(string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }

    [HttpGet("address")][Authorize]
    public async Task<ActionResult<AddressDto>> GetUserAddress()
    {
        var user = await _userManager.FindByUserByClaimsPrincipleWithAddressAsync(User) ;
        return _mapper.Map<Address,AddressDto>(user.Address);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut("address")]
    public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto addressDto)
    {
        var user = await _userManager.FindByUserByClaimsPrincipleWithAddressAsync(User);
        user.Address= _mapper.Map<AddressDto,Address>(addressDto);
        var result = await _userManager.UpdateAsync(user);
        if(result.Succeeded)
            return Ok(_mapper.Map<Address,AddressDto>(user.Address));
        
        return BadRequest("Failed to update address");
    }
    
}