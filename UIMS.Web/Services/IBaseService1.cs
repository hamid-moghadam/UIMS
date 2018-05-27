using System.Collections.Generic;
using System.Threading.Tasks;
using UIMS.Web.Models;
using UIMS.Web.Models.Interfaces;
using UIMS.Web.DTO;
using System.Linq.Expressions;
using System;

namespace UIMS.Web.Services
{
    public interface IBaseService<TModel, TInsertModel,TUpdateModel, TViewModel>
        where TModel : class, IKey<int>
        where TUpdateModel : BaseModel
        where TViewModel : BaseModel
    {
        Task<TModel> AddAsync(TInsertModel model);
        IEnumerable<TViewModel> GetAll();
        Task<PaginationViewModel<TViewModel>> GetAll(int page, int pageSize);
        Task<PaginationViewModel<TCustomViewModel>> GetAll<TCustomViewModel>(int page, int pageSize);
        Task<TViewModel> GetAsync(int id);
        Task<TModel> GetAsync(Expression<Func<TModel, bool>> expression);
        Task<bool> IsExistsAsync(Expression<Func<TModel, bool>> expression);
    }
}