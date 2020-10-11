namespace Owt.Web
{
    using System;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Web.Http;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            MediaTypeHeaderValue appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => string.Equals(t.MediaType, "application/xml", StringComparison.InvariantCultureIgnoreCase));
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
        }
    }
}