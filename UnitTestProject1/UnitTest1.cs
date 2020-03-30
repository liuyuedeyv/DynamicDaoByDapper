using Autofac;
using Autofac.Extras.DynamicProxy;
using DapperExtensionsDemo.Dao;
using DapperExtensionsDemo.DynamicService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        IContainer _container;

        public UnitTest1()
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.Register(c => DynamicService.CreateInstanceByInterface(typeof(IDaoWffins))).As<IDaoWffins>()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(InterceptorDao));
            containerBuilder.RegisterType<InterceptorDao>();
            _container = containerBuilder.Build();
        }


        [TestMethod]
        public void TestMethod1()
        {
            var dao = _container.Resolve<IDaoWffins>();

            var type = typeof(IDaoWffins);
            Console.WriteLine($"type:{type      }");
            //DapperExtensionsDemo.Dao.IDaoWffins

            string agentClass =
                $"  public class AgentIDaoWffins:{typeof(IDaoWffins) } {{";


            string methodStr = string.Empty;
            List<MethodInfo> methods = new List<MethodInfo>();
            methods.AddRange(type.GetMethods());
            methods.AddRange(type.GetInterfaces().SelectMany(i => i.GetMethods()));

            foreach (var method in methods)
            {
                methodStr = $"public {method.ReturnType } {method.Name}(";
                foreach (var p in method.GetParameters())
                {
                    methodStr += $"{p.ParameterType.ToString().Replace("`1[", "<").Replace("]", ">")} {p.Name},";
                }
                methodStr = methodStr.TrimEnd(',');
                methodStr += ") ";
                methodStr += "{";
                methodStr += " throw new System.Exception();";
                methodStr += "}";
                agentClass += methodStr;
            }
            agentClass += "} ";
            Console.WriteLine(agentClass);


            SyntaxTree tree = CSharpSyntaxTree.ParseText(agentClass);
            var syntaxTrees = new List<SyntaxTree>();
            syntaxTrees.Add(tree);

            var option = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var references = new List<MetadataReference>();

            var t1 = CSharpCompilation.Create("AgentDao.dll", syntaxTrees, references, option);
            var list2 = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).Select(
                (x) =>
             MetadataReference.CreateFromFile(x.Location)
            );
            foreach (var item in list2)
            {
                Console.WriteLine($"{item.ToString()}");
            }
            var compilation = t1.AddReferences(list2);
            var result = compilation.Emit(@"E:\—ßœ∞demo\DapperExtensionsDemo\DapperExtensionsDemo\AgentDao.dll");
            if (result.Success)
            {
                Console.WriteLine("±‡“Î≥…π¶");
            }
            else
            {
                Console.WriteLine("±‡“Î ß∞‹");
            }
        }
    }

}


