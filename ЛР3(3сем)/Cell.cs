namespace GameWalk
{
    public enum CellState
    {
        Normal,
        Forward,
        DoubleForward,
        DoubleBack,
        Backward
    }

    public class Cell
    {
        public int Position { get; set; }
        public CellState State { get; set; }

        public Cell(int position)
        {
            Position = position;
            State = CellState.Normal;
        }
    }
}
