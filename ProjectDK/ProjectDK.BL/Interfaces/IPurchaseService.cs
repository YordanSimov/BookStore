using ProjectDK.Models.Models;

namespace ProjectDK.BL.Interfaces
{
    public interface IPurchaseService
    {
        Task<Purchase?> SavePurchase(Purchase purchase);

        Task<Guid> DeletePurchase(Purchase purchase);

        Task<IEnumerable<Purchase>> GetPurchases(int userId);
    }
}
