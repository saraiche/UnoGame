using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnoEntitys;
using Dtos;

namespace Logic
{
    public class DtosToEntitys
    {
       
        public credentials DtoCredentialsToEntity(DTOCredentials dTOCredentials)
        {
            credentials result = new credentials();
            if(dTOCredentials.Id == null)
            {
                result.username = dTOCredentials.Username;
                result.email = dTOCredentials.Email;
                result.password = dTOCredentials.Password;
            }
            else
            {
                result.Id = dTOCredentials.Id;
                result.username = dTOCredentials.Username;
                result.email = dTOCredentials.Email;
                result.password = dTOCredentials.Password;
            }
            return result;
        }  

    }
}
