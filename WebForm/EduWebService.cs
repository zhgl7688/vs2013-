using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebForm.EduModels;


namespace WebForm
{
    public class EduWebService
    {
        cn.edu.ecnu.datawebservice.ECNUWebService ecnuweb = new cn.edu.ecnu.datawebservice.ECNUWebService();//申明Webservice
        cn.edu.ecnu.datawebservice.MySoapHeader header = new cn.edu.ecnu.datawebservice.MySoapHeader();
        public EduWebService()
        {
            AddSoapHeader();
        }
        public void AddSoapHeader()
        {

            ecnuweb.MySoapHeaderValue = header;
        }
        public DataSet GetAll(string BMBH)
        {
            header.Username = "ECNUZZJG";
            header.Password = "1AE803A7BED23DB28523A3F38020240E";
            return ecnuweb.IDC_GET_SJBM(BMBH);
        }
        FoWoSoft.Platform.Organize borganize = new FoWoSoft.Platform.Organize();
        /// <summary>
        /// 根据用户id获取用户信息
        /// </summary>
        /// <param name="useId"></param>
        /// <returns></returns>
        public EduUser GetUser(string useId)
        {
            EduUser user = null;
            header.Username = "PUB_USER";
            header.Password = "EA83AB5DA244E73DB35F5C7AD3C9C42CB331B1EF690487C117481468BC61B1AB";
            var usertable=  ecnuweb.PUBLIC_IDC_GET_XSJBXX(useId, "", 1, 1).Tables[0];
            if (usertable.Columns.Count>1&&usertable.Rows.Count>0){
                user=new EduUser (){
                     XH=usertable.Rows[0]["XH"].ToString(),
                     XM =usertable.Rows[0]["XM"].ToString(),
                     BMBH  =usertable.Rows[0]["BMBH"].ToString(),
                  BMMC  =usertable.Rows[0]["BMMC"].ToString(),

                };
            }
            return user;

        }

        //重建组织时用
        public void AddOrganize(DataSet ds)
        {
            FoWoSoft.Platform.Guid_id guidIdService = new FoWoSoft.Platform.Guid_id();
            var dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                var dr = dt.Rows[i];
                #region 部门编号转换成GUID
                string id = dr["BMBH"].ToString();
                if (id.Trim() == "0")
                {
                    var ss = 5;
                }
                var guidId = guidIdService.Get(id);
                Guid org1ID;
                if (guidId == null)
                {
                    org1ID = GetGuid(id);

                    //插入对应表guid--id
                    FoWoSoft.Data.Model.Guid_id guidIdModel = new FoWoSoft.Data.Model.Guid_id()
                    {
                        GuidId = org1ID,
                        useId = id
                    };
                    guidIdService.Add(guidIdModel);
                }
                else
                {
                    org1ID = guidId.GuidId;
                }
                dr["BMBH"] = org1ID;
                #endregion
                var orgrion = borganize.Get(org1ID);
                if (orgrion != null) continue;
                #region 转上级编号为GUID
                id = dr["SJBM"].ToString();
                guidId = guidIdService.Get(id);
                if (guidId == null)
                {
                    org1ID = GetGuid(id);
                    //插入对应表guid--id
                    FoWoSoft.Data.Model.Guid_id guidIdModel = new FoWoSoft.Data.Model.Guid_id()
                    {
                        GuidId = org1ID,
                        useId = id
                    };
                    guidIdService.Add(guidIdModel);
                }
                else
                {
                    org1ID = guidId.GuidId;
                }
                dr["SJBM"] = org1ID;
                #endregion

                #region 插入组织结构
                FoWoSoft.Data.Model.Organize org = new FoWoSoft.Data.Model.Organize();

                org.ID = Guid.Parse(dr["BMBH"].ToString());
                org.Name = dr["BMMC"].ToString();
                org.Note = null;
                org.Number = "";// org.Number + "," + org1ID.ToString().ToLower();
                org.ParentID = Guid.Parse(dr["SJBM"].ToString());
                org.Sort = 0;// borganize.GetMaxSort(org.ID);
                org.Status = 0;
                org.Type = 0;// type.ToInt();
                org.Depth = 0;// org.Depth + 1;

                //using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope())
                //  {

                borganize.Add(org);
                //更新父级[ChildsLength]字段
                //borganize.UpdateChildsLength(org.ID);
                // scope.Complete();
                // } 
                #endregion

            }
        }
        /// <summary>
        /// 重建组织结构
        /// </summary>
        /// <param name="g">首Guid</param>
        public void organizeResize(Guid g)
        {

            var first = borganize.Get(g);
            var s = borganize.GetChilds(first.ID);
            if (s == null) return;
            foreach (var org in s)
            {
                org.Number = org.ParentID.ToString() + "," + first.ID.ToString().ToLower();
                borganize.Update(org);
                borganize.UpdateChildsLength(org.ID);
                organizeResize(org.ID);

            }

        }

        public Guid GetGuid(string id)
        {

            Guid result;
            switch (id)
            {
                case "-1": result = Guid.Parse("00000000-0000-0000-0000-000000000000"); break;
                case "0": result = Guid.Parse("04F12BEB-D99D-43DF-AC9A-3042957D6BDA"); break;
                default: result = Guid.NewGuid();
                    break;
            }
            return result;
        }
    }

}