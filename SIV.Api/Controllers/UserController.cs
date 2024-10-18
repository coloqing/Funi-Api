using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SIV.Api.Authorization;
using SIV.Api.Authorization.Models;
using SqlSugar;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using Util.DTO;
using SIV.Util;
using SIV.Api.DTO;

namespace SIV.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly SqlSugarClient sqlSugarClient;
        private readonly SSORequirement ssoRequirement;

        public UserController(SqlSugarClient sqlSugarClient, SSORequirement ssoRequirement)
        {
            this.sqlSugarClient = sqlSugarClient;
            this.ssoRequirement = ssoRequirement;
        }


        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("All")]
        public async Task<PageResult<User>> GetAllUser(int? pageIndex = 1, int? pageRow = 10)
        {
            var query = sqlSugarClient.Queryable<User>().Where(x => !x.IsDeleted);
            var result = await query.GetPageResultAsync("createTime", "desc", pageIndex.Value, pageRow.Value);
            return result;
        }

        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost("ChangePassword")]
        [Authorize]
        public AjaxResult<string> ChangePassword(ChangePassword userInfo)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token))
            {
                result.Success = false;
                result.Message = "没有登录";
                return result;
            }

            // 如果令牌存在，通常格式为 "Bearer {token}"
            if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer "))
            {
                token = token["Bearer ".Length..].Trim();
            }

            var tokenModel = TokenHelper.ResolveToken(token);

            var loginUser = sqlSugarClient.Queryable<User>().Where(x => x.Id == tokenModel.UserID && !x.IsDeleted).ToList().FirstOrDefault();

            if (loginUser == null)
            {
                result.Success = false;
                result.Message = "当前登录用户不存在";
                return result;
            }

            var user = sqlSugarClient.Queryable<User>().Where(x => x.Id == userInfo.Id && !x.IsDeleted).ToList().FirstOrDefault();

            if (user == null)
            {
                result.Success = false;
                result.Message = "用户不存在";
                return result;
            }

            if (loginUser.LoginName != "admin" && loginUser.Id != userInfo.Id)
            {
                result.Success = false;
                result.Message = "没有权限访问此用户数据";
                return result;
            }

            if (loginUser.LoginName == "admin")
            {
                user.Password = MD5(userInfo.NewPassword);

                var count = sqlSugarClient.Updateable(user).ExecuteCommand();
                if (count > 0)
                {
                    result.Success = true;
                    result.Message = "修改成功";
                    return result;
                }

                result.Success = false;
                result.Message = "修改失败";
                return result;
            }

            if (user.Password != MD5(userInfo.Password))
            {
                result.Success = false;
                result.Message = "旧密码不正确";
                return result;
            }


            user.Password = MD5(userInfo.NewPassword);

            var count1 = sqlSugarClient.Updateable(user).ExecuteCommand();
            if (count1 > 0)
            {
                result.Success = true;
                result.Message = "修改成功";
                return result;
            }

            result.Success = false;
            result.Message = "修改失败";
            return result;
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("ModUserInfo")]
        [Authorize]
        public AjaxResult<string> ModUserInfo(UserInfo userInfo)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token))
            {
                result.Success = false;
                result.Message = "没有登录";
                return result;
            }

            // 如果令牌存在，通常格式为 "Bearer {token}"
            if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer "))
            {
                token = token["Bearer ".Length..].Trim();
            }

            var tokenModel = TokenHelper.ResolveToken(token);

            var loginUser = sqlSugarClient.Queryable<User>().Where(x => x.Id == tokenModel.UserID && !x.IsDeleted).ToList().FirstOrDefault();

            if (loginUser == null)
            {
                result.Success = false;
                result.Message = "当前登录用户不存在";
                return result;
            }

            var user = sqlSugarClient.Queryable<User>().Where(x => x.Id == userInfo.Id && !x.IsDeleted).ToList().FirstOrDefault();

            if (user == null)
            {
                result.Success = false;
                result.Message = "用户不存在";
                return result;
            }

            if (loginUser.LoginName != "admin" && loginUser.Id != userInfo.Id)
            {
                result.Success = false;
                result.Message = "没有权限访问此用户数据";
                return result;
            }

            if (!string.IsNullOrEmpty(userInfo.Name))
                user.Name = userInfo.Name;

            if (!string.IsNullOrEmpty(userInfo.Avatar))
                user.Avatar = userInfo.Avatar;

            user.UpdateTime = DateTime.Now;

            if (!string.IsNullOrEmpty(userInfo.Introduction))
                user.Introduction = userInfo.Introduction;

            if (!string.IsNullOrEmpty(userInfo.LoginName))
                user.LoginName = userInfo.LoginName;

            if (!string.IsNullOrEmpty(userInfo.Phone))
                user.Phone = userInfo.Phone;

            var count = sqlSugarClient.Updateable(user).ExecuteCommand();

            if (count > 0)
            {
                result.Success = true;
                result.Message = "修改成功";
                return result;
            }
            result.Success = false;
            result.Message = "修改失败";
            return result;
        }



        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public AjaxResult<UserInfo> UserInfo(int id)
        {
            var result = new AjaxResult<UserInfo>();
            result.Code = 200;

            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token))
            {
                result.Success = false;
                result.Message = "没有登录";
                return result;
            }

            // 如果令牌存在，通常格式为 "Bearer {token}"
            if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer "))
            {
                token = token["Bearer ".Length..].Trim();
            }

            var tokenModel = TokenHelper.ResolveToken(token);

            var loginUser = sqlSugarClient.Queryable<User>().Where(x => x.Id == tokenModel.UserID && !x.IsDeleted).ToList().FirstOrDefault();

            if (loginUser == null)
            {
                result.Success = false;
                result.Message = "当前登录用户不存在";
                return result;
            }

            var user = sqlSugarClient.Queryable<User>().Where(x => x.Id == id && !x.IsDeleted).ToList().FirstOrDefault();

            if (user == null)
            {
                result.Success = false;
                result.Message = "用户不存在";
                return result;
            }

            if (loginUser.LoginName != "admin" && loginUser.Id != id)
            {
                result.Success = false;
                result.Message = "没有权限访问此用户数据";
                return result;
            }

            var userInfo = new UserInfo
            {
                LoginName = user.LoginName,
                Name = user.Name,
                Id = user.Id,
                RoleId = user.RoleId,
                Introduction = user.Introduction,
                Avatar = user.Avatar,
                Phone = user.Phone
            };

            var role = sqlSugarClient.Queryable<Role>().Where(x => x.Id == user.RoleId && !x.IsDeleted).ToList().FirstOrDefault();
            if (role != null)
                userInfo.Roles = new string[] { role.Name };

            result.Success = true;
            result.Data = userInfo;

            result.Message = "查询成功";

            return result;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public AjaxResult<object> Login(LoginModel model)
        {
            model.LoginName = model.LoginName.Trim();
            model.Password = model.Password.Trim();

            var result = new AjaxResult<object>();
            result.Code = 200;

            if (string.IsNullOrEmpty(model.LoginName) || string.IsNullOrEmpty(model.Password))
            {
                result.Success = false;
                result.Message = "账号且密码不能为空";
                return result;
            }

            var ps = MD5(model.Password);

            var user = sqlSugarClient.Queryable<User>().Where(x => !x.IsDeleted && x.LoginName == model.LoginName && x.Password == ps).ToList().FirstOrDefault();

            if (user == null)
            {
                result.Success = false;
                result.Message = "登录失败";
                return result;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.LoginName.ToString()),
            };


            var tokenJson = TokenHelper.IssueToken(claims.ToArray(), ssoRequirement);

            result.Success = true;
            result.Data = tokenJson;
            return result;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Signup")]
        public AjaxResult<string> SignUp(LoginModel model)
        {
            model.LoginName = model.LoginName.Trim();
            model.Password = model.Password.Trim();

            var result = new AjaxResult<string>();
            result.Code = 200;

            if (string.IsNullOrEmpty(model.LoginName) || string.IsNullOrEmpty(model.Password))
            {
                result.Success = false;
                result.Message = "账号且密码不能为空";
                return result;
            }
            var role = sqlSugarClient.Queryable<Role>().ToList().First(x => x.Name == "无权限");

            var existUser = sqlSugarClient.Queryable<User>().Where(x => !x.IsDeleted && x.LoginName == model.LoginName).ToList();

            if (existUser.Count > 0)
            {
                result.Success = false;
                result.Message = "用户已存在";
                return result;
            }

            var ps = MD5(model.Password);

            var user = new User
            {
                Name = model.LoginName,
                LoginName = model.LoginName,
                Password = ps,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                RoleId = role.Id,
                Phone = model.Phone,
                Avatar = model.Avatar,
                Introduction = model.Introduction
            };

            sqlSugarClient.Insertable(user).ExecuteCommand();

            result.Success = true;
            result.Message = "注册成功";
            return result;
        }

        /// <summary>
        /// 更改用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("ModRole")]
        [Authorize]
        public AjaxResult<string> ModRole(int userId, int roleId)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var user = sqlSugarClient.Queryable<User>().First(x => x.Id == userId && !x.IsDeleted);
            if (user == null)
            {
                result.Success = false;
                result.Message = "用户不存在";
                return result;
            }

            var role = sqlSugarClient.Queryable<Role>().First(x => x.Id == roleId && !x.IsDeleted);
            if (role == null)
            {
                result.Success = false;
                result.Message = "角色不存在";
                return result;
            }

            user.RoleId = roleId;
            user.UpdateTime = DateTime.Now;

            var count = sqlSugarClient.Updateable(user).ExecuteCommand();

            if (count > 0)
            {
                result.Success = true;
                result.Message = "更新成功";
                return result;
            }

            result.Success = false;
            result.Message = "更新失败";
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost("Delete")]
        [Authorize]
        public AjaxResult<string> Delete(DeleteUser duser)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token))
            {
                result.Success = false;
                result.Message = "没有登录";
                return result;
            }

            // 如果令牌存在，通常格式为 "Bearer {token}"
            if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer "))
            {
                token = token["Bearer ".Length..].Trim();
            }

            var tokenModel = TokenHelper.ResolveToken(token);

            var loginUser = sqlSugarClient.Queryable<User>().Where(x => x.Id == tokenModel.UserID && !x.IsDeleted).ToList().FirstOrDefault();

            if (loginUser == null)
            {
                result.Success = false;
                result.Message = "当前登录用户不存在";
                return result;
            }

            var user = sqlSugarClient.Queryable<User>().Where(x => x.Id == duser.Id && !x.IsDeleted).ToList().FirstOrDefault();

            if (user == null)
            {
                result.Success = false;
                result.Message = "用户不存在";
                return result;
            }

            if (loginUser.LoginName != "admin" && loginUser.Id != duser.Id)
            {
                result.Success = false;
                result.Message = "没有权限访问此用户数据";
                return result;
            }

            user.IsDeleted = true;
            user.UpdateTime = DateTime.Now;

            var count = sqlSugarClient.Updateable(user).ExecuteCommand();

            if (count > 0)
            {
                result.Success = true;
                result.Message = "删除成功";
                return result;
            }

            result.Success = false;
            result.Message = "删除失败";
            return result;
        }

        [NonAction]
        public string MD5(string encypString, string? codeType = null, string? spliter = null)
        {
            try
            {
                var m5 = System.Security.Cryptography.MD5.Create();
                var encoder = codeType != null ? Encoding.GetEncoding(codeType) : Encoding.Default;
                byte[] inputBye = encoder.GetBytes(encypString);
                byte[] outputBye = m5.ComputeHash(inputBye);
                string result = System.BitConverter.ToString(outputBye);
                return spliter == null ? result.Replace("-", "").ToUpper() : result.Replace("-", spliter).ToUpper();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public class LoginModel
    {
        [Required]
        public string LoginName { get; set; }
        [Required]
        public string Password { get; set; }
        public string? Phone { get; set; }
        public string? Avatar { get; set; }
        public string? Introduction { get; set; }
    }

    public class UserInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LoginName { get; set; }
        public int RoleId { get; set; }
        public string[] Roles { get; set; }
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        public string? Avatar { get; set; }
        public string? Introduction { get; set; }
        public string? Phone { get; set; }
    }

    public class ChangePassword
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }

    public class DeleteUser
    {
        public int Id { get; set; }
    }
}
