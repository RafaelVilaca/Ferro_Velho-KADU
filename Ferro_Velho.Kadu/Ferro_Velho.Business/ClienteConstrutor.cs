using Ferro_Velho.Repositorio;

namespace Ferro_Velho.Business
{
    public class ClienteConstrutor
    {
        public static ClientesBo clienteBo()
        {
            return new ClientesBo(new ClienteRepositorio());
        }
    }
}
