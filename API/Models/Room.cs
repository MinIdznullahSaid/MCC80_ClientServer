namespace API.Models;

public class Room
{
    public Guid Guid { get; set; }
    public int floor { get; set; }
    public int capacity { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }

}

