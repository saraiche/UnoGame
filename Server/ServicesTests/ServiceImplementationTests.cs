using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using UnoEntitys;

namespace IdataService.Tests
{
    [TestClass()]
    public class ServiceImplementationTests
    {
        private DTOCredentials Credentials { get; set; }
        private DTOCredentials Credentials2 { get; set; }
        private DTOPlayer PlayerDTO { get; set; }
        private ServiceImplementation ServiceImplementation { get; set; }

        private Credentials CredentialsEntity { get; set; }

        private List<string> Friends { get; set; }
        [TestInitialize]
        public void TestInitialize()
        {
            Credentials = new DTOCredentials();
            Credentials2 = new DTOCredentials();
            ServiceImplementation = new ServiceImplementation();
            CredentialsEntity = new Credentials();
            Friends = new List<string>();



            Credentials.Email = "qwdsa@gmail.com";
            Credentials.Username = "BenitoBellako";
            Credentials.Password = "AS1N7&$";

            Credentials2.Email = "qwdsdsaa@gmail.com";
            Credentials2.Username = "Locomotora";
            Credentials2.Password = "i8Y2vR/Cys8MOYmdLdS8MlMh7zsGukXpOXw+cFs49wRDPRX+xzcrNeRJ2nkjKDmrm0moRcgdaz4SA4PmzFVoUBR5uxCgiAcVsNZ8u+6yiKSQ+fE9at11i/fBNThCd28LTrJzcuoqrap4aIG0cQH+kQB1sv6jWD1h7GZ1MV/bWmuT7MEwS73dGPoFmGkGs6z1OYy6treZ2hAfyMujhLz3/Q";

            CredentialsEntity.username =  Credentials.Username;
            CredentialsEntity.password =   Credentials.Password;
            CredentialsEntity.email =   Credentials.Email ;

            Friends.Add(Credentials2.Username);
        }

        [TestMethod()]
        public void AddCredentialsTest()
        {
            
            Assert.AreEqual(ServiceImplementation.AddCredentials(Credentials), 2);
            Assert.AreEqual(ServiceImplementation.AddCredentials(Credentials2), 1);
        }

        

        [TestMethod()]
        public void IsUserTest()
        {
            Credentials2.Password = "AS1N7&ds12$";
            Assert.IsTrue(ServiceImplementation.IsUser(Credentials2));
        }

        [TestMethod()]
        public void DtoCredentialsToEntityTest()
        {
            Credentials result = ServiceImplementation.DtoCredentialsToEntity(Credentials);
            Assert.AreEqual(result.username, CredentialsEntity.username);
            Assert.AreEqual(result.password, CredentialsEntity.password);
            Assert.AreEqual(result.email, CredentialsEntity.email);
            
        }

        [TestMethod()]
        public void SearchUserTest()
        {
            Assert.IsTrue(ServiceImplementation.SearchUser(Credentials));
        }

      

        [TestMethod()]
        public void AddFriendTest()
        {
            Assert.IsTrue(ServiceImplementation.AddFriend(Credentials.Username,Credentials2.Username));
        }

        [TestMethod()]
        public void GetFriendsTest()
        {
            Assert.AreEqual(ServiceImplementation.GetFriends(Credentials.Username),Friends);
        }

        [TestMethod()]
        public void SearchUserByUsernameTest()
        {
            Assert.AreEqual(ServiceImplementation.SearchUserByUsername(Credentials.Username), Credentials);
        }

        [TestMethod()]
        public void GetPlayerTest()
        {
            PlayerDTO = ServiceImplementation.GetPlayer(Credentials.Username);
            Assert.IsNotNull(ServiceImplementation.GetPlayer(Credentials.Username));
        }

        [TestMethod()]
        public void SetPlayerTest()
        {
            PlayerDTO.Credentials.Password = "t8l6azxIsKqcFterGByJW6zfEwYdXY2eERGfiBuZz866NPIlNMwJguvvCkb5rHNFCIC0hl";
            Assert.AreEqual(ServiceImplementation.SetPlayer(PlayerDTO,PlayerDTO.Credentials.Username),1);
        }

        [TestMethod()]
        public void ModifyPasswordTest()
        {
            Assert.IsTrue(ServiceImplementation.ModifyPassword(Credentials.Username, "t8l6azxIsKqcFterGByJW6zfEwYdXY2eERGfiBuZz866NPIlNMwJguvvCkb5rHNFCIC0hl/7tAx4qf1TyPFYCw"));
        }

        [TestMethod()]
        public void DeleteFriendTest()
        {
            Assert.IsTrue(ServiceImplementation.DeleteFriend(Credentials.Username, Credentials2.Username));
        }
    }
}

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