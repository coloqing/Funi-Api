using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace SAC.Entity
{
    ///<summary>
    ///解析转换后数据
    ///</summary>

    public partial class TB_PARSING_DATAS_Base
    {
        public TB_PARSING_DATAS_Base()
        {


        }
        /// <summary>
        /// Desc:主键ID
        /// Default:
        /// Nullable:False
        /// </summary>           
        /// </summary>           
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "主键ID")]
        public long id { get; set; }

        /// <summary>
        /// Desc:源设备号,参见章节5.1.2
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = ":源设备号,参见章节5.1.2", Length = 10)]
        public string ysbh { get; set; }
        /// <summary>
        /// Desc:宿设备号,参见章节5.1.2
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = ":宿设备号,参见章节5.1.2", Length = 10)]
        public string ssbh { get; set; }

        /// <summary>
        /// Desc:报文类型,参见章节5.1.4OCS发送到空调系统的心跳报文	7000 空调系统发送到OCS的心跳报文	7001 空调系统发送到OCS的数据报文	7020
        /// Default:
        /// Nullable:True
        /// </summary>         
        [SugarColumn(ColumnDescription = "Desc:报文类型,参见章节5.1.4OCS发送到空调系统的心跳报文 7000 空调系统发送到OCS的心跳报文 7001 空调系统发送到OCS的数据报文 7020")]
        public int bwlx { get; set; }


        /// <summary>
        /// Desc:列车号和网络MVB协议中列车号保持一致
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "列车号和网络MVB协议中列车号保持一致", Length = 50)]
        public string lch { get; set; }

        /// <summary>
        /// Desc:车厢号车厢号序号	设备名称	安装位置	设备号	IP规划 1.	OCS1	A1	40	CN391:10.0.1.162 //2.	OCS2 A2	41	CN391:10.0.6.162//3.	空调 A1	70	MVB协议中为准//4.	空调 B1	71	MVB协议中为准//5.空调 C1	72	MVB协议中为准//6.	空调 C2	73	MVB协议中为准//7.	空调 B2	74	MVB协议中为准//8.	空调 A2	75	MVB协议中为准
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "车厢号序号\t设备名称\t安装位置\t设备号\tIP规划 1.\tOCS1\tA1\t40\tCN391:10.0.1.162 //2.\tOCS2 A2\t41\tCN391:10.0.6.162//3.\t空调 A1\t70\tMVB协议中为准//4.\t空调 B1\t71\tMVB协议中为准//5.空调 C1\t72\tMVB协议中为准//6.\t空调 C2\t73\tMVB协议中为准//7.\t空调 B2\t74\tMVB协议中为准//8.\t空调 A2\t75\tMVB协议中为准", Length = 50)]
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
        /// Desc:特征字节 数据头特征码0x55
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "特征字节 数据头特征码0x55")]
        public int tzzj_1 { get; set; }

        /// <summary>
        /// Desc:协议版本X(版本号格式X.Y)
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "协议版本X(版本号格式X.Y)")]
        public int xybbx { get; set; }

        /// <summary>
        /// Desc:协议版本Y(版本号格式Y)
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "协议版本X(版本号格式Y)")]
        public int xybby { get; set; }

        /// <summary>
        /// Desc:帧号  周期变化的0~65535
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "帧号  周期变化的0~65535")]
        public int zh { get; set; }

        /// <summary>
        /// Desc:帧号  周期变化的0~65535
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "源系统主机ID")]
        public int yxtzjid { get; set; }
        /// <summary>
        /// Desc:源系统代号0x6D
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = ":源系统代号0x6D")]
        public int yxtdh { get; set; }
        /// <summary>
        /// Desc:数据长度
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "数据长度")]
        public int sjcd { get; set; }
        /// <summary>
        /// Desc:空调反馈来自TCMS下发的数据
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "空调反馈来自TCMS下发的数据")]
        public int tcssj { get; set; }


        /// <summary>
        /// Desc:HVAC生命信号H
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "HVAC生命信号")]
        public int hvacsmxhh { get; set; }

        /// <summary> 
        /// Desc:软件版本 如0x1234对应显示为V18.3.4 XXXXXXXX ．XXXX ．XXXX
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "软件版本 XXXXXXXX ．XXXX ．XXXX", Length = 20)]
        public string software_version { get; set; }

        /// <summary> 
        /// Desc:机组1目标温度
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1目标温度")]
        public double jz1mbwd { get; set; }

        /// <summary> 
        /// Desc:机组1客室温度
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1客室温度")]
        public double jz1kswd { get; set; }

        /// <summary> 
        /// Desc:机组1室外温度
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1室外温度")]
        public double jz1swwd { get; set; }

        /// <summary> 
        /// Desc:客室温度传感器1温度
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "客室温度传感器1温度")]
        public double jz1kswdcgq1wd { get; set; }

        /// <summary> 
        /// Desc:机组1送风传感器1温度
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1送风传感器1温度")]
        public double jz1sfcgq1wd { get; set; }

        /// <summary> 
        /// Desc:机组1送风传感器2温度
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1送风传感器2温度")]
        public double jz1sfcgq2wd { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机1排气温度
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机1排气温度")]
        public double jz1ysj1pqwd { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机2排气温度
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机2排气温度")]
        public double jz1ysj2pqwd { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机1吸气温度
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机1吸气温度")]
        public double jz1ysj1xqwd { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机2吸气温度
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机2吸气温度")]
        public double jz1ysj2xqwd { get; set; }

        /// <summary> 
        /// Desc:机组1空气质量检测模块温度
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1空气质量检测模块温度")]
        public double jz1kqzljcmkwd { get; set; }

        /// <summary> 
        /// Desc:机组1CO2浓度
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1CO2浓度")]
        public int jz1co2nd { get; set; }

        /// <summary> 
        /// Desc:机组1PM2.5浓度
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1PM2.5浓度")]
        public int jz1pm2d5nd { get; set; }

        /// <summary> 
        /// Desc:机组1TVOC浓度
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1TVOC浓度")]
        public int jz1tvocnd { get; set; }

        /// <summary> 
        /// Desc:客室湿度值
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1客室湿度值")]
        public int kssdz { get; set; }

        [SugarColumn(ColumnDescription = "机组1客室湿度值")]
        public int jz1kssdz { get; set; }


        /// <summary> 
        /// Desc:机组1新风阀1开度
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1新风阀1开度")]
        public int jz1xff1kd { get; set; }

        /// <summary> 
        /// Desc:机组1新风阀2开度
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1新风阀2开度")]
        public int jz1xff2kd { get; set; }

        /// <summary> 
        /// Desc:机组1回风阀1开度
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1回风阀1开度")]
        public int jz1hff1kd { get; set; }

        /// <summary> 
        /// Desc:机组1回风阀2开度
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1回风阀2开度")]
        public int jz1hff2kd { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机1高压压力
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机1高压压力")]
        public int jz1ysj1gyyl { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机1低压压力
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机1低压压力")]
        public int jz1ysj1dyyl { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机2高压压力
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机2高压压力")]
        public int jz1ysj2gyyl { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机2低压压力
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机2低压压力")]
        public int jz1ysj2dyyl { get; set; }

        /// <summary> 
        /// Desc:机组1滤网压力值
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1滤网压力值")]
        public int jz1lwylz { get; set; }

        /// <summary> 
        /// Desc:机组1通风机1 U相电流值
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1通风机1 U相电流值")]
        public double jz1tfj1uxdlz { get; set; }

        /// <summary> 
        /// Desc:机组1通风机1 V相电流值
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1通风机1 V相电流值")]
        public double jz1tfj1vxdlz { get; set; }

        /// <summary> 
        /// Desc:机组1通风机1 W相电流值
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1通风机1 W相电流值")]
        public double jz1tfj1wxdlz { get; set; }

        /// <summary> 
        /// Desc:机组1通风机2 U相电流值
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1通风机2 U相电流值")]
        public double jz1tfj2uxdlz { get; set; }

        /// <summary> 
        /// Desc:机组1通风机2 V相电流值
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1通风机2 V相电流值")]
        public double jz1tfj2vxdlz { get; set; }

        /// <summary> 
        /// Desc:机组1通风机2 W相电流值
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1通风机2 W相电流值")]
        public double jz1tfj2wxdlz { get; set; }

        /// <summary> 
        /// Desc:机组1冷凝风机1 U相电流值
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1冷凝风机1 U相电流值")]
        public double jz1lnfj1uxdlz { get; set; }

        /// <summary> 
        /// Desc:机组1冷凝风机1 V相电流值
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1冷凝风机1 V相电流值")]
        public double jz1lnfj1vxdlz { get; set; }

        /// <summary> 
        /// Desc:机组1冷凝风机1 W相电流值
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1冷凝风机1 W相电流值")]
        public double jz1lnfj1wxdlz { get; set; }

        /// <summary> 
        /// Desc:机组1冷凝风机2 U相电流值
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1冷凝风机2 U相电流值")]
        public double jz1lnfj2uxdlz { get; set; }

        /// <summary> 
        /// Desc:机组1冷凝风机2 V相电流值
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1冷凝风机2 V相电流值")]
        public double jz1lnfj2vxdlz { get; set; }

        /// <summary> 
        /// Desc:机组1冷凝风机2 W相电流值
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1冷凝风机2 W相电流值")]
        public double jz1lnfj2wxdlz { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机1 U相电流值
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机1 U相电流值")]
        public double jz1ysj1uxdlz { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机1 V相电流值
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机1 V相电流值")]
        public double jz1ysj1vxdlz { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机1 W相电流值
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机1 W相电流值")]
        public double jz1ysj1wxdlz { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机2 U相电流值
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机2 U相电流值")]
        public double jz1ysj2uxdlz { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机2 V相电流值
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机2 V相电流值")]
        public double jz1ysj2vxdlz { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机2 W相电流值
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机2 W相电流值")]
        public double jz1ysj2wxdlz { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机1频率
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机1频率")]
        public int jz1ysj1pl { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机2频率
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机2频率")]
        public int jz1ysj2pl { get; set; }

        /// <summary> 
        /// Desc:机组1变频器1功率
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1变频器1功率")]
        public double jz1bpq1gl { get; set; }

        /// <summary> 
        /// Desc:机组1变频器2功率
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1变频器2功率")]
        public double jz1bpq2gl { get; set; }

        /// <summary> 
        /// Desc:机组1变频器1输出电压
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1变频器1输出电压")]
        public double jz1bpq1scdy { get; set; }

        /// <summary> 
        /// Desc:机组1变频器2输出电压
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1变频器2输出电压")]
        public double jz1bpq2scdy { get; set; }

        /// <summary> 
        /// Desc:机组1空调能耗
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1空调能耗")]
        public int jz1ktnh { get; set; }

        /// <summary> 
        /// Desc:机机组1冷凝风机1累计工作时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1冷凝风机1累计工作时间")]
        public int jz1lnfj1ljgzsj { get; set; }

        /// <summary> 
        /// Desc:机组1冷凝风机2累计工作时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1冷凝风机2累计工作时间")]
        public int jz1lnfj2ljgzsj { get; set; }

        /// <summary> 
        /// Desc:机组1冷凝风机1轴承累计工作时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1冷凝风机1轴承累计工作时间")]
        public int jz1lnfj1zcljgzsj { get; set; }

        /// <summary> 
        /// Desc:机组1冷凝风机2轴承累计工作时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1冷凝风机2轴承累计工作时间")]
        public int jz1lnfj2zcljgzsj { get; set; }

        /// <summary> 
        /// Desc:机组1通风机1累计工作时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1通风机1累计工作时间")]
        public int jz1tfj1ljgzsj { get; set; }

        /// <summary> 
        /// Desc:机组1通风机2累计工作时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1通风机2累计工作时间")]
        public int jz1tfj2ljgzsj { get; set; }

        /// <summary> 
        /// Desc:机组1通风机1轴承累计工作时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1通风机1轴承累计工作时间")]
        public int jz1tfj1zcljgzsj { get; set; }

        /// <summary> 
        /// Desc:机组1通风机2轴承累计工作时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1通风机2轴承累计工作时间")]
        public int jz1tfj2zcljgzsj { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机1累计工作时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机1累计工作时间")]
        public int jz1ysj1ljgzsj { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机2累计工作时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机2累计工作时间")]
        public int jz1ysj2ljgzsj { get; set; }

        /// <summary> 
        /// Desc:机组1空气净化器累计工作时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1空气净化器累计工作时间")]
        public int jz1kqjhqljgzsj { get; set; }

        /// <summary> 
        /// Desc:机组1紫外线灯累计工作时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1紫外线灯累计工作时间")]
        public int jz1zwxdljgzsj { get; set; }

        /// <summary> 
        /// Desc:机组1空气净化器灯管累计工作时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1空气净化器灯管累计工作时间")]
        public int jz1kqjhqdgljgzsj { get; set; }

        /// <summary> 
        /// Desc:机组1紫外线灯灯管累计工作时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1紫外线灯灯管累计工作时间")]
        public int jz1zwxddgljgzsj { get; set; }

        /// <summary> 
        /// Desc:机组1通风机紧急通风接触器动作次数
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1通风机紧急通风接触器动作次数")]
        public int jz1tfjjjtfjcqdzcs { get; set; }

        /// <summary> 
        /// Desc:机组1通风机接触器动作次数
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1通风机接触器动作次数")]
        public int jz1tfjjcqdzcs { get; set; }

        /// <summary> 
        /// Desc:机组1通风机2接触器动作次数
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1通风机2接触器动作次数")]
        public int jz1tfj2jcqdzcs { get; set; }

        /// <summary> 
        /// Desc:机组1冷凝风机接触器动作次数
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1冷凝风机接触器动作次数")]
        public int jz1lnfjjcqdzcs { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机1接触器动作次数
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机1接触器动作次数")]
        public int jz1ysj1jcqdzcs { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机2接触器动作次数
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机2接触器动作次数")]
        public int jz1ysj2jcqdzcs { get; set; }

        /// <summary> 
        /// Desc:机组1主回路1 漏电流值
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1主回路1 漏电流值")]
        public double jz1zhl1ldlz { get; set; }

        /// <summary> 
        /// Desc:机组1主回路2 漏电流值
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1主回路2 漏电流值")]
        public double jz1zhl2ldlz { get; set; }

        /// <summary> 
        /// Desc:压缩机变频器1 PFC温度
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "压缩机变频器1 PFC温度")]
        public double ysjbpq1pfcwd { get; set; }

        /// <summary> 
        /// Desc:压缩机变频器1 IGBT温度
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "压缩机变频器1 IGBT温度")]
        public double ysjbpq1igbtwd { get; set; }

        /// <summary> 
        /// Desc:压缩机变频器2 PFC温度
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "压缩机变频器2 PFC温度")]
        public double ysjbpq2pfcwd { get; set; }

        /// <summary> 
        /// Desc:压缩机变频器2 IGBT温度
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "压缩机变频器2 IGBT温度")]
        public double ysjbpq2igbtwd { get; set; }

        /// <summary> 
        /// Desc:通风模式
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "通风模式")]
        public int tfms { get; set; }

        /// <summary> 
        /// Desc:紧急通风模式
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "紧急通风模式")]
        public int jjtfms { get; set; }

        /// <summary> 
        /// Desc:手动冷模式
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "手动冷模式")]
        public int sdlms { get; set; }

        /// <summary> 
        /// Desc:UIC自动模式
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "UIC自动模式")]
        public int uiczdms { get; set; }

        /// <summary> 
        /// Desc:预冷模式
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "预冷模式")]
        public int ylms { get; set; }

        /// <summary> 
        /// Desc:全新风
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "全新风")]
        public int qxf { get; set; }

        /// <summary> 
        /// Desc:除菌模式
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "除菌模式")]
        public int cjms { get; set; }

        /// <summary> 
        /// Desc:停机模式
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "停机模式")]
        public int tjms { get; set; }

        /// <summary> 
        /// Desc:空调减载完成
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "空调减载完成")]
        public int ktjzwc { get; set; }

        /// <summary> 
        /// Desc:车外火灾模式
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "车外火灾模式")]
        public int cwhzms { get; set; }

        /// <summary> 
        /// Desc:车内火灾模式
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "车内火灾模式")]
        public int cnhzms { get; set; }

        /// <summary> 
        /// Desc:空调控制模式
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "空调控制模式")]
        public int ktkzms { get; set; }

        /// <summary> 
        /// Desc:机组1综合自检结果
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1综合自检结果")]
        public int jz1zhzjjg1 { get; set; }

        /// <summary> 
        /// Desc:机组1综合自检结果
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1综合自检结果")]
        public int jz1zhzjjg2 { get; set; }

        /// <summary> 
        /// Desc:机组1高压自检结果
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1高压自检结果")]
        public int jz1gyzjjg1 { get; set; }

        /// <summary> 
        /// Desc:机组1高压自检结果
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1高压自检结果")]
        public int jz1gyzjjg2 { get; set; }

        /// <summary> 
        /// Desc:机组1紫外线灯状态
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1紫外线灯状态")]
        public int jz1zwxdzt { get; set; }

        /// <summary> 
        /// Desc:机组1空气净化器状态
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1空气净化器状态")]
        public int jz1kqjhqzt { get; set; }

        /// <summary> 
        /// Desc:机组1通风机1状态
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1通风机1状态")]
        public int jz1tfj1zt { get; set; }

        /// <summary> 
        /// Desc:机组1通风机2状态
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1通风机2状态")]
        public int jz1tfj2zt { get; set; }

        /// <summary> 
        /// Desc:机组1冷凝风机1状态
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1冷凝风机1状态")]
        public int jz1lnfj1zt { get; set; }

        /// <summary> 
        /// Desc:机组1冷凝风机2状态
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1冷凝风机2状态")]
        public int jz1lnfj2zt { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机机1状态
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机机1状态")]
        public int jz1ysj1zt { get; set; }

        /// <summary> 
        /// Desc:机组1压缩机机2状态
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "机组1压缩机机2状态")]
        public int jz1ysjj2zt { get; set; }

        /// <summary> 
        /// Desc:紧急通风状态
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "紧急通风状态")]
        public int jjtfzt { get; set; }


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

        /// <summary> 
        /// Desc:CRC
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "CRC")]
        public int CRC { get; set; }
        /// <summary>
        /// Desc:创建时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "年月日时分秒.毫秒")]
        public DateTime rq { get; set; }
        /// <summary>
        /// Desc:创建时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "创建时间")]
        public DateTime create_time { get; set; }
       
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "")]
        public long? timestamp { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "kafka offset")]
        public long? offset { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(ColumnDescription = "partition")]
        public int? partition { get; set; }

    }
}
