using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Servicio.Shared.Models
{
    [Table("Medida")]
    public class Medida
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ColumnaGridViewAtributo(Name = "Id")]
        public int Id { get; set; } = 0;
        [ColumnaGridViewAtributo(Name = "Codigo")]
        public string Codigo { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Descripcion")]
        public string Descripcion { get; set; } = "";
    }
}
