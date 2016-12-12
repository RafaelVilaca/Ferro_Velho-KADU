using Ferro_Velho.Repositorio;

namespace Ferro_Velho.Business
{
    public class CaixaConstrutor
    {
        public static CaixaBo caixaBo()
        {
            return new CaixaBo(new CaixaRepositorio());
        }
    }
}
