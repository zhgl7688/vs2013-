using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoWoSoft.Platform
{
    public class Guid_id
    {
        private FoWoSoft.Data.Interface.IGuid_id dataGuid_id;
        public Guid_id()
        {
            this.dataGuid_id = Data.Factory.Factory.GetGuid_id();
        }


        /// <summary>
        /// 新增
        /// </summary>
        public int Add(FoWoSoft.Data.Model.Guid_id model)
        {

            return dataGuid_id.Add(model);
        }
        /// <summary>
        /// 根据主键查询一条记录
        /// </summary>
        public FoWoSoft.Data.Model.Guid_id Get(int id)
        {
            return dataGuid_id.Get(id);
        }
        /// <summary>
        /// 根据GuidId查询一条记录
        /// </summary>
        public FoWoSoft.Data.Model.Guid_id Get(Guid GuidId)
        {
            return dataGuid_id.Get(GuidId);
        }
        /// <summary>
        /// 根据useId查询一条记录
        /// </summary>
        public FoWoSoft.Data.Model.Guid_id Get(string useId)
        {
            return dataGuid_id.Get(useId);
        }

        public List<FoWoSoft.Data.Model.Guid_id> GetAll()
        {
            return dataGuid_id.GetAll();

        }
    }
}
