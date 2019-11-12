using DFC.Api.JobProfiles.Data.DataModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DFC.Api.JobProfiles.Repository.CosmosDb
{
    public interface ICosmosRepository<T>
        where T : BaseDataModel
    {
        Task<bool> PingAsync();

        Task<IList<T>> GetData(Expression<Func<T, T>> selector, Expression<Func<T, bool>> filter);
    }
}