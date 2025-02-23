using Backend.Models;

namespace Backend.Dtos;

public class HelloDto
{
    public string Message { get; set; }

    public static HelloDto Of(HelloModel model)
    {
        return new HelloDto
        {
            Message = model.Message
        };
    }
}
