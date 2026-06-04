using System.Data;
using Dapper;
using DataAbstractions.Dapper;
using OffshoreInsights.Infrastructure.Services.Interfaces;

namespace OffshoreInsights.Infrastructure.Services;

public class SqlDataAccessor(IDataAccessor dataAccessor) : ISqlDataAccessor
{
    public async Task<T?> GetAsync<T>(string query, DynamicParameters parameters, CancellationToken ct) 
        where T : class
    {
        var result = await dataAccessor.QueryAsync<T?>(
            new CommandDefinition(query, parameters, cancellationToken: ct));
        return result.FirstOrDefault();
    }

    public async Task<T> QuerySingleAsync<T>(string query, DynamicParameters parameters, CancellationToken ct)
    {
        return await dataAccessor.QuerySingleAsync<T>(
            new CommandDefinition(query, parameters, cancellationToken: ct));
    }

    public async Task<IEnumerable<T>> LoadDataAsync<T, U>(string storedProcedure, U parameters, CancellationToken ct)
    {
        return await dataAccessor.QueryAsync<T>(new CommandDefinition(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure, cancellationToken: ct));
    }

    public async Task<int> InsertAsync(string command, DynamicParameters parameters, CancellationToken ct)
    {
        return await dataAccessor.QuerySingleAsync<int>(
            new CommandDefinition(command, parameters, cancellationToken: ct));
    }

    public async Task<int> UpdateAsync(string command, DynamicParameters parameters, CancellationToken ct)
    {
        return await dataAccessor.ExecuteAsync(
            new CommandDefinition(command, parameters, cancellationToken: ct));
    }

    public async Task SaveDataAsync<T>(string storedProcedure, T parameters, CancellationToken ct)
    {
        await dataAccessor.ExecuteAsync(new CommandDefinition(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure, cancellationToken: ct));
    }
}