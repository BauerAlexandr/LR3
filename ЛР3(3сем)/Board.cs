using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GameWalk
{
    public class Board
    {
        public List<Cell> Cells { get; set; }
        public List<Player> Players { get; set; }

        public Board(int size)
        {
            Cells = new List<Cell>(size);
            for (int i = 0; i < size; i++)
            {
                Cells.Add(new Cell(i));
            }
            Players = new List<Player>();
        }

        public void AddPlayer(Player player)
        {
            Players.Add(player);
        }

        public void MovePlayer(Player player, int steps)
        {
            player.Move(steps);
            if (player.Position >= Cells.Count)
            {
                player.Position = Cells.Count - 1;
            }
            HandleCellState(player);
        }

        private void HandleCellState(Player player)
        {
            bool shouldContinue = true;
            while (shouldContinue)
            {
                shouldContinue = false;
                Console.WriteLine($"{player.Name} ({player.Color}) теперь на клетке {player.Position}");

                if (player.Position >= Cells.Count)
                {
                    player.Position = Cells.Count - 1;
                }

                Cell currentCell = Cells[player.Position];
                switch (currentCell.State)
                {
                    case CellState.Forward:
                        Console.WriteLine("Шаг вперед на 1 клетку.");
                        player.Move(1);
                        shouldContinue = true;
                        break;
                    case CellState.DoubleForward:
                        Console.WriteLine("Шаг вперед на 2 клетки.");
                        player.Move(2);
                        shouldContinue = true;
                        break;
                    case CellState.DoubleBack:
                        Console.WriteLine("Шаг назад на 2 клетки.");
                        player.Move(-2);
                        shouldContinue = true;
                        break;
                    case CellState.Backward:
                        Console.WriteLine("Шаг назад на 1 клетку.");
                        player.Move(-1);
                        shouldContinue = true;
                        break;
                    default:
                        break;
                }
            }
        }

        public string SaveState()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Board LoadState(string savedState)
        {
            return JsonConvert.DeserializeObject<Board>(savedState);
        }
    }
}
