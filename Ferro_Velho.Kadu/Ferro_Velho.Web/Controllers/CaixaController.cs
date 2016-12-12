using Ferro_Velho.Business;
using Ferro_Velho.Entidades;
using System;
using System.Web.Mvc;

namespace Ferro_Velho.Web.Controllers
{
    public class CaixaController : Controller
    {
        private readonly CaixaBo caixaBo;

        public CaixaController()
        {
            caixaBo = CaixaConstrutor.caixaBo();
        }

        // GET: Caixa
        public ActionResult Index(string dataInicial, string dataFinal, string nome, int? pagina)
        {
            var novaDataInicial = !String.IsNullOrEmpty(dataInicial) ? Convert.ToDateTime(dataInicial) : Convert.ToDateTime("1/1/1753");
            var novaDataFinal = !String.IsNullOrEmpty(dataFinal) ? Convert.ToDateTime(dataFinal) : DateTime.MaxValue;
            var lista = caixaBo.ListarTodos(novaDataInicial, novaDataFinal, nome, pagina);
            return View(lista);
        }

        public PartialViewResult GetAtendimentosByCliente(int id, int? pagina)
        {
            int paginas = pagina ?? 1;
            var atendimentos = caixaBo.ListarPorId(id, paginas);
            return PartialView("_Lista", atendimentos);
        }

        public ActionResult Cadastro(int? id, int? pagina)
        {
            int paginas = pagina ?? 1;
            if (id.HasValue)
            {
                ViewBag.Atendimentos = caixaBo.ListarPorId(id.Value, paginas);
                return View(caixaBo.ListarPorId(id.Value, paginas));
            }
            else
            {
                ViewBag.Atendimentos = caixaBo.ListarTodos(null, null, null, null);
                return View();
            }
        }

        public ActionResult Baixar(int id)
        {
            CaixaVo caixa = new CaixaVo();
            caixa.ID_Cliente = id;

            caixaBo.Baixar(id);

            return RedirectToAction("Caixa");
        }

        public ActionResult Excluir(int id)
        {
            CaixaVo caixa = new CaixaVo();
            caixa.ID_Caixa = id;

            caixaBo.Excluir(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Cadastrar(CaixaVo caixa)
        {
            TempData["mensagem"] = caixaBo.Salvar(caixa);
            if (TempData["mensagem"].ToString() == "Transacao efetuada com Sucesso!!!")
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