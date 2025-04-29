using Core.Models;
using Core.Services;
using Database;

namespace Services;

public class SessionService : ISessionService
{
    private readonly IRepository<string, Session> _sessions;
    
    public SessionService(IRepository<string, Session> sessions)
    {
        _sessions = sessions;
    }

    public Session CreateSession(string owner, ISet<int> points)
    {
        if (points.Count < 1)
        {
            throw new InvalidPointsException(points);
        }

        if (points.Any(point => point <= 0))
        {
            throw new InvalidPointsException(points);
        }

        var ret = new Session
        {
            Owner = owner,
            Points = points,
            Revealed = false,
            Votes = new Dictionary<string, int>()
        };
        ret.Votes[owner] = -1;
        _sessions.Put(ret);
        return ret;
    }

    public Session? GetSession(string id)
    {
        return _sessions.Find(id);
    }

    public void Join(string id, string user)
    {
        var found = _sessions.Find(id);
        if (found == null)
        {
            throw new NoSuchSessionException();
        }

        if (found.Owner == user)
        {
            return;
        }

        found.Votes.TryAdd(user, -1);
        
        _sessions.Update(found);
    }

    public SessionStatus GetSessionStatus(string id)
    {
        var found = _sessions.Find(id);
        if (found == null)
        {
            throw new NoSuchSessionException();
        }

        if (found.Revealed)
        {
            return new SessionStatus
            {
                Revealed = found.Revealed,
                Points = found.Votes
            };
        }

        var votes = found.Votes;

        foreach (var key in votes.Keys)
        {
            if (votes[key] != -1)
            {
                votes[key] = 0;
            }
        }

        return new SessionStatus
        {
            Revealed = false,
            Points = votes
        };
    }

    public SessionData CalculateSessionData(string id)
    {
        var found = _sessions.Find(id);
        if (found == null)
        {
            throw new NoSuchSessionException();
        }

        if (!found.Revealed)
        {
            throw new NotRevealedSessionException();
        }

        var values = found.Votes.Values.Where(v => v > 0).ToList();
        return new SessionData
        {
            VotesCount = values.Count,
            AveragePoints = values.Average(),
            MinPoint = values.Min(),
            MaxPoint = values.Max(),
        };
    }

    public void Vote(string id, string user, int point)
    {
        var found = _sessions.Find(id);
        if (found == null)
        {
            throw new NoSuchSessionException();
        }

        if (!found.Votes.ContainsKey(user))
        {
            throw new NoSuchUserInSessionException();
        }

        if (found.Revealed)
        {
            throw new RevealedSessionException();
        }

        if (!found.Points.Contains(point))
        {
            throw new InvalidPointException(point, found.Points);
        }
        
        found.Votes[user] = point;

        if (found.Votes.Values.All(v => v != -1))
        {
            found.Revealed = true;
        }
        
        _sessions.Update(found);
    }

    public void Reveal(string id, string user)
    {
        var found = _sessions.Find(id);
        if (found == null)
        {
            throw new NoSuchSessionException();
        }

        if (found.Owner != user)
        {
            throw new UserNotSessionOwnerException();
        }

        if (found.Revealed)
        {
            return;
        }

        if (found.Votes.Values.All(v => v == -1))
        {
            throw new EmptySessionException();
        }
        
        found.Revealed = true;
        
        _sessions.Update(found);
    }
}