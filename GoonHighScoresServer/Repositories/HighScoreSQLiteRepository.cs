using System.Data;
using System.Data.SQLite;
using GoonHighScoresServer.Interfaces;
using GoonHighScoresServer.Models;
using Microsoft.Extensions.Options;

namespace GoonHighScoresServer.Repositories
{
    public class HighScoreSQLiteRepository : IHighScoreRepository
    {
        private readonly HighScoreSQLiteRepositoryOptions _options;

        public HighScoreSQLiteRepository(IOptions<HighScoreSQLiteRepositoryOptions> options)
        {
            _options = options.Value;
        }

        public async Task<List<Character>> GetCharacters()
        {
            List<Character> characters = new List<Character>(10);

            using(SQLiteConnection connection = new SQLiteConnection(_options.ConnectionString))
            {
                await connection.OpenAsync();
                using(SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT * from Character";
                    command.Prepare();

                    using(SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            int id = reader.GetInt32(reader.GetOrdinal("Id"));
                            string characterName = reader.GetString(reader.GetOrdinal("Name"));
                            string discordUserId = reader.GetString(reader.GetOrdinal("DiscordUserId"));

                            Character character = new Character()
                            {
                                Id = id,
                                Name = characterName,
                                DiscordUserId = discordUserId,
                            };
                            characters.Add(character);
                        }
                    }
                }

                await connection.CloseAsync();
            }

            return characters;
        }

        public async Task<int?> GetMostRecentOverallXp(int characterId)
        {
            int? overallXp = null;

            using(SQLiteConnection connection = new SQLiteConnection(_options.ConnectionString))
            {
                await connection.OpenAsync();
                using(SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT MAX(Xp) as MaxXp from XpDrop WHERE CharacterId = @characterId AND SkillId = @skillId";
                    command.Parameters.AddWithValue("@characterId", characterId);
                    command.Parameters.AddWithValue("@skillId", (int)Enums.OsrsSkill.Overall);
                    command.Prepare();

                    object? result = await command.ExecuteScalarAsync();
                    if(result != DBNull.Value)
                        overallXp = Convert.ToInt32(result);
                }

                await connection.CloseAsync();
            }

            return overallXp;
        }

        public async Task<Dictionary<int, int>> GetMostRecentXpDrops(int characterId)
        {
            Dictionary<int, int> mostRecentXpDrops = new Dictionary<int, int>();

            using(SQLiteConnection connection = new SQLiteConnection(_options.ConnectionString))
            {
                await connection.OpenAsync();
                using(SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT MAX(Xp) as MaxXp, SkillId from XpDrop WHERE CharacterId = @characterId Group BY SkillId";
                    command.Parameters.AddWithValue("@characterId", characterId);
                    command.Prepare();

                    using(SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            int xp = reader.GetInt32(reader.GetOrdinal("MaxXp"));
                            int skillId = reader.GetInt32(reader.GetOrdinal("SkillId"));

                            mostRecentXpDrops.Add(skillId, xp);
                        }
                    }
                }

                await connection.CloseAsync();
            }

            return mostRecentXpDrops;
        }

        public async Task SaveXpDrops(List<XpDrop> xpDrops, string processingTime)
        {
            using(SQLiteConnection connection = new SQLiteConnection(_options.ConnectionString))
            {
                await connection.OpenAsync();
                using SQLiteTransaction transaction = connection.BeginTransaction();
                using(SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO XpDrop (CharacterId, SkillId, Xp, Level, Rank, Timestamp) VALUES (@characterId, @skillId, @xp, @level, @rank, @timeStamp)";
                    SQLiteParameter characterIdParameter = command.Parameters.Add("@characterId", DbType.Int32);
                    SQLiteParameter skillIdParameter = command.Parameters.Add("@skillId", DbType.Int32);
                    SQLiteParameter xpParameter = command.Parameters.Add("@xp", DbType.Int32);
                    SQLiteParameter levelParameter = command.Parameters.Add("@level", DbType.Int32);
                    SQLiteParameter rankParameter = command.Parameters.Add("@rank", DbType.Int32);
                    command.Parameters.AddWithValue("@timeStamp", processingTime);

                    foreach (XpDrop xpDrop in xpDrops)
                    {
                        characterIdParameter.Value = xpDrop.CharacterId;
                        skillIdParameter.Value = xpDrop.SkillId;
                        xpParameter.Value = xpDrop.Xp;
                        levelParameter.Value = xpDrop.Level;
                        rankParameter.Value = xpDrop.Rank;

                        command.ExecuteNonQuery();
                    }
                }
                transaction.Commit();

                await connection.CloseAsync();
            }
        }

        public class HighScoreSQLiteRepositoryOptions
        {
            public required string ConnectionString { get; set; }
        }
    }
}
