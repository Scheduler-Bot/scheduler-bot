﻿using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SchedulerBot.Database.Core;
using SchedulerBot.Database.Interfaces;

namespace SchedulerBot.Database.Repositories
{
	/// <inheritdoc />
	public abstract class BaseRepository<T> : IRepository<T> where T : class
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BaseRepository{T}"/> class.
		/// </summary>
		/// <param name="dbContext">The database context.</param>
		/// <exception cref="ArgumentNullException">dbContext</exception>
		protected BaseRepository(SchedulerBotContext dbContext)
		{
			DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			DbSet = DbContext.Set<T>();
		}

		/// <summary>
		/// Gets the database context.
		/// </summary>
		// ReSharper disable once MemberCanBePrivate.Global
		protected SchedulerBotContext DbContext { get; }

		/// <summary>
		/// Gets the database set.
		/// </summary>
		protected DbSet<T> DbSet { get; }

		/// <inheritdoc />
		public virtual async Task<T> AddAsync(T entity)
		{
			EntityEntry<T> entityEntry = await DbContext.AddAsync(entity);

			return entityEntry.Entity;
		}

		/// <inheritdoc />
		public virtual T Update(T entity)
		{
			EntityEntry<T> entityEntry = DbContext.Update(entity);

			return entityEntry.Entity;
		}

		/// <inheritdoc />
		public virtual T Delete(T entity)
		{
			EntityEntry<T> entityEntry = DbContext.Remove(entity);

			return entityEntry.Entity;
		}
	}
}
