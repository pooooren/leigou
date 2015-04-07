using System.ServiceModel.Activation;
using System.Web.Http;
using System.Web.Routing;

namespace Rest
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);

            RegisterRoutes();
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
        }

        private void RegisterRoutes()
        {
            RouteTable.Routes.Add(new ServiceRoute("rest", new WebServiceHostFactory(), typeof(Resource)));
        }
    }
}
