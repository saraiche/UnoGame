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
            Credentials entityCredential = this.DtoCredentialsToEntity(credentials); 

            try
            {
                using (unoDbModelContainer context = new unoDbModelContainer())
                {
                    //buscar un jugador repetido
                    Player findPlayer = context.PlayerSet1.Where(x => x.Credentials.username == credentials.Username).FirstOrDefault();
                    if(findPlayer == null)
                    {
                        Images images = new Images { Id = 1 };
                        context.ImagesSet1.Attach(images);
                        Player player = new Player { wins = 0, losts = 0, Images = images };
                        entityCredential.Player = player;
                        context.CredentialsSet1.Add(entityCredential);
                        context.SaveChanges();
                        result = true;
                    }
                    else
                    {
                        throw new Exception("El nombre de usuario ya existe");
                    }
                    
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
        public Credentials DtoCredentialsToEntity(DTOCredentials dTOCredentials)
        {
            Credentials result = new Credentials();
            result.Id = 0;
            result.username = dTOCredentials.Username;
            result.email = dTOCredentials.Email;
            result.password = dTOCredentials.Password;
            return result;
        }
    }
}
