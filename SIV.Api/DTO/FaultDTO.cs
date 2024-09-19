using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIV.Api.DTO
{
    public class FaultDTO
    {
        /// <summary>
        /// Desc:列车号和网络MVB协议中列车号保持一致
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "列车号和网络MVB协议中列车号保持一致", Length = 50)]
        public string lch { get; set; }

        /// <summary>
        /// Desc:车厢号
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "车厢号")]
        public string cxh { get; set; }

        /// <summary>
        /// 车厢号别名(ABCCBA)
        /// </summary>
        [SugarColumn(ColumnDescription = "车厢号别名")]
        public string cxhName { get; set; }

        /// <summary>
        /// Desc:设备编码
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "设备编码", Length = 50)]
        public string device_code { get; set; }

        /// <summary>
        /// Desc:源系统主机ID
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "源系统主机ID")]
        public int yxtzjid { get; set; }

        /// <summary> 
        /// Desc:机组1主回路1断路器故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1主回路1断路器故障")]
        public int jz1zhl1dlqgz { get; set; }

        /// <summary> 
        /// Desc:机组1主回路2断路器故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1主回路2断路器故障")]
        public int jz1zhl2dlqgz { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机1断路器故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机1断路器故障")]
        public int jz1ysj1dlqgz { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机2断路器故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机2断路器故障")]
        public int jz1ysj2dlqgz { get; set; }

        /// <summary> 
        /// Desc:机组1通风机接触器故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1通风机接触器故障")]
        public int jz1tfjjcqgz { get; set; }

        /// <summary> 
        /// Desc:机组1通风机紧急通风接触器故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1通风机紧急通风接触器故障")]
        public int jz1tfjjjtfjcqgz { get; set; }

        /// <summary> 
        /// Desc:机组1冷凝风机接触器故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1冷凝风机接触器故障")]
        public int jz1lnfjjcqgz { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机1接触器故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机1接触器故障")]
        public int jz1ysj1jcqgz { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机2接触器故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机2接触器故障")]
        public int jz1ysj2jcqgz { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机1高压故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机1高压故障")]
        public int jz1ysj1gygz { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机1低压故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机1低压故障")]
        public int jz1ysj1dygz { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机2高压故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机2高压故障")]
        public int jz1ysj2gygz { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机2低压故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机2低压故障")]
        public int jz1ysj2dygz { get; set; }

        /// <summary> 
        /// Desc:中压故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "中压故障")]
        public int zygz { get; set; }

        /// <summary> 
        /// Desc:紧急通风逆变器故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "紧急通风逆变器故障")]
        public int jjtfnbqgz { get; set; }

        /// <summary> 
        /// Desc:机组1紫外线灯故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1紫外线灯故障")]
        public int jz1zwxdgz { get; set; }

        /// <summary> 
        /// Desc:机组1通风机1过载故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1通风机1过载故障")]
        public int jz1tfj1gzgz { get; set; }

        /// <summary> 
        /// Desc:机组1通风机2过载故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1通风机2过载故障")]
        public int jz1tfj2gzgz { get; set; }

        /// <summary> 
        /// Desc:机组1冷凝风机1过载故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1冷凝风机1过载故障")]
        public int jz1lnfj1gzgz { get; set; }

        /// <summary> 
        /// Desc:机组1冷凝风机2过载故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1冷凝风机2过载故障")]
        public int jz1lnfj2gzgz { get; set; }

        /// <summary> 
        /// Desc:机组1变频器1故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1变频器1故障")]
        public int jz1bpq1gz { get; set; }

        /// <summary> 
        /// Desc:机组1变频器2故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1变频器2故障")]
        public int jz1bpq2gz { get; set; }

        /// <summary> 
        /// Desc:机组1变频器1通讯故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1变频器1通讯故障")]
        public int jz1bpq1txgz { get; set; }

        /// <summary> 
        /// Desc:机组1变频器2通讯故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1变频器2通讯故障")]
        public int jz1bpq2txgz { get; set; }

        /// <summary> 
        /// Desc:机组1新风阀1故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1新风阀1故障")]
        public int jz1xff1gz { get; set; }

        /// <summary> 
        /// Desc:机组1新风阀2故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1新风阀2故障")]
        public int jz1xff2gz { get; set; }

        /// <summary> 
        /// Desc:机组1回风阀1故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1回风阀1故障")]
        public int jz1hff1gz { get; set; }

        /// <summary> 
        /// Desc:机组1回风阀2故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1回风阀2故障")]
        public int jz1hff2gz { get; set; }

        /// <summary> 
        /// Desc:客室温度传感器1故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "客室温度传感器1故障")]
        public int kswdcgq1gz { get; set; }

        /// <summary> 
        /// Desc:机组1回风传感器故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1回风传感器故障")]
        public int jz1hfcgqgz { get; set; }

        /// <summary> 
        /// Desc:机组1新风传感器故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1新风传感器故障")]
        public int jz1xfcgqgz { get; set; }

        /// <summary> 
        /// Desc:机组1送风传感器1故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1送风传感器1故障")]
        public int jz1sfcgq1gz { get; set; }

        /// <summary> 
        /// Desc:机组1送风传感器2故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1送风传感器2故障")]
        public int jz1sfcgq2gz { get; set; }

        /// <summary> 
        /// Desc:机组1空气净化器故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1空气净化器故障")]
        public int jz1kqjhqgz { get; set; }

        /// <summary> 
        /// Desc:机机组1压缩机1排气温度传感器故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机1排气温度传感器故障")]
        public int jz1ysj1pqwdcgqgz { get; set; }


        /// <summary> 
        /// Desc:机组1压缩机1吸气温度传感器故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机1吸气温度传感器故障")]
        public int jz1ysj1xqwdcgqgz { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机2排气温度传感器故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机2排气温度传感器故障")]
        public int jz1ysj2pqwdcgqgz { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机2吸气温度传感器故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机2吸气温度传感器故障")]
        public int jz1ysj2xqwdcgqgz { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机1排气温度故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机1排气温度故障")]
        public int jz1ysj1pqwdgz { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机2排气温度故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机2排气温度故障")]
        public int jz1ysj2pqwdgz { get; set; }

        /// <summary> 
        /// Desc:机组1采集模块1通讯故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1采集模块1通讯故障")]
        public int jz1cjmk1txgz { get; set; }

        /// <summary> 
        /// Desc:机组1采集模块2通讯故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1采集模块2通讯故障")]
        public int jz1cjmk2txgz { get; set; }

        /// <summary> 
        /// Desc:机组1空气质量检测模块故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1空气质量检测模块故障")]
        public int jz1kqzljcmkgz { get; set; }

        /// <summary> 
        /// Desc:机组1高压压力传感器1故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1高压压力传感器1故障")]
        public int jz1gyylcgq1gz { get; set; }

        /// <summary> 
        /// Desc:机组1低压压力传感器1故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1低压压力传感器1故障")]
        public int jz1dyylcgq1gz { get; set; }

        /// <summary> 
        /// Desc:机组1高压压力传感器2故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1高压压力传感器2故障")]
        public int jz1gyylcgq2gz { get; set; }

        /// <summary> 
        /// Desc:机组1低压压力传感器2故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1低压压力传感器2故障")]
        public int jz1dyylcgq2gz { get; set; }

        /// <summary> 
        /// Desc:机组1轻微故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1轻微故障")]
        public int jz1qwgz { get; set; }

        /// <summary> 
        /// Desc:机组1中等故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1中等故障")]
        public int jz1zdgz { get; set; }

        /// <summary> 
        /// Desc:机组1严重故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1严重故障")]
        public int jz1yzgz { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机1过载故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机1过载故障")]
        public int jz1ysj1gzgz { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机2过载故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机2过载故障")]
        public int jz1ysj2gzgz { get; set; }

        /// <summary> 
        /// Desc:机组1紫外线灯1故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1紫外线灯1故障")]
        public int jz1zwxd1gz { get; set; }

        /// <summary> 
        /// Desc:机组1紫外线灯2故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1紫外线灯2故障")]
        public int jz1zwxd2gz { get; set; }

        /// <summary> 
        /// Desc:机组1通风机1高速接触器故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1通风机1高速接触器故障")]
        public int jz1tfj1gsjcqgz { get; set; }

        /// <summary> 
        /// Desc:机组1通风机2高速接触器故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1通风机2高速接触器故障")]
        public int jz1tfj2gsjcqgz { get; set; }

        /// <summary> 
        /// Desc:机组1通风机1低速接触器故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1通风机1低速接触器故障")]
        public int jz1tfj1dsjcqgz { get; set; }

        /// <summary> 
        /// Desc:机组1通风机2低速接触器故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1通风机2低速接触器故障")]
        public int jz1tfj2dsjcqgz { get; set; }

        /// <summary> 
        /// Desc:废排风机过载
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "废排风机过载")]
        public int fpfjgz { get; set; }

        /// <summary> 
        /// Desc:废排风机接触器故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "废排风机接触器故障")]
        public int fpfjjcqgz { get; set; }

        /// <summary> 
        /// Desc:废排风机紧急通风接触器故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "废排风机紧急通风接触器故障")]
        public int fpfjjjtfjcqgz { get; set; }

        /// <summary> 
        /// Desc:废排风阀故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "废排风阀故障")]
        public int fpffgz { get; set; }

        /// <summary> 
        /// Desc:防火阀故障
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "防火阀故障")]
        public int fhfgz { get; set; }
    }
}
