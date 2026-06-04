using Dapper;

namespace OffshoreInsights.Infrastructure.Services.Interfaces;

public interface ISqlDataAccessor
{
    Task<T?> GetAsync<T>(string query, DynamicParameters parameters, CancellationToken ct) 
        where T : class;
    
    Task<T> QuerySingleAsync<T>(string query, DynamicParameters parameters, CancellationToken ct);
    
    Task<IEnumerable<T>> LoadDataAsync<T, U>(string storedProcedure, U parameters, CancellationToken ct);
    
    Task<int> InsertAsync(string command, DynamicParameters parameters, CancellationToken ct);
    
    Task<int> UpdateAsync(string command, DynamicParameters parameters, CancellationToken ct);
    
    Task SaveDataAsync<T>(string storedProcedure, T parameters, CancellationToken ct);
}