using NUnit.Framework;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace GameWalk.Tests
{
    [TestFixture]
    public class BoardTests
    {
        [Test]
        public void TestAddPlayer()
        {
            var board = new Board(10);
            var player = new Player("Player1", ChipColor.Red);
            board.AddPlayer(player);

            Assert.AreEqual(1, board.Players.Count);
            Assert.AreEqual("Player1", board.Players[0].Name);
            Assert.AreEqual(ChipColor.Red, board.Players[0].Color);
        }

        [Test]
        public void TestMovePlayer()
        {
            var board = new Board(10);
            var player = new Player("Player1", ChipColor.Red);
            board.AddPlayer(player);

            board.MovePlayer(player, 3);

            Assert.AreEqual(3, player.Position);
        }

        [Test]
        public void TestCellCustomization()
        {
            var board = new Board(10);
            board.Cells[3].State = CellState.DoubleForward;

            Assert.AreEqual(CellState.DoubleForward, board.Cells[3].State);
        }

        [Test]
        public void TestSaveLoadState()
        {
            var board = new Board(10);
            var player = new Player("Player1", ChipColor.Red);
            board.AddPlayer(player);
            board.MovePlayer(player, 3);

            string savedState = board.SaveState();
            var loadedBoard = Board.LoadState(savedState);

            Assert.AreEqual(1, loadedBoard.Players.Count);
            Assert.AreEqual("Player1", loadedBoard.Players[0].Name);
            Assert.AreEqual(3, loadedBoard.Players[0].Position);
            Assert.AreEqual(ChipColor.Red, loadedBoard.Players[0].Color);
        }

        [Test]
        public void TestRecursiveCellHandling()
        {
            var board = new Board(10);
            board.Cells[5].State = CellState.DoubleForward;
            board.Cells[6].State = CellState.Backward;

            var player = new Player("Player1", ChipColor.Red);
            board.AddPlayer(player);

            board.MovePlayer(player, 6);
            Assert.AreEqual(7, player.Position);  // Player should end up at position 7 after hitting cell 6 (backward to 5) and then cell 5 (double forward)
        }

        [Test]
        public void TestForwardCellHandling()
        {
            var board = new Board(10);
            board.Cells[2].State = CellState.Forward;

            var player = new Player("Player1", ChipColor.Red);
            board.AddPlayer(player);

            board.MovePlayer(player, 2);
            Assert.AreEqual(3, player.Position);  // Player should move to position 3 after hitting cell 2 (forward)
        }

        [Test]
        public void TestBackwardCellHandling()
        {
            var board = new Board(10);
            board.Cells[4].State = CellState.Backward;

            var player = new Player("Player1", ChipColor.Red);
            board.AddPlayer(player);

            board.MovePlayer(player, 4);
            Assert.AreEqual(3, player.Position);  // Player should move back to position 3 after hitting cell 4 (backward)
        }

        [Test]
        public void TestPlayerWinCondition()
        {
            var board = new Board(5);
            var player = new Player("Player1", ChipColor.Red);
            board.AddPlayer(player);

            board.MovePlayer(player, 5);
            Assert.AreEqual(4, player.Position);  // Player should be at the last cell
        }

        [Test]
        public void TestAddMultiplePlayers()
        {
            var board = new Board(10);
            var player1 = new Player("Player1", ChipColor.Red);
            var player2 = new Player("Player2", ChipColor.Blue);
            board.AddPlayer(player1);
            board.AddPlayer(player2);

            Assert.AreEqual(2, board.Players.Count);
            Assert.AreEqual("Player1", board.Players[0].Name);
            Assert.AreEqual("Player2", board.Players[1].Name);
        }

        [Test]
        public void TestMultipleMoves()
        {
            var board = new Board(10);
            var player = new Player("Player1", ChipColor.Red);
            board.AddPlayer(player);

            board.MovePlayer(player, 3);
            Assert.AreEqual(3, player.Position);

            board.MovePlayer(player, 2);
            Assert.AreEqual(5, player.Position);
        }

        [Test]
        public void TestMoveBeyondBoard()
        {
            var board = new Board(5);
            var player = new Player("Player1", ChipColor.Red);
            board.AddPlayer(player);

            board.MovePlayer(player, 10);
            Assert.AreEqual(4, player.Position);  // Player should be at the last cell
        }

        [Test]
        public void TestDoubleBackCellHandling()
        {
            var board = new Board(10);
            board.Cells[4].State = CellState.DoubleBack;

            var player = new Player("Player1", ChipColor.Red);
            board.AddPlayer(player);

            board.MovePlayer(player, 4);
            Assert.AreEqual(2, player.Position);  // Player should move back to position 2 after hitting cell 4 (double back)
        }

        [Test]
        public void TestMovePlayerToNegativePosition()
        {
            var board = new Board(10);
            var player = new Player("Player1", ChipColor.Red);
            board.AddPlayer(player);

            board.MovePlayer(player, 1); // Move to position 1
            board.Cells[1].State = CellState.Backward;

            board.MovePlayer(player, 0); // Trigger backward move to position -1
            Assert.AreEqual(0, player.Position); // Player should stay at position 0
        }

        [Test]
        public void TestCellStateNormal()
        {
            var board = new Board(10);
            board.Cells[5].State = CellState.Normal;

            Assert.AreEqual(CellState.Normal, board.Cells[5].State);
        }

        [Test]
        public void TestInvalidCellStateChange()
        {
            var board = new Board(10);

            Assert.Throws<System.ArgumentOutOfRangeException>(() => board.Cells[10].State = (CellState)10);
        }

       

        [Test]
        public void TestInvalidIntInputFormat()
        {
            Assert.Throws<System.FormatException>(() => int.Parse("Invalid"));
        }

        

        [Test]
        public void TestSaveGameToFile()
        {
            var board = new Board(10);
            var player = new Player("Player1", ChipColor.Red);
            board.AddPlayer(player);

            string savedState = board.SaveState();
            System.IO.File.WriteAllText("test_save.json", savedState);

            Assert.IsTrue(System.IO.File.Exists("test_save.json"));
        }

        [Test]
        public void TestLoadGameFromFile()
        {
            var board = new Board(10);
            var player = new Player("Player1", ChipColor.Red);
            board.AddPlayer(player);

            string savedState = board.SaveState();
            System.IO.File.WriteAllText("test_load.json", savedState);

            string loadedState = System.IO.File.ReadAllText("test_load.json");
            var loadedBoard = Board.LoadState(loadedState);

            Assert.AreEqual(board.Players[0].Name, loadedBoard.Players[0].Name);
        }
    }
}
