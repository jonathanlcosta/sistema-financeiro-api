using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using SistemaFinanceiros.Dominio.Mensageria.Interfaces;

namespace SistemaFinanceiros.Infra.Mensageria.Servicos
{
    public class MensageriaServico : IMensageriaServico
    {
        private readonly ConnectionFactory connectionFactory;

        public MensageriaServico()
        {
            connectionFactory = new ConnectionFactory
            {
                HostName = "localhost"
            };
        }

        public void Publish(string queue, byte[] mensagem)
        {
            using (var connection = connectionFactory.CreateConnection())
            {
                using(var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queue,
                    durable:false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: queue,
                        basicProperties: null,
                        body: mensagem
                    );
                }
            }
        }
    }
}