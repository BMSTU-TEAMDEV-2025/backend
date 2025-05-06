using Core.Models;

namespace Core.Services;

public interface ISessionService
{
    Session CreateSession(string owner, ISet<int> points);

    Session? GetSession(string id);

    void Join(string id, string user);
    
    SessionStatus GetSessionStatus(string id);

    SessionData CalculateSessionData(string id);

    void Vote(string id, string user, int point);

    void Reveal(string id, string user);
}

public class SessionException(int statusCode, string message) : Exception(message)
{
    public int StatusCode { get; set; } = statusCode;
}

public class NotRevealedSessionException() : SessionException(400, "Session not revealed") { }

public class RevealedSessionException() : SessionException(400, "Session revealed") { }

public class NoSuchSessionException() : SessionException(404, "No such session") { }

public class InvalidPointException(int value, IEnumerable<int> acceptable) 
    : SessionException(400, "Invalid point value: " + value + ", acceptable: " + string.Join(", ", acceptable)) { }

public class InvalidPointsException(IEnumerable<int> points) 
    : SessionException(400, "Invalid points: " + string.Join(", ", points)) { }

public class NoSuchUserInSessionException() : SessionException(400, "No such user in session") {}

public class EmptySessionException() : SessionException(400, "Cannot reveal empty session") {}

public class UserNotSessionOwnerException() : SessionException(403, "Cannot open a session that you do not own") { }

public class SessionStatus
{
    public bool Revealed { get; set; }
    
    public IDictionary<string, int> Points { get; set; }
}

public class SessionData
{
    public int VotesCount { get; set; }
    
    public double AveragePoints { get; set; }
    
    public int MaxPoint { get; set; }
    
    public int MinPoint { get; set; }
}