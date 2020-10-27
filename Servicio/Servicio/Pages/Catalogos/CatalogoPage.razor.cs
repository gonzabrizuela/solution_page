using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Servicio.Shared.Models;
using Syncfusion.Blazor.FileManager;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Servicio.Pages.Catalogos
{
    public class CatalogoPageBase : ComponentBase
    {
        [Inject] protected CustomHttpClient Http { get; set; }
        [Inject] protected IJSRuntime JsRuntime { get; set; }

        public string[] Items = new string[] { "Open", "|", "Delete", "Download", "Rename", "|", "Details", "Custom" };
        public string[] ToolbarItems = new string[] { "NewFolder", "Upload", "Delete", "Download", "Rename", "SortBy", "Refresh", "Selection", "View", "Details", "Custom" };
        public bool Enabled = true;
        public bool Disabled = false;


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
    }
}

