using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Services.Tests
{
    [TestClass()]
    public class ServiceImplementationTests
    {
        private Dictionary<string, List<DTOUserChat>> Rooms = new Dictionary<string, List<DTOUserChat>>();
        private ServiceImplementation serviceImplementation = new ServiceImplementation();
        
        public void CreateSomeRooms()
        {
            List<DTOUserChat> users = new List<DTOUserChat>();
            DTOUserChat dto = new DTOUserChat()
            {
                UserName = "username23"
            };
            users.Add(dto);
            serviceImplementation.Rooms.Add("147852", users);

            List<DTOUserChat> users2 = new List<DTOUserChat>();
            DTOUserChat dto2 = new DTOUserChat()
            {
                UserName = "username"
            };
            users.Add(dto);
            serviceImplementation.Rooms.Add("789456", users);

            List<DTOUserChat> users3 = new List<DTOUserChat>();
            DTOUserChat dto3 = new DTOUserChat()
            {
                UserName = "lolopol"
            };
            DTOUserChat dto4 = new DTOUserChat()
            {
                UserName = "saraiche"
            };
            users.Add(dto3);
            users.Add(dto4);
            serviceImplementation.Rooms.Add("987645", users);
        }


        [TestMethod()]
        public void JoinTestNoCode()
        {
            List<DTOUserChat> users = new List<DTOUserChat>();
            Rooms.Add("123456", users);
            var result = serviceImplementation.Join("juan", "122456");
            Assert.IsFalse(result);
        }


        [TestMethod()]
        public void NewRoomTestWithNoContext()
        {
            Assert.ThrowsException<System.NullReferenceException>(() => serviceImplementation.NewRoom("juan"));
        }

        [TestMethod()]
        public void DeletePlayerTest()
        {
            CreateSomeRooms();
            var result = serviceImplementation.DeletePlayer("147852", "username23");

            Assert.IsTrue(result);

        }

        [TestMethod()]
        public void DeletePlayerNotPlayerFoundTest()
        {
            CreateSomeRooms();
            var result = serviceImplementation.DeletePlayer("789456", "username23");

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void GetPlayersByInvitationCodeTest()
        {
            CreateSomeRooms();
            List<string> players = new List<string>();
            players.Add("username");
            players.Add("saraiche");

            var result = serviceImplementation.GetPlayersByInvitationCode("987645");

            Assert.AreEqual(result, players);
        }

    }
}