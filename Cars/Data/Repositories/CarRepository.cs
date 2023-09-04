using Cars.Data.Entities;
using Dapper;

namespace Cars.Data.Repositories;

// Resources
// https://github.com/DapperLib/Dapper
// https://github.com/DapperLib/Dapper/tree/main/Dapper.SqlBuilder
public class CarRepository
{
    readonly DatabaseConnectionFactory databaseConnectionFactory;

    public CarRepository(DatabaseConnectionFactory databaseConnectionFactory)
    {
        this.databaseConnectionFactory = databaseConnectionFactory;
    }

    public async Task<IEnumerable<Car>> GetAll(bool returnDeletedRecords = false)
    {
        var builder = new SqlBuilder();
        var sqlTemplate = builder.AddTemplate("SELECT * FROM car /**where**/");
        if (!returnDeletedRecords)
        {
            builder.Where("is_deleted=0");
        }
        using var db = databaseConnectionFactory.GetConnection();
        return await db.QueryAsync<Car>(sqlTemplate.RawSql,sqlTemplate.Parameters);
    }
    
    public async Task<Car> Get(int id)
    {
        var query = "select * from car where id=@id";
        using var db = databaseConnectionFactory.GetConnection();
        return await db.QuerySingleOrDefaultAsync<Car>(query, new {id});
    }
    
    
}