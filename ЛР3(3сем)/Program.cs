using System;
using System.Collections.Generic;

namespace GameWalk
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("1 - Начать игру\n2 - Загрузить игру\n3 - Выйти");
                int choice = GetValidIntInput();

                if (choice == 1)
                {
                    StartNewGame();
                }
                else if (choice == 2)
                {
                    LoadGame();
                }
                else if (choice == 3)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Неверный выбор, попробуйте снова.");
                }
            }
        }

        static void StartNewGame()
        {
            Console.WriteLine("Введите количество клеток на поле (минимум 10): ");
            int cellCount = GetValidIntInput(10, 1000); // Установим разумный максимум

            Console.WriteLine("Введите количество игроков: ");
            int playerCount = GetValidIntInput(1, 10); // Установим разумный максимум

            Board board = new Board(cellCount);
            HashSet<string> playerNames = new HashSet<string>();

            for (int i = 1; i <= playerCount; i++)
            {
                string playerName;
                while (true)
                {
                    Console.WriteLine($"Введите имя для игрока {i}: ");
                    playerName = Console.ReadLine();

                    if (!playerNames.Contains(playerName))
                    {
                        playerNames.Add(playerName);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Это имя уже используется. Пожалуйста, выберите другое имя.");
                    }
                }

                Console.WriteLine($"Выберите цвет для игрока {i} (1-Красный, 2-Синий, 3-Зеленый, 4-Желтый): ");
                int colorChoice = GetValidIntInput(1, 4);
                ChipColor color = ChipColor.Red;

                switch (colorChoice)
                {
                    case 1:
                        color = ChipColor.Red;
                        break;
                    case 2:
                        color = ChipColor.Blue;
                        break;
                    case 3:
                        color = ChipColor.Green;
                        break;
                    case 4:
                        color = ChipColor.Yellow;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор цвета. По умолчанию установлен красный.");
                        break;
                }

                board.AddPlayer(new Player(playerName, color));
            }

            CustomizeCells(board);

            GameLoop(board);
        }

        static void LoadGame()
        {
            string savedState = System.IO.File.ReadAllText("game_save.json");
            Board board = Board.LoadState(savedState);

            GameLoop(board);
        }

        static void CustomizeCells(Board board)
        {
            Console.WriteLine("Хотите настроить состояние клеток? (д/н): ");
            string choice = Console.ReadLine().ToLower();

            while (choice != "д" && choice != "н")
            {
                Console.WriteLine("Неверный выбор. Пожалуйста, введите 'д' для да или 'н' для нет.");
                choice = Console.ReadLine().ToLower();
            }

            if (choice == "д")
            {
                while (true)
                {
                    Console.WriteLine("Введите номер клетки для настройки (0 для выхода): ");
                    int cellNumber = GetValidIntInput(0, board.Cells.Count - 1);
                    if (cellNumber == 0)
                    {
                        break;
                    }

                    Console.WriteLine($"Выберите новое состояние для клетки {cellNumber} (1-Обычная, 2-Вперед, 3-Вперед х2, 4-Назад х2, 5-Назад): ");
                    int stateChoice = GetValidIntInput(1, 5);
                    CellState state = CellState.Normal;

                    switch (stateChoice)
                    {
                        case 1:
                            state = CellState.Normal;
                            break;
                        case 2:
                            state = CellState.Forward;
                            break;
                        case 3:
                            state = CellState.DoubleForward;
                            break;
                        case 4:
                            state = CellState.DoubleBack;
                            break;
                        case 5:
                            state = CellState.Backward;
                            break;
                        default:
                            Console.WriteLine("Неверный выбор состояния. По умолчанию установлено обычное состояние.");
                            break;
                    }

                    board.Cells[cellNumber].State = state;
                }
            }
        }

        static void GameLoop(Board board)
        {
            Dice dice = new Dice();

            while (true)
            {
                foreach (Player player in board.Players)
                {
                    Console.WriteLine($"Ход {player.Name}. Нажмите Enter, чтобы бросить кубик.");
                    Console.ReadLine();

                    int roll = dice.Roll();
                    Console.WriteLine($"{player.Name} выбросил {roll}");
                    board.MovePlayer(player, roll);

                    if (player.Position >= board.Cells.Count - 1)
                    {
                        Console.WriteLine($"{player.Name} победил!");
                        return;
                    }
                }

                Console.WriteLine("1 - Продолжить\n2 - Сохранить и выйти");
                int choice = GetValidIntInput();

                if (choice == 2)
                {
                    string saveState = board.SaveState();
                    System.IO.File.WriteAllText("game_save.json", saveState);
                    return;
                }
                else if (choice != 1)
                {
                    Console.WriteLine("Неверный выбор, попробуйте снова.");
                    break;
                }
            }
        }

        static int GetValidIntInput()
        {
            while (true)
            {
                try
                {
                    return int.Parse(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Неверный ввод. Пожалуйста, введите число.");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Число слишком большое или слишком маленькое. Пожалуйста, введите допустимое число.");
                }
            }
        }

        static int GetValidIntInput(int min, int max)
        {
            while (true)
            {
                try
                {
                    int input = int.Parse(Console.ReadLine());
                    if (input < min || input > max)
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                    return input;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Неверный ввод. Пожалуйста, введите число.");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Число слишком большое или слишком маленькое. Пожалуйста, введите допустимое число.");
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine($"Неверный ввод. Пожалуйста, введите число от {min} до {max}.");
                }
            }
        }
    }
}
