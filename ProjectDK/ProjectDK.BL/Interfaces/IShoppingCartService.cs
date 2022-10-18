using ProjectDK.Models.Models;

namespace ProjectDK.BL.Interfaces
{
    public interface IShoppingCartService
    {
        Task AddToCart(int userId, Book book);

        Task<ShoppingCart?> RemoveFromCart(ShoppingCart shoppingCart);

        Task<bool> EmptyCart(int userId);

        Task<IEnumerable<Book>> GetContent(int userId);

        Task FinishPurchase(int userId);
        Task<ShoppingCart?> GetShoppingCart(int userId);
    }
}