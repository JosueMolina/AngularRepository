
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebScraping.Data;
using System.Data.Entity;
namespace WebScraping.Repositorio
{
     internal class LocalDataRepository
    {

         public static EnlacesDTO[] ObtenerEnlaces(){
         using (EnlacesDataBaseEntities _contexto = new EnlacesDataBaseEntities())
            {
                return (from e in _contexto.Enlaces
                        select new EnlacesDTO
                        {
                            Id = e.Id,
                            Nombre = e.Nombre,
                            Link = e.Link,
                            NumeroPuntuaciones = e.NumeroPuntuaciones,
                            Puntuacion = e.Puntuacion,
                            IdCategoria = e.IdCategoria
                        }).ToArray();
            }
         
         }
         
    }
}