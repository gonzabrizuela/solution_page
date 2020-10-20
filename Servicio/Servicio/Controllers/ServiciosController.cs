using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servicio.Server.DataAccess;
using Servicio.Shared.Models;

namespace Servicio.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {
        //private IHostingEnvironment hostingEnv;
        private readonly AppDbContext _context;

        public ServiciosController(AppDbContext context)
        {
            _context = context;
        }

        //// GET: api/Servicios
        ///
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Service>>> GetServicios()
        //{
        //    return await _context.Servicios.ToListAsync();
        //}
        // GET: api/Operario
        [HttpGet]
        public IEnumerable<Service> Get()
        {
            var xitem = _context.Servicios.ToList();
            return xitem;
        }

        // GET: api/Servicios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Service>> GetServicios(string id)
        {
            var Servicios = await _context.Servicios.FindAsync(id);

            if (Servicios == null)
            {
                return NotFound();
            }

            return Servicios;
        }

        // PUT: api/Servicios/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServicios(string id, Service Servicios)
        {
            if (id != Servicios.PEDIDO)
            {
                return BadRequest();
            }

            _context.Entry(Servicios).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiciosExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // fILE

        //public SampleDataController(IHostingEnvironment env)
        //{
        //    this.hostingEnv = env;
        //}

        //[HttpPost("[action]")]
        //public void Save(IList<IFormFile> chunkFile, IList<IFormFile> UploadFiles)
        //{
        //    long size = 0;
        //    try
        //    {
        //        foreach (var file in UploadFiles)
        //        {
        //            var filename = ContentDispositionHeaderValue
        //                    .Parse(file.ContentDisposition)
        //                    .FileName
        //                    .Trim('"');
        //            var folders = filename.Split('/');
        //            var uploaderFilePath = hostingEnv.ContentRootPath;
        //            // for Directory upload
        //            if (folders.Length > 1)
        //            {
        //                for (var i = 0; i < folders.Length - 1; i++)
        //                {
        //                    var newFolder = uploaderFilePath + $@"\{folders[i]}";
        //                    Directory.CreateDirectory(newFolder);
        //                    uploaderFilePath = newFolder;
        //                    filename = folders[i + 1];
        //                }
        //            }
        //            filename = uploaderFilePath + $@"\{filename}";
        //            size += file.Length;
        //            if (!System.IO.File.Exists(filename))
        //            {
        //                using (FileStream fs = System.IO.File.Create(filename))
        //                {
        //                    file.CopyTo(fs);
        //                    fs.Flush();
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Response.Clear();
        //        Response.StatusCode = 204;
        //        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File failed to upload";
        //        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
        //    }
        //}
        //[HttpPost("[action]")]
        //public void Remove(IList<IFormFile> UploadFiles)
        //{
        //    try
        //    {
        //        var filename = hostingEnv.ContentRootPath + $@"\{UploadFiles[0].FileName}";
        //        if (System.IO.File.Exists(filename))
        //        {
        //            System.IO.File.Delete(filename);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Response.Clear();
        //        Response.StatusCode = 200;
        //        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File removed successfully";
        //        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
        //    }
        //}


        //

        // POST: api/Servicios
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Service>> PostServicios(Service Servicios)
        {
            _context.Servicios.Add(Servicios);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ServiciosExists(Servicios.PEDIDO))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetServicios", new { id = Servicios.PEDIDO }, Servicios);
        }

        // DELETE: api/Servicios/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Service>> DeleteServicios(string id)
        {
            var Servicios = await _context.Servicios.FindAsync(id);
            if (Servicios == null)
            {
                return NotFound();
            }

            _context.Servicios.Remove(Servicios);
            await _context.SaveChangesAsync();

            return Servicios;
        }

        private bool ServiciosExists(string id)
        {
            return _context.Servicios.Any(e => e.PEDIDO == id);
        }
    }
}