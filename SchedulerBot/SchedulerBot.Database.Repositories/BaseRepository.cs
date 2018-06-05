using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SchedulerBot.Database.Interfaces;

namespace SchedulerBot.Database.Repositories
{
	public class BaseRepository<T> : IRepository<T> where T : class
	{
		protected DbContext DbContext { get; set; }

		protected DbSet<T> DbSet { get; set; }

		public BaseRepository(DbContext dbContext)
		{
			DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			DbSet = DbContext.Set<T>();
		}

		/// <inheritdoc />
		public virtual IQueryable<T> GetAll()
		{
			return DbSet;
		}

		/// <inheritdoc />
		public virtual T GetById(long id)
		{
			return DbSet.Find(id);
		}

		/// <inheritdoc />
		public virtual async Task<T> AddAsync(T entity)
		{
			EntityEntry<T> entityEntry = await DbContext.AddAsync(entity);
			return entityEntry.Entity;
		}

		/// <inheritdoc />
		public virtual void Update(T entity)
		{
			DbContext.Update(entity);
		}

		/// <inheritdoc />
		public virtual bool Delete(long id)
		{
			T entity = GetById(id);
			if (entity != null)
			{
				Delete(entity);
				return true;
			}
			return false;
		}

		/// <inheritdoc />
		public void Delete(T entity)
		{
			EntityEntry dbEntityEntry = DbContext.Entry(entity);
			dbEntityEntry.State = EntityState.Deleted;
		}

		protected void RollBackChangesInContext()
		{
			List<EntityEntry> changedEntries = DbContext.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();
			foreach (EntityEntry entry in changedEntries.Where(x => x.State == EntityState.Modified))
			{
				entry.CurrentValues.SetValues(entry.OriginalValues);
				entry.State = EntityState.Unchanged;
			}

			foreach (EntityEntry entry in changedEntries.Where(x => x.State == EntityState.Added))
			{
				entry.State = EntityState.Detached;
			}

			foreach (EntityEntry entry in changedEntries.Where(x => x.State == EntityState.Deleted))
			{
				entry.State = EntityState.Unchanged;
			}
		}
	}
}
