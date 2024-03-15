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

		public async Task<IActionResult> ImagenesZapa(int? posicion, int idproducto)
		{
			if(posicion == null)
			{
				posicion = 1;
			}
			ModelPaginacionImagenes model = await this.repo.GetImagenesZapa(posicion.Value, idproducto);
			Zapatilla zapa = await this.repo.FindZapatilla(idproducto);
			ViewData["ZAPASELECCIONADA"] = zapa;
			ViewData["REGISTROS"] = model.Registros;
			ViewData["ZAPATILLA"] = idproducto;
			int siguiente = posicion.Value + 1;
			//DEBEMOS COMPROBAR QUE NO PASAMOS DEL NUMERO DE REGISTROS
			if (siguiente > model.Registros)
			{
				//EFECTO OPTICO
				siguiente = model.Registros;
			}
			int anterior = posicion.Value - 1;
			if (anterior < 1)
			{
				anterior = 1;
			}
			ViewData["ULTIMO"] = model.Registros;
			ViewData["SIGUIENTE"] = siguiente;
			ViewData["ANTERIOR"] = anterior;
			ViewData["POSICION"] = posicion;
			return View(model.Imagen);
		}
	}
}
