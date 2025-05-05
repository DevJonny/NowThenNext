using System.Text.Json;

namespace NowThenNext.Web.Features.PlacesList;

public class Place
{
    public Place() { }

    public Place(string name, string description, string group)
    {
        Name = name;
        Description = description;
        Group = group;
    }
    
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Group { get; set; }
    
    public string? ImageData { get; set; }
}