using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos.Session;

public class SessionVoteDto
{
    [Required]
    public int Point { get; set; }
}