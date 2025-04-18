namespace NowThenNext.Web.Features.PlacesList;

public record Place(string Name, string Description)
{
    public IReadOnlyCollection<Place>? Places { get; init; } = [];
};