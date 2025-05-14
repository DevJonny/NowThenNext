using System.Text.Json.Serialization;

namespace NowThenNext.Web.Features.FoodList;

public class Food
{
    public Food() { }

    public Food(string name)
    {
        Name = name;
    }
    
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    
    public string? ImageData { get; set; }
    
    [JsonIgnore]
    public bool Selected { get; set; }
}