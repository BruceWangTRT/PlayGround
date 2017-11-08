using ClassLibraryA.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aerospike.Client;
using ClassLibraryA.Utility;

namespace AerospikeCacheService
{
    public class BaseCacheService// : ICacheService
    {
        private const string DefaultNameSpace = "test";
        private const string DefaultSet = "bruceset";
        public static AerospikeClient GetClient()
        {
            ClientPolicy policy = new ClientPolicy();
//            policy.writePolicyDefault = new WritePolicy()
//            {
//#if DEBUG
//                timeout = 60000,
//#else
//                timeout = 10000,
//#endif
//                maxRetries = 5,
//            };
//            policy.readPolicyDefault = new Policy()
//            {
//                timeout = 10000,
//                maxRetries = 5,
//            };
            var aerospikeHosts = GetAerospikeHosts();
            AerospikeClient client = new AerospikeClient(policy, aerospikeHosts);
            return client;
        }

        private static Host[] GetAerospikeHosts()
        {
            var hostsStr = "192.168.86.102:3000";
            var hosts = hostsStr.Split(',');
            return hosts.Select(host =>
            {
                var split = host.Split(':');
                return new Host(split[0], Int32.Parse(split[1]));
            }).ToArray();
        }



        public bool SetValue<T>(string userKey, T obj, string nameSpace = DefaultNameSpace, string set = DefaultSet, int ttlSec = -1)
        {
            if(string.IsNullOrWhiteSpace(userKey)) return false;
            try
            {
                var client = GetClient();
                WritePolicy writePolicy = null;
                if (ttlSec != -1)
                {
                    writePolicy = new WritePolicy
                    {
                        expiration = ttlSec
                    };
                }
                client.Put(writePolicy, new Key(nameSpace, set, userKey), new Bin(null, obj.SerializeToStringWithDefaultSettings()));
                return true;
            }
            catch (Exception e)
            {
                //log
                return false;
            }
        }

        public T GetValue<T>(string userKey, string nameSpace = DefaultNameSpace, string set = DefaultSet)
        {
            if (string.IsNullOrWhiteSpace(userKey)) return default(T);
            try
            {
                var client = GetClient();
                var record = client.Get(null, new Key(nameSpace, set, userKey));
                return Deserialize<T>(record);
            }
            catch (Exception e)
            {
                //log
                return default(T);
            }
        }

        private T Deserialize<T>(Record record, string bin = null)
        {

            if (record == null) return default(T);

            var value = record.GetValue(bin ?? string.Empty) as string;

            var result = value.DeserializeToTypeWithDefaultSettings<T>();
            return result;
        }
    }
}
