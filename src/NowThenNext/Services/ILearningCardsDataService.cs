using NowThenNext.Models;

namespace NowThenNext.Services;

public interface ILearningCardsDataService
{
    Task<List<LearningCategory>> GetAllCategoriesAsync();
    Task<List<LearningCard>> GetCardsByCategoryAsync(string categoryId);
    Task AddCustomCategoryAsync(LearningCategory category);
    Task AddCustomCardAsync(LearningCard card);
    Task DeleteCustomCardAsync(string cardId);
    Task DeleteCustomCategoryAsync(string categoryId);
    Task<string?> GetRawCustomDataJsonAsync();
    Task SetRawCustomDataJsonAsync(string json);
    Task ClearCustomDataAsync();
}
