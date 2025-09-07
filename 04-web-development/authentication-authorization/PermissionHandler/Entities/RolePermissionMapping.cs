namespace Entities;

public class RolePermissionsMapping
{
    public int Id { get; set; }
    public int RoleId { get; set; }
    public required string Permission { get; set; }
    public virtual Roles? Role { get; set; }
}