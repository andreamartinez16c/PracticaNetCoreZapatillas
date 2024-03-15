using Microsoft.EntityFrameworkCore;
using PracticaNetCoreZapatillas.Models;

namespace PracticaNetCoreZapatillas.Data
{
	public class ZapasContext: DbContext
	{
		public ZapasContext(DbContextOptions<ZapasContext> options) : base(options) { }
		public DbSet<Zapatilla> Zapas { get; set; }
		public DbSet<Imagen> Imagenes { get; set;}
	}
}
