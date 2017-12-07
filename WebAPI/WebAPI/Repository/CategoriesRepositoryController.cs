using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Repository
{
  public class CategoriesRepositoryController : ApiController
  {

    public HttpResponseMessage Post(Category category)
    {
      ModelState.Remove("Id");

      if (category == null)
      {
        return Request.CreateResponse(HttpStatusCode.BadRequest);
      }

      if (!ModelState.IsValid)
      {
        return Request.CreateResponse(HttpStatusCode.BadRequest, GetErrorsData());
      }
      
      var response = AddCategory(category);

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

    private Response<Category> AddCategory(Category category)
    {
      var response = new Response<Category>();

      try
      {
        using (AngularRepositoryEntities context = new AngularRepositoryEntities())
        {

          var localCategory = context.Category.SingleOrDefault(c => c.CategoryName == category.CategoryName);

          if (localCategory != null)
          {
            category.Id = localCategory.Id;
            response.ResponseObject = category;
          } else if (!context.Category.Any(c => c.CategoryName == category.CategoryName))
          {
            context.Category.Add(category);
            context.SaveChanges();
            response.ResponseObject = category;
          }

          response.Success = true;
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
      catch(Exception ex)
      {
        //something happened, handling the error here
      }
      
      return response;
    }

  }
}