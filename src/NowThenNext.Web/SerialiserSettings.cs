using System.Text.Json;

namespace NowThenNext.Web;

public class SerialiserSettings
{
    public static readonly JsonSerializerOptions Standard = new() 
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };
}