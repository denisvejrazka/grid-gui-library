namespace gLibrary.Core.Saving
{
    public class GridState
    {
        public int Rows { get; set; }
        public int Columns { get; set; }

        public List<List<int>> GridValues { get; set; } = new();

        public int MovesMade { get; set; }
        public int CurrentPlayer { get; set; }

        public Dictionary<string, string> Metadata { get; set; } = new();
    }
}
