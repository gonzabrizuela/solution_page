using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servicio.FileManager.Base;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Servicio.FileManager.Base
{
    public interface PhysicalFileProviderBase : FileProviderBase
    {        
        void RootFolder(string folderName);
        void SetRules(AccessDetails details);
    }
}
