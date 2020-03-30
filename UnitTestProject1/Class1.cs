public class AgentDaoWffins : DapperExtensionsDemo.Dao.IDaoWffins
{
    public System
.Int32 Insert(System.Collections.Generic.List<DapperExtensionsDemo.Entity.WFinsentity> finsentity)
    { throw new System.Exception(); }
    public System.Int32 Insert(DapperExtensionsDemo.Entity.WFinsentity finsentity)
    {
        throw new System.Exception()
;
    }
    public DapperExtensionsDemo.Entity.WFinsentity SelectById(System.String id)
    {
        throw new System.Exception();
    }
    public DapperExtensionsDemo.Dao.DBContext DB
    {
        get
; set;
    }
    public AgentDaoWffins(DapperExtensionsDemo.Dao.DBContext DB)
    {
        this.DB = DB
;
    }
}