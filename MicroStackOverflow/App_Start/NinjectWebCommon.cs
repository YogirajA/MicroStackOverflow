using System.Configuration;
using MicroStackOverflow.Services.Dapper;
using MicroStackOverflow.Services.Massive;
using MicroStackOverflow.Services.Petapoco;
using MicroStackOverflow.Services.SimpleData;
using SimpleData.DAL.Infrastructure;
using DapperDatabaseContext = Dapper.DAL.Infrastructure.DatabaseContext;
using PetaPocoDatabaseContext = PetaPoco.DAL.Infrastructure.DatabaseContext;

[assembly: WebActivator.PreApplicationStartMethod(typeof(MicroStackOverflow.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(MicroStackOverflow.App_Start.NinjectWebCommon), "Stop")]

namespace MicroStackOverflow.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
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
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            var connectionStringName = "StackOverflow";
            var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            var dapperdatabaseContext = new DapperDatabaseContext(connectionString);
            kernel.Bind<IDapperPostsServices>()
                  .To<DapperDapperPostsServices>()
                  .WithConstructorArgument("databaseContext", dapperdatabaseContext);

            var petapocoContext = new PetaPocoDatabaseContext(connectionStringName);
            kernel.Bind<IPetaPocoPostsServices>()
                  .To<PetaPocoPostsServices>()
                  .WithConstructorArgument("databaseContext", petapocoContext);

            var simpleDataContext = new DatabaseContext(connectionString);
            kernel.Bind<ISimpleDataPostsServices>()
                  .To<SimpleDataPostsServices>()
                  .WithConstructorArgument("databaseContext", simpleDataContext);

            kernel.Bind<IMassivePostsServices>()
                .To<MassivePostsServices>()
                .WithConstructorArgument("connectionStringName", connectionStringName);

        }        
    }
}
