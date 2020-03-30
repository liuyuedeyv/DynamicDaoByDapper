# DynamicDaoByDapper
利用dapper组件实现mybatis注解编写dao的功能
`
   [Table("tablename")]
    public interface IDaoWffins
    {
        [Insert("dataid,name,cdate,status")]
        int Update(WFinsentity finsentity);

        //基本增删改
        [Insert("id,dataid,name,cdate,status")]
        int Insert(List<WFinsentity> finsentity);
        
        /// <summary>
        /// 其他需要从DI容器获取的对象
        /// </summary>
        public DBContext DB { get; set; }
        //复杂查询
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
`
