// <copyright file="IGenericRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WPFScholifyApp.DAL.ClassRepository
{
    using System.Collections.Generic;

    public interface IGenericRepository<T>
        where T : class
    {
        IEnumerable<T> GetAll();

        T GetById(object id);

        void Insert(T obj);

        void Update(T obj);

        void Delete(object id);

        void Save();
    }
}
