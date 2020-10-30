using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Servicio.Shared.Models;
using Syncfusion.Blazor.FileManager;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.RichTextEditor.Internal;
using Syncfusion.ExcelExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Servicio.Pages.Servicios
{
    public class ServicioPageBase : ComponentBase
    {
        [Inject] protected CustomHttpClient Http { get; set; }
        [Inject] protected IJSRuntime JsRuntime { get; set; }
        protected SfGrid<Service> Grid;
        public string NroPedido = "";
        public bool Enabled = true;
        public bool Disabled = false;
        public bool Visible { get; set; } = true;

        public string[] Items = new string[] { "Open", "|", "Delete", "Download", "|", "Details" };
        public string[] ToolbarItems = new string[] { "NewFolder", "Upload", "Delete", "Download", "SortBy", "Refresh", "Selection", "View", "Details" };
        public void toolbarClick(ToolbarClickEventArgs args)
        {
            if (args.Item.Text == "Custom")
            {
                // Perform the operation based on your requirement.
                System.Diagnostics.Debug.Write("Custom item clicked");
            }
        }
        public void menuClick(MenuClickEventArgs args)
        {
            if (args.Item.Text == "Custom")
            {
                // Perform the operation based on your requirement.
                System.Diagnostics.Debug.WriteLine("Custom item clicked");
            }
        }
        public class SIoNO
        {
            public string Text { get; set; }
        }
        public List<SIoNO> SIoNOData = new List<SIoNO> {
            new SIoNO() {Text= "SI"},
            new SIoNO() {Text= "NO"}};

        protected List<Service> servicios = new List<Service>();
        protected List<Modelo> modelos = new List<Modelo>();
        protected List<Medida> medidas = new List<Medida>();
        protected List<Serie> series = new List<Serie>();
        protected List<Orificio> orificios = new List<Orificio>();
        protected List<Sobrepresion> sobrepresiones = new List<Sobrepresion>();
        protected List<Tipo> tipos = new List<Tipo>();
        protected List<Estado> estados = new List<Estado>();
        protected List<Trabajosefec> trabajosEfectuados = new List<Trabajosefec>();
        protected List<Marca> marcas = new List<Marca>();
        protected List<Operario> operarios = new List<Operario>();
        protected List<Celdas> celdas = new List<Celdas>();

        protected List<Object> Toolbaritems = new List<Object>(){
        "Search",
        "Edit",
        "Delete",
        "Print",
        new Syncfusion.Blazor.Navigations.ItemModel { Text = "Copy", TooltipText = "Copy", PrefixIcon = "e-copy", Id = "copy" },
        "ExcelExport"
    };

        protected override async Task OnInitializedAsync()
        {
            servicios = await Http.GetFromJsonAsync<List<Service>>("api/Servicios");
            modelos = await Http.GetFromJsonAsync<List<Modelo>>("api/Modelo");
            medidas = await Http.GetFromJsonAsync<List<Medida>>("api/Medida");
            series = await Http.GetFromJsonAsync<List<Serie>>("api/Serie");
            orificios = await Http.GetFromJsonAsync<List<Orificio>>("api/Orificio");
            sobrepresiones = await Http.GetFromJsonAsync<List<Sobrepresion>>("api/Sobrepresion");
            tipos = await Http.GetFromJsonAsync<List<Tipo>>("api/Tipo");
            estados = await Http.GetFromJsonAsync<List<Estado>>("api/Estado");
            trabajosEfectuados = await Http.GetFromJsonAsync<List<Trabajosefec>>("api/TrabajosEfec");
            marcas = await Http.GetFromJsonAsync<List<Marca>>("api/Marca");
            operarios = await Http.GetFromJsonAsync<List<Operario>>("api/Operario");
            celdas = await Http.GetFromJsonAsync<List<Celdas>>("api/Celdas");

            await base.OnInitializedAsync();
        }

        public void ActionBeginHandler(ActionEventArgs<Service> args)
        {
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.BeginEdit)
            {
                this.Enabled = false;
            }
            else
            {
                this.Enabled = true;
            }
        }
        public async Task ActionBegin(ActionEventArgs<Service> args)
        {
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {
                HttpResponseMessage response;
                bool found = servicios.Any(o => o.PEDIDO == args.Data.PEDIDO);
                Service ur = new Service();

                if (!found)
                {
                    args.Data.PEDIDO = servicios.Max(s => s.PEDIDO) + 1;
                    response = await Http.PostAsJsonAsync("api/Servicios", args.Data);
                }
                else
                {
                    if (args.Data.PEDIDOANT != "")
                    {
                        foreach (var rep in servicios)
                        {
                            if (args.Data.PEDIDOANT == rep.PEDIDO)
                            {
                                if (args.Data.CLIENTE != rep.CLIENTE)
                                {
                                    bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Quiere traer los datos del pedido anterior?");
                                    if (isConfirmed)
                                    {
                                        if (args.Data.PEDIDOANT == rep.PEDIDO)
                                        {
                                            args.Data.CLIENTE = rep.CLIENTE;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    response = await Http.PutAsJsonAsync($"api/Servicios/{args.Data.PEDIDO}", args.Data);
                }

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {

                }
            }

            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
            {
                await EliminarServicio(args);
            }
        }

        private async Task EliminarServicio(ActionEventArgs<Service> args)
        {
            try
            {
                if (args.Data != null)
                {
                    bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", $"Seguro de que desea eliminar la reparacion {args.Data.PEDIDO}?");
                    if (isConfirmed)
                    {
                        //servicios.Remove(servicios.Find(m => m.PEDIDO == args.Data.PEDIDO));
                        await Http.DeleteAsync($"api/Servicios/{args.Data.PEDIDO}");
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task ClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            if (args.Item.Text == "Copy")
            {
                if (this.Grid.SelectedRecords.Count > 0)
                {
                    foreach (Service selectedRecord in this.Grid.SelectedRecords)
                    {
                        bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Seguro de que desea copiar el Servicios / la reparacion?");
                        if (isConfirmed)
                        {
                            Service Nuevo = new Service();

                            Nuevo.PEDIDO = servicios.Max(s => s.PEDIDO) + 1;
                            Nuevo.FECHA = selectedRecord.FECHA;
                            Nuevo.CLIENTE = selectedRecord.CLIENTE;
                            Nuevo.PLANTA = selectedRecord.PLANTA;
                            Nuevo.OCOMPRA = selectedRecord.OCOMPRA;
                            Nuevo.REMITOREC = selectedRecord.REMITOREC;
                            Nuevo.REMITO = selectedRecord.REMITO;
                            Nuevo.IDENTIFICACION = selectedRecord.IDENTIFICACION;
                            Nuevo.NSERIE = selectedRecord.NSERIE;
                            Nuevo.MARCA = selectedRecord.MARCA;
                            Nuevo.MODELO = selectedRecord.MODELO;
                            Nuevo.MEDIDA = selectedRecord.MEDIDA;
                            Nuevo.SERIE = selectedRecord.SERIE;
                            Nuevo.ORIFICIO = selectedRecord.ORIFICIO;
                            Nuevo.AREA = selectedRecord.AREA;
                            Nuevo.FLUIDO = selectedRecord.FLUIDO;
                            Nuevo.AÑO = selectedRecord.AÑO;
                            Nuevo.PRESION = selectedRecord.PRESION;
                            Nuevo.TEMP = selectedRecord.TEMP;
                            Nuevo.PRESIONBANCO = selectedRecord.PRESIONBANCO;
                            Nuevo.SOBREPRESION = selectedRecord.SOBREPRESION;
                            Nuevo.CONTRAPRESION = selectedRecord.CONTRAPRESION;
                            Nuevo.TIPO = selectedRecord.TIPO;
                            Nuevo.RESORTE = selectedRecord.RESORTE;
                            Nuevo.SERVICIO = selectedRecord.SERVICIO;
                            Nuevo.ENSRECEP = selectedRecord.ENSRECEP;
                            Nuevo.ESTADO = selectedRecord.ESTADO;
                            Nuevo.PRESIONRECEP = selectedRecord.PRESIONRECEP;
                            Nuevo.FUGAS = selectedRecord.FUGAS;
                            Nuevo.PRESIONFUGA = selectedRecord.PRESIONFUGA;
                            Nuevo.CAMBIOPRESION = selectedRecord.CAMBIOPRESION;
                            Nuevo.PRESIONSOLIC = selectedRecord.PRESIONSOLIC;
                            Nuevo.CAMBIOREPUESTO = selectedRecord.CAMBIOREPUESTO;
                            Nuevo.CODRESORTE = selectedRecord.CODRESORTE;
                            Nuevo.REPUESTOS = selectedRecord.REPUESTOS;
                            Nuevo.TRABAJOSEFEC = selectedRecord.TRABAJOSEFEC;
                            Nuevo.TRABAJOSACCES = selectedRecord.TRABAJOSACCES;
                            Nuevo.MANOMETRO = selectedRecord.MANOMETRO;
                            Nuevo.FECMANTANT = selectedRecord.FECMANTANT;
                            Nuevo.PEDIDOANT = selectedRecord.PEDIDOANT;
                            Nuevo.ENSAYOCONTRAP = selectedRecord.ENSAYOCONTRAP;
                            Nuevo.RESP = selectedRecord.RESP;
                            Nuevo.CONTROLO = selectedRecord.CONTROLO;
                            Nuevo.POP = selectedRecord.POP;
                            Nuevo.RESPTECNICO = selectedRecord.RESPTECNICO;
                            Nuevo.OPDS = selectedRecord.OPDS;
                            Nuevo.ACTA = selectedRecord.ACTA;
                            Nuevo.PRESENCIAINSPEC = selectedRecord.PRESENCIAINSPEC;
                            Nuevo.DESCARTICULO = selectedRecord.DESCARTICULO;
                            Nuevo.OBSERV = selectedRecord.OBSERV;

                            var response = await Http.PostAsJsonAsync("api/Servicios", Nuevo);

                            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                            {
                                Grid.Refresh();
                                var servicio = await response.Content.ReadFromJsonAsync<Service>();
                                await InvokeAsync(StateHasChanged);
                                Nuevo.PEDIDO = servicio.PEDIDO;
                                servicios.Add(Nuevo);
                                var itemsJson = JsonSerializer.Serialize(servicio);
                                Console.WriteLine(itemsJson);
                                //toastService.ShowToast($"Registrado Correctemente.Vale {StockGuardado.VALE}", TipoAlerta.Success);
                                servicios.OrderByDescending(o => o.PEDIDO);
                            }

                        }
                    }
                }
            }
            if (args.Item.Text == "Excel Export")
            {
                if (this.Grid.SelectedRecords.Count > 0)
                {
                    foreach (Service selectedRecord in this.Grid.SelectedRecords)
                    {
                        ExcelExportProperties Props = new ExcelExportProperties();
                        List<ExcelCell> ExcelCells1 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 2, RowSpan = 2, Value = "ARBROS S.A.", Style = new ExcelStyle() { FontColor = "#001C8C", FontSize = 19 , HAlign = ExcelHorizontalAlign.Center, VAlign = ExcelVerticalAlign.Center, Bold = true } },
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() { Index = 3, ColSpan = 2, RowSpan = 1, Value = "CERTIFICADO DE MANTENIMIENTO Y CALIBRACIÓN", Style = new ExcelStyle() { FontColor = "#001C8C", FontSize = 12, HAlign = ExcelHorizontalAlign.Center, VAlign = ExcelVerticalAlign.Center, Bold = true } },
                            new ExcelCell() {Index = 4, Value = "" },
                            new ExcelCell() { Index = 5, ColSpan = 2, RowSpan = 1, Value = $"Fecha: {DateTime.Today.Day} / {DateTime.Today.Month} / {DateTime.Today.Year}", Style = new ExcelStyle() { FontColor = "#001C8C", FontSize = 12, HAlign = ExcelHorizontalAlign.Center, VAlign = ExcelVerticalAlign.Center, Bold = true } },
                        };
                        List<ExcelCell> ExcelCells2 = new List<ExcelCell> {
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() { Index = 3, ColSpan = 2, RowSpan = 1, Value = "VALVULA DE SEGURIDAD Y ALIVIO", Style = new ExcelStyle() { FontColor = "#001C8C", FontSize = 12, HAlign = ExcelHorizontalAlign.Center, VAlign = ExcelVerticalAlign.Center, Bold = true } },
                            new ExcelCell() {Index = 4, Value = "" },
                            new ExcelCell() { Index = 5, ColSpan = 2, RowSpan = 1, Value = $"Pedido Número: {selectedRecord.PEDIDO}", Style = new ExcelStyle() { FontColor = "#001C8C", FontSize = 12, HAlign = ExcelHorizontalAlign.Center, VAlign = ExcelVerticalAlign.Center, Bold = true } },
                        };
                        List<ExcelCell> ExcelCells3 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 6, Value = "ENSAYOS EFECTUADOS EN BANCO CON PULMÓN HIDRO NEUMÁTICO. FLUIDO DE PRUEBA: AIRE A TEMPERATURA AMBIENTE", Style = new ExcelStyle() { FontColor = "#000000", FontSize = 10, HAlign = ExcelHorizontalAlign.Center, VAlign = ExcelVerticalAlign.Center, Bold = true}}};
                        List<ExcelCell> ExcelCells4 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 6, Value = "GENERALIDADES", Style = new ExcelStyle() { FontColor = "#000000", FontSize = 10, HAlign = ExcelHorizontalAlign.Center, VAlign = ExcelVerticalAlign.Center, Bold = true, Italic = true, Underline = true }}};
                        List<ExcelCell> ExcelCells5 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Cliente: {selectedRecord.CLIENTE}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Planta: {selectedRecord.PLANTA}"}};
                        List<ExcelCell> ExcelCells6 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Orden de Compra: {selectedRecord.OCOMPRA}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Remito Recepción: {selectedRecord.REMITOREC}"}};
                        List<ExcelCell> ExcelCells7 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Observaciones: {selectedRecord.OBSERV}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Descripción Artículo: {selectedRecord.DESCARTICULO}"}};
                        List<ExcelCell> ExcelCells8 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Pedido Anterior: {selectedRecord.PEDIDOANT}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Fecha de Mantenimiento Anterior: {selectedRecord.FECMANTANT}"}};
                        List<ExcelCell> ExcelCells9 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 6, Value = $"Remito: {selectedRecord.REMITO}"}};
                        List<ExcelCell> ExcelCells10 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 6, Value = "DATOS DE PLACA", Style = new ExcelStyle() { FontColor = "#000000", FontSize = 10, HAlign = ExcelHorizontalAlign.Center, VAlign = ExcelVerticalAlign.Center, Bold = true, Italic = true, Underline = true }}};
                        List<ExcelCell> ExcelCells11 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"TAG: {selectedRecord.IDENTIFICACION}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Marca: {selectedRecord.MARCA}"}};
                        List<ExcelCell> ExcelCells12 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Número de Serie: {selectedRecord.NSERIE}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Modelo: {selectedRecord.MODELO}"}};
                        List<ExcelCell> ExcelCells13 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Medida: {selectedRecord.MEDIDA}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Serie: {selectedRecord.SERIE}"}};
                        List<ExcelCell> ExcelCells14 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Orificio: {selectedRecord.ORIFICIO}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Año: {selectedRecord.AÑO}"}};
                        List<ExcelCell> ExcelCells15 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Area: {selectedRecord.AREA}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Fluido: {selectedRecord.FLUIDO}"}};
                        List<ExcelCell> ExcelCells16 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Sobrepresión: {selectedRecord.SOBREPRESION}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Presión: {selectedRecord.PRESION}"}};
                        List<ExcelCell> ExcelCells17 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Contrapresión: {selectedRecord.CONTRAPRESION}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Tipo: {selectedRecord.TIPO}"}};
                        List<ExcelCell> ExcelCells18 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Temperatura: {selectedRecord.TEMP}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Resorte: {selectedRecord.RESORTE}"}};
                        List<ExcelCell> ExcelCells19 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Presión en Banco: {selectedRecord.PRESIONBANCO}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Servicio: {selectedRecord.SERVICIO}"}};
                        List<ExcelCell> ExcelCells20 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 6, Value = "ENSAYOS A LA RECEPCIÓN", Style = new ExcelStyle() { FontColor = "#000000", FontSize = 10, HAlign = ExcelHorizontalAlign.Center, VAlign = ExcelVerticalAlign.Center, Bold = true, Italic = true, Underline = true }}};
                        List<ExcelCell> ExcelCells21 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Ensayo a la Recepcion: {selectedRecord.ENSRECEP}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Estado: {selectedRecord.ESTADO}"}};
                        List<ExcelCell> ExcelCells22 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Presión Ensayo Recepción: {selectedRecord.PRESIONRECEP}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Fugas: {selectedRecord.FUGAS}"}};
                        List<ExcelCell> ExcelCells23 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Presión de Fuga: {selectedRecord.PRESIONFUGA}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Cambio de Presión: {selectedRecord.CAMBIOPRESION}"}};
                        List<ExcelCell> ExcelCells24 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 6, Value = $"Presión Solicitada: {selectedRecord.PRESIONSOLIC}"}};
                        List<ExcelCell> ExcelCells25 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 6, Value = "TRABAJOS EFECTUADOS", Style = new ExcelStyle() { FontColor = "#000000", FontSize = 10, HAlign = ExcelHorizontalAlign.Center, VAlign = ExcelVerticalAlign.Center, Bold = true, Italic = true, Underline = true }}};
                        List<ExcelCell> ExcelCells26 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Cambio de Repuestos: {selectedRecord.CAMBIOREPUESTO}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Repuestos: {selectedRecord.REPUESTOS}"}};
                        List<ExcelCell> ExcelCells27 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Código de Resorte: {selectedRecord.CODRESORTE}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Ensayo Contrapresion: {selectedRecord.ENSAYOCONTRAP}"}};
                        List<ExcelCell> ExcelCells28 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Trabajos efectuados: {selectedRecord.TRABAJOSEFEC}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Trabajos Accesorios: {selectedRecord.TRABAJOSACCES}"}};
                        List<ExcelCell> ExcelCells29 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Responsable: {selectedRecord.RESP}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Controló: {selectedRecord.CONTROLO}"}};
                        List<ExcelCell> ExcelCells30 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"POP: {selectedRecord.POP}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Responsable Técnico: {selectedRecord.RESPTECNICO}"}};
                        List<ExcelCell> ExcelCells31 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"OPDS: {selectedRecord.OPDS}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Acta: {selectedRecord.ACTA}"}};
                        List<ExcelCell> ExcelCells32 = new List<ExcelCell> {
                            new ExcelCell() { Index = 1, ColSpan = 3, Value = $"Presencia Inspector: {selectedRecord.PRESENCIAINSPEC}"},
                            new ExcelCell() {Index = 2, Value = "" },
                            new ExcelCell() {Index = 3, Value = "" },
                            new ExcelCell() { Index = 4, ColSpan = 3, Value = $"Responsable Técnico: {selectedRecord.RESPTECNICO}"}};
                        List<ExcelRow> ExcelRows = new List<ExcelRow> {
                            new ExcelRow() { Index = 1,  Cells = ExcelCells1 },
                            new ExcelRow() { Index = 2,  Cells = ExcelCells2 },
                            new ExcelRow() { Index = 3,  Cells = ExcelCells3 },
                            new ExcelRow() { Index = 4,  Cells = ExcelCells4 },
                            new ExcelRow() { Index = 5,  Cells = ExcelCells5 },
                            new ExcelRow() { Index = 6,  Cells = ExcelCells6 },
                            new ExcelRow() { Index = 7,  Cells = ExcelCells7 },
                            new ExcelRow() { Index = 8,  Cells = ExcelCells8 },
                            new ExcelRow() { Index = 9,  Cells = ExcelCells9 },
                            new ExcelRow() { Index = 10,  Cells = ExcelCells10 },
                            new ExcelRow() { Index = 11,  Cells = ExcelCells11 },
                            new ExcelRow() { Index = 12,  Cells = ExcelCells12 },
                            new ExcelRow() { Index = 13,  Cells = ExcelCells13 },
                            new ExcelRow() { Index = 14,  Cells = ExcelCells14 },
                            new ExcelRow() { Index = 15,  Cells = ExcelCells15 },
                            new ExcelRow() { Index = 16,  Cells = ExcelCells16 },
                            new ExcelRow() { Index = 17,  Cells = ExcelCells17 },
                            new ExcelRow() { Index = 18,  Cells = ExcelCells18 },
                            new ExcelRow() { Index = 19,  Cells = ExcelCells19 },
                            new ExcelRow() { Index = 20,  Cells = ExcelCells20 },
                            new ExcelRow() { Index = 21,  Cells = ExcelCells21 },
                            new ExcelRow() { Index = 22,  Cells = ExcelCells22 },
                            new ExcelRow() { Index = 23,  Cells = ExcelCells23 },
                            new ExcelRow() { Index = 24,  Cells = ExcelCells24 },
                            new ExcelRow() { Index = 25,  Cells = ExcelCells25 },
                            new ExcelRow() { Index = 26,  Cells = ExcelCells26 },
                            new ExcelRow() { Index = 27,  Cells = ExcelCells27 },
                            new ExcelRow() { Index = 28,  Cells = ExcelCells28 },
                            new ExcelRow() { Index = 29,  Cells = ExcelCells29 },
                            new ExcelRow() { Index = 30,  Cells = ExcelCells30 },
                            new ExcelRow() { Index = 31,  Cells = ExcelCells31 },
                            new ExcelRow() { Index = 32,  Cells = ExcelCells32 }};
                        List<ExcelCell> FooterCells1 = new List<ExcelCell> { new ExcelCell() { ColSpan = 6, Value = "Thank you for your business!", Style = new ExcelStyle() { FontColor = "#C67878", HAlign = ExcelHorizontalAlign.Center, Bold = true } } };
                        List<ExcelCell> FooterCells2 = new List<ExcelCell> { new ExcelCell() { ColSpan = 6, Value = "!Visit Again!", Style = new ExcelStyle() { FontColor = "#C67878", HAlign = ExcelHorizontalAlign.Center, Bold = true } } };
                        ExcelHeader Header = new ExcelHeader()
                        {
                            HeaderRows = 32,
                            Rows = ExcelRows
                        };
                        Props.Header = Header;
                        Props.FileName = $"Reporte{selectedRecord.PEDIDO}.xlsx";
                        await this.Grid.ExcelExport(Props);
                    }
                }
                //await this.Grid.ExcelExport();
            }
            if (args.Item.Text == "Edit")
            {
                if (this.Grid.SelectedRecords.Count > 0)
                {
                    foreach (Service selectedRecord in this.Grid.SelectedRecords)
                    {
                        NroPedido = selectedRecord.PEDIDO;
                    }
                }
            }
        }

        public void Refresh()
        {
            Grid.Refresh();
        }
    }
}
