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
                            string characterName = reader.GetString(reader.GetOrdinal("Name"));
                            string discordUserId = reader.GetString(reader.GetOrdinal("DiscordUserId"));

                            Character character = new Character()
                            {
                                Name = characterName,
                                DiscordUserId = discordUserId
                            };
                            characters.Add(character);
                        }
                    }
                }

                await connection.CloseAsync();
            }

            return characters;
        }

        public class HighScoreSQLiteRepositoryOptions
        {
            public required string ConnectionString { get; set; }
        }
    }
}
