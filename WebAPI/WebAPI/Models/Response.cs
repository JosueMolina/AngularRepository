namespace WebAPI.Models
{
  public class Response<T>
  where T : class {

    public T ResponseObject;
    public bool Success;

    public Response(T responseObject = null, bool success = false)
    {
      ResponseObject = responseObject;
      Success = success ;
    }

  }
}