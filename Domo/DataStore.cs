﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domo
{
    public class DataStore : IDataStore
    {
        private IList<IRepository> _repositories = new List<IRepository>();

        public void Dispose()
        {
            RepositoryChanged = null;
            this.DeleteAllRepositories();
        }

        public void AddRepository(IRepository repository)
        {
            _repositories.Add(repository);
            RepositoryChanged?.Invoke(this, new RepositoryChangeArgs 
                { ChangeType = RepositoryChangedEvent.RepositoryAdded, Repository = repository });
            repository.RepositoryChanged += (sender, args) => RepositoryChanged?.Invoke(sender, args);
        }

        public IReadOnlyList<IRepository> GetRepositories()
            => _repositories.ToList();

        public void DeleteRepository(IRepository repository)
        {
            _repositories.Remove(repository);
            repository.Dispose();
            RepositoryChanged?.Invoke(this, new RepositoryChangeArgs 
                { ChangeType = RepositoryChangedEvent.RepositoryDeleted, Repository = repository});
        }

        public IRepository GetRepository(Type type)
            => _repositories.FirstOrDefault(r => r.ValueType == type);

        public event EventHandler<RepositoryChangeArgs> RepositoryChanged;
    }
}
