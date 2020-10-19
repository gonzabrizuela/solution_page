using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Servicio.Shared.Models
{
    [Table("Serie")]
	public class Serie
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[ColumnaGridViewAtributo(Name = "Id")]
		public int Id { get; set; } = 0;
		[ColumnaGridViewAtributo(Name = "Codigo de serie")]
		public string Codigo { get; set; } = "";
		[ColumnaGridViewAtributo(Name = "Medida")]
		public string Medida { get; set; } = "";
	}
}
