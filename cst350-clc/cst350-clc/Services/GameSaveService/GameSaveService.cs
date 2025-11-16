using cst350_clc.Models.GameSave;

namespace cst350_clc.Services.GameSaveService
{
    public class GameSaveService : IGameSaveService
    {
        private readonly IGameSaveDAO _gameSaveDAO;

        public GameSaveService(IGameSaveDAO gameSaveDAO)
		{
			_gameSaveDAO = gameSaveDAO;
		}

        public async Task<int> AddGameSave(GameSaveModel gameSave)
        {
            return await _gameSaveDAO.AddGameSave(gameSave);
        }

        public async Task DeleteGameSave(int gameSaveId)
        {
            var saveModel = await _gameSaveDAO.GetGameSaveById(gameSaveId);
            if (saveModel != null)
			    await _gameSaveDAO.DeleteGameSave(gameSaveId);
        }

        public async Task<IEnumerable<GameSaveModel>> GetAllGameSaves()
        {
            return await _gameSaveDAO.GetAllGameSaves();
        }

        public async Task<GameSaveModel> GetGameSaveById(int gameSaveId)
        {
            return await _gameSaveDAO.GetGameSaveById(gameSaveId);
        }

        public async Task<IEnumerable<GameSaveModel>> GetGameSavesByUserId(int userId)
        {
            return await _gameSaveDAO.GetGameSavesByUserId(userId);
        }

        public async Task UpdateGameSave(GameSaveModel gameSave)
        {
            await _gameSaveDAO.UpdateGameSave(gameSave);
        }
    }
}
