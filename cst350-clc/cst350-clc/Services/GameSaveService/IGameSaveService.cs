using cst350_clc.Models.GameSave;

namespace cst350_clc.Services.GameSaveService
{
    public interface IGameSaveService
    {
        Task<IEnumerable<GameSaveModel>> GetAllGameSaves();
        Task<GameSaveModel> GetGameSaveById(int gameSaveId);
        Task<IEnumerable<GameSaveModel>> GetGameSavesByUserId(int userId);
        Task<int> AddGameSave(GameSaveModel gameSave);
        Task UpdateGameSave(GameSaveModel gameSave);
        Task DeleteGameSave(int gameSaveId);
    }
}
