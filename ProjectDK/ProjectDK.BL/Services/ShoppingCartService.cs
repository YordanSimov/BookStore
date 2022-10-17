using Microsoft.Extensions.Logging;
using ProjectDK.BL.Interfaces;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.Models;

namespace ProjectDK.BL.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly Dictionary<int, List<Book>> dict;
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly ILogger<ShoppingCartService> logger;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository, ILogger<ShoppingCartService> logger)
        {
            dict = new Dictionary<int, List<Book>>();
            this.shoppingCartRepository = shoppingCartRepository;
            this.logger = logger;
        }
        public async Task AddToCart(int userId, Book book)
        {
            var shoppingCart = await shoppingCartRepository.GetShoppingCart(userId);

            if (shoppingCart != null)
            {
                shoppingCart.Books.ToList().Add(book);
            }
            else
            {
                var shCart = new ShoppingCart()
                {
                    Id = Guid.NewGuid(),
                    Books = new List<Book>() { book },
                    UserId = userId,
                };
                await shoppingCartRepository.AddToCart(shCart);
            }


            //if (!dict.ContainsKey(userId))
            //{
            //    dict.Add(userId, new List<Book>() { book });
            //}
            //else
            //{
            //    dict[userId].Add(book);
            //}
        }

        public async Task<bool> EmptyCart(int userId)
        {
            if (await shoppingCartRepository.GetShoppingCart(userId) != null)
            {
                await shoppingCartRepository.EmptyCart(userId);
                return true;
            }
            return false;

            //    try
            //    {
            //        if (dict.ContainsKey(userId))
            //        {
            //            dict.Remove(userId);
            //            return Task.CompletedTask;
            //        }
            //        else
            //        {
            //            throw new InvalidOperationException($"There is no shopping cart for {userId}");
            //        }
            //    }
            //    catch (InvalidOperationException ex)
            //    {
            //        logger.LogError(ex.Message);
            //    }
            //    return null;
        }

        public async Task FinishPurchase(int userId)
        {
            await shoppingCartRepository.FinishPurchase(userId);

            //if (dict.ContainsKey(userId))
            //{
            //    var list = new List<Book>();
            //    foreach (var item in dict.FirstOrDefault(x => x.Key == userId).Value)
            //    {
            //        list.Add(item);
            //    }

            //    var purchase = new Purchase()
            //    {
            //        UserId = userId,
            //        Books = list,
            //        TotalMoney = list.Select(x => x.Price).Sum(),
            //    };
            //    await purchaseRepository.SavePurchase(purchase);
            //}
        }

        public async Task<IEnumerable<Book>> GetContent(int userId)
        {
            return await shoppingCartRepository.GetContent(userId);
            //if (dict.ContainsKey(userId))
            //{
            //    var list = new List<Book>();
            //    foreach (var item in dict.FirstOrDefault(x => x.Key == userId).Value)
            //    {
            //        list.Add(item);
            //    }
            //    return list;
            //}
            // return Enumerable.Empty<Book>();
        }

        public async Task<ShoppingCart?> GetShoppingCart(int userId)
        {
            return await shoppingCartRepository.GetShoppingCart(userId);
        }

        public async Task<ShoppingCart?> RemoveFromCart(ShoppingCart shoppingCart)
        {
            var cart = await shoppingCartRepository.GetShoppingCart(shoppingCart.UserId);

            if (cart == null)
            {
                logger.LogError("You should create shopping cart first");
                return null;
            }

            await shoppingCartRepository.RemoveFromCart(shoppingCart);
            return cart;

            //try
            //{
            //    if (dict.ContainsKey(userId))
            //    {
            //        var bookCheck = dict[userId].FirstOrDefault(x => x.Id == book.Id);

            //        if (bookCheck != null)
            //        {
            //            dict[userId].Remove(bookCheck);
            //            return Task.CompletedTask;
            //        }
            //        else
            //        {
            //            throw new NullReferenceException("Book to remove is invalid");
            //        }
            //    }
            //}
            //catch (NullReferenceException ex)
            //{
            //    logger.LogError(ex.Message);
            //}
            //return null;
        }
    }
}
