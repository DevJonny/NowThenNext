namespace NowThenNext.Web.Models;

public class Place
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Group { get; set; }
}