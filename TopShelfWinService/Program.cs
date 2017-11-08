using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ninject.Modules;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Topshelf;
using Topshelf.Ninject;

namespace TopShelfWinService
{
    public class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.UseNinject(new TopShelfWinServiceModule());
                x.Service<ITopShelfMqListenService>(s =>
                {
                    s.ConstructUsingNinject();
                    s.WhenStarted(sender => sender.Start());
                    s.WhenStopped(sender => sender.Stop());
                });
                x.RunAsLocalSystem();

                x.SetServiceName("TestTopShelfService");
                x.SetDisplayName("TestTopShelfService");
                x.SetDescription("TestTopShelfService");
            });
        }
    }

    public interface ITopShelfMqListenService
    {
        void Start();
        void Stop();
    }

    public abstract class BaseTopShelfMqListenService : ITopShelfMqListenService
    {
        public abstract void ExecuteTask(string userRequest);
        public void Start()
        {
            var myThread = new Thread(InnerStart);
            myThread.IsBackground = true;
            myThread.Start();
        }

        protected virtual void InnerStart()
        {
        }

        public void Stop()
        {
            //Thread myThread = new Thread(InnerStop);
            //myThread.IsBackground = true;
            //myThread.Start();
            InnerStop();
        }

        protected virtual void InnerStop()
        {
        }
    }

    public class MyTopShelfMqListenService : BaseTopShelfMqListenService
    {
        private const string HostName = "localhost";
        private const string UserName = "guest";
        private const string Password = "guest";
        private const string VirtualHost = "";
        private int Port = 0;

        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;

        protected override void InnerStart()
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = HostName,
                UserName = UserName,
                Password = Password
            };

            //_connectionFactory.AutomaticRecoveryEnabled = true;
            //_connectionFactory.ContinuationTimeout = TimeSpan.MaxValue;

            if (string.IsNullOrEmpty(VirtualHost) == false)
                _connectionFactory.VirtualHost = VirtualHost;
            if (Port > 0)
                _connectionFactory.Port = Port;

            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _connection.AutoClose = true;
            _channel.BasicQos(0, 1, false); //Qos=Quality of Service

            var consumer = new EventingBasicConsumer(model: _channel);

            consumer.Received += (model, ea) =>
            {
                SpawnTask(ea.Body);
            };

            _channel.BasicConsume(queue: "hubHost", noAck: true, consumer: consumer);
        }
        private void SpawnTask(byte[] body)
        {
            var response = Encoding.Default.GetString(body);
            //var responseObj = Converter.ConvertToType<UserRequest>(response);
            Task spawnedTask = new Task(() => ExecuteTask(""));
            spawnedTask.Start();
        }

        public override void ExecuteTask(string userRequest)
        {
            
        }

        protected override void InnerStop()
        {
            _channel.Close();
        }
    }

    public class TopShelfWinServiceModule: NinjectModule
    {
        public override void Load()
        {
            Bind<ITopShelfMqListenService>().To<MyTopShelfMqListenService>().InSingletonScope();
        }
    }
}
