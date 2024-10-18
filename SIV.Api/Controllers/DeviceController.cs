using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIV.Api.DTO;
using SqlSugar;
using SIV.Util;
using Util.DTO;
using SIV.Api.Models;

namespace SIV.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly SqlSugarClient sqlSugarClient;
        private IMapper mapper;

        public DeviceController(SqlSugarClient sqlSugarClient)
        {
            this.sqlSugarClient = sqlSugarClient;

            var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<DeviceDTO, Device>().ReverseMap().ForAllMembers(opt => opt.Condition((src, dest, svalue, dvalue) => svalue != null));
                }
            );
            mapper = config.CreateMapper();
        }

        [HttpGet]
        public async Task<PageResult<DeviceDTO>> Get(string? lineName = default , string? trainNum = default ,int? pageIndex = 1, int? pageRow = 10)
        { 
            var query = sqlSugarClient.Queryable<Device>().Where(x => !x.IsDeleted);
            query.WhereIF(!string.IsNullOrEmpty(lineName),x=>x.LineName == lineName);
            query.WhereIF(!string.IsNullOrEmpty(trainNum),x=>x.LineName == trainNum);
            var result = await query.GetPageResultAsync("createTime", "desc", pageIndex.Value, pageRow.Value);

            return new PageResult<DeviceDTO>
            {
                Data = mapper.Map<List<DeviceDTO>>(result.Data),
                Total = result.Total,
                Success = true,
                Code = 200,
                Message = "查询成功"
            };
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult<string> Post([FromBody] DeviceDTO device)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var dbdevice = mapper.Map<Device>(device);

            dbdevice.CreateTime = DateTime.Now;
            dbdevice.UpdateTime = DateTime.Now;
            dbdevice.IsDeleted = false;

            var count = sqlSugarClient.Insertable(dbdevice).ExecuteCommand();

            if (count > 0)
            {
                result.Success = true;
                result.Message = "新增成功";
            }
            else
            {
                result.Success = false;
                result.Message = "新增失败";
            }

            return result;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public AjaxResult<string> Update(DeviceDTO device)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var dbdevice = sqlSugarClient.Queryable<Device>().First(x => x.Id == device.Id && !x.IsDeleted);

            mapper.Map(device, dbdevice);

            dbdevice.UpdateTime = DateTime.Now;

            var count = sqlSugarClient.Updateable(dbdevice).ExecuteCommand();

            if (count > 0)
            {
                result.Success = true;
                result.Message = "更新成功";
            }
            else
            {
                result.Success = false;
                result.Message = "更新失败";
            }

            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dDevice"></param>
        /// <returns></returns>
        [HttpPost("Delete")]
        public AjaxResult<string> Delete(DeleteDevice dDevice)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var dbdevice = sqlSugarClient.Queryable<Device>().First(x => x.Id == dDevice.Id && !x.IsDeleted);

            dbdevice.UpdateTime = DateTime.Now;
            dbdevice.IsDeleted = true;

            var count = sqlSugarClient.Updateable(dbdevice).ExecuteCommand();

            if (count > 0)
            {
                result.Success = true;
                result.Message = "删除成功";
            }
            else
            {
                result.Success = false;
                result.Message = "删除失败";
            }

            return result;
        }
    }

    public class DeleteDevice
    {
        public int Id { get; set; }
    }
}
