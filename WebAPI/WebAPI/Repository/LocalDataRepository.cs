using System.Linq;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Repository
{
  internal class LocalDataRepository
  {
    public static LinkDTO[] GetLinks()
    {
      using (AngularRepositoryEntities _context = new AngularRepositoryEntities())
      {
        return (from e in _context.Link select new LinkDTO {
                  Id = e.Id,
                  Name = e.Name,
                  LinkValue = e.LinkValue,
                  NumberOfRatings = 1,
                  Rating = 1,
                  IdCategory = e.IdCategory
                }).ToArray();
      }

    }

  }
}