using Backend.Dtos;
using Core.Models;
using Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("/")]
public class HelloController : ControllerBase
{
    private readonly IHelloService _hellos;
    
    public HelloController(IHelloService hellos)
    {
        _hellos = hellos;
    }
    
    [HttpGet("/hello")]
    public string SayHello()
    {
        return "Hello, World!";
    }
    
    [HttpGet("/hello/{id}")]
    public HelloDto? GetHello(string id)
    {
        var found = _hellos.GetHello(id);
        if (found != null) return HelloDto.Of(found);
        Response.StatusCode = 404;
        return null;
    }
    
    [HttpPost("/hello")]
    public IdDto PostHello(HelloDto dto)
    {
        return new IdDto
        {
            Id = _hellos.PutHello(new HelloModel
            {
                Message = dto.Message,
            })
        };
    }
}
