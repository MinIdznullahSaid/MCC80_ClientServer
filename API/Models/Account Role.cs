namespace API.Models;

public class AccountRole
{
    public Guid Guid { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public Guid AccountId { get; set; }
    public Guid RoleId { get; set; }

}

