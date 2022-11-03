using Logic.Proxy;
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
       public bool SearchUser(string username)
        {
            bool result = false;
            Proxy.DTOCredentials searchUser = new Proxy.DTOCredentials();
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

        public string SendMail(string email, string emailSubject)
        {
            string code = "";
            code = (new Random().Next(100000, 999999)).ToString();
            bool result = dataServiceClient.SendMail(email, emailSubject, "El código es: " + code);
            if (!result)
            {
                code = "";
            }
            return code;
        }
    }
}