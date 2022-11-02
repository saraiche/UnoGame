using Logic.DataServiceReference;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.ServiceModel.Configuration;
using Utilities;

namespace Logic
{
    public class CallDataService
    {
        DataServiceReference.DataServiceClient dataServiceClient = new DataServiceReference.DataServiceClient();
        
        public int AddCredentials(string username, string password, string email)
        {
            int result = 0;
            DataServiceReference.DTOCredentials dTOcredentials = new DataServiceReference.DTOCredentials();
            
  
            dTOcredentials.Username = username;
            dTOcredentials.Password = Security.ComputeSHA256Hash(password);
            dTOcredentials.Email = email;
            result = dataServiceClient.AddCredentials(dTOcredentials);
            return result;
        }

        public bool IsUser(string username, string password)
        {
            bool result = false;
            DataServiceReference.DTOCredentials isUser = new DataServiceReference.DTOCredentials();
            isUser.Password = password;
            isUser.Username = username;
            try
            {
                result = dataServiceClient.IsUser(isUser);
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public bool SearchUser(string username)
        {
            bool result = false;
            DataServiceReference.DTOCredentials searchUser = new DataServiceReference.DTOCredentials();
            searchUser.Username = username;
            try
            {
                result = dataServiceClient.SearchUser(searchUser);
            }
            catch (EntityException ex)
            {
                throw new EntityException(ex.Message);
            }
            return result;
        }
       
    }
}