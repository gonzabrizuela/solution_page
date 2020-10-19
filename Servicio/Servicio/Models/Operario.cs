using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Servicio.Shared.Models
{
    [Table("Operario")]
    public class Operario
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ColumnaGridViewAtributo(Name = "Codigo operario")]
        public int CG_OPER { get; set; } = 0;
        [ColumnaGridViewAtributo(Name = "descripcion operario")]
        public string DES_OPER { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Codigo de turno")]
        public int CG_TURNO { get; set; } = 0;
        [ColumnaGridViewAtributo(Name = "Rendimiento")]
        public decimal RENDIM { get; set; } = 0;
        [ColumnaGridViewAtributo(Name = "Fecha final")]
        public DateTime? FE_FINAL { get; set; }
        [ColumnaGridViewAtributo(Name = "Horas final")]
        public int HS_FINAL { get; set; } = 0;
        [ColumnaGridViewAtributo(Name = "Codigo categoria Operario")]
        public string CG_CATEOP { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Valor por Hora")]
        public decimal VALOR_HORA { get; set; } = 0;
        [ColumnaGridViewAtributo(Name = "Moneda")]
        public string MONEDA { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Activo")]
        public bool ACTIVO { get; set; } = false;
        [ColumnaGridViewAtributo(Name = "Codigo compañia")]
        public int CG_CIA { get; set; } = 0;
        [ColumnaGridViewAtributo(Name = "Usuario")]
        public string USUARIO { get; set; }
    }
}
