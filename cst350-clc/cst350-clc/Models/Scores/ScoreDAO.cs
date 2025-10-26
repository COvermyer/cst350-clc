// cst350-clc/Models/Scores/ScoreDAO.cs
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace cst350_clc.Models.Scores
{
   
    public class ScoreDAO
    {
        
        private const string CONNECTION_STRING =
            "datasource=localhost;port=8889;uid=root;pwd=root;database=minesweeperapp;";

        public void EnsureSchema()
        {
            using var conn = new MySqlConnection(CONNECTION_STRING);
            conn.Open();
            var sql = @"
                CREATE TABLE IF NOT EXISTS scores (
                    id INT AUTO_INCREMENT PRIMARY KEY,
                    username VARCHAR(255) NULL,
                    difficulty TINYINT NOT NULL,
                    score INT NOT NULL,
                    time_taken INT NOT NULL,
                    played_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
                );
                CREATE INDEX IF NOT EXISTS idx_scores_user
                    ON scores(username, difficulty, score DESC, time_taken ASC, played_at DESC);
                CREATE INDEX IF NOT EXISTS idx_scores_top
                    ON scores(score DESC, time_taken ASC, played_at DESC);
            ";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        public int Insert(ScoreModel score)
        {
            using var conn = new MySqlConnection(CONNECTION_STRING);
            conn.Open();
            const string sql = @"
                INSERT INTO scores (username, difficulty, score, time_taken, played_at)
                VALUES (@u, @d, @s, @t, @p);
                SELECT LAST_INSERT_ID();";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@u", string.IsNullOrWhiteSpace(score.Username) ? (object)DBNull.Value : score.Username);
            cmd.Parameters.AddWithValue("@d", score.Difficulty);
            cmd.Parameters.AddWithValue("@s", score.Score);
            cmd.Parameters.AddWithValue("@t", score.TimeTaken);
            cmd.Parameters.AddWithValue("@p", score.PlayedAt);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public ScoreModel? GetPersonalBest(string username, int difficulty)
        {
            using var conn = new MySqlConnection(CONNECTION_STRING);
            conn.Open();
            const string sql = @"
                SELECT id, username, difficulty, score, time_taken, played_at
                FROM scores
                WHERE ((@u IS NULL AND username IS NULL) OR username=@u)
                  AND difficulty=@d
                ORDER BY score DESC, time_taken ASC, played_at DESC
                LIMIT 1;";
            using var cmd = new MySqlCommand(sql, conn);
            object userParam = string.IsNullOrWhiteSpace(username) ? DBNull.Value : username;
            cmd.Parameters.AddWithValue("@u", userParam);
            cmd.Parameters.AddWithValue("@d", difficulty);
            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;
            return new ScoreModel
            {
                Id = r.GetInt32("id"),
                Username = r.IsDBNull("username") ? "" : r.GetString("username"),
                Difficulty = r.GetInt32("difficulty"),
                Score = r.GetInt32("score"),
                TimeTaken = r.GetInt32("time_taken"),
                PlayedAt = r.GetDateTime("played_at")
            };
        }

        public IEnumerable<ScoreModel> Top(int take = 10)
        {
            using var conn = new MySqlConnection(CONNECTION_STRING);
            conn.Open();
            using var cmd = new MySqlCommand($@"
                SELECT id, username, difficulty, score, time_taken, played_at
                FROM scores
                ORDER BY score DESC, time_taken ASC, played_at DESC
                LIMIT {take};", conn);
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                yield return new ScoreModel
                {
                    Id = r.GetInt32("id"),
                    Username = r.IsDBNull("username") ? "" : r.GetString("username"),
                    Difficulty = r.GetInt32("difficulty"),
                    Score = r.GetInt32("score"),
                    TimeTaken = r.GetInt32("time_taken"),
                    PlayedAt = r.GetDateTime("played_at")
                };
            }
        }
    }
}
