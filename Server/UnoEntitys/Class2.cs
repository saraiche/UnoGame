using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UnoEntitys;

namespace ConsoleApp1
{
    public class Class2
    {
        public Class2()
        {

        }
        public void Metodo()
        {
            credentials _credentials = new credentials
            {
                username = "saraiche",
                password = "123456"
            };
            bool result = false;
            try
            {
                using (unoDbModelContainer _context = new unoDbModelContainer())
                {
                    var query = from credent in _context.credentialsSet
                                where credent.username == _credentials.username && credent.password == _credentials.password
                                select credent.username;
                    if (query.Count() > 0)
                    {
                        result = true;
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            Console.WriteLine("El resultado es: ", result);
        }

    }
}
