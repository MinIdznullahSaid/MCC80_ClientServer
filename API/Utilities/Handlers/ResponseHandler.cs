namespace API.Utilities.Handlers;

public class ResponseHandler<Entity>
{
    public int Code { get; set; }
    public string Status { get; set; }
    public string Message { get; set; }
    public Entity? Data { get; set; }
}
