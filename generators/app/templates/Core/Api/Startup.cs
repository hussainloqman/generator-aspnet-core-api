using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using Framework.Data.EF;
using Microsoft.Owin;
using Owin;
using <%=baseName%>.Data;
using System.Data.Entity;
using System.Reflection;
using System.Web.Http;
using System.Linq;
using <%=baseName%>.Utilities;
using <%=baseName%>.Utilities.TransactionalActions;

[assembly: OwinStartup(typeof(<%=baseName%>.Api.Startup))]

namespace <%=baseName%>.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var webApiConfig = new HttpConfiguration();
            var container = IoCConfiguration(webApiConfig);
            WebApiConfiguration(webApiConfig, container);

            //register middlewares 
            app.UseAutofacMiddleware(container);
            app.UseWebApi(webApiConfig);
            app.UseAutofacWebApi(webApiConfig);
        }

        private void WebApiConfiguration(HttpConfiguration webApiConfig, ILifetimeScope container)
        {
            webApiConfig.MapHttpAttributeRoutes();

            webApiConfig.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            webApiConfig.Filters.Add(new TransactionalActionFilter());
        }

        private MapperConfiguration AutoMapperConfiguration()
        {
            return new MapperConfiguration(mapperConfig =>
            {
                var methodsToCall = from type in this.GetType().Assembly.GetTypes()
                                    where type.IsClass && !type.IsAbstract &&
                                          type.GetCustomAttribute<MapperAttribute>() != null
                                    select type.GetMethod(MapperAttribute.DEFAULT_METHOD_NAME);

                foreach (var method in methodsToCall)
                    method.Invoke(null, new object[] { mapperConfig });
            });
        }

        private ILifetimeScope IoCConfiguration(HttpConfiguration webApiConfig)
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterHttpRequestMessage(webApiConfig);

            var mapperConfig = AutoMapperConfiguration();
            builder.RegisterInstance(mapperConfig).As<IConfigurationProvider>();
            builder.RegisterInstance(mapperConfig.CreateMapper()).As<IMapper>();

            // register services here ....

            builder.RegisterType<DataStore>().As<DbContext>().InstancePerRequest();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerRequest();
            return builder.Build();
        }
    }
}
