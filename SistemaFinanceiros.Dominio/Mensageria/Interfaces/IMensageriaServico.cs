using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaFinanceiros.Dominio.Mensageria.Interfaces
{
    public interface IMensageriaServico
    {
        void Publish(string queue, byte[] mensagem);
    }
}