using DapperExtensionsDemo.BaseDynamicDao;
using DapperExtensionsDemo.Entity;
using System.Collections.Generic;
namespace DapperExtensionsDemo.Dao
{
    /// <summary>
    /// 需要动态实现的dao
    /// </summary>
    [Table("wffins")]
    public interface IDaoWffins
    {
        [Insert("id,dataid,name,cdate,status")]
        int Insert(List<WFinsentity> finsentity);


        [Insert("id,dataid,name,cdate,status")]
        int Insert(WFinsentity finsentity);

        [Select("id,name,dataid")]
        WFinsentity SelectById(string id);

        /// <summary>
        /// 数据库对象
        /// </summary>
        public DBContext DB { get; set; }

        WFinsentity Select(string id)
        {
            System.Console.WriteLine($"通过属性注入对象获取的值：{DB?.DBName}");

            return new WFinsentity()
            {
                ID = "aaa",
                Name = "bbb",
                DataId = "ccc"
            };
        }
    }

    /// <summary>
    /// 需要注入的对象
    /// </summary>
    public class DBContext
    {
        public DBContext()
        {
            this.DBName = "dbname";
        }
        public string DBName { get; set; }
    }
}
