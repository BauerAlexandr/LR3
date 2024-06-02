using NUnit.Framework;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Numerics;

namespace GameWalk.Tests
{
    [TestFixture]
    public class BoardTests
    {
        [Test]
        public void TestAddPlayer()
        {
            var board = new Board(10);
            var player = new Player("Player1");
            board.AddPlayer(player);

            Assert.AreEqual(1, board.Players.Count);
            Assert.AreEqual("Player1", board.Players[0].Name);
        }

        [Test]
        public void TestMovePlayer()
        {
            var board = new Board(10);
            var player = new Player("Player1");
            board.AddPlayer(player);

            board.MovePlayer(player, 3);

            Assert.AreEqual(3, player.Position);
        }

        [Test]
        public void TestSaveLoadState()
        {
            var board = new Board(10);
            var player = new Player("Player1");
            board.AddPlayer(player);
            board.MovePlayer(player, 3);

            string savedState = board.SaveState();
            var loadedBoard = Board.LoadState(savedState);

            Assert.AreEqual(1, loadedBoard.Players.Count);
            Assert.AreEqual("Player1", loadedBoard.Players[0].Name);
            Assert.AreEqual(3, loadedBoard.Players[0].Position);
        }
    }
}
