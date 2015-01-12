using BillBoard.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using WebMatrix.WebData;

namespace BillBoard
{
    // Примечание: Инструкции по включению классического режима IIS6 или IIS7 
    // см. по ссылке http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static BillBoard.MvcApplication.InitializeSimpleMembershipAttribute.SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
        public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                // Обеспечение однократной инициализации ASP.NET Simple Membership при каждом запуске приложения
                LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
            }

            public class SimpleMembershipInitializer
            {
                public SimpleMembershipInitializer()
                {
                    Database.SetInitializer<DatabaseContext>(null);

                    try
                    {
                        using (var context = new DatabaseContext())
                        {
                            if (!context.Database.Exists())
                            {
                                // Создание базы данных SimpleMembership без схемы миграции Entity Framework
                                ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                            }
                        }

                        WebSecurity.InitializeDatabaseConnection("DatabaseConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);

                        SimpleRoleProvider roles = (SimpleRoleProvider)Roles.Provider;
                        SimpleMembershipProvider membership = (SimpleMembershipProvider)Membership.Provider;

                        if (!roles.RoleExists("Admin"))
                        {
                            roles.CreateRole("Admin");
                        }
                        if (membership.GetUser("admin", false) == null)
                        {
                            membership.CreateUserAndAccount("admin", "qwe123");
                            roles.AddUsersToRoles(new[] { "admin" }, new[] { "Admin" });
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException("Не удалось инициализировать базу данных ASP.NET Simple Membership. Чтобы получить дополнительные сведения, перейдите по адресу: http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                    }
                }
            }
        }
    }
}