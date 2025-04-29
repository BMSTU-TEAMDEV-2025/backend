using System.Security.Claims;
using Backend.Dtos.Session;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
public class SessionController : ControllerBase
{
    
    private readonly ISessionService _sessions;
    private readonly ILogger<SessionController> _logger;

    public SessionController(ISessionService sessions, ILogger<SessionController> logger)
    {
        _sessions = sessions;
        _logger = logger;
    }

    private string GetUserId()
    {
        return User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
    }
    
    [HttpGet("/sessions/{id}")]
    public object? Get(string id)
    {
        try
        {
            var found = _sessions.GetSession(id);
            if (found != null) return SessionGetDto.FromSession(found);
            Response.StatusCode = 404;
            return null;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при получении данных сессии");
            Response.StatusCode = 500;
            return new SessionErrorDto
            {
                Error = e.Message
            };
        }
    }

    [HttpGet("/sessions/{id}/status")]
    public object GetStatus(string id)
    {
        try
        {
            return _sessions.GetSessionStatus(id);
        }
        catch (SessionException e)
        {
            Response.StatusCode = e.StatusCode;
            return new SessionErrorDto
            {
                Error = e.Message
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при получении статуса сессии");
            Response.StatusCode = 500;
            return new SessionErrorDto
            {
                Error = e.Message
            };
        }
    }
    
    [HttpGet("/sessions/{id}/data")]
    public object? GetData(string id)
    {
        try
        {
            return _sessions.CalculateSessionData(id);
        }
        catch (SessionException e)
        {
            Response.StatusCode = e.StatusCode;
            return new SessionErrorDto
            {
                Error = e.Message
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при получении статуса сессии");
            Response.StatusCode = 500;
            return new SessionErrorDto
            {
                Error = e.Message
            };
        }
    }
    
    [HttpPost("/sessions")]
    [Authorize]
    public object Post([FromBody] PostSessionDto dto)
    {
        var userId = GetUserId();
        try
        {
            var session = _sessions.CreateSession(userId, dto.Points);
            return new SessionIdDto
            {
                SessionId = session.Id!
            };
        }
        catch (SessionException e)
        {
            Response.StatusCode = e.StatusCode;
            return new SessionErrorDto
            {
                Error = e.Message
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при создании сессии");
            Response.StatusCode = 500;
            return new SessionErrorDto
            {
                Error = e.Message
            };
        }
    }

    [HttpPost("/sessions/{id}/join")]
    [Authorize]
    public SessionErrorDto? Join(string id)
    {
        var userId = GetUserId();
        try
        {
            _sessions.Join(id, userId);
            return null;
        }
        catch (SessionException e)
        {
            Response.StatusCode = e.StatusCode;
            return new SessionErrorDto
            {
                Error = e.Message
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при присоединии к сессии");
            Response.StatusCode = 500;
            return new SessionErrorDto
            {
                Error = e.Message
            };
        }
    }

    [HttpPost("/sessions/{id}/vote")]
    [Authorize]
    public SessionErrorDto? Vote(string id, [FromBody] SessionVoteDto dto)
    {
        var userId = GetUserId();
        try
        {
            _sessions.Vote(id, userId, dto.Point);
            return null;
        }
        catch (SessionException e)
        {
            Response.StatusCode = e.StatusCode;
            return new SessionErrorDto
            {
                Error = e.Message
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при голосовании в сессии");
            Response.StatusCode = 500;
            return new SessionErrorDto
            {
                Error = e.Message
            };
        }
    }

    [HttpPost("/sessions/{id}/reveal")]
    [Authorize]
    public SessionErrorDto? Reveal(string id)
    {
        var userId = GetUserId();
        try
        {
            _sessions.Reveal(id, userId);
            return null;
        }
        catch (SessionException e)
        {
            Response.StatusCode = e.StatusCode;
            return new SessionErrorDto
            {
                Error = e.Message
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при раскрытии сессии");
            Response.StatusCode = 500;
            return new SessionErrorDto
            {
                Error = e.Message
            };
        }
    }
}
