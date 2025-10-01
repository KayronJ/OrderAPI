using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.Domain.Entities
{

    /*
    •Não é possível cadastrar 2 (duas) ou mais ocorrências do mesmo tipo em um intervalo de 10 (dez) minutos entre a última
    ocorrência cadastrada.

    • Quando for cadastrada a segunda ocorrência para o mesmo pedido, ela será uma ocorrência finalizadora. Ao registrar a
    segunda ocorrência, é necessário salvá-la no banco de dados como IndFinalizadora = true;

    • Caso o pedido tenha sido finalizado com uma ocorrência de EntregueComSucesso, deve-se cadastrar o pedido como
    IndEntregue = true. Caso tenha sido finalizado com insucesso - qualquer outra ocorrência diferente de
    EntregueComSucesso - deve-se salvar no banco como IndEntregue = false.

    • Ao excluir uma ocorrência, deve-se analisar se o pedido está concluído. Se entrar nesta condição, a ocorrência não pode
    ser cadastrada ou excluída.
     */
    public class Order
    {

        public int OrderId { get; set; }
        public int OrderNumber { get; set; }
        public List<Occurrence> Occurrences { get; set; }
        public DateTime OrderTime { get; set; }
        public bool DeliveredInd { get; set; }


        public Order(int orderNumber, bool deliveredInd = false)
        {
            OrderNumber = orderNumber;
            OrderTime = DateTime.UtcNow;
            DeliveredInd = deliveredInd;
        }
    }
}
