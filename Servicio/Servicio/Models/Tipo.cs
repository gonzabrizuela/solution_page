using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Servicio.Shared.Models
{
    [Table("Tipo")]
    public class Tipo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ColumnaGridViewAtributo(Name = "Id")]
        public int Id { get; set; } = 0;
        [ColumnaGridViewAtributo(Name = "Descripcion")]
        public string Descripcion { get; set; } = "";
    }
}
