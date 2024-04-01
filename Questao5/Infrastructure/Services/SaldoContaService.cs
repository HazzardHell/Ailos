
using Microsoft.Data.Sqlite;
using Dapper;
using Polly;
using Polly.Retry;
using Questao5.Infrastructure.Sqlite;
using Questao5.Domain;

public class SaldoContaService
{
    private readonly RetryPolicy _retryPolicy;
    private readonly DatabaseConfig _databaseConfig;

    public SaldoContaService(DatabaseConfig databaseConfig)
    {
        _retryPolicy = Policy.Handle<Exception>()
                             .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        _databaseConfig = databaseConfig;
    }

    public decimal ConsultarSaldo(SaldoRequest request)
    {
        // Realizar validações de negócio
        if (!ContaCorrenteExiste(request.IdentificacaoContaCorrente,_databaseConfig))
            throw new Exception("Conta corrente não encontrada.");

        if (!ContaCorrenteAtiva(request.IdentificacaoContaCorrente, _databaseConfig))
            throw new Exception("Conta corrente inativa.");

        // Calcular saldo consultando as movimentações
        decimal saldo = CalcularSaldo(request.IdentificacaoContaCorrente);

        return saldo;
    }

    private bool ContaCorrenteExiste(string identificacaoContaCorrente, DatabaseConfig databaseConfig)
    {
        using var connection = new SqliteConnection(databaseConfig.Name);

        // Consulta SQL para verificar se a conta corrente existe com base no ID fornecido
        var query = "SELECT * FROM contacorrente Where numero = @IdContaCorrente";

        // Executa a consulta SQL e obtém o resultado        
        var dataSet = connection.Query(query, new { IdContaCorrente = identificacaoContaCorrente });

        // Retorna true se a contagem for maior que zero (ou seja, a conta corrente existe)
        // Retorna false caso contrário
        return dataSet.Count() > 0;
    }

    public bool ContaCorrenteAtiva(string identificacaoContaCorrente, DatabaseConfig databaseConfig)
    {
        using var connection = new SqliteConnection(databaseConfig.Name);

        // Consulta SQL para verificar se a conta corrente está ativa com base no ID fornecido
        var query = "SELECT ativo FROM contacorrente WHERE numero = @IdContaCorrente AND Ativo = 1";

        // Executa a consulta SQL e obtém o status da conta corrente
        var dataSet = connection.Query(query, new { IdContaCorrente = identificacaoContaCorrente });

        // Retorna true se a conta corrente estiver ativa
        // Retorna false caso contrário
        return dataSet.Count() > 0;
    }

    private decimal CalcularSaldo(string identificacaoContaCorrente)
    {
        // Lógica para calcular o saldo da conta corrente consultando as movimentações
        using var connection = new SqliteConnection(_databaseConfig.Name);

        var query = "SELECT idcontacorrente FROM contacorrente WHERE numero = @numeroConta";

        var idContaCorrente = connection.Query(query, new { numeroConta = identificacaoContaCorrente });
        var idContaCorrenteList = idContaCorrente.ToList();

        dynamic PrimeiraLinha = idContaCorrenteList[0];
        var valor = PrimeiraLinha.idcontacorrente;

        query = string.Empty;

        // Consulta SQL para calcular o saldo da conta corrente
        query = @"
            SELECT 
                COALESCE(SUM(CASE WHEN tipomovimento = 'C' THEN valor ELSE 0 END), 0) AS Creditos,
                COALESCE(SUM(CASE WHEN tipomovimento = 'D' THEN valor ELSE 0 END), 0) AS Debitos
            FROM movimento
            WHERE idcontacorrente = @IdContaCorrente";

        // Executa a consulta SQL e obtém os resultados
        var result = connection.QueryFirstOrDefault<(decimal Creditos, decimal Debitos)>(query, new { IdContaCorrente = valor });

        //var result = connection.Query(query, new { IdContaCorrente = valor });

        // Calcula o saldo da conta corrente
        decimal saldo = result.Creditos - result.Debitos;

        // Retorna o saldo calculado
        return saldo;       
        
    }
}
