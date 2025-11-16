using cst350_clc.Models.GameSave;
using MineSweeper;
using MySql.Data.MySqlClient;

namespace cst350_clc.Services.GameSaveService
{
    public class GameSaveDAO : IGameSaveDAO
    {
        private const string CONNECTION_STRING = "datasource=localhost;port=8889;uid=root;pwd=root;database=minesweeperapp;";

        public async Task<int> AddGameSave(GameSaveModel gameSave)
        {
            using (MySqlConnection conn = new MySqlConnection(CONNECTION_STRING))
            {
                string sql = "INSERT INTO `games` (userId, dateSaved, saveData) VALUES (@userId, @dateSaved, @saveData)";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", gameSave.UserId);
                    cmd.Parameters.AddWithValue("@dateSaved", gameSave.DateSaved);
                    cmd.Parameters.AddWithValue("@saveData", gameSave.SaveData);

                    // attempt execution
                    conn.Open();
                    await cmd.ExecuteNonQueryAsync();
                    return Convert.ToInt32(cmd.LastInsertedId);
                }
            }
        }

        public async Task DeleteGameSave(int gameSaveId)
        {
            using (MySqlConnection conn = new MySqlConnection(CONNECTION_STRING))
            {
                string sql = "DELETE FROM `games` WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", gameSaveId);

                conn.Open();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteGameSave(GameSaveModel gameSave)
        {
            await DeleteGameSave(gameSave.Id);
        }

        public async Task<IEnumerable<GameSaveModel>> GetAllGameSaves()
        {
            List<GameSaveModel> saves = new List<GameSaveModel>();
            using (MySqlConnection conn = new MySqlConnection(CONNECTION_STRING))
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM `games`", conn);
                conn.Open();
                MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                while (reader.Read()) 
                { 
                    saves.Add(new GameSaveModel
                    {
                        Id = reader.GetInt32("id"),
                        UserId = reader.GetInt32("userId"),
                        DateSaved = reader.GetDateTime("dateSaved"),
                        SaveData = reader.GetString("saveData")
                    });
                }
                reader.Close();
                return saves;
            }
        }

        public async Task<GameSaveModel> GetGameSaveById(int gameSaveId)
        {
            GameSaveModel model = null;
            using (MySqlConnection conn = new MySqlConnection(CONNECTION_STRING))
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM `games` WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", gameSaveId);

                conn.Open();
                MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();
                if (reader.Read())
				{
					model = new GameSaveModel
					{
						Id = reader.GetInt32("id"),
						UserId = reader.GetInt32("userId"),
						DateSaved = reader.GetDateTime("dateSaved"),
						SaveData = reader.GetString("saveData")
					};
				}
                reader.Close();
                return model;
			}
        }

        public async Task<IEnumerable<GameSaveModel>> GetGameSavesByUserId(int userId)
        {
            GameSaveModel model = null;
            List<GameSaveModel> gameSaves = new List<GameSaveModel>();
            using (MySqlConnection conn = new MySqlConnection(CONNECTION_STRING))
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM `games` WHERE userId = @userId", conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                conn.Open();
                MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();
                while (reader.Read())
                {
                    gameSaves.Add(new GameSaveModel
                    {
                        Id = reader.GetInt32("id"),
                        UserId = reader.GetInt32("userId"),
                        DateSaved = reader.GetDateTime("dateSaved"),
                        SaveData = reader.GetString("saveData")
                    });
                }
                reader.Close();
                return gameSaves;
            }
        }

        public async Task UpdateGameSave(GameSaveModel gameSave)
        {
            using (MySqlConnection conn = new MySqlConnection(CONNECTION_STRING))
            {
                MySqlCommand cmd = new MySqlCommand("UPDATE `games` SET userId = @userId, dateSaved = @dateSaved, saveData = @saveData WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", gameSave.Id);
                cmd.Parameters.AddWithValue("@userId", gameSave.UserId);
                cmd.Parameters.AddWithValue("@dateSaved", gameSave.DateSaved);
                cmd.Parameters.AddWithValue("@saveData", gameSave.SaveData);
                conn.Open();
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
