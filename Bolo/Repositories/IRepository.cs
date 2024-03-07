public interface IRepository<T>
{
    IEnumerable<T> GetAll();
    T GetById(int id);
    int Add(T entity);
    void Update(T entity);
    void Delete(int id);
    // Other methods like GetById, Add, Update, Delete etc.
}
