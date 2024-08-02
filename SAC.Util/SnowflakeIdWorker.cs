using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Threading;

namespace SAC.Util
{
    /// <summary>
    /// 雪花算法获取ID
    /// </summary>
    public class SnowflakeIdWorker
    {

        private const long twepoch = 1288834974657L; // Twitter的起始时间戳（2010-11-04T01:42:54.657Z）  
        private const int workerIdBits = 5; // 机器ID所占的位数  
        private const int datacenterIdBits = 5; // 数据中心ID所占的位数  
        private const long maxWorkerId = -1L ^ -1L << workerIdBits; // 机器ID的最大值  
        private const long maxDatacenterId = -1L ^ -1L << datacenterIdBits; // 数据中心ID的最大值  
        private const int sequenceBits = 12; // 序列号占用的位数  

        private long workerId;
        private long datacenterId;
        private long sequence = 0L;

        private long lastTimestamp = -1L;

        public SnowflakeIdWorker(long workerId, long datacenterId)
        {

            if (workerId > maxWorkerId || workerId < 0)
            {
                throw new ArgumentException(string.Format("worker Id can't be greater than {0} or less than 0", maxWorkerId));
            }

            if (datacenterId > maxDatacenterId || datacenterId < 0)
            {
                throw new ArgumentException(string.Format("datacenter Id can't be greater than {0} or less than 0", maxDatacenterId));
            }

            this.workerId = workerId;
            this.datacenterId = datacenterId;
        }

        public long NextId()
        {
            lock (this)
            {
                long timestamp = TimeGen();

                if (timestamp < lastTimestamp)
                {
                    throw new Exception(string.Format("Clock moved backwards. Refusing to generate id for {0} milliseconds", lastTimestamp - timestamp));
                }

                if (lastTimestamp == timestamp)
                {
                    sequence = sequence + 1 & maxGeneratedIdAtMillis;
                    if (sequence == 0)
                    {
                        timestamp = TilNextMillis(lastTimestamp);
                    }
                }
                else
                {
                    sequence = 0L;
                }

                lastTimestamp = timestamp;

                return timestamp - twepoch << workerIdBits + datacenterIdBits + sequenceBits |
                       datacenterId << workerIdBits |
                       workerId << sequenceBits |
                       sequence;
            }
        }

        private long TilNextMillis(long lastTimestamp)
        {
            long timestamp = TimeGen();
            while (timestamp <= lastTimestamp)
            {
                timestamp = TimeGen();
            }
            return timestamp;
        }

        private long TimeGen()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        private long maxGeneratedIdAtMillis = -1L ^ -1L << sequenceBits;
    }
}
