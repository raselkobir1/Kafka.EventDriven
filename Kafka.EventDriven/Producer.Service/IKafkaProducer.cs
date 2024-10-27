using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producer.Service
{
    public interface IKafkaProducer
    {
        public Task Producer<T>(string topic, string key, T message); 
    }
}
