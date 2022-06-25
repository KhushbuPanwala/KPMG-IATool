using EFCore.BulkExtensions;
using InternalAuditSystem.DomailModel.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InternalAuditSystem.DomailModel.DataRepository
{
    public class DataRepository : IDataRepository
    {
        #region Private Members
        private readonly InternalAuditSystemContext _dbContext;
        private readonly IDbConnection _dbConnection;
        #endregion

        #region Constructor
        public DataRepository(InternalAuditSystemContext dbContext)
        {
            _dbContext = dbContext;
            _dbConnection = _dbContext.Database.GetDbConnection();
        }
        #endregion

        #region Destructor
        ~DataRepository()
        {
            _dbConnection.Close();
            _dbConnection.Dispose();
        }
        #endregion

        #region Public Methods


        /// <summary>
        /// This return first element that definately matches with the provided expression
        /// </summary>
        /// <typeparam name="T">Model class to get the data</typeparam>
        /// <param name="predicate">Expression to retrieve data</param>
        /// <returns>First element that matches all the condition</returns>
        public T First<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var dbSet = CreateDbSet<T>();
            return dbSet.First(predicate);
        }

        /// <summary>
        /// This return first element that definately matches with the provided expression asynchronously
        /// </summary>
        /// <typeparam name="T">Model class to get the data</typeparam>
        /// <param name="predicate">Expression to retrieve data</param>
        /// <returns>First element that matches all the condition</returns>
        public async Task<T> FirstAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var dbSet = CreateDbSet<T>();
            return await dbSet.FirstAsync(predicate);
        }


        /// <summary>
        /// Adds entity to the database.
        /// </summary>
        /// <typeparam name="T">Model class to create DbSet</typeparam>
        /// <param name="entity">Entity to add.</param>
        public EntityEntry<T> Add<T>(T entity) where T : class
        {
            var dbSet = CreateDbSet<T>();
            return dbSet.Add(entity);
        }


        /// <summary>
        /// Adds entities to the database asynchronously
        /// </summary>
        /// <typeparam name="T">Model class to create DbSet.</typeparam>
        /// <param name="entities">Entities to add.</param>
        /// <returns>Returns list of added entites</returns>
        public async Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class
        {
            var dbSet = CreateDbSet<T>();
            await dbSet.AddRangeAsync(entities);
        }

        /// <summary>
        /// Adds the given collection of entities into context 
        /// </summary>
        /// <typeparam name="T">Model class to add the data</typeparam>
        /// <param name="entities">List of entities that need to be added</param>
        /// <returns>Void</returns>
        public void AddRange<T>(IEnumerable<T> entities) where T : class
        {
            var dbSet = CreateDbSet<T>();
            dbSet.AddRange(entities);
        }


        /// <summary>
        /// Checks where data exist in database or not asynchronously
        /// </summary>
        /// <typeparam name="T">Model class to create DbSet.</typeparam>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>Bool value whether data exist or not</returns>
        public async Task<bool> AnyAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var dbSet = CreateDbSet<T>();
            return await dbSet.AnyAsync(predicate);
        }
        /// <summary>
        /// Checks where data exist in database or not.
        /// </summary>
        /// <typeparam name="T">Model class to create DbSet.</typeparam>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>Bool value whether data exist or not</returns>
        public bool Any<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var dbSet = CreateDbSet<T>();
            return dbSet.Any(predicate);
        }
        /// <summary>
        /// Checks where data exist in database or not with async.
        /// </summary>
        /// <typeparam name="T">Model class to create DbSet.</typeparam>
        /// <returns>Bool value whether data exist or not</returns>
        public async Task<bool> AnyAsync<T>() where T : class
        {
            var dbSet = CreateDbSet<T>();
            return await dbSet.AnyAsync();
        }


        /// <summary>
        /// Method to find data from database using its guid asynchronously.
        /// </summary>
        /// <typeparam name="T">Model class to create DbSet.</typeparam>
        /// <param name="id">Guid of data i.e primary key.</param>
        /// <returns>Returns model class</returns>
        public async Task<T> FindAsync<T>(Guid? id) where T : class
        {
            var dbSet = CreateDbSet<T>();
            return await dbSet.FindAsync(id);
        }

        /// <summary>
        /// Method to find data from database using its id.
        /// </summary>
        /// <typeparam name="T">Model class to create DbSet.</typeparam>
        /// <param name="id">Id of data i.e primary key.</param>
        /// <returns>Returns model class</returns>
        public T Find<T>(int id) where T : class
        {
            var dbSet = CreateDbSet<T>();
            return dbSet.Find(id);
        }

        /// <summary>
        /// Retrieves all the data.
        /// </summary>
        /// <typeparam name="T">Model class to create DbSet.</typeparam>
        /// <returns>List of all the elements.</returns>
        public IQueryable<T> GetAll<T>() where T : class
        {
            var dbSet = CreateDbSet<T>();
            return dbSet.AsQueryable();
        }

        /// <summary>
        /// Remove entity from the database.
        /// </summary>
        /// <typeparam name="T">Model class to create DbSet.</typeparam>
        /// <param name="entity">Entity to remove.</param>
        public EntityEntry<T> Remove<T>(T entity) where T : class
        {
            var dbSet = CreateDbSet<T>();
            return dbSet.Remove(entity);
        }

        /// <summary>
        /// Remove entities from the database.
        /// </summary>
        /// <typeparam name="T">Model class to create DbSet.</typeparam>
        /// <param name="entities">Entities to remove.</param>
        /// <returns>Void</returns>
        public void RemoveRange<T>(IEnumerable<T> entities) where T : class
        {
            var dbSet = CreateDbSet<T>();
            dbSet.RemoveRange(entities);
        }

        /// <summary>
        /// Method to commit database transaction.
        /// </summary>
        public void CommitTransaction()
        {
            _dbContext.Database.CommitTransaction();
        }

        /// <summary>
        /// Method to rollback database transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            _dbContext.Database.RollbackTransaction();
        }

        /// <summary>
        /// Updates entity in the database.
        /// </summary>
        /// <typeparam name="T">Model class to create DbSet.</typeparam>
        /// <param name="entity">Entity to update.</param>
        public EntityEntry<T> Update<T>(T entity) where T : class
        {
            var dbSet = CreateDbSet<T>();
            return dbSet.Update(entity);
        }

        /// <summary>
        /// Updates entities in the database.
        /// </summary>
        /// <typeparam name="T">Model class to create DbSet.</typeparam>
        /// <param name="entity">Entities to update.</param>
        /// <returns>Void</returns>
        public void UpdateRange<T>(IEnumerable<T> entities) where T : class
        {
                var dbSet = CreateDbSet<T>();
                dbSet.UpdateRange(entities);
        }

        /// <summary>
        /// Updates entities to the database.
        /// </summary>
        /// <typeparam name="T">Model class to create DbSet.</typeparam>
        /// <param name="entities">Entities to update.</param>
        /// <returns>Void</returns>
        public void AttachRange<T>(IEnumerable<T> entities) where T : class
        {
            var dbSet = CreateDbSet<T>();
            dbSet.AttachRange(entities);
        }

        /// <summary>
        /// Filters data based on predicate.
        /// </summary>
        /// <typeparam name="T">Model class to create DbSet.</typeparam>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>IQueryable containing filtered elements.</returns>
        public IQueryable<T> Where<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var dbSet = CreateDbSet<T>();
            return dbSet.Where(predicate);
        }

        /// <summary>
        /// Using select statement for selecting columns
        /// </summary>
        /// <typeparam name="T">Model class to create DbSet</typeparam>
        /// <typeparam name="Tresult">The return value of the method </typeparam>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>IQueryable containing filtered elements.</returns>
        public IQueryable<Tresult> Select<T, Tresult>(Expression<Func<T, Tresult>> predicate) where T : class
        {
            var dbSet = CreateDbSet<T>();
            return dbSet.Select(predicate);
        }

        /// <summary>
        /// Adds entity to the database asynchronously
        /// </summary>
        /// <typeparam name="T">Model class to create DbSet.</typeparam>
        /// <param name="entity">Entity to add.</param>
        public async Task<T> AddAsync<T>(T entity) where T : class
        {
            var dbSet = CreateDbSet<T>();
            var addedData =await dbSet.AddAsync(entity);
            return addedData.Entity;
        }

        /// <summary>
        /// Calls SaveChanges on the db context asynchronously
        /// </summary>
        /// <returns>Returns Int</returns>
        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Detach tracking changes in current context
        /// </summary>
        /// <returns>void</returns>
        public void  DetachEntityEntry<T>(T entry) where T : class
        {
              _dbContext.Entry(entry).State = EntityState.Detached;
        }

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>Returns Int</returns>
        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// This return first element that definately matches with the provided expression
        /// otherwise return null if no element found asynchronously
        /// </summary>
        /// <typeparam name="T">Model class to get the data</typeparam>
        /// <param name="predicate">Expression to retrieve data</param>
        /// <returns>First element that matches all the condition otherwise null</returns>
        public async Task<T> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var dbSet = CreateDbSet<T>();
            return await dbSet.FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// This return first element that definately matches with the provided expression
        /// otherwise return null if no element found
        /// </summary>
        /// <typeparam name="T">Model class to get the data</typeparam>
        /// <param name="predicate">Expression to retrieve data</param>
        /// <returns>First element that matches all the condition otherwise null</returns>
        public T FirstOrDefault<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var dbSet = CreateDbSet<T>();
            return dbSet.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Method to get no of rows satisfing the applied condition
        /// </summary>
        /// <typeparam name="T">Model class to fetch DbSet.</typeparam>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>Filtered elements count</returns>
        public Task<int> CountAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var dbSet = CreateDbSet<T>();
            return dbSet.CountAsync(predicate);
        }

        /// <summary>
        /// Method to get entity using SingleOrDefaultAsync
        /// </summary>
        /// <typeparam name="T">Model class to fetch DbSet.</typeparam>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>Filtered elements</returns>
        public Task<T> SingleOrDefaultAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var dbSet = CreateDbSet<T>();
            return dbSet.SingleOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Method to load virtual data in an entity
        /// </summary>
        /// <typeparam name="T">Model class to get entry</typeparam>
        /// <typeparam name="propertyExpression">Lambda expression to find vitual data</typeparam>
        /// <returns>Task</returns>      
        public async Task LoadReferenceForEntry<T>(T entry, Expression<Func<T, object>> propertyExpression) where T : class
        {
            await _dbContext.Entry(entry).Reference(propertyExpression).LoadAsync();
        }

        /// <summary>
        /// Method to load virtual data in an entity
        /// </summary>
        /// <typeparam name="T">Model class to get entry</typeparam>
        /// <typeparam name="propertyExpression">Lambda expression to find vitual data as IEnumerable</typeparam>
        /// <returns>Task</returns>
        public async Task LoadCollectionForEntry<T>(T entry, Expression<Func<T, IEnumerable<object>>> propertyExpression) where T : class
        {
            await _dbContext.Entry(entry).Collection(propertyExpression).LoadAsync();
        }

        /// <summary>
        /// Asynchronously returns the last element of a sequence that
        /// specify a specific condition
        /// </summary>
        /// <typeparam name="T">Model class to get the data</typeparam>
        /// <param name="predicate">Expression to retrieve data</param>
        /// <returns>Task</returns>
        public async Task<T> LastOrDefaultAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var dbSet = CreateDbSet<T>();
            return await dbSet.LastOrDefaultAsync(predicate);
        }
        /// <summary>
        /// Gets count of db rows
        /// </summary>
        /// <typeparam name="T">Model class to get the data</typeparam>
        /// <returns>Task</returns>
        public async Task<int> CountAsync<T>() where T : class
        {
            var dbSet = CreateDbSet<T>();
            return await dbSet.CountAsync();
        }

        /// <summary>
        /// Method to begin database transaction.
        /// </summary>
        public IDbContextTransaction BeginTransaction()
        {
            return _dbContext.Database.BeginTransaction();
        }
       
        /// <summary>
        /// Add data in bulk using bulk extentions methods asynchronously
        /// </summary>
        /// <typeparam name="T">Model class to create DbSet.</typeparam>
        /// <param name="entities">Entities to add.</param>
        public async Task BulkInsertAsync<T>(List<T> entities) where T : class
        {
            await _dbContext.BulkInsertAsync(entities);
        }
        /// <summary>
        /// Delete bulk data from db ashynchronously
        /// </summary>
        /// <typeparam name="T">Model class</typeparam>
        /// <param name="entities">List of data that are to be deleted in bulk</param>
        /// <returns>Task</returns>
        public async Task BulkDeleteAsync<T>(List<T> entities) where T : class
        {
            await _dbContext.BulkDeleteAsync(entities);
        }

        /// <summary>
        /// Update bulk data in db ashynchronously
        /// </summary>
        /// <typeparam name="T">Model class</typeparam>
        /// <param name="entities">List of data that are to be updated in bulk</param>
        /// <returns>Task</returns>
        public async Task BulkUpdateAsync<T>(List<T> entities) where T : class
        {
            await _dbContext.BulkUpdateAsync(entities);
        }
        /// <summary>
        /// This will update data when any PK matches otherwise will insert data in db asynchronously
        /// </summary>
        /// <typeparam name="T">Model class</typeparam>
        /// <param name="entities">List of data that are to be inserted or updated in bulk</param>
        /// <returns>Task</returns>
        public async Task BulkInsertOrUpdateAsync<T>(List<T> entities) where T : class
        {
            await _dbContext.BulkInsertOrUpdateAsync(entities);
        }

        #endregion


        #region Private Methods
        /// <summary>
        /// Creates DbSet.
        /// </summary>
        /// <typeparam name="T">
        /// Model class for creating set.
        /// </typeparam>
        /// <returns>A DbSet object.</returns>
        private DbSet<T> CreateDbSet<T>() where T : class
        {
            return _dbContext.Set<T>();
        }
        #endregion
    }
}
