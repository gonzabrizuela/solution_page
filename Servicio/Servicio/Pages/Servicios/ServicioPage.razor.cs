using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Servicio.Shared.Models;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
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

        public bool Enabled = true;
        public bool Disabled = false;

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

        protected List<Object> Toolbaritems = new List<Object>(){
        "Search",
        "Add",
        "Edit",
        "Delete",
        "Print",
        new ItemModel { Text = "Copy", TooltipText = "Copy", PrefixIcon = "e-copy", Id = "copy" },
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
                    response = await Http.PostAsJsonAsync("api/Servicio", args.Data);

                }
                else
                {

                    response = await Http.PutAsJsonAsync($"api/Servicio/{args.Data.PEDIDO}", args.Data);
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
                    bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Seguro de que desea eliminar el servicio / la reparacion?");
                    if (isConfirmed)
                    {
                        //servicios.Remove(servicios.Find(m => m.PEDIDO == args.Data.PEDIDO));
                        await Http.DeleteAsync($"api/Servicio/{args.Data.PEDIDO}");
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

                            //Nuevo.PEDIDO = operarios.Max(s => s.PEDIDO) + 1;
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

                            var response = await Http.PostAsJsonAsync("api/Servicio", Nuevo);

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
                await this.Grid.ExcelExport();
            }
        }

        public void Refresh()
        {
            Grid.Refresh();

        }
    }
}
