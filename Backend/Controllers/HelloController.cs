using Backend.Dtos;
using Backend.Models;
using Database;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Backend.Controllers;

[ApiController]
[Route("/")]
public class HelloController(IRepository<ObjectId, HelloModel> hellos) : ControllerBase
{
    [HttpGet("/hello")]
    public string SayHello()
    {
        return "Hello, World!";
    }

    [HttpGet("/hello/{id}")]
    public HelloDto? GetHello(ObjectId id)
    {
        var found = hellos.Find(id);
        if (found == null)
        {
            Response.StatusCode = 404;
            return null;
        }
        return HelloDto.Of(found);
    }

    [HttpPost("/hello")]
    public IdDto PostHello(HelloDto dto)
    {
        var model = new HelloModel
        {
            Message = dto.Message,
        };
        hellos.Put(model);
        return new IdDto
        {
            Id = model.Id.ToString(),
        };
    }
}
