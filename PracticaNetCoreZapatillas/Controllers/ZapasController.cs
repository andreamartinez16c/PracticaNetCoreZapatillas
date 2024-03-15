using Microsoft.AspNetCore.Mvc;
using PracticaNetCoreZapatillas.Models;
using PracticaNetCoreZapatillas.Repositories;

namespace PracticaNetCoreZapatillas.Controllers
{
	public class ZapasController : Controller
	{
		private RepositoryZapas repo;
		
		public ZapasController(RepositoryZapas repo)
		{
			this.repo = repo;
		}

		public async Task<IActionResult> Index()
		{
			List<Zapatilla> zapatillas = await this.repo.GetZapasAsync();
			return View(zapatillas);
		}

		public async Task<IActionResult> ImagenesZapa(int idproducto)
		{
			ModelPaginacionImagenes model = await this.repo.GetImagenesZapa(1, idproducto);
			ViewData["ZAPATILLA"] = model.Zapatilla;
			return View();
		}

		public async Task<IActionResult> _PartialPaginacionImagenes(int? posicion, int idproducto)
		{
			if(posicion == null)
			{
				posicion = 1;
			}
			ModelPaginacionImagenes model = await this.repo.GetImagenesZapa(posicion.Value, idproducto);
			int numRegistros = model.Registros;
			int siguiente = posicion.Value + 1;
			if (siguiente > numRegistros)
			{
				siguiente = numRegistros;
			}
			int anterior = posicion.Value - 1;
			if (anterior < 1)
			{
				anterior = 1;
			}
			ViewData["SIGUIENTE"] = siguiente;
			ViewData["ANTERIOR"] = anterior;
			ViewData["ULTIMO"] = model.Registros;
			ViewData["ZAPATILLA"] = model.Zapatilla;
			ViewData["POSICION"] = posicion;
			return PartialView("_PartialPaginacionImagenes", model.Imagen);

		}

		public async Task<IActionResult> InsertarImagenes()
		{
			List<Zapatilla> zapatillas = await this.repo.GetZapasAsync();
			return View(zapatillas);
		}

		[HttpPost]
		public async Task<IActionResult> InsertarImagenes(List<string> imagenes, int idproducto)
		{
			await this.repo.CreateImagenAsync(imagenes, idproducto);
			return RedirectToAction("ImagenesZapa", new {idproducto = idproducto});
		}
	}
}
