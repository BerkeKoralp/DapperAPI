// IDatabaseService.cs
namespace DapperApi.Services{
public interface IDatabaseService<T>
{
    void CreateTable(string tableName, string tableSchema);
      void AlterTable(string tableName, string columnName, string columnType);
    T Add(T entity);
    IEnumerable<T> GetAll();
    T GetById(int id);

    bool Delete(int id);
}
}