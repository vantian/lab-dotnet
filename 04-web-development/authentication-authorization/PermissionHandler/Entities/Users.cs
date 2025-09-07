namespace Entities;

public class Users
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int? RoleId { get; set; }

    public virtual Roles? Role { get; set; }
}