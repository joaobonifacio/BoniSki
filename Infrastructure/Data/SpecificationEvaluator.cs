using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            //Aqui dizemos-lhe o que incluir
            query = spec.Includes.Aggregate(query,(current, include) => current.Include(include));

            return query;
        }
    }
}