using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    public class DatosRepositorioController : ApiController
    {

        [HttpPost]
        [ActionName("guardarPuntuacion")]
        public HttpResponseMessage guardarPuntuacion(guardarPuntuacionObject miObjeto)
        {
            
            Enlaces enlace = BuscarEnlace(miObjeto.enlacePuntuado);

            if (enlace == null || !SumarPuntuacion(miObjeto.puntuacion, enlace.Nombre))
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            return Request.CreateResponse(HttpStatusCode.OK);

        }

        private Enlaces BuscarEnlace(string enlace) {
            using (EnlacesDataBaseEntities _contexto = new EnlacesDataBaseEntities())
            {
                return _contexto.Enlaces.Where(e => e.Nombre == enlace).SingleOrDefault();
            }
        }

        private bool SumarPuntuacion(double puntuacion, string nombreEnlace){
            bool sumado = false;
            using (EnlacesDataBaseEntities _contexto = new EnlacesDataBaseEntities())
            {
                try
                {
                    decimal puntuacionActual = _contexto.Enlaces.Where(e => e.Nombre == nombreEnlace).SingleOrDefault().Puntuacion;
                    int numeroPuntuaciones = _contexto.Enlaces.Where(e => e.Nombre == nombreEnlace).SingleOrDefault().NumeroPuntuaciones;
                    decimal nuevaPuntuacion = puntuacionActual + (decimal)puntuacion;
                    Enlaces enlace = _contexto.Enlaces.Where(e => e.Nombre == nombreEnlace).SingleOrDefault();
                    enlace.Puntuacion = nuevaPuntuacion;
                    enlace.NumeroPuntuaciones++;
                    _contexto.SaveChanges();

                    sumado = true;
                }
                catch (Exception)
                {
                }
                
            }
            return sumado;
        }

        public HttpResponseMessage Get(bool localData = true)
        {
            if (localData)
            {
                EnlacesDTO[] enlaces = LocalDataRepository.ObtenerEnlaces();
                return Request.CreateResponse(HttpStatusCode.OK, enlaces);
            }

            string Json = CargarDatosXML();
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(Json, Encoding.UTF8, "application/json");
            return response;

        }

        public HttpResponseMessage Post(Enlaces enlace)
        {
            ModelState.Remove("Id");

            if (enlace == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (GuardarEnlaces(enlace))
                        return Request.CreateResponse(HttpStatusCode.Created);
                }
                catch (Exception ex)
                {
                    //throw;
                }
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, GetErrorsData());
        }

        private IEnumerable<string> GetErrorsData()
        {
            return ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage));
        }
        
        XmlDocument srcDoc = new XmlDocument(); // source XML from source web page
        XmlElement xmlelemRoot;// XmlElement variable used to construct source XML

        private string CargarDatosXML()
        {

            xmlelemRoot = srcDoc.CreateElement("", "Listas", "");
            srcDoc.AppendChild(xmlelemRoot);

            WebClient webClient = new WebClient();
            const string strUrl = "https://github.com/jmcunningham/AngularJS-Learning";
            byte[] reqHTML;
            reqHTML = webClient.DownloadData(strUrl);
            UTF8Encoding objUTF8 = new UTF8Encoding();
            string pageContent = objUTF8.GetString(reqHTML);
            string ministerContent = string.Empty;

            // use regular expression to find matching data portion \"http://[A-Za-z.]+.ca/[0-9A-Za-z./&=;\\?]+\"
            //Regex r = new Regex("<div class=\"grid_3 noborder center\"><a href=\"biography.asp\\?MPPID=[0-9]+\"><imgsrc=\"http://www.premier.gov.on.ca/photos/team/[A-Za-z]+.jpg\"width=\"144\" height=\"171\" alt=\"[A-Za-z .]+'s Biography\" /></a></div><div class=\"grid_3\"><h3><a href=\"biography.asp\\?MPPID=[0-9]+\"title=\"[A-Za-z .]+'s Biography\">[A-Za-z .]+</a></h3><p>[A-Za-z .,’'-]+(<br />[A-Za-z .,’'-]+)+</p><ul>(<li><a href=\"http://[A-Za-z.]+.ca/[0-9A-Za-z./&=;\\?]+\">[0-9A-Za-z .,-’;&#]+</a></li>)+</ul></div>");
            //Regex r = new Regex("<h2><a name=\"[A-Za-z 0-9.\\#-,-’;&]+\" class=\"anchor\" href=\"#[A-Za-z 0-9.\\#-,-’;&]+\"><span class=\"octicon octicon-link\"></span></a>[A-Za-z 0-9.\\#-,-’;&*]+</h2><ul>(<li><a href=\"https?://[A-Za-z]+.[A-Za-z 0-9_!.:.&;#*/?=-]+\">[A-Za-z 0-9.:.&;#*/?=()-]+</a></li>)+</ul>");
            Regex r = new Regex("<h2><a name=\"[A-Za-z 0-9.\\#-,-’;&/]+\" class=\"anchor\" href=\"#[A-Za-z 0-9.\\#-,-’;&/]+\"><span class=\"octicon octicon-link\"></span></a>[A-Za-z 0-9.\\#-,-’;&*]+</h2><ul>(<li><a href=\"https?://[A-Za-z]+.[A-Za-z 0-9_!:.&;#*/?=-]+\">[A-Za-z 0-9:.&;#*/?=()-]+</a></li>)+");
            pageContent = pageContent.Replace("\r", "").Replace("\n", "").Replace("\t", "");
            MatchCollection mcl = r.Matches(pageContent);

            // loop through each minister to construct the source XML
            foreach (Match ml in mcl)
            {
                string xmlNode = ml.Groups[0].Value.Replace("imgsrc", "img src").Replace("width", " width").Replace("title", " title").Replace("\\\"", "\"");

                XmlReader xmlReader = XmlReader.Create(new StringReader("<Lista>" + xmlNode + "</ul></Lista>"));

                xmlelemRoot.AppendChild(srcDoc.ReadNode(xmlReader));
            }

            string JsonText = JsonConvert.SerializeXmlNode(srcDoc);
            return JsonText;
        }

        private bool GuardarEnlaces(Enlaces enlace)
        {
            bool guardado = false;

            try
            {
                using (EnlacesDataBaseEntities _contexto = new EnlacesDataBaseEntities())
                {
                    _contexto.Enlaces.Add(enlace);
                    _contexto.SaveChanges();
                    guardado = true;
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Diagnostics.Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }


            }


            return guardado;
            
        }

    }

    public class guardarPuntuacionObject {
        public string enlacePuntuado { get; set; }
        public double puntuacion { get; set; }
    }
}
