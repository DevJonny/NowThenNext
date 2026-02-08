using System.Text.Json;
using Microsoft.JSInterop;
using NowThenNext.Models;

namespace NowThenNext.Services;

public class LearningCardsDataService : ILearningCardsDataService
{
    private readonly IJSRuntime _jsRuntime;
    private const string StorageKey = "learning-cards";

    public LearningCardsDataService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<List<LearningCategory>> GetAllCategoriesAsync()
    {
        var builtInCategories = GetBuiltInCategories();
        var customData = await GetCustomDataAsync();
        var customCategories = customData.Categories
            .OrderBy(c => c.CreatedAt)
            .ToList();

        var result = new List<LearningCategory>(builtInCategories.Count + customCategories.Count);
        result.AddRange(builtInCategories);
        result.AddRange(customCategories);
        return result;
    }

    public async Task<List<LearningCard>> GetCardsByCategoryAsync(string categoryId)
    {
        // Check built-in categories first
        var builtInCards = GetBuiltInCards(categoryId);
        if (builtInCards != null)
            return builtInCards;

        // Check custom categories
        var customData = await GetCustomDataAsync();
        return customData.Cards
            .Where(c => c.CategoryId == categoryId)
            .ToList();
    }

    public async Task AddCustomCategoryAsync(LearningCategory category)
    {
        if (category.IsBuiltIn)
            return;

        var customData = await GetCustomDataAsync();
        customData.Categories.Add(category);
        await SaveCustomDataAsync(customData);
    }

    public async Task AddCustomCardAsync(LearningCard card)
    {
        if (card.IsBuiltIn)
            return;

        var customData = await GetCustomDataAsync();
        customData.Cards.Add(card);
        await SaveCustomDataAsync(customData);
    }

    public async Task DeleteCustomCardAsync(string cardId)
    {
        var customData = await GetCustomDataAsync();
        customData.Cards.RemoveAll(c => c.Id == cardId);
        await SaveCustomDataAsync(customData);
    }

    public async Task DeleteCustomCategoryAsync(string categoryId)
    {
        // Check that it's not a built-in category
        var builtInIds = GetBuiltInCategories().Select(c => c.Id).ToHashSet();
        if (builtInIds.Contains(categoryId))
            return;

        var customData = await GetCustomDataAsync();
        customData.Categories.RemoveAll(c => c.Id == categoryId);
        customData.Cards.RemoveAll(c => c.CategoryId == categoryId);
        await SaveCustomDataAsync(customData);
    }

    private static List<LearningCategory> GetBuiltInCategories() =>
    [
        LearningCardsBuiltInData.GetShapesCategory(),
        LearningCardsBuiltInData.GetColoursCategory(),
        LearningCardsBuiltInData.GetAnimalsCategory(),
        LearningCardsBuiltInData.GetDinosaursCategory()
    ];

    private static List<LearningCard>? GetBuiltInCards(string categoryId) => categoryId switch
    {
        "shapes" => LearningCardsBuiltInData.GetShapesCards(),
        "colours" => LearningCardsBuiltInData.GetColoursCards(),
        "animals" => LearningCardsBuiltInData.GetAnimalsCards(),
        "dinosaurs" => LearningCardsBuiltInData.GetDinosaursCards(),
        _ => null
    };

    private async Task<CustomLearningData> GetCustomDataAsync()
    {
        try
        {
            var json = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", StorageKey);
            if (string.IsNullOrEmpty(json))
                return new CustomLearningData();

            return JsonSerializer.Deserialize<CustomLearningData>(json) ?? new CustomLearningData();
        }
        catch
        {
            return new CustomLearningData();
        }
    }

    private async Task SaveCustomDataAsync(CustomLearningData data)
    {
        var json = JsonSerializer.Serialize(data);
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
        }
        catch (JSException ex) when (ex.Message.Contains("QuotaExceededError") ||
                                      ex.Message.Contains("quota", StringComparison.OrdinalIgnoreCase))
        {
            throw new StorageQuotaExceededException("Storage quota exceeded. Please delete some data to free up space.", ex);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to save learning cards data: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Internal data structure for persisting custom categories and cards to localStorage.
    /// Only custom (user-created) items are stored; built-in data is never persisted.
    /// </summary>
    private class CustomLearningData
    {
        public List<LearningCategory> Categories { get; set; } = [];
        public List<LearningCard> Cards { get; set; } = [];
    }
}
