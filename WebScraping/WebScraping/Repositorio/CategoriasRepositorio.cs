using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Newtonsoft.Json;
using System.Web.Http;
using WebScraping.Data;
using System.Data.Entity.Validation;

namespace WebScraping.Repositorio
{
    public class CategoriasRepositorioController : ApiController
    {

        public HttpResponseMessage Post(Categorias categoria)
        {

            if (categoria == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            if (ModelState.IsValid)
            {
                if (guardarCategoria(categoria))
                    return Request.CreateResponse(HttpStatusCode.Created, categoria);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest);

        }

        private bool guardarCategoria(Categorias parmcategoria)
        {

            bool guardado = false;

            using (EnlacesDataBaseEntities _contexto = new EnlacesDataBaseEntities())
            {
                try
                {
                    _contexto.Categorias.Add(parmcategoria);
                    _contexto.SaveChanges();
                    guardado = true;
                }
                catch (DbEntityValidationException DBValidation)
                {
                    foreach (var ValidationErrors in DBValidation.EntityValidationErrors)
                    {
                        foreach (var validationError in ValidationErrors.ValidationErrors)
                        {
                            System.Diagnostics.Trace.TraceInformation("Data Field : {0}, Error Message {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }

            return guardado;
        }
    }
}