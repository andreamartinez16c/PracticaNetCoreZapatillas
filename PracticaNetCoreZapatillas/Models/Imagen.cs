using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PracticaNetCoreZapatillas.Models
{
	[Table("IMAGENESZAPASPRACTICA")]
	public class Imagen
	{

		[Key]
		[Column("IDIMAGEN")]
		public int IdImagen { get; set; }
		[Column("IDPRODUCTO")]
		public int IdProducto { get; set; }
		[Column("IMAGEN")]
		public string Img { get; set; }
	}
}
