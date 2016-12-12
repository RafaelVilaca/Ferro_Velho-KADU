using Ferro_Velho.Business;
using Ferro_Velho.Entidades;
using System.Web.Mvc;

namespace Ferro_Velho.Web.Controllers
{
    public class MaterialController : Controller
    {
        private readonly MateriaisBo materialBo;

        public MaterialController()
        {
            materialBo = MaterialConstrutor.materialBo();
        }

        // GET: Material
        public ActionResult Index(string descricao, int? pagina)
        {
            var lista = materialBo.ListarTodos(descricao, pagina);
            return View(lista);
        }

        public ActionResult Cadastro(int? id, int? pagina)
        {
            int paginas = pagina ?? 1;
            if (id.HasValue)
            {
                return View(materialBo.ListarPorId(id.Value, paginas));
            }
            else
            {
                return View();
            }
        }

        public ActionResult Excluir(int id)
        {
            MateriaisVo material = new MateriaisVo();
            material.ID_Material = id;

            materialBo.Excluir(id);

            return RedirectToAction("Index");
        }

        public ActionResult DisExcluir(int id)
        {
            MateriaisVo material = new MateriaisVo();
            material.ID_Material = id;

            materialBo.DisExcluir(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Cadastrar(MateriaisVo material)
        {
            TempData["mensagem"] = materialBo.Salvar(material);
            if (TempData["mensagem"].ToString() != "Existe campos em Branco, preencha-os por favor!")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Cadastro");
            }
        }
    }
}