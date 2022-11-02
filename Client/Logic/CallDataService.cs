using Logic.Proxy;
using System;
using System.Collections.Generic;
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
        Proxy.DataServiceClient dataServiceClient = new Proxy.DataServiceClient();
        public int AddCredentials(string username, string password, string email)
        {
            int result = 0;
            Proxy.DTOCredentials dTOcredentials = new Proxy.DTOCredentials();
            dTOcredentials.Username = username;
            dTOcredentials.Password = Security.ComputeSHA256Hash(password);
            dTOcredentials.Email = email;
            result = dataServiceClient.AddCredentials(dTOcredentials);
            return result;
        }

        public bool IsUser(string username, string password)
        {
            bool result = false;
            Proxy.DTOCredentials isUser = new Proxy.DTOCredentials();
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
       
    }
}