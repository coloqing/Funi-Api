﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SIV.Api.Models;
using SqlSugar;
using SIV.Util;
using Util.DTO;
using SIV.Api.DTO;
using Aspose.Cells.Drawing;

namespace SIV.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainDeviceController : ControllerBase
    {
        private readonly SqlSugarClient sqlSugarClient;
        private IMapper mapper;

        public TrainDeviceController(SqlSugarClient sqlSugarClient)
        {
            this.sqlSugarClient = sqlSugarClient;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<R_Train_DeviceDTO, R_Train_Device>().ReverseMap().ForAllMembers(opt => opt.Condition((src, dest, svalue, dvalue) => svalue != null));
                cfg.CreateMap<Device, DeviceDTO>().ReverseMap().ForAllMembers(opt => opt.Condition((src, dest, svalue, dvalue) => svalue != null));
            }
            );
            mapper = config.CreateMapper();
        }

        /// <summary>
        /// 获取列车设备
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageRow"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageResult<R_Train_DeviceDTO>> Get(int? pageIndex = 1, int? pageRow = 10)
        {
            var query = sqlSugarClient.Queryable<R_Train_Device>().Where(x => !x.IsDeleted);
            var result = await query.GetPageResultAsync("createTime", "desc", pageIndex.Value, pageRow.Value);

            return new PageResult<R_Train_DeviceDTO>
            {
                Data = mapper.Map<List<R_Train_DeviceDTO>>(result.Data),
                Total = result.Total,
                Success = true,
                Code = 200,
                Message = "查询成功"
            };
        }

        /// <summary>
        /// 获取列车的所有设备、部件、指标、信号量 信息。
        /// 两个参数可以传任意一个
        /// </summary>
        /// <param name="trainId"></param>
        /// <param name="trainNum"></param>
        /// <returns></returns>
        [HttpGet("AllDeviceSignal")]
        public async Task<AjaxResult<object>> GetAllDeviceSignal(int? trainId = default, string? trainNum = default)
        {
            AjaxResult<object> result = new AjaxResult<object> { Code = 200, Success = false };

            if (trainId == default && trainNum == default)
            {
                result.Message = "trainId 和 trainNum 不能都为空";
                return result;
            }

            var exp = Expressionable.Create<R_Train_Device>().And(x => !x.IsDeleted);

            exp.AndIF(trainId != default, x => x.TrainId == trainId);
            exp.AndIF(trainNum != default, x => x.TrainNum == trainNum);

            var rTrainDevice = sqlSugarClient.Queryable<R_Train_Device>().Where(exp.ToExpression()).ToList();

            DeviceSignalDTO deviceSignalDTO = new();
            deviceSignalDTO.Devices = new();

            var devices = sqlSugarClient.Queryable<Device>().Where(x => !x.IsDeleted && rTrainDevice.Select(x => x.DeviceId).ToList().Contains(x.Id)).ToList();

            deviceSignalDTO.Devices.AddRange(mapper.Map<List<DeviceDTO>>(devices));
            deviceSignalDTO.DeviceDM = new();

            var componentIds = new List<int>();
            foreach (var item in devices.Select(x => x.ComponentIds).ToList())
            {
                var ids = item.Split(",");
                componentIds.AddRange(ids.ToList().ConvertAll(x => int.Parse(x)));
            }

            componentIds = componentIds.Distinct().ToList();

            var components = sqlSugarClient.Queryable<Component>().Where(x => !x.IsDeleted && componentIds.Contains(x.Id)).ToList();

            var indicatorIds = new List<int>();

            foreach (var item in components.Select(x => x.IndicatorsIds).ToList())
            {
                var ids = item.Split(",");
                indicatorIds.AddRange(ids.ToList().ConvertAll(x => int.Parse(x)));
            }

            indicatorIds = indicatorIds.Distinct().ToList();

            var indicators = sqlSugarClient.Queryable<Indicators>().Where(x => !x.IsDeleted && indicatorIds.Contains(x.Id)).ToList();

            foreach (var device in devices)
            {
                var deviceDM = new DeviceDM
                {
                    Id = device.Id,
                    Name = device.Name,
                    SN = device.SN,
                    Components = new List<ComponentDM>(),
                    CoachType = rTrainDevice.First(x => x.DeviceId == device.Id).CoachType
                };

                // 设备所有的部件
                foreach (var component in components.Where(x => device.ComponentIds.Split(",").ToList().ConvertAll(x => int.Parse(x)).Contains(x.Id)))
                {
                    var cdm = new ComponentDM
                    {
                        Id = component.Id,
                        Name = component.Name,
                        SN = component.SN,
                        Indicators = new()
                    };

                    //部件所有的指标
                    foreach (var indecator in indicators.Where(x => component.IndicatorsIds.Split(",").ToList().ConvertAll(x => int.Parse(x)).Contains(x.Id)))
                    {
                        var indcDTO = new IndicatorsDTO
                        {
                            Name = indecator.Name,
                            Id = indecator.Id,
                            Max = indecator.Max,
                            Min = indecator.Min,
                            SignalCode = indecator.SignalCode
                        };

                        cdm.Indicators.Add(indcDTO);
                    }

                    deviceDM.Components.Add(cdm);

                }

                deviceSignalDTO.DeviceDM.Add(deviceDM);
            }

            result.Data = deviceSignalDTO;
            return result;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="trainDevice"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult<string> Post([FromBody] R_Train_DeviceDTO trainDevice)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var dbtrainDevice = mapper.Map<R_Train_Device>(trainDevice);

            dbtrainDevice.CreateTime = DateTime.Now;
            dbtrainDevice.UpdateTime = DateTime.Now;
            dbtrainDevice.IsDeleted = false;

            var count = sqlSugarClient.Insertable(dbtrainDevice).ExecuteCommand();

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
        /// <param name="trainDevice"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public AjaxResult<string> Update(R_Train_DeviceDTO trainDevice)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var dbtrainDevice = sqlSugarClient.Queryable<R_Train_Device>().First(x => x.Id == trainDevice.Id && !x.IsDeleted);

            mapper.Map(trainDevice, dbtrainDevice);

            dbtrainDevice.UpdateTime = DateTime.Now;

            var count = sqlSugarClient.Updateable(dbtrainDevice).ExecuteCommand();

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
        /// <param name="deleteEntityId"></param>
        /// <returns></returns>
        [HttpPost("Delete")]
        public AjaxResult<string> Delete(DeleteEntityId deleteEntityId)
        {
            var result = new AjaxResult<string>();
            result.Code = 200;

            var dbtrainDevice = sqlSugarClient.Queryable<Device>().First(x => x.Id == deleteEntityId.Id && !x.IsDeleted);

            dbtrainDevice.UpdateTime = DateTime.Now;
            dbtrainDevice.IsDeleted = true;

            var count = sqlSugarClient.Updateable(dbtrainDevice).ExecuteCommand();

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

    public class DeviceSignalDTO
    {
        public List<DeviceDM> DeviceDM { get; set; }

        public List<DeviceDTO> Devices { get; set; }
    }

    public class DeviceDM
    {
        public int Id { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 设备编码
        /// </summary>
        public string SN { get; set; }
        public string CoachType { get; set; }

        public List<ComponentDM> Components { get; set; }
    }

    public class ComponentDM
    {
        public int Id { get; set; }
        /// <summary>
        /// 部件编号
        /// </summary>
        public string SN { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        public List<IndicatorsDTO> Indicators { get; set; }
    }

}
