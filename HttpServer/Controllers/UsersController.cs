using System.Security.Claims;
using Backend.Dtos.User;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _usersService;
    
    public UsersController(IUserService usersService)
    {
        _usersService = usersService;
    }

    private string GetUserId()
    {
        return User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
    }

    [HttpGet]
    [Authorize]
    public GetUserDto? GetSelf()
    {
        var id = GetUserId();
        var found = _usersService.GetUser(id);
        if (found != null) return GetUserDto.Of(found);
        Response.StatusCode = 404;
        return null;
    }
    
    [HttpGet("/{id}")]
    public GetUserDto? GetUser(string id)
    {
        var found = _usersService.GetUser(id);
        if (found != null) return GetUserDto.Of(found);
        Response.StatusCode = 404;
        return null;
    }
}