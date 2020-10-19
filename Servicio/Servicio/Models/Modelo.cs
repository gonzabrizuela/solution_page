using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Servicio.Shared.Models
{
    [Table("Modelo")]
	public class Modelo
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[ColumnaGridViewAtributo(Name = "Id")]
		public int Id { get; set; } = 0;
		[ColumnaGridViewAtributo(Name = "Descripcion")]
		public string Descripcion { get; set; } = "";
	}
}
