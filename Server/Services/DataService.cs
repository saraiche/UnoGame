using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using UnoEntitys;

namespace Services
{
    public class DataService : IDataService
    {
        public bool AddCredentials(DTOCredentials credentials)
        {
            bool result = false;
            credentials entityCredential = this.DtoCredentialsToEntity(credentials); 

            try
            {
                using (unoDbModelContainer _context = new unoDbModelContainer())
                {

                    images _images = new images { Id = 1 };
                    _context.imagesSet.Attach(_images);
                    player _player = new player { wins = 0, losts = 0, images = _images };
                    entityCredential.player = _player;
                    _context.credentialsSet.Add(entityCredential);

                    _context.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public bool AddImages()
        {
            throw new NotImplementedException();
        }
        //esta no esta en la interfaz por que el cliente no accedera a este metodo
        public credentials DtoCredentialsToEntity(DTOCredentials dTOCredentials)
        {
            credentials result = new credentials();
            if (dTOCredentials.Id == null)
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
