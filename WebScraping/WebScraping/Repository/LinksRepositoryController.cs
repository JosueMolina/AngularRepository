using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Web.Http;
using WebScraping.Data;
using WebScraping.Models;

namespace WebScraping.Repository
{
  public class LinksRepositoryController : ApiController
  {

    public HttpResponseMessage Get(bool localData = true)
    {
      if (localData)
      {
        var links = LocalDataRepository.GetLinks();
        return Request.CreateResponse(HttpStatusCode.OK, links);
      }

      var json = GetXmlData();
      var response = Request.CreateResponse(HttpStatusCode.OK);
      response.Content = new StringContent(json, Encoding.UTF8, "application/json");
      return response;
    }

    public HttpResponseMessage Post(Link link)
    {
      ModelState.Remove("Id");

      if (link == null)
      {
        return Request.CreateResponse(HttpStatusCode.BadRequest);
      }

      if (!ModelState.IsValid)
      {
        return Request.CreateResponse(HttpStatusCode.BadRequest, GetErrorsData());
      }
      
      var response = AddLink(link);

      if (response.Success)
      {
        return Request.CreateResponse(HttpStatusCode.Created, response.ResponseObject);
      }

      return Request.CreateResponse(HttpStatusCode.BadRequest, GetErrorsData());
    }

    private IEnumerable<string> GetErrorsData()
    {
      return ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage));
    }

    private Response<Link> AddLink(Link link)
    {

      var response = new Response<Link>();

      try
      {
        using (AngularRepositoryEntities context = new AngularRepositoryEntities())
        {

          var localLink = context.Link.SingleOrDefault(l => l.Name == link.Name);

          if (localLink != null)
          {
            link.Id = localLink.Id;
            link.Rating = localLink.Rating;
            link.NumberOfRatings = localLink.NumberOfRatings;
            response.ResponseObject = link;
          }
          else if (!context.Link.Any(l => l.Name == link.Name))
          {
            context.Link.Add(link);
            context.SaveChanges();
            response.ResponseObject = link;
          }  
            
          response.Success = true;
        }
      }
      catch (DbEntityValidationException dbValidation)
      {
        foreach (var validationErrors in dbValidation.EntityValidationErrors)
        {
          foreach (var validationError in validationErrors.ValidationErrors)
          {
            System.Diagnostics.Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
          }
        }
      }
      catch (Exception ex)
      {
        //something happened
      }
      return response;
    }

    private string GetXmlData()
    {

      const string strUrl = "https://github.com/jmcunningham/AngularJS-Learning";

      // source XML from source web page
      XmlDocument srcDoc = new XmlDocument();
      // XmlElement variable used to construct source XML  
      XmlElement xmlElemRoot = srcDoc.CreateElement(string.Empty, "Lists", string.Empty); ;

      srcDoc.AppendChild(xmlElemRoot);

      WebClient webClient = new WebClient();
      var reqHTML = webClient.DownloadData(strUrl);
      UTF8Encoding objUTF8 = new UTF8Encoding();
      string pageContent = objUTF8.GetString(reqHTML);

      Regex r = new Regex("<h2><a href=\"#[A-Za-z 0-9.\\#-,-’;&/]+\" aria-hidden=\"true\" class=\"anchor\" id=\"[A-Za-z 0-9.\\#-,-’;&/]+\"><svg aria-hidden=\"true\" class=\"octicon octicon-link\" height=\"16\" version=\"1.1\" viewBox=\"0 0 16 16\" width=\"16\"><path fill-rule=\"evenodd\" d=\"[A-Za-z 0-9.\\#-,-’;&*]+\"></path></svg></a>[A-Za-z 0-9.\\#-,-’;&*\\/]+</h2><ul>(<li>(.)+?<a href=\"(.)+?\">(.)+?</a>(.)?</li>)</ul>");
      pageContent = pageContent.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("— 1:10:50", "");
      MatchCollection mcl = r.Matches(pageContent);

      foreach (Match ml in mcl)
      {
        string xmlNode = ml.Groups[0].Value.Replace("imgsrc", "img src").Replace("width", " width").Replace("title", " title").Replace("\\\"", "\"");

        XmlReader xmlReader = XmlReader.Create(new StringReader("<List>" + xmlNode + "</List>"));

        xmlElemRoot.AppendChild(srcDoc.ReadNode(xmlReader));
      }

      string JsonText = JsonConvert.SerializeXmlNode(srcDoc);
      System.Diagnostics.Debug.WriteLine("JSON TEXT : " + JsonText);
      return JsonText;
    }

  }
}
