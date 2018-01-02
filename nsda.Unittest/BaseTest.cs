using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using nsda.Utilities;
using nsda.Utilities.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace nsda.unittest
{
    [TestClass]
    public class BaseTest
    {
        public static IContainer AutofacContainer;

        static BaseTest()
        {
            InitTest();
        }

        private static void InitTest()
        {

            //autofac初始化
            string loadingPattern = "^nsda";
            var builder = new ContainerBuilder();
            Type baseType = typeof(IDependency);

            //AppDomain.CurrentDomain.Load("nsda.Utilities");
            AppDomain.CurrentDomain.Load("nsda.Model");
            AppDomain.CurrentDomain.Load("nsda.Repository");
            AppDomain.CurrentDomain.Load("nsda.Services");

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var assembliesFilter = assemblies
                            .Where(assembly => Regex.IsMatch(assembly.FullName, loadingPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled))
                           .ToArray();

            builder.RegisterAssemblyTypes(assembliesFilter)
                             .Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract)
                             .AsSelf()   //自身服务，用于没有接口的类
                             .AsImplementedInterfaces()  //接口服务
                             .PropertiesAutowired()  //属性注入
                             .InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);  //保证生命周期基于请求

            builder.RegisterType<MySqlDBContext>().As<IDBContext>().InstancePerLifetimeScope(); //MySql数据库工作单元

            var container = builder.Build();
            AutofacContainer = container;
        }
    }
}
