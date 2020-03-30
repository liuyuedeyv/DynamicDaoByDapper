using Castle.DynamicProxy;
using Dapper;
using DapperExtensionsDemo.BaseDynamicDao;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace DapperExtensionsDemo.Dao
{
    public class InterceptorDao : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {

            Console.WriteLine($"start{invocation.Method.Name}");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var insertAttribute = invocation.Method.GetCustomAttributes(typeof(InsertAttribute), false).FirstOrDefault() as InsertAttribute;
            if (insertAttribute != null)
            {
                invocation.ReturnValue = 0;
                var colsP = $"@{insertAttribute.Fields.Replace(",", ",@")}";
                var sql = $"insert into wffins ({insertAttribute.Fields}) values ({colsP })";
                //Console.WriteLine($"sql:{sql}");
                Console.WriteLine($"拼接sql，耗时{stopwatch.ElapsedMilliseconds }毫秒");

                using (var conn = GetConnection())
                {
                    Console.WriteLine($"获取DB连接，耗时{stopwatch.ElapsedMilliseconds }毫秒");
                    invocation.ReturnValue = conn.Execute(sql, invocation.Arguments[0]);
                    Console.WriteLine($"执行完成，耗时{stopwatch.ElapsedMilliseconds }毫秒");
                }

            }
            var selectAttribute = invocation.Method.GetCustomAttributes(typeof(SelectAttribute), false).FirstOrDefault() as SelectAttribute;


            if (selectAttribute != null)
            {
                var method = typeof(DapperProxy).GetMethod("QueryFirst");
                method = method.MakeGenericMethod(invocation.Method.ReturnType);
                object[] parms = new object[1];
                parms[0] = $"select {selectAttribute.Fields ?? "*"  } from wffins where id='{invocation.Arguments[0]}'";
                invocation.ReturnValue = method.Invoke(null, parms);
            }
            if (invocation.ReturnValue == null)
            {
                invocation.Proceed();
            }
            Console.WriteLine($"end{invocation.Method.Name }");
        }


        //var sql = $"insert into {TableCode} ({cols}) values ({colsP })";
        //Console.WriteLine($"sql:{sql}");


        MySqlConnection GetConnection()
        {
            var dbConnstr = "server=127.0.0.1;database=fastdev;uid=root;pwd=1234.com";
            return new MySql.Data.MySqlClient.MySqlConnection(dbConnstr);
        }
    }


    public class DapperProxy
    {
        public static T QueryFirst<T>(string sql)
        {
            using (var conn = GetConnection())
            {
                return conn.QueryFirst<T>(sql);
            }
        }
        static MySqlConnection GetConnection()
        {
            var dbConnstr = "server=127.0.0.1;database=fastdev;uid=root;pwd=1234.com";
            return new MySql.Data.MySqlClient.MySqlConnection(dbConnstr);
        }
    }

}
