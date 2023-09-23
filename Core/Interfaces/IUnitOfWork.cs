using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //Vai buscar o generic repo para o type específico
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

        //Return number of changes in our db
        Task<int> Complete();
    }
}