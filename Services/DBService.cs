// Services/DatabaseService.cs
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Data.SqlClient;


namespace DapperApi.Services{
public class DatabaseService<T> : IDatabaseService<T>
{
    private readonly string _connectionString;

    public DatabaseService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default");
    }

    public T Add(T entity)
    {
         try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string tableName = typeof(T).Name.ToLower() ; 
                var properties = typeof(T).GetProperties().Where(p => !Attribute.IsDefined(p, typeof(System.ComponentModel.DataAnnotations.KeyAttribute)));
                var columnNames = string.Join(", ", properties.Select(parameter => parameter.Name));
                var paramNames = string.Join(", ", properties.Select(parameter => "@" + parameter.Name));
                
                string sqlQueryToAdd = $@"
                    INSERT INTO {tableName} ({columnNames})
                    OUTPUT INSERTED.*
                    VALUES ({paramNames})";
                
                return connection.QuerySingle<T>(sqlQueryToAdd, entity);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new Exception("Error adding entity", ex);
        }
    }

    public void CreateTable(string tableName, string tableSchema)
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // SQL command to create a table
                string createTableQuery = $@"
                    CREATE TABLE {tableName} (
                        {tableSchema}
                    )";

                // Execute the query
                connection.Execute(createTableQuery);
            }
        }
        catch (Exception ex)
        {
            // Log the exception (for simplicity, writing to console)
            Console.WriteLine(ex.Message);
            throw new Exception("Error creating table", ex);
        }
    }
     public void AlterTable(string tableName, string columnName, string columnType)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string alterTableQuery = $@"
                        ALTER TABLE {tableName}
                        ADD {columnName} {columnType}";

                    connection.Execute(alterTableQuery);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error altering table", ex);
            }
        }

    public IEnumerable<T> GetAll()
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string tableName = typeof(T).Name.ToLower() ;
                string sqlQuery = $"SELECT * FROM {tableName}";
                return connection.Query<T>(sqlQuery);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new Exception("Error retrieving entities", ex);
        }
    }

    public T GetById(int id)
    {
       
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string tableName = typeof(T).Name.ToLower() ; // 
                string sqlQuery = $"SELECT * FROM {tableName} WHERE {typeof(T).Name}Id = @Id";
                return connection.QuerySingleOrDefault<T>(sqlQuery, new { Id = id });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new Exception("Error retrieving entity", ex);
        }
    }
    public bool Delete(int id)
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string tableName = typeof(T).Name.ToLower(); // assuming table name is plural of entity type
                string sqlQuery = $"DELETE FROM {tableName} WHERE {typeof(T).Name}Id = @Id";
                int affectedRows = connection.Execute(sqlQuery, new { Id = id });
                return affectedRows > 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new Exception("Error deleting entity", ex);
        }
    }
    }
}
