using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.Session;

public class PostSessionDto
{
    [Required]
    public ISet<int> Points { get; set; }
}