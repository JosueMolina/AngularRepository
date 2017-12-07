using System.Web.Optimization;

namespace WebAPI
{
  public class BundleConfig
  {
    public static void RegisterBundles(BundleCollection bundles)
    {
      bundles.Add(new StyleBundle("~/Content/bootstrap").Include("~/Content/bootstrap.css"));
    }
  }
}