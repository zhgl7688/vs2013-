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
        cn.edu.ecnu.userinfo.SMSWebService useinfoService = new cn.edu.ecnu.userinfo.SMSWebService();
        cn.edu.ecnu.userinfo.MySoapHeader useheader = new cn.edu.ecnu.userinfo.MySoapHeader();
        string colName = "mobile";
        public EduWebService()
        {
            AddSoapHeader();
        }
        public DataTable GetUseinfo(string id)
        {
            useheader.Username = "ECNUSMSP";
            useheader.Password = "27CD9E00930A17C1800C1464A3EEEE63";
            useinfoService.MySoapHeaderValue = useheader;
            var result = useinfoService.Get_UserInfo(id).Tables[0];
            return result;
        }
        public string GetMobile(string id)
        {
            var result = GetUseinfo(id);
            if (result.Columns.Contains(colName))
            {
                return result.Rows[0][colName].ToString();
            }
            else
            {
                var error= result.Rows[0][0].ToString();
                FoWoSoft.Platform.Log.Add(string.Format("获取手机错误({0})",id+ error), error, FoWoSoft.Platform.Log.Types.其它分类);

                return "";
            }

        }
        public DataTable SetUseinfo(string id, string moblie)
        {
            useheader.Username = "ECNUSMSP";
            useheader.Password = "27CD9E00930A17C1800C1464A3EEEE63";
            useinfoService.MySoapHeaderValue = useheader;
            return useinfoService.Set_UserInfo(id, moblie).Tables[0];
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
            header.Password = "EA83AB5DA244E73D3D0BB7E5772635527AC72FEE1D7CA4A067FB4A9ADE985C8337942475A6CE8088";
            // "EA83AB5DA244E73D3D0BB7E5772635527AC72FEE1D7CA4A067FB4A9ADE985C8337942475A6CE8088";
            // "EA83AB5DA244E73DB35F5C7AD3C9C42CB331B1EF690487C117481468BC61B1AB";
            var usertable = ecnuweb.PUBLIC_IDC_GET_XSJBXX(useId, "", 1, 1).Tables[0];
            if (usertable.Columns.Count > 1 && usertable.Rows.Count > 0)
            {
                user = new EduUser()
                {
                    XH = usertable.Rows[0]["XH"].ToString(),
                    XM = usertable.Rows[0]["XM"].ToString(),
                    BMBH = usertable.Rows[0]["BMBH"].ToString(),
                    BMMC = usertable.Rows[0]["BMMC"].ToString(),



                };
            }
            return user;

        }

        public void ss(string qq)
        {
            GetAllUserByDPCODE(qq);
        }
        /// <summary>
        /// 根据部门id获取部门下用户信息
        /// </summary>
        /// <param name="useId"></param>
        /// <returns></returns>
        public DataTable GetAllUserByDPCODE(string DPCODE)
        {
            EduUser user = null;
            header.Username = "PUB_USER";
            header.Password = "EA83AB5DA244E73D3D0BB7E577263552E0D88B8DFADB966F63CD5E6CB99A080F8A5A7EBC1CC9F3A1";
            //"FAED74BF942BA9F7B0EBF88BACE2CE3EB262CE34F3470DAFCE4BFF112FA7A6700BD796F9B9C9648040463E82B7FDC4DDF940DD5457EBE7C10666BB48686E252FE4DA2291BDE0F89B491137C8F46F66830AE46C1E65C4B62A";// "EA83AB5DA244E73DB35F5C7AD3C9C42CB331B1EF690487C117481468BC61B1AB";
            var usertable = ecnuweb.PUBLIC_IDC_GET_XSJBXX(DPCODE, "", 100, 1).Tables[0];
            //if (usertable.Columns.Count > 1 && usertable.Rows.Count > 0)
            //{
            //    user = new EduUser()
            //    {
            //        XH = usertable.Rows[0]["XH"].ToString(),
            //        XM = usertable.Rows[0]["XM"].ToString(),
            //        BMBH = usertable.Rows[0]["BMBH"].ToString(),
            //        BMMC = usertable.Rows[0]["BMMC"].ToString(),

            //    };
            //}
            return usertable;

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
        FoWoSoft.Platform.UsersRelation usersRelations = new FoWoSoft.Platform.UsersRelation();
        /// <summary>
        /// 重建组织结构
        /// </summary>
        /// <param name="g">首Guid</param>
        public void organizeResize1(string number, Guid g, int depth)
        {

            var first = borganize.Get(g);
            var s = borganize.GetChilds(first.ID);
            foreach (var org in s)
            {
                var ss = borganize.GetChilds(org.ID);
                var uu = usersRelations.GetAllByOrganizeID(org.ID);
                org.Type = 2;
                org.Number = number + "," + org.ID.ToString().ToLower();
                org.Depth = depth;
                org.ChildsLength = ss.Count + uu.Count;
                borganize.Update(org);

                if (ss.Count > 0)
                    organizeResize1(org.Number, org.ID, depth + 1);

            }

        }
        public Guid GetGuid(string id)
        {

            Guid result;
            switch (id)
            {
                case "-1": result = Guid.Parse("00000000-0000-0000-0000-000000000000"); break;
                case "0": result = Guid.Parse("04F12BEB-D99D-43DF-AC9A-3042957D6BDA"); break;
                default:
                    result = Guid.NewGuid();
                    break;
            }
            return result;
        }
    }

}