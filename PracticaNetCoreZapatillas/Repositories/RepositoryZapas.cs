using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PracticaNetCoreZapatillas.Data;
using PracticaNetCoreZapatillas.Models;
using System.Data;

namespace PracticaNetCoreZapatillas.Repositories
{
	public class RepositoryZapas
	{
		private ZapasContext context;
		public RepositoryZapas(ZapasContext context)
		{
			this.context = context;
		}

		public async Task<List<Zapatilla>> GetZapasAsync()
		{
			return await this.context.Zapas.ToListAsync();
		}

		public async Task<Zapatilla> FindZapatilla(int idproducto)
		{
			return await this.context.Zapas.FirstOrDefaultAsync(x => x.IdProducto == idproducto);
		}

		public async Task<ModelPaginacionImagenes> GetImagenesZapa(int posicion, int idproducto)
		{
			string sql = "SP_IMGAGENES_ZAPATILLA @posicion, @idproducto, @registros out";
			SqlParameter pamPosicion = new SqlParameter("@posicion", posicion);
			SqlParameter pamIdProducto = new SqlParameter("@idproducto", idproducto);
			SqlParameter pamRegistros = new SqlParameter("@registros", -1);
			pamRegistros.Direction = ParameterDirection.Output;

			var consulta = this.context.Imegenes.FromSqlRaw(sql, pamPosicion, pamIdProducto, pamRegistros);
			var datos= await consulta.ToListAsync();
			Imagen imagen = datos.FirstOrDefault();
			int registros = (int)pamRegistros.Value;
			return new ModelPaginacionImagenes
			{
				Registros = registros, 
				Imagen =imagen
			}
		}
	}
}
