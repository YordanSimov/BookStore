namespace ProjectDK.DL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();

        T? GetById(int id);

        T Add(T input);

        void Update(T input);

        T? Delete(int id);
    }
}
