using Microsoft.Data.Sqlite;
using Dapper;
using Polly;
using Polly.Retry;
using Questao5.Infrastructure.Sqlite;
using System;
using System.Linq;
using static Questao5.Domain.MovimentacaoModels;


public class MovimentacaoContaService
{
    private readonly RetryPolicy _retryPolicy;
    private readonly DatabaseConfig _databaseConfig;

    public MovimentacaoContaService(DatabaseConfig databaseConfig)
    {
        _retryPolicy = Policy.Handle<Exception>()
                             .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        _databaseConfig = databaseConfig;
    }

    public MovimentacaoResponse MovimentarConta(MovimentacaoRequest request)
    {
        
        return _retryPolicy.Execute(() =>
        {
            // Lógica para movimentar a conta, incluindo as validações de negócio
            if (!ContaCorrenteExiste(request.IdentificacaoContaCorrente,_databaseConfig))
                throw new Exception("Conta corrente não encontrada.");

            if (!ContaCorrenteAtiva(request.IdentificacaoContaCorrente, _databaseConfig))
                throw new Exception("Conta corrente inativa.");

            if (request.ValorMovimentado <= 0)
                throw new Exception("Valor da movimentação inválido.");

            if (request.TipoMovimento != 'C' && request.TipoMovimento != 'D')
                throw new Exception("Tipo de movimento inválido.");

            // Persistir os dados na tabela MOVIMENTO
            int idMovimento = PersistirMovimento(request, _databaseConfig);

            // Retornar a resposta
            return new MovimentacaoResponse { IdMovimento = idMovimento };
        });
    }

    private bool ContaCorrenteExiste(string identificacaoContaCorrente, DatabaseConfig databaseConfig)
    {
        using var connection = new SqliteConnection(databaseConfig.Name);

        // Consulta SQL para verificar se a conta corrente existe com base no ID fornecido
        var query = "SELECT * FROM contacorrente Where numero = @IdContaCorrente";

        // Executa a consulta SQL e obtém o resultado        
        var dataSet = connection.Query(query, new { IdContaCorrente = identificacaoContaCorrente});        

        // Retorna true se a contagem for maior que zero (ou seja, a conta corrente existe)
        // Retorna false caso contrário
        return dataSet.Count()>0;
    }

    public bool ContaCorrenteAtiva(string identificacaoContaCorrente, DatabaseConfig databaseConfig)
    {
        using var connection = new SqliteConnection(databaseConfig.Name);

        // Consulta SQL para verificar se a conta corrente está ativa com base no ID fornecido
        var query = "SELECT ativo FROM contacorrente WHERE numero = @IdContaCorrente AND ativo = 1";

        // Executa a consulta SQL e obtém o status da conta corrente
        var dataSet = connection.Query(query, new { IdContaCorrente = identificacaoContaCorrente });

        // Retorna true se a conta corrente estiver ativa
        // Retorna false caso contrário
        return dataSet.Count() > 0;
    }

    public int PersistirMovimento(MovimentacaoRequest request, DatabaseConfig databaseConfig)
    {
        using var connection = new SqliteConnection(databaseConfig.Name);

        var query = "SELECT idcontacorrente FROM contacorrente WHERE numero = @numeroConta";
        
        var idContaCorrente = connection.Query(query, new { numeroConta = request.IdentificacaoContaCorrente });
        var idContaCorrenteList = idContaCorrente.ToList();

        dynamic PrimeiraLinha = idContaCorrenteList[0];
        var valor = PrimeiraLinha.idcontacorrente;
        
        if (idContaCorrente == null)
            throw new Exception("Numero de conta corrente inválido.");

        query = string.Empty;
        // Consulta SQL para inserir os dados do movimento na tabela MOVIMENTO
        query = @"INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) 
                      VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)";

        // Executa a consulta SQL e obtém o ID do movimento persistido
        var idMovimento = connection.Execute(query, new
        {
            IdMovimento = Guid.NewGuid().ToString(), // Gera um novo ID de movimento
            IdContaCorrente = valor,
            DataMovimento = DateTime.UtcNow,
            TipoMovimento = request.TipoMovimento,
            Valor = request.ValorMovimentado
        });

        // Retorna o ID do movimento persistido
        return idMovimento;
    }
}
