using ProjectDK.Models.Models;

namespace ProjectDK.DL.Interfaces
{
    public interface IPurchaseRepository
    {
        Task<Purchase?> SavePurchase(Purchase purchase);

        Task<Guid> DeletePurchase(Purchase purchase);

        Task<IEnumerable<Purchase>> GetPurchases(int userId);
    }
}
