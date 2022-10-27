using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Services
{
    [ServiceContract]
    public interface IDataService
    {
        [OperationContract]
        bool AddImages();

        [OperationContract]
        bool AddCredentials(DTOCredentials credentials);

        [OperationContract]
        bool isUser(DTOCredentials credentials);
        

    }
    [DataContract]
    public class DTOCredentials
    {

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Email { get; set; }
    }
    [DataContract]
    public class DTOPlayer
    {
        [DataMember]
        public string Image { get; set; }
        [DataMember]
        public DTOCredentials Credentials { get; set; }

    }
}
