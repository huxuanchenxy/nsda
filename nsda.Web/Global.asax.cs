using Autofac;
using Autofac.Integration.Mvc;
using nsda.Utilities;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace nsda.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            AutofacMvcRegister();
        }


        #region 依赖注入
        private void AutofacMvcRegister()
        {
            // 加载程序集列表
            string loadingPattern = "^nsda";
            var builder = new ContainerBuilder();

            //注入的基类型
            Type baseType = typeof(IDependency);

            var assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToList();
            var assembliesFilter = assemblies
                            .Where(assembly => Regex.IsMatch(assembly.FullName, loadingPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled))
                           .ToArray();  //过滤程序集

            //注册所以继承IDependency接口的类
            builder.RegisterAssemblyTypes(assembliesFilter)
                 .Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract)
                 .AsSelf()   //自身服务，用于没有接口的类
                 .AsImplementedInterfaces()  //接口服务
                 .PropertiesAutowired()  //属性注入
                 .InstancePerLifetimeScope();    //设定生命周期，基于请求

            builder.RegisterType<MySqlDBContext>().As<IDBContext>().InstancePerLifetimeScope(); //MySql数据库工作单元

            //controller 注入
            builder.RegisterControllers(System.Reflection.Assembly.GetExecutingAssembly()).PropertiesAutowired();
            // 过滤器注入
            builder.RegisterFilterProvider();

            var container = builder.Build(); //这句话一定要放在所有的Register 的后面

            //注意：Controller的构造函数需公开public
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container)); //替换
        }

        #endregion 依赖注入

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            LogUtils.LogError("Application_Error", exception);
        }
    }
}
