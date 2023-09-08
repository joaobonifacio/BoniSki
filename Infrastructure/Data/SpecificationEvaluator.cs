using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, 
            ISpecification<TEntity> spec)
        {
            var query = inputQuery;

            if(spec.Criteria != null)
            {
                //T será Product e depois temos de especificar o nosso criteria
                query = query.Where(spec.Criteria);
            }

            //Agora para os OrderBy
            if(spec.OrderBy != null)
            {
                //OrderBy = x=>x.Name || x=>x.Price
                query = query.OrderBy(spec.OrderBy);
            }

            if(spec.OrderByDescending != null)
            {
                //OrderBy = x=>x.Name || x=>x.Price
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            if(spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }
            
            //Aqui dizemos-lhe o que incluir
            query = spec.Includes.Aggregate(query,(current, include) => current.Include(include));

            return query;
        }
    }
}