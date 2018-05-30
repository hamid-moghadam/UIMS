using System.Threading.Tasks;
using UIMS.Web.Models;

namespace UIMS.Web.Services
{
    public interface IBaseServiceProvider<TModel> where TModel : class, IKey<int>
    {
        Task<TModel> AddAsync(TModel model);
        void Remove(TModel model);
        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
}