using System.Linq;
using WebScraping.Data;

namespace WebScraping.Repository
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