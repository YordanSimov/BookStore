using ProjectDK.BL.Interfaces;
using ProjectDK.DL.Interfaces;
using ProjectDK.Models.Models;

namespace ProjectDK.BL.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository purchaseRepository;

        public PurchaseService(IPurchaseRepository purchaseRepository)
        {
            this.purchaseRepository = purchaseRepository;
        }
        public async Task<Guid> DeletePurchase(Purchase purchase)
        {
            return await purchaseRepository.DeletePurchase(purchase);
        }

        public async Task<IEnumerable<Purchase>> GetPurchases(int userId)
        {
            return await purchaseRepository.GetPurchases(userId);
        }

        public async Task<Purchase?> SavePurchase(Purchase purchase)
        {
            return await purchaseRepository.SavePurchase(purchase);
        }
    }
}
