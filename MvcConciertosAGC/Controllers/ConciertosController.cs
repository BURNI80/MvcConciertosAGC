using Microsoft.AspNetCore.Mvc;
using MvcConciertosAGC.Helpers;
using MvcConciertosAGC.Models;
using MvcConciertosAGC.Service;

namespace MvcConciertosAGC.Controllers
{
    public class ConciertosController : Controller
    {
        private ServiceApiConciertos service;
        private ServiceStorageS3 serviceS3;
        private string s3Url;

        public ConciertosController(ServiceApiConciertos service, ServiceStorageS3 serviceS3)
        {
            this.service = service;

            SecretAWS secretos = HelperSecret.GetSecret().Result;
            this.s3Url = secretos.S3;
            this.serviceS3 = serviceS3;
        }

        public async Task<IActionResult> Index()
        {
            List<Evento> eventos = await this.service.GetEventos();

            ViewData["CATEGORIAS"] = await this.service.GetCategorias();
            ViewData["S3"] = this.s3Url;

            return View(eventos);
        }

        [HttpPost]
        public async Task<IActionResult> Index(int idcat)
        {
            List<Evento> eventos = await this.service.GetEventosCategoria(idcat);

            ViewData["CATEGORIAS"] = await this.service.GetCategorias();
            ViewData["S3"] = this.s3Url;

            return View(eventos);
        }


        public async Task<IActionResult> Create()
        { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int id, string nombre, string artista, int idcategoria, IFormFile imagen)
        {
            Evento evento = new Evento
            {
                Id = id,
                Nombre = nombre,
                Artista = artista,
                IdCategoria = idcategoria,
                Imagen = imagen.FileName
            };
            using (Stream stream = imagen.OpenReadStream())
            {
                await this.serviceS3.UploadFileAsync(imagen.FileName, stream);
            }

            await this.service.CreateEvento(evento);
            return RedirectToAction("Index");
        }






    }
}
