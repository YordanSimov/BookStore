using ProjectDK.Models.Models;

namespace ProjectDK.BL.Interfaces
{
    public interface IAdditionalInfoProvider
    {
        Task<string> GetAdditionalInfo(Book book);
    }
}