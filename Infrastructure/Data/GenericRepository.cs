using System.Security.Cryptography.X509Certificates;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext ctx;

        public GenericRepository(StoreContext context) 
        {  
            ctx = context;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await ctx.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await ctx.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        //Se o ProductRepository chamar o Generic e lhe passar Product, ctx=ctx.Products  
        public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
        {
             return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        //Se o ProductRepository chamar o Generic e lhe passar Product, ctx=ctx.Products
        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            //O ctx.Set<T>().AsQueryable() é o IQueryable<TEntity> inputQuery do SpecificationEvaluator
            //O T será, por exemplo Product
            return SpecificationEvaluator<T>.GetQuery(ctx.Set<T>().AsQueryable(), specification);
        }

        public void Add(T entity)
        {
            ctx.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            ctx.Set<T>().Attach(entity);
            ctx.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            ctx.Set<T>().Remove(entity);
        }
    }
}