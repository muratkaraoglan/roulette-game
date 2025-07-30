namespace _Project.Scripts.Core.Services
{
    public interface ISaveService<T>
    {
        void Save(T data);
        T Load();
    }
}