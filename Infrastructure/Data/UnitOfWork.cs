using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext ctx;
        private Hashtable repositories;
        
        public UnitOfWork(StoreContext storeContext)
        {
            this.ctx=storeContext;
        }

        public async Task<int> Complete()
        {
            return await ctx.SaveChangesAsync();
        }

        public void Dispose()
        {
            ctx.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            //Check se já existe pelo menos 1 repo; se não instancia 1
            if(repositories == null)
            {
                repositories = new Hashtable();
            }
            
            //Type da entidade do repository
            var type = typeof(TEntity).Name;

            //Check se já temos um repository desse tipo
            if(!repositories.ContainsKey(type))
            {
                //Se não tivermos, cria instance de generic repository desse type

                //cria type de repository -> no caso, generic
                var repositoryType = typeof(GenericRepository<>);
                
                //cria instance de repository e passa-lhe  o context da db
                //1a instance de genericRepository<Order>, por exemplo
                var repositoryInstance = Activator.CreateInstance(repositoryType
                    .MakeGenericType(typeof(TEntity)), ctx);

                //Add type(key) e repository(value) instance à hashtable
                repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<TEntity>) repositories[type];
        }
    }
}