using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PracticaNetCoreZapatillas.Data;
using PracticaNetCoreZapatillas.Models;
using System.Data;


#region PROCEDURES
/*create procedure SP_IMAGENES_ZAPATILLA
(@posicion int, @idproducto int
, @registros int out)
as
select @registros = count(IDIMAGEN) from IMAGENESZAPASPRACTICA
where IDPRODUCTO=@idproducto
select IDIMAGEN, IDPRODUCTO, IMAGEN from
(select cast(
ROW_NUMBER() OVER (ORDER BY IMAGEN) as int) AS POSICION
, IDIMAGEN, IDPRODUCTO, IMAGEN
from IMAGENESZAPASPRACTICA
where IDPRODUCTO=@idproducto) as QUERY
WHERE  QUERY.POSICION = @posicion
GO*/
#endregion

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
			string sql = "SP_IMAGENES_ZAPATILLA @posicion, @idproducto, @registros out";
			SqlParameter pamPosicion = new SqlParameter("@posicion", posicion);
			SqlParameter pamIdProducto = new SqlParameter("@idproducto", idproducto);
			SqlParameter pamRegistros = new SqlParameter("@registros", -1);
			pamRegistros.Direction = ParameterDirection.Output;

			var consulta = this.context.Imagenes.FromSqlRaw(sql, pamPosicion, pamIdProducto, pamRegistros);
			var datos = await consulta.ToListAsync();
			/*Imagen imagen = datos.FirstOrDefault();*/
			List<Imagen> imagenes = datos;
			int registros = (int)pamRegistros.Value;
			return new ModelPaginacionImagenes
			{
				Zapatilla = await this.FindZapatilla(idproducto),
				Registros = registros,
				Imagen = imagenes.FirstOrDefault()
			};
		}


		public async Task CreateImagenAsync(List<string> imagenes, int idproducto)
		{
			
            foreach (string imagen in imagenes)
			{
                int idImage = await this.context.Imagenes.MaxAsync(x => x.IdImagen) + 1;
                await this.context.Imagenes.AddAsync
				(
					new Imagen
					{
						IdImagen = idImage,
						IdProducto = idproducto,
						Img = imagen
					}
				);
				await this.context.SaveChangesAsync();
			}
		}
	}
}
