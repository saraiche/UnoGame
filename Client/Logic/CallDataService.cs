using Logic.DataServiceReference;
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
        DataServiceReference.DataServiceClient dataServiceClient = new DataServiceReference.DataServiceClient();
        public void AddCredentials(string username, string password, string email)
        {
            DataServiceReference.DTOCredentials dTOcredentials = new DataServiceReference.DTOCredentials();
  
            dTOcredentials.Username = username;    
            dTOcredentials.Password = password;
            dTOcredentials.Email = email;
            dataServiceClient.AddCredentials(dTOcredentials);
        }

        public bool IsUser(string username, string password)
        {
            bool result = false;
            DataServiceReference.DTOCredentials isUser = new DataServiceReference.DTOCredentials();
            isUser.Password = password;
            isUser.Username = username;
            try
            {
                result = dataServiceClient.isUser(isUser);
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }
       
    }
}