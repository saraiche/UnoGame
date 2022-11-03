using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unoProyect.Proxy;
using unoProyect.Security;


namespace unoProyect.Logic
{
     public class CallDataService
    {
        Proxy.DataServiceClient dataServiceClient = new Proxy.DataServiceClient();
        public int AddCredentials(string username, string password, string email)
        {
            int result = 0;
            Proxy.DTOCredentials dTOcredentials = new Proxy.DTOCredentials();
            dTOcredentials.Username = username;
            dTOcredentials.Password = Utilities.ComputeSHA256Hash(password);
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
