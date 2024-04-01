namespace Questao5.Domain
{
    public class MovimentacaoModels
    {
        // Classe para representar os dados da requisição de movimentação
        public class MovimentacaoRequest
        {
            public string IdentificacaoRequisicao { get; set; }
            public string IdentificacaoContaCorrente { get; set; }
            public decimal ValorMovimentado { get; set; }
            public char TipoMovimento { get; set; } // 'C' para Crédito, 'D' para Débito
        }

        // Classe para representar os dados da resposta da movimentação
        public class MovimentacaoResponse
        {
            public int IdMovimento { get; set; }
        }

    }
}
