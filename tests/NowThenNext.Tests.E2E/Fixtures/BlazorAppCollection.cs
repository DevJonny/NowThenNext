namespace NowThenNext.Tests.E2E.Fixtures;

/// <summary>
/// Collection definition for sharing the BlazorAppFixture across test classes.
/// Tests using [Collection("BlazorApp")] will share a single server instance.
/// </summary>
[CollectionDefinition("BlazorApp")]
public class BlazorAppCollection : ICollectionFixture<BlazorAppFixture>
{
}
