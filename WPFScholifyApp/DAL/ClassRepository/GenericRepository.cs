// <copyright file="GenericRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp.DAL.ClassRepository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public class GenericRepository<T> : IGenericRepository<T>
        where T : class
    {
        private SchoolContext? context = null;
        private DbSet<T>? table = null;

        public GenericRepository()
        {
            this.context = new SchoolContext();
            this.table = this.context.Set<T>();
        }

        public GenericRepository(SchoolContext context)
        {
            this.context = context;
            this.table = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return this.table.ToList();
        }

        public T GetById(object id)
        {
            return this.table?.Find(id) ?? throw new InvalidOperationException("Entity not found");
        }

        public void Insert(T obj)
        {
            this.table?.Add(obj);
        }

        public void Update(T obj)
        {
            this.table?.Attach(obj);
            if (this.context != null)
            {
                this.context.Entry(obj).State = EntityState.Modified;
            }
        }

        public void Delete(object id)
        {
            T? existing = this.table?.Find(id);
            this.table?.Remove(existing);
        }

        public void Save()
        {
            this.context?.SaveChanges();
        }
    }
}
