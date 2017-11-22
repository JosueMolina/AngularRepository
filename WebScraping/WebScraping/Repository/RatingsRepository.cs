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

    public HttpResponseMessage Get(string linkName)
    {
      if (linkName == null)
      {
        return Request.CreateResponse(HttpStatusCode.BadRequest);
      }

      var response = GettingRate(linkName);

      if (response.Success && response.ResponseObject.Rating > 0)
      {
        decimal rate = (response.ResponseObject.Rating / response.ResponseObject.NumberOfRatings) ?? 0;
        rate = Math.Ceiling(rate * 100) / 100;
        return Request.CreateResponse(HttpStatusCode.OK, rate);
      }

      return Request.CreateResponse(HttpStatusCode.BadRequest, GetErrorsData());
    }
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
        decimal rate = (response.ResponseObject.Rating / response.ResponseObject.NumberOfRatings) ?? 0;
        rate = Math.Ceiling(rate * 100) / 100;
        return Request.CreateResponse(HttpStatusCode.Created, rate);
      }

      return Request.CreateResponse(HttpStatusCode.BadRequest, GetErrorsData());
    }

    private IEnumerable<string> GetErrorsData()
    {
      return ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage));
    }

     private Response<Link> GettingRate(string linkName)
    {
      var response = new Response<Link>();

      try
      {
        using (AngularRepositoryEntities context = new AngularRepositoryEntities())
        {

          var localLink = context.Link.SingleOrDefault(l => l.Name == linkName);

          if (localLink != null)
          {
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
      catch (Exception ex)
      {
        //Something happened
      }
      
      return response;
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
            localLink.Rating += ratingModel.rate;

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
      catch (Exception ex)
      {
        //Something happened
      }
      
      return response;
    }

  }
}