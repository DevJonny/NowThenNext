using Microsoft.AspNetCore.Components;
using NowThenNext.Models;

namespace NowThenNext.Components;

public partial class ImageTile
{
    [Parameter, EditorRequired]
    public ImageItem Image { get; set; } = default!;

    [Parameter]
    public EventCallback<string> OnFavoriteToggle { get; set; }

    [Parameter]
    public EventCallback<string> OnDelete { get; set; }

    private string GetImageSrc()
    {
        // Handle both data URL format and raw base64
        if (Image.Base64Data.StartsWith("data:"))
        {
            return Image.Base64Data;
        }
        // Default to JPEG if raw base64
        return $"data:image/jpeg;base64,{Image.Base64Data}";
    }

    private async Task OnFavoriteClicked()
    {
        if (OnFavoriteToggle.HasDelegate)
        {
            await OnFavoriteToggle.InvokeAsync(Image.Id);
        }
    }

    private async Task OnDeleteClicked()
    {
        if (OnDelete.HasDelegate)
        {
            await OnDelete.InvokeAsync(Image.Id);
        }
    }
}
