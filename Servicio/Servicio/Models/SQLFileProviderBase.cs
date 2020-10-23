using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servicio.FileManager.Base;

#if EJ2_DNX
using System.Web.Mvc;
using System.IO.Packaging;
using System.Web;
#else
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
#endif

namespace Servicio.FileManager.Base
{
    public interface SQLFileProviderBase : FileProviderBase
    {
        void SetSQLConnection(string connectionStringName, string tableName, string tableID);
    }

}
