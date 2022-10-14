namespace ProjectDK.Models.Models
{
    public interface ICacheItem<T>
    {
        T GetKey();
    }
}
