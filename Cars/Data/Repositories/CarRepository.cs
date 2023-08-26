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
            builder.Where("deleted=0 OR deleted IS NULL");
        }
        using var db = databaseConnectionFactory.GetConnection();
        return await db.QueryAsync<Car>(sqlTemplate.RawSql,sqlTemplate.Parameters);
    }
    
    public async Task<Car?> Get(int id)
    {
        var query = "select * from car where id=@id";
        using var db = databaseConnectionFactory.GetConnection();
        return await db.QuerySingleOrDefaultAsync<Car>(query, new {id});
    }
    
    public async Task<int> UpsertAsync(Car car)
    {
        using var db = databaseConnectionFactory.GetConnection();
        var sql = @"
        DECLARE @InsertedRows AS TABLE (Id int);

        MERGE INTO Car AS target
        USING (SELECT @Id AS Id, @Make AS Make, @Model AS Model, @Model_Year AS Model_Year, @Price AS Price, @Deleted AS Deleted) AS source 
        ON target.Id = source.Id
        WHEN MATCHED THEN 
            UPDATE SET 
                Make = source.Make, 
                Model = source.Model, 
                Model_Year = source.Model_Year, 
                Price = source.Price, 
                Deleted = source.Deleted
        WHEN NOT MATCHED THEN
            INSERT (Make, Model, Model_Year, Price, Deleted)
            VALUES (source.Make, source.Model, source.Model_Year, source.Price, source.Deleted)
            OUTPUT inserted.Id INTO @InsertedRows
        ;

        SELECT Id FROM @InsertedRows;
    ";

        var newId = await db.QuerySingleOrDefaultAsync<int>(sql, car);
        return newId == 0 ? car.Id : newId;
    }
    public async Task<int> DeleteAsync(int id)
    {
        using var db = databaseConnectionFactory.GetConnection();
        var query = "UPDATE car SET Deleted = 1 WHERE Id = @Id";
        return await db.ExecuteAsync(query, new { Id = id });
    }
    
}