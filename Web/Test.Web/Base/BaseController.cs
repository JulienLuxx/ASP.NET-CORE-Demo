using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Mvc;

namespace Test.Web.Base
{
    /// <summary>
    /// 基础控制器
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserId
        {
            get
            {
                var result = -1;
                var userId = User.Claims.FirstOrDefault(x => x.Type.Equals("UserId"));
                if (null != userId)
                {
                    int.TryParse(userId.Value, out result);
                }
                return result;
            }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get
            {
                var result = string.Empty;
                var userName = User.Claims.FirstOrDefault(x => x.Type.Equals(JwtClaimTypes.Name));
                if (null != userName)
                {
                    result = userName.Value;
                }
                return result;
            }
        }
    }
}