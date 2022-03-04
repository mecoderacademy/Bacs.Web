using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bacs.Data.Repository
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        private BacsContext _context;
        internal DbSet<TEntity> dbSet;
        public GenericRepository(BacsContext context)
        {
            _context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public IEnumerable<TEntity> GetAll(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }

        }
        public  TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public  void Insert(TEntity entity)
        {
              _context.Set<TEntity>().Add(entity);
           
        }
        public  void Update(TEntity entityToUpdate)
        {
                dbSet.Attach(entityToUpdate);
                _context.Entry(entityToUpdate).State = EntityState.Modified;
        }


        public void Save()
        {
            _context.SaveChanges();
        }
        private void Dispose()
        {
            _context.Dispose();
        }
    }
}

