using Ferro_Velho.Business;
using Ferro_Velho.Entidades;
using System.Web.Mvc;

namespace Ferro_Velho.Web.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ClientesBo clienteBo;

        public ClienteController()
        {
            clienteBo = ClienteConstrutor.clienteBo();
        }

        // GET: Cliente
        public ActionResult Index(string nomeCliente, int? pagina)
        {
            var lista = clienteBo.ListarTodos(nomeCliente, pagina);
            return View(lista);
        }

        public ActionResult Cadastro(int? id, int? pagina)
        {
            int paginas = pagina ?? 1;
            if (id.HasValue)
            {
                return View(clienteBo.ListarPorId(id.Value, paginas));
            }
            else
            {
                return View();
            }
        }

        public ActionResult Excluir(int id)
        {
            ClienteVo cliente = new ClienteVo();
            cliente.ID_Cliente = id;

            clienteBo.Excluir(id);

            return RedirectToAction("Index");
        }

        public ActionResult DisExcluir(int id)
        {
            ClienteVo cliente = new ClienteVo();
            cliente.ID_Cliente = id;

            clienteBo.DisExcluir(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Cadastrar(ClienteVo cliente)
        {
            if (cliente.ID_Cliente == 0)
            {
                cliente.Ativo = true;
            }
            TempData["mensagem"] = clienteBo.Salvar(cliente);
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