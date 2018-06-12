using BookShopSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopSystem.Web.Core
{
   public class SessionData
    {
        /// <summary>
        /// Session数据
        /// </summary>
        public class Admin
        {
            /// <summary>
            /// 登录用户
            /// </summary>
            public static LoginEntity LoginedUser
            {
                get { return SessionManager.GetSession<LoginEntity>(EnumMgr.SessionFlag.AdminUser.ToString()); }
                set
                {
                    if (value == null)
                    {
                        SessionManager.ClearSession(EnumMgr.SessionFlag.AdminUser.ToString());
                    }
                    else
                    {
                        SessionManager.SetSession<LoginEntity>(EnumMgr.SessionFlag.AdminUser.ToString(), value);
                    }
                }
            }

            /// <summary>
            /// 是否登录
            /// </summary>
            public static bool IsLogined
            {
                get { return LoginedUser == null ? false : true; }
            }
        }

        public class Member
        {
            /// <summary>
            /// 登录用户
            /// </summary>
            public static MemberEntity LoginedUser
            {
                get { return SessionManager.GetSession<MemberEntity>(EnumMgr.SessionFlag.LoginUser.ToString()); }
                set
                {
                    if (value == null)
                    {
                        SessionManager.ClearSession(EnumMgr.SessionFlag.LoginUser.ToString());
                    }
                    else
                    {
                        SessionManager.SetSession<MemberEntity>(EnumMgr.SessionFlag.LoginUser.ToString(), value);
                    }
                }
            }

            /// <summary>
            /// 是否登录
            /// </summary>
            public static bool IsLogined
            {
                get { return LoginedUser == null ? false : true; }
            }
        }
    }
}
