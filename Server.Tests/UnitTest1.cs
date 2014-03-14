using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Creatures;
using Server.Tests.MockObjects;

namespace Server.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var dummyObject = new DummyModels.DummyObjectBase();
            Assert.AreEqual(1, dummyObject.UniqueId);
            
            var dummyObject2 = new DummyModels.DummyObjectBase();
            Assert.AreEqual(2, dummyObject2.UniqueId);
        }

        [TestMethod]
        public void PlayerLoading()
        {
            var player = new Player(new MockedCreatureDataProvider(), new MockedClientConnection());
            player.Initialize();

            Assert.AreEqual("MockedPlayer", player.Name);
        }

        [TestMethod]
        public void LoadMap()
        {
            
        }
    }
}
