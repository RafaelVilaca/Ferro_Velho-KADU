using Ferro_Velho.Business;
using Ferro_Velho.Entidades;
using Ferro_Velho.Web.Models;
using System.Web.Mvc;

namespace Ferro_Velho.Web.Controllers
{
    public class TelefoneController : Controller
    {
        private readonly TelefoneBo telefoneBo;

        public TelefoneController()
        {
            telefoneBo = TelefoneConstrutor.telefoneBo();
        }

        // GET: Telefones
        public ActionResult Index(string nome, int? pagina)
        {
            var lista = telefoneBo.ListarTodos(nome, pagina);
            return View(lista);
        }

        public PartialViewResult GetTelefonesByCliente(int id, int? pagina)
        {
            int paginas = pagina ?? 1;
            var telefones = telefoneBo.ListarPorId(id, paginas);
            return PartialView("_ListaCliente", telefones);
        }

        public ActionResult Cadastro(int? id, int? pagina)
        {
            int paginas = pagina ?? 1;
            if (id.HasValue)
            {
                ViewBag.Telefones = telefoneBo.ListarPorId(id.Value, paginas);
                return View(telefoneBo.ListarPorId(id.Value, paginas));
            }
            else
            {
                ViewBag.Telefones = telefoneBo.ListarTodos(null, null);
                return View();
            }
        }

        public ActionResult Excluir(int id)
        {
            MateriaisVo material = new MateriaisVo();
            material.ID_Material = id;

            telefoneBo.Excluir(id);

            return RedirectToAction("Index");
        }

        //public ActionResult Editar(TelefoneVo telefone)
        //{
        //    telefoneBo.Salvar(telefone);
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public ActionResult Cadastrar(TelefoneModel telefoneRq)
        {
            var telefone = new TelefoneVo();
            telefone.ID_Cliente = telefoneRq.ID_Cliente;
            telefone.Telefone = telefoneRq.Telefone;
            telefone.Tipo_Telefone = telefoneRq.Tipo_Telefone;
            telefone.ID_Telefone = telefoneRq.ID_Telefone;
            //telefoneBo.Salvar(telefone);
            TempData["mensagem"] = telefoneBo.Salvar(telefone);
            if (TempData["mensagem"].ToString() == "Telefone Existente, corrija o Telefone ou Atualize!"
                || TempData["mensagem"].ToString() == "Existe campos em Branco, preencha-os por favor!")
            {
                return RedirectToAction("Cadastro");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}