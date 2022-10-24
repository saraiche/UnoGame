using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.ServiceModel.Configuration;

namespace Logic
{
    public class CallDataService
    {
        public void AddCredentials(string username, string password, string email)
        {
            DataServiceReference.DataServiceClient dataServiceClient = new DataServiceReference.DataServiceClient();
            DataServiceReference.DTOCredentials credentials = new DataServiceReference.DTOCredentials();
  
            credentials.Username = username;    
            credentials.Password = password;
            credentials.Email = email;
            dataServiceClient.AddCredentials(credentials);
        }
       
    }
}