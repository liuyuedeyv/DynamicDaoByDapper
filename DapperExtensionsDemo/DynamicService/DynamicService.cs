using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DapperExtensionsDemo.DynamicService
{
    public static class DynamicService
    {
        public static string CreateInstanceByInterface(Type type)
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var dllName = $"AgentDao.dll";
            var fullpath = Path.Combine(basePath, dllName);
            var calssName = $"Agent{type.ToString().Split('.').Last().TrimStart('I')}";

            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
            }
            if (CreateDll(type, dllName, fullpath))
            {
                //var assembly = Assembly.LoadFrom(fullpath);
                //var instance = assembly.CreateInstance(calssName);

                //return instance;

                return fullpath;
            }
            else
            {
                throw new Exception($"编译{type}失败，请检查");
            }
        }



        static bool CreateDll(Type type, string dllName, string fullName)
        {
            var calssName = type.ToString().Split('.').Last().TrimStart('I');

            string agentClass =
                $"  public class Agent{calssName}:{type} {{";

            string methodStr = string.Empty;

            List<MethodInfo> methods = new List<MethodInfo>();
            methods.AddRange(type.GetMethods().Where(m => m.CustomAttributes.Count() > 0));
            methods.AddRange(type.GetInterfaces().SelectMany(i => i.GetMethods().Where(m => methods.Where(n => n.Name == m.Name).Count() == 0)));

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
            //属性 

            string propertyStr = string.Empty;
            string constructorParams = $" public Agent{calssName}(";
            string constructorBody = " {";

            foreach (var property in type.GetProperties())
            {
                propertyStr = $" public {property.PropertyType } {property.Name} {{get;set;}}";
                agentClass += propertyStr;

                constructorParams += $"{property.PropertyType } {property.Name},";
                constructorBody += $" this.{property.Name }={property.Name}; ";
            }
            constructorParams = constructorParams.TrimEnd(',') + ")";
            constructorBody = constructorBody + "} ";
            agentClass += constructorParams + constructorBody;

            agentClass += "} ";

            Console.WriteLine(agentClass);

            SyntaxTree tree = CSharpSyntaxTree.ParseText(agentClass);
            var syntaxTrees = new List<SyntaxTree>();
            syntaxTrees.Add(tree);

            var option = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var references = new List<MetadataReference>();

            var t1 = CSharpCompilation.Create(dllName, syntaxTrees, references, option);
            var list2 = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).Select(
                (x) =>
             MetadataReference.CreateFromFile(x.Location)
            );
            var compilation = t1.AddReferences(list2);
            var result = compilation.Emit(fullName);
            return result.Success;
        }
    }
}
