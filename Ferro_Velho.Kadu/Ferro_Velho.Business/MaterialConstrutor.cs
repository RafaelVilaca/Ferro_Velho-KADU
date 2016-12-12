using Ferro_Velho.Repositorio;

namespace Ferro_Velho.Business
{
    public class MaterialConstrutor
    {
        public static MateriaisBo materialBo()
        {
            return new MateriaisBo(new MaterialRepositorio());
        }
    }
}
