using Microsoft.AspNetCore.Components;
using Servicio.Shared.Models;
using Syncfusion.Blazor.Navigations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.HelperService
{
    public class BuscadorEmergenteService
    {
        public object Selecteg { get; set; }

        public event Func<string, object, string[], bool, bool, bool, bool, 
            bool, bool, EventCallback<object>, Task> OnShow;

        public event Func<Task> OnHide;



        public async Task Show(string Titulo, object DataSouce, string[] ColumnasMostar, bool PermiteAgregar,
            bool PermiteEditar, bool PermiteEliminar, bool HabilitaPaginacion, bool HabilitarBuscadorColumna, 
            bool HabilitarMenuColumna, EventCallback<object> EnviarSeleccionado)
        {
            await OnShow?.Invoke(Titulo, DataSouce, ColumnasMostar, PermiteAgregar,
            PermiteEditar, PermiteEliminar, HabilitaPaginacion, HabilitarBuscadorColumna, 
            HabilitarMenuColumna, EnviarSeleccionado);
        }

        

        public async Task Hide()
        {
            await OnHide?.Invoke();
        }

    }
}
