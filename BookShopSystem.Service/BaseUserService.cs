using BookShopSystem.DataAccess.Context;
using BookShopSystem.DataAccess.Entity;
using BookShopSystem.Models;
using BookShopSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopSystem.Service
{
    /// <summary>
    /// 用户
    /// </summary>
    public class BaseUserService
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="account">帐号</param>
        /// <param name="type">类型 0普通用户 1、管理员</param>
        /// <returns>用户信息</returns>
        public BaseUser GetUserInfoByAccount(string account, int type = 0)
        {
            using (var context = new BookShopContext())
            {
                var sql = from u in context.BaseUser where u.Account == account && u.RoleType == type select u;
                return sql.FirstOrDefault();
            }
        }

        /// <summary>
        /// 检测帐号是否存在
        /// </summary>
        /// <param name="account">帐号</param>
        /// <returns>bool</returns>
        public bool GetAccoutExistCheck(string account)
        {
            string sql = "select count(*) from [dbo].[BaseUser] where [RoleType]=0 and [Account]=@Account";
            var param = new SqlParameter[] {
               new SqlParameter("@Account", SqlDbType.VarChar) { Value = account }
            };
            bool flag = false;
            using (var ctx = new BookShopContext())
            {
                flag = ctx.Database.SqlQuery<int>(sql, param).FirstOrDefault() > 0;
            }
            return flag;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">待添加的实体</param>
        /// <returns>执行结果</returns>
        public bool Add(BaseUser model)
        {
            using (var ctx = new BookShopContext())
            {
                ctx.BaseUser.Add(model);
                return ctx.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 更新个人职业
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="professionId">职业</param>
        /// <returns></returns>
        public bool UpdateProfession(long userId, long professionId)
        {
            string sql = string.Format("update [dbo].[BaseUser] set [ProfessionId]={0} where [Id]={1}", professionId, userId);
            using (var ctx = new BookShopContext())
            {
                return ctx.Database.ExecuteSqlCommand(sql) > 0;
            }
        }

        /// <summary>
        /// 获取人员管理列表
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="recordCount">总记录条数</param>
        /// <returns></returns>
        public List<UserMgrEntity> GetUserMgrList(string key, int pageIndex, int PageSize, out int recordCount)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append(" SELECT *,P.[Name] AS ProfessionName FROM [dbo].[BaseUser] BU  ");
            strSQL.Append(" LEFT JOIN [dbo].[Profession] P ON P.Id = BU.[ProfessionId] WHERE BU.[RoleType]=0 ");
            List<SqlParameter> paramList = new List<SqlParameter>();
            if (!string.IsNullOrWhiteSpace(key))
            {
                strSQL.Append("AND ( BU.[Account] LIKE (@Key) OR P.Name LIKE (@Key) ) ");
                paramList.Add(new SqlParameter("@Key", SqlDbType.VarChar) { Value =  string.Format("%{0}%", key) });
            }
            string countSql = strSQL.ToString().PageCount();

            List<UserMgrEntity> list = new List<UserMgrEntity>();
            using (var ctx = new BookShopContext())
            {
                if (paramList.Count > 0)
                {
                    recordCount = ctx.Database.SqlQuery<int>(countSql, paramList.ToArray()).FirstOrDefault();
                    list = ctx.Database.SqlQuery<UserMgrEntity>(strSQL.ToString(), paramList.ToCopyArray()).ToList();
                }
                else
                {
                    recordCount = ctx.Database.SqlQuery<int>(countSql).FirstOrDefault();
                    list = ctx.Database.SqlQuery<UserMgrEntity>(strSQL.ToString()).ToList();
                }
            }
            return list;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">待删除的主键</param>
        /// <returns>执行结果</returns>
        public bool Delete(long id)
        {
            string sql = string.Format("delete from [dbo].[BaseUser] where [Id]={0}",id);
            using (var ctx = new BookShopContext())
            {
                return ctx.Database.ExecuteSqlCommand(sql) > 0;
            }
        }
    }
}
