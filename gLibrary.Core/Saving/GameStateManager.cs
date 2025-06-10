using System.Text.Json;

namespace gLibrary.Core.Saving
{
    public class GameStateManager
    {
        private const string DefaultSavePath = "../saved_game.json";

        public void SaveGame(ISaveableGame game, string? filePath = null)
        {
            string path = filePath ?? DefaultSavePath;
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            var state = game.ToGameState();
            var json = JsonSerializer.Serialize(state, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }

        public bool LoadGame(ISaveableGame game, string? filePath = null)
        {
            string path = filePath ?? DefaultSavePath;
            if (!File.Exists(path)) return false;

            var json = File.ReadAllText(path);
            var state = JsonSerializer.Deserialize<GridState>(json);
            if (state == null) return false;

            game.FromGameState(state);
            return true;
        }
    }
}
