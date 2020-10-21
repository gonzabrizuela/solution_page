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

namespace Servicio.Pages.Modelos
{
    public class ModeloPageBase : ComponentBase
    {
        [Inject] protected CustomHttpClient Http { get; set; }
        [Inject] protected IJSRuntime JsRuntime { get; set; }
        protected SfGrid<Modelo> Grid;

        public bool Enabled = true;
        public bool Disabled = false;

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

        public void ActionBeginHandler(ActionEventArgs<Modelo> args)
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
        public async Task ActionBegin(ActionEventArgs<Modelo> args)
        {
            if (args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {
                HttpResponseMessage response;
                bool found = modelos.Any(o => o.Id == args.Data.Id);
                Modelo ur = new Modelo();

                if (!found)
                {
                    response = await Http.PostAsJsonAsync("api/Modelos", args.Data);

                }
                else
                {

                    response = await Http.PutAsJsonAsync($"api/Modelos/{args.Data.Id}", args.Data);
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

        private async Task EliminarServicio(ActionEventArgs<Modelo> args)
        {
            try
            {
                if (args.Data != null)
                {
                    bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Seguro de que desea eliminar el modelo / la reparacion?");
                    if (isConfirmed)
                    {
                        //servicios.Remove(servicios.Find(m => m.PEDIDO == args.Data.PEDIDO));
                        await Http.DeleteAsync($"api/Modelo/{args.Data.Id}");
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
                    foreach (Modelo selectedRecord in this.Grid.SelectedRecords)
                    {
                        bool isConfirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Seguro de que desea copiar el Modelo / la reparacion?");
                        if (isConfirmed)
                        {
                            Modelo Nuevo = new Modelo();

                            //Nuevo.PEDIDO = operarios.Max(s => s.PEDIDO) + 1;
                            Nuevo.Descripcion = selectedRecord.Descripcion;
                          

                            var response = await Http.PostAsJsonAsync("api/Modelo", Nuevo);

                            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                            {
                                Grid.Refresh();
                                var modelo = await response.Content.ReadFromJsonAsync<Modelo>();
                                await InvokeAsync(StateHasChanged);
                                Nuevo.Id = modelo.Id;
                                modelos.Add(Nuevo);
                                var itemsJson = JsonSerializer.Serialize(modelo);
                                Console.WriteLine(itemsJson);
                                //toastService.ShowToast($"Registrado Correctemente.Vale {StockGuardado.VALE}", TipoAlerta.Success);
                                modelos.OrderByDescending(o => o.Id);
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
