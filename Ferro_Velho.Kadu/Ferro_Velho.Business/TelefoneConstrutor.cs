using Ferro_Velho.Repositorio;

namespace Ferro_Velho.Business
{
    public class TelefoneConstrutor
    {
        public static TelefoneBo telefoneBo()
        {
            return new TelefoneBo(new TelefoneRepositorio());
        }
    }
}
