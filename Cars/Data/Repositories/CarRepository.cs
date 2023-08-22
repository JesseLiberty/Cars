using Cars.Data.Entities;
using Dapper;

namespace Cars.Data.Repositories;

public class CarRepository
{
    readonly DatabaseConnectionFactory databaseConnectionFactory;

    public CarRepository(DatabaseConnectionFactory databaseConnectionFactory)
    {
        this.databaseConnectionFactory = databaseConnectionFactory;
    }

    public async Task<IEnumerable<Car>> GetAll()
    {
        using var db = databaseConnectionFactory.GetConnection();
        return await db.QueryAsync<Car>("select * from car");
    }
    
}