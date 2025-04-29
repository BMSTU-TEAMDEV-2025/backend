namespace Backend.Dtos.Session;

public class SessionGetDto
{
    public string Id { get; set; }
    public string Owner { get; set; }
    public ISet<int> Points { get; set; }
    public bool Revealed { get; set; }

    public static SessionGetDto FromSession(Core.Models.Session session)
    {
        return new SessionGetDto
        {
            Id = session.Id!,
            Owner = session.Owner!,
            Points = session.Points,
        };
    }
}