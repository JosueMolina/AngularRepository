using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebScraping.Data;
using WebScraping.Models;

namespace WebScraping.Repository
{
  public class RatingsRepositoryController : ApiController
  {

    public HttpResponseMessage Post(RatingModel rating)
    {

      if (rating == null)
      {
        return Request.CreateResponse(HttpStatusCode.BadRequest);
      }

      if (!ModelState.IsValid)
      {
        return Request.CreateResponse(HttpStatusCode.BadRequest, GetErrorsData());
      }

      var response = Rating(rating);

      if (response.Success)
      {
        return Request.CreateResponse(HttpStatusCode.Created, response.ResponseObject.Rating);
      }

      return Request.CreateResponse(HttpStatusCode.BadRequest, GetErrorsData());
    }

    private IEnumerable<string> GetErrorsData()
    {
      return ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage));
    }

    private Response<Link> Rating(RatingModel ratingModel)
    {
      var response = new Response<Link>();

      try
      {
        using (AngularRepositoryEntities context = new AngularRepositoryEntities())
        {

          var localLink = context.Link.SingleOrDefault(l => l.Name == ratingModel.title);

          if (localLink != null)
          {
            localLink.NumberOfRatings = localLink.NumberOfRatings + 1;

            decimal actualRate = localLink.Rating ?? 0;

            actualRate = (actualRate + ratingModel.rate) / localLink.NumberOfRatings ?? 0;
            actualRate = Math.Ceiling(actualRate * 100) / 100;
            localLink.Rating = actualRate;
            context.SaveChanges();

            response.ResponseObject = localLink;
            response.Success = true;
          } 
        }
      }
      catch (DbEntityValidationException dbValidation)
      {
        foreach (var ValidationErrors in dbValidation.EntityValidationErrors)
        {
          foreach (var validationError in ValidationErrors.ValidationErrors)
          {
            System.Diagnostics.Trace.TraceInformation("Data Field : {0}, Error Message {1}", validationError.PropertyName, validationError.ErrorMessage);
          }
        }
      }
      
      return response;
    }

  }
}