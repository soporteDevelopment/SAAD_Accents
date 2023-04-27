using ADEntities.Models;
using ADEntities.Queries.IRepository;
using ADEntities.Queries.Repository;
using ADSystem.Manager;
using ADSystem.Manager.IManager;
using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ADSystem.App_Start
{
    /// <summary>
    /// IoCConfiguration
    /// </summary>
    public class IoCConfiguration
    {
        public static void RegisterControllers(ContainerBuilder builder)
        {
            builder.RegisterControllers
            (Assembly.GetExecutingAssembly());
        }

        public static void RegisterRepos(ContainerBuilder builder)
        {
            builder.RegisterType<CatalogViewRepository>()
                .As<ICatalogViewRepository>().InstancePerRequest();

            builder.RegisterType<CatalogViewManager>()
                .As<ICatalogViewManager>().InstancePerRequest();

            builder.RegisterType<CatalogDetailViewRepository>()
                .As<ICatalogDetailViewRepository>().InstancePerRequest();

            builder.RegisterType<CatalogDetailViewManager>()
                .As<ICatalogDetailViewManager>().InstancePerRequest();

            builder.RegisterType<CatalogDetailDevolutionRepository>()
                .As<ICatalogDetailDevolutionRepository>().InstancePerRequest();

            builder.RegisterType<CatalogDetailDevolutionManager>()
                .As<ICatalogDetailDevolutionManager>().InstancePerRequest();

            builder.RegisterType<CatalogProviderRepository>()
               .As<ICatalogProviderRepository>().InstancePerRequest();

            builder.RegisterType<CatalogProviderManager>()
                .As<ICatalogProviderManager>().InstancePerRequest();

            builder.RegisterType<PaymentMonthRepository>()
                .As<IPaymentMonthRepository>().InstancePerRequest();

            builder.RegisterType<PaymentMonthManager>()
                .As<IPaymentMonthManager>().InstancePerRequest();

            builder.RegisterType<BranchRepository>()
                .As<IBranchRepository>().InstancePerRequest();

            builder.RegisterType<BranchManager>()
                .As<IBranchManager>().InstancePerRequest();

            builder.RegisterType<CustomerOriginRepository>()
                .As<ICustomerOriginRepository>().InstancePerRequest();

            builder.RegisterType<CustomerOriginManager>()
                .As<ICustomerOriginManager>().InstancePerRequest();

            builder.RegisterType<TicketsManager>()
                .As<ITicketsManager>().InstancePerRequest();

            builder.RegisterType<TicketsRepository>()
                .As<ITicketsRepository>().InstancePerRequest();

            builder.RegisterType<StatusTicketManager>()
                .As<IStatusTicketManager>().InstancePerRequest();

            builder.RegisterType<StatusTicketRepository>()
            .As<IStatusTicketRepository>().InstancePerRequest();

            builder.RegisterType<PriorityTicketManager>()
                .As<IPriorityTicketManager>().InstancePerRequest();

            builder.RegisterType<PriorityTicketRepository>()
            .As<IPriorityTicketRepository>().InstancePerRequest();

            builder.RegisterType<TypeTicketManager>()
                .As<ITypeTicketManager>().InstancePerRequest();

            builder.RegisterType<TypeTicketRepository>()
            .As<ITypeTicketRepository>().InstancePerRequest();

            builder.RegisterType<CatalogBrandManager>()
               .As<ICatalogBrandManager>().InstancePerRequest();

            builder.RegisterType<CatalogBrandRepository>()
            .As<ICatalogBrandRepository>().InstancePerRequest();

            builder.RegisterType<CategoryManager>()
               .As<ICategoryManager>().InstancePerRequest();

            builder.RegisterType<CategoryRepository>()
            .As<ICategoryRepository>().InstancePerRequest();

            builder.RegisterType<SubcategoryManager>()
               .As<ISubcategoryManager>().InstancePerRequest();

            builder.RegisterType<SubcategoryRepository>()
            .As<ISubcategoryRepository>().InstancePerRequest();

            builder.RegisterType<BranchOfficeManager>()
               .As<IBranchOfficeManager>().InstancePerRequest();

            builder.RegisterType<BranchOfficeRepository>()
            .As<IBranchOfficeRepository>().InstancePerRequest();

            builder.RegisterType<CatalogBankAccountManager>()
               .As<ICatalogBankAccountManager>().InstancePerRequest();

            builder.RegisterType<CatalogBankAccountRepository>()
            .As<ICatalogBankAccountRepository>().InstancePerRequest();

            builder.RegisterType<CatalogTerminalTypeManager>()
               .As<ICatalogTerminalTypeManager>().InstancePerRequest();

            builder.RegisterType<CatalogTerminalTypeRepository>()
            .As<ICatalogTerminalTypeRepository>().InstancePerRequest();
        }

        public static void RegisterClass(ContainerBuilder builder)
        {
            builder.Register(z => new admDB_SAADDBEntities()).
                            InstancePerRequest();
        }

        public static void Configure()
        {
            ContainerBuilder builder = new ContainerBuilder();

            RegisterControllers(builder);
            RegisterRepos(builder);
            RegisterClass(builder);

            IContainer contenedor = builder.Build();

            DependencyResolver.SetResolver
                (new AutofacDependencyResolver(contenedor));
        }
    }
}