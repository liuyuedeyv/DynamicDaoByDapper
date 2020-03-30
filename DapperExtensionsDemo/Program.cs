using DapperExtensionsDemo.Dao;
using DapperExtensionsDemo.Entity;
using Autofac;
using Autofac.Extras.DynamicProxy;
using System;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using DapperExtensionsDemo.BaseDynamicDao;
using Newtonsoft.Json;
using System.Reflection;

namespace DapperExtensionsDemo
{
    class Program
    {

        static IContainer _container;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                ContainerBuilder containerBuilder = new ContainerBuilder();
                containerBuilder.RegisterType<DBContext>().SingleInstance();
                containerBuilder.AddDynamicDao(DynamicService.DynamicService.CreateInstanceByInterface(typeof(IDaoWffins)));
                _container = containerBuilder.Build();
                var dao = _container.Resolve<IDaoWffins>();

                var result = dao.Select("testid");
                Console.WriteLine(JsonConvert.SerializeObject(result));


                stopwatch.Stop();
                Console.WriteLine($"Init complete,耗时{stopwatch.ElapsedMilliseconds }毫秒");


                stopwatch.Restart();
                var entity = dao.SelectById("00001PH9EC3TQ0000A02");
                stopwatch.Stop();
                Console.WriteLine($"方法执行 complete,耗时{stopwatch.ElapsedMilliseconds }毫秒");
                Console.WriteLine(JsonConvert.SerializeObject(entity));


                stopwatch.Restart();
                entity = dao.SelectById("00001PH9EC3TQ0000A02");
                stopwatch.Stop();
                Console.WriteLine($"方法执行 complete,耗时{stopwatch.ElapsedMilliseconds }毫秒");
                Console.WriteLine(JsonConvert.SerializeObject(entity));


                stopwatch.Restart();
                entity = dao.Select("00001PH9EC3TQ0000A02");
                stopwatch.Stop();
                Console.WriteLine($"方法执行 complete,耗时{stopwatch.ElapsedMilliseconds }毫秒");
                Console.WriteLine(JsonConvert.SerializeObject(entity));



                //InserData();
                //InserData();
                //InserData();

                //DapperInsert();
                //DapperInsert();
                //DapperInsert();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }

        static void DapperInsert()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            using (var conn = GetConnection())
            {
                conn.Open();
                var sql = "insert into wffins (id,dataid,cdate,status,name) values (?id,?dataid,?cdate,?status,?name)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.Add("id", MySqlDbType.VarChar, 20);
                cmd.Parameters.Add("dataid", MySqlDbType.VarChar, 20);
                cmd.Parameters.Add("cdate", MySqlDbType.DateTime);
                cmd.Parameters.Add("status", MySqlDbType.Int16, 20);
                cmd.Parameters.Add("name", MySqlDbType.VarChar, 20);

                cmd.Parameters["id"].Value = Guid.NewGuid().ToString().Substring(0, 20);
                cmd.Parameters["dataid"].Value = "123123";
                cmd.Parameters["cdate"].Value = DateTime.Now;
                cmd.Parameters["status"].Value = 1;
                cmd.Parameters["name"].Value = "ado.net";

                var count = cmd.ExecuteNonQuery();
            }
            stopwatch.Stop();
            Console.WriteLine($"原生ado.net 执行完成耗时：{stopwatch.ElapsedMilliseconds }毫秒");
        }

        static MySqlConnection GetConnection()
        {
            var dbConnstr = "server=127.0.0.1;database=fastdev;uid=root;pwd=1234.com";
            return new MySql.Data.MySqlClient.MySqlConnection(dbConnstr);
        }
        static void InserData()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            WFinsentity entity = new WFinsentity();
            entity.Name = "name";
            entity.ID = Guid.NewGuid().ToString().Substring(0, 20);
            entity.DataId = DateTime.Now.ToString();
            entity.Cdate = DateTime.Now;

            var dao = _container.Resolve<IDaoWffins>();
            var counnt = dao.Insert(entity);

            Console.WriteLine($"count:{counnt}");
            stopwatch.Stop();
            Console.WriteLine($"插入完成,耗时{stopwatch.ElapsedMilliseconds }毫秒");
        }
    }
}
