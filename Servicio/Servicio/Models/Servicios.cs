using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Servicio.Shared.Models
{
    [Table("Servicios")]
    public class Service
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ColumnaGridViewAtributo(Name = "Pedido")]
        public string PEDIDO { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Fecha")]
        public DateTime? FECHA { get; set; }
        [ColumnaGridViewAtributo(Name = "Cliente")]
        public string CLIENTE { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Planta")]
        public string PLANTA { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Orden de Compra")]
        public string OCOMPRA { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Remito Recepcion")]
        public string REMITOREC { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Remito")]
        public string REMITO { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Identificacion")]
        public string IDENTIFICACION { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Numero de Serie")]
        public string NSERIE { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Marca")]
        public string MARCA { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Modelo")]
        public string MODELO { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Medida")]
        public string MEDIDA { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Serie")]
        public string SERIE { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Orificio")]
        public string ORIFICIO { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Area")]
        public string AREA { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Fluido")]
        public string FLUIDO { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Año")]
        public string AÑO { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Presion")]
        public string PRESION { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Temperatura")]
        public string TEMP { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Presion en Banco")]
        public string PRESIONBANCO { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Sobrepresion")]
        public string SOBREPRESION { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Contrapresion")]
        public string CONTRAPRESION { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Tipo")]
        public string TIPO { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Resorte")]
        public string RESORTE { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Servicio")]
        public string SERVICIO { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Ensayo a la recepcion")]
        public string ENSRECEP { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Estado")]
        public string ESTADO { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Presion recepcion")]
        public string PRESIONRECEP { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Fugas")]
        public string FUGAS { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Presion de fuga")]
        public string PRESIONFUGA { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Cambio de presion")]
        public string CAMBIOPRESION { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Presion solicitada")]
        public string PRESIONSOLIC { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Cambio de repuesto")]
        public string CAMBIOREPUESTO { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Codigo de resorte")]
        public string CODRESORTE { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Repuestos")]
        public string REPUESTOS { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Trabajos efectuados")]
        public string TRABAJOSEFEC { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Trabajos accesorios")]
        public string TRABAJOSACCES { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Manometro")]
        public string MANOMETRO { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Fecha de mantenimiento anterior")]
        public string FECMANTANT { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Pedido anterior")]
        public string PEDIDOANT { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Ensayo contrapresion")]
        public string ENSAYOCONTRAP { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Responsable")]
        public string RESP { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Controlo")]
        public string CONTROLO { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "POP")]
        public string POP { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Responsable tecnico")]
        public string RESPTECNICO { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "OPDS")]
        public string OPDS { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Acta")]
        public string ACTA { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Presencia inspector")]
        public string PRESENCIAINSPEC { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Descripcion articulo")]
        public string DESCARTICULO { get; set; } = "";
        [ColumnaGridViewAtributo(Name = "Observaciones")]
        public string OBSERV { get; set; } = "";
    }
}
