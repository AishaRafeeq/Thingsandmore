public class ManageUserViewModel
{
    public required string Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public IList<string> Roles { get; set; } = new List<string>();
}