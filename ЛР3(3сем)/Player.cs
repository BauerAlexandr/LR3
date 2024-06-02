namespace GameWalk
{
    public enum ChipColor
    {
        Red,
        Blue,
        Green,
        Yellow
    }

    public class Player
    {
        public string Name { get; set; }
        public int Position { get; set; }
        public ChipColor Color { get; set; }

        public Player(string name, ChipColor color)
        {
            Name = name;
            Position = 0;
            Color = color;
        }

        public void Move(int steps)
        {
            Position += steps;
        }
    }
}
