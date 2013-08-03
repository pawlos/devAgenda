using System.Data.Entity;
using DevAgenda.Domain;
using DevAgenda.Domain.Events;
using DevAgenda.Domain.Handlers;
using DevAgenda.Domain.Models;
using DevAgenda.Domain.Repositories;
using DevAgenda.Domain.Repositories.Interfaces;
using DevAgenda.WebApp.Models;
using DevAgenda.WebApp.Services;

[assembly: WebActivator.PreApplicationStartMethod(typeof(DevAgenda.WebApp.App_Start.NinjectMVC3), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(DevAgenda.WebApp.App_Start.NinjectMVC3), "Stop")]

namespace DevAgenda.WebApp.App_Start
{
    using System.Reflection;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Mvc;

    public static class NinjectMVC3 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestModule));
            DynamicModuleUtility.RegisterModule(typeof(HttpApplicationInitializationModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
          kernel.Bind<DbContext>().To<DevAgendaCtx>().InRequestScope();
          
          kernel.Bind<IUserRepository>().To<UserRepository>().InRequestScope();
          kernel.Bind<IEventRepository>().To<EventRepository>().InRequestScope();
          kernel.Bind<ITagsRepository>().To<TagsRepository>().InRequestScope();
          kernel.Bind<IActionRepository>().To<ActionRepository>().InRequestScope();
          kernel.Bind<ILocationRepository>().To<LocationRepository>().InRequestScope();

          kernel.Bind<ILocationsQueryCache>().To<LocationsQueryCache>().InRequestScope();

          kernel.Bind<Handles<EventCreated>>().To<EventCreatedHandler>().InRequestScope();
          kernel.Bind<Handles<EventRSVPd>>().To<EventRSVPdHandler>().InRequestScope();
          kernel.Bind<Handles<EventRSVPUndoed>>().To<EventRSVPUndoedHandler>().InRequestScope();
        }        
    }
}
