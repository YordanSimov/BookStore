using ProjectDK.Models.Models;

namespace ProjectDK.DL.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task AddToCart(ShoppingCart shoppingCart);

        Task<ShoppingCart?> RemoveFromCart(ShoppingCart shoppingCart);

        Task EmptyCart(int userId);

        Task<IEnumerable<Book>> GetContent(int userId);

        Task FinishPurchase(int userId);

        Task<ShoppingCart?> GetShoppingCart(int userId);
    }
}