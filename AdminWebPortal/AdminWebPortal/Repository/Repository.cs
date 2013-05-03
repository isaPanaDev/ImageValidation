using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects.DataClasses;
using System.Linq.Expressions;
using System.Data;
using System.Data.Objects;

namespace AdminWebPortal.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private ObjectContext _context;
        private readonly IObjectSet<TEntity> _objectSet;

        public Repository(ObjectContext context)
        {
            _context = context;
            _objectSet = _context.CreateObjectSet<TEntity>();
            //_context.ContextOptions.LazyLoadingEnabled = true; Default!
        }

        public Repository(ObjectContext context, bool lazyLoading)
        {
            _context = context;
            _objectSet = _context.CreateObjectSet<TEntity>();
            _context.ContextOptions.LazyLoadingEnabled = lazyLoading;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void SaveChanges(SaveOptions options)
        {
            _context.SaveChanges(options);
        }

        public void Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _objectSet.AddObject(entity);
        }

        public void Edit(TEntity entity)
        {
            _objectSet.Attach(entity);
            _context.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);
        }

        public void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _objectSet.DeleteObject(entity);
        }
        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var records = Find(predicate);

            foreach (var record in records)
            {
                Delete(record);
            }
        }

        public void Delete(ISpecification<TEntity> specification)
        {
            throw new NotImplementedException();
        }

        public void DeleteRelatedEntities(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var releatedEntities =
                ((IEntityWithRelationships)entity).RelationshipManager.GetAllRelatedEnds().SelectMany(
                    e => e.CreateSourceQuery().OfType<TEntity>()).ToList();
            foreach (var releatedEntity in releatedEntities)
            {
                _objectSet.DeleteObject(releatedEntity);
            }
            _objectSet.DeleteObject(entity);

        }

        public IEnumerable<TEntity> GetAll()
        {
            return _objectSet.AsEnumerable();
        }

        public IEnumerable<TEntity> GetAllPaged(int page, int pageSize)
        {
            return _objectSet.AsEnumerable().Skip(pageSize).Take(page);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _objectSet.Where(predicate).AsEnumerable();
        }

        public IEnumerable<TEntity> Find(ISpecification<Func<TEntity, bool>> specification)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> FindPaged(int page, int pageSize, Expression<Func<TEntity, bool>> predicate)
        {
            return _objectSet.Where(predicate).Skip(pageSize).Take(page).AsEnumerable();
        }

        public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return _objectSet.Single(predicate);
        }

        public TEntity Single(ISpecification<Func<TEntity, bool>> specification)
        {
            throw new NotImplementedException();
        }

        public TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return _objectSet.First(predicate);
        }

        public TEntity First(ISpecification<Func<TEntity, bool>> specification)
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            return _objectSet.Count();
        }

        public int Count(Expression<Func<TEntity, bool>> criteria)
        {
            throw new NotImplementedException();
        }

        public int Count(ISpecification<TEntity> specification)
        {
            throw new NotImplementedException();
        }

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (_context == null) return;
            _context.Dispose();
            _context = null;
        }
        #endregion
    }
}