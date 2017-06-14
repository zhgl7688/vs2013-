using ConsoleFoWo.cn.edu.ecnu.datawebservice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleFoWo
{
    class Program
    {
        static ECNUWebService ecnuweb = new ECNUWebService();
        static MySoapHeader header = new MySoapHeader();
        static void Main(string[] args)
        {
            Console.WriteLine("-------人员重构---------");
            ecnuweb.MySoapHeaderValue = header; int ssk;
            var ss = new FoWoOAEntities().Guid_id.ToList();
            for (int i = 0; i < ss.Count; i++)
            {
              
                string userID = ss[i].useId.Trim();
  Console.WriteLine("-------人员重构{0}---------", userID);
 if (userID=="0206")
      ssk=0;
                CreateUser(userID);

              //  Console.WriteLine("-------人员重构{0}---------",userID);

            }
            Console.WriteLine("-------组织重构---------");
            using (var entity = new FoWoOAEntities())
            {
                Guid first = Guid.Parse("04F12BEB-D99D-43DF-AC9A-3042957D6BDA");
                var rist = entity.Organize.FirstOrDefault(s => s.ID == first);
                organizeResize1(entity, "04F12BEB-D99D-43DF-AC9A-3042957D6BDA", first, 1);
            }

            Console.WriteLine("-------组织重构结束---------");
            Console.Read();
        }
        /// <summary>
        /// 重建组织结构
        /// </summary>
        /// <param name="g">首Guid</param>
        public static void organizeResize1(FoWoOAEntities entity, string number, Guid g, int depth)
        {

            var first = entity.Organize.FirstOrDefault(s => s.ID == g);
            var orgList = entity.Organize.Where(s => s.ParentID == first.ID).ToList();
            foreach (var org in orgList)
            {

                var ss = entity.Organize.Where(s => s.ParentID == org.ID).Count();
                var uu = entity.UsersRelation.Where(s => s.OrganizeID == org.ID).Count();
                org.Type = 2;
                org.Number = number + "," + org.ID.ToString().ToLower();
                org.Depth = depth;
                org.ChildsLength = ss + uu;

                if (ss > 0)
                    organizeResize1(entity, org.Number, org.ID, depth + 1);
            }

        }
        public static void CreateUser(string number1)
        {

            var users = GetAllUserByDPCODE(number1);
            Console.WriteLine("-------CreateUser{0}---------", users.Rows.Count);
            CreateNewUser(users);

        }
        public static DataTable GetAllUserByDPCODE(string DPCODE)
        {

            header.Username = "PUB_USER";
            header.Password = "EA83AB5DA244E73D3D0BB7E577263552E0D88B8DFADB966F63CD5E6CB99A080F8A5A7EBC1CC9F3A1";
            var usertable = ecnuweb.PUBLIC_IDC_GET_XSJBXX(DPCODE, "", 100, 1).Tables[0];
            return usertable;
        }
        public static EduUser GetUser(string useId)
        {
            EduUser user = null;
            header.Username = "PUB_USER";
            header.Password = "EA83AB5DA244E73D3D0BB7E5772635527AC72FEE1D7CA4A067FB4A9ADE985C8337942475A6CE8088";
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

        public static void CreateNewUser(DataTable users)
        {
            var userlist = new FoWoOAEntities().Users.ToList();
            using (var entity = new FoWoOAEntities())
            {
                if (users.Columns.Count > 1 && users.Rows.Count > 0)
                {
                    for (int j = 0; j < users.Rows.Count; j++)
                    {
                      
                        var account = users.Rows[j][0].ToString();
                        Console.WriteLine("-------CreateNewUser{0}---------", account);

                        var user = userlist.FirstOrDefault(s => s.Account == account);
                        if (user != null) continue;

                        //根据UserId获取从远程用户信息
                        var userInfoEdu = GetUser(account);
                        if (userInfoEdu == null) continue;
                        Console.WriteLine("-------userInfoEdu{0}---------", userInfoEdu.BMMC);
                        var userId = Guid.NewGuid();
                        //更新用户信息
                        entity.Users.Add(new Users()
                       {
                           Account = account,
                           ID = userId,
                           Name = userInfoEdu.XM,
                           Status = 0,
                           Password = "1234",
                           Sort = 1,
                           Note = ""
                       });
                        //创建组织关系

                        var guidId = entity.Guid_id.FirstOrDefault(s => s.useId == userInfoEdu.BMBH);
                        entity.UsersRelation.Add(new UsersRelation()
                       {
                           OrganizeID = guidId.GuidId,
                           UserID = userId,
                           Sort = 1,
                           IsMain = 1
                       });
                        Console.WriteLine("-------UsersRelation{0}---------", userId);
                        //更新组织下人员的个数
                        //new FoWoSoft.Platform.Organize().UpdateChildsLength(guidId.GuidId);
                        //创建用户角色
                        entity.UsersRole.Add(new UsersRole()
                        {
                            RoleID = Guid.Parse("0CF2ABB1-5F90-4FB3-8FA9-B53628B92879"),
                            MemberID = userId,
                            IsDefault = true
                        });
                        Console.WriteLine("-------UsersRole{0}---------", userId);
                    }
                    Console.WriteLine("-------SaveChanges{0}---------");
                    entity.Configuration.AutoDetectChangesEnabled = false;
                    entity.SaveChanges();
                }
            }
        }
    }
}