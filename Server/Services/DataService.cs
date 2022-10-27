using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using UnoEntitys;
using Utilities;

namespace Services
{
    public class DataService : IDataService
    {
        public bool AddCredentials(DTOCredentials credentials)
        {
            //ciframos la contraseña
            bool result = false;
            credentials.Password = Security.ComputeSHA256Hash(credentials.Password);
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
        

        public bool IsUser(DTOCredentials credentials)
        {
            bool flag = false;
            credentials.Password = Security.ComputeSHA256Hash(credentials.Password);
            Credentials entityCredential = this.DtoCredentialsToEntity(credentials);
            try
            {
                using (unoDbModelContainer dataBase = new unoDbModelContainer())
                {
                    DTOPlayer dTOPlayer = new DTOPlayer();
                    //buscar credenciales
                    Credentials findCredentials = dataBase.CredentialsSet1.Where(x => x.username == credentials.Username && x.password == credentials.Password).FirstOrDefault();
                    if (findCredentials != null)
                    {
                        flag = true;
                    }
                    return flag;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        //Esta no esta en la interfaz por que el cliente no accedera a este metodo
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
