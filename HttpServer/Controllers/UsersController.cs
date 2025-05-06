using Backend.Dtos;
using Core.Models;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("/")]
public class UsersController : ControllerBase
{
    private readonly IUserService _usersService;
    
    public UsersController(IUserService usersService)
    {
        _usersService = usersService;
    }
    
    [HttpGet("/users/{id}")]
    public GetUserDto? GetUsers(string id)
    {
        var found = _usersService.GetUser(id);
        if (found != null) return GetUserDto.Of(found);
        Response.StatusCode = 404;
        return null;
    }
    
    [HttpPost("/users")]
    public IdDto PostUser(PostUserDto dto)
    {
        return new IdDto
        {
            Id = _usersService.PutUser(dto.Email, dto.Password)
        };
    }
}