
# Repogitory interfaces

- [Basic repositories](#basic-repositories)
  - [IReadDataRepository{TEntity, TKey} Interface](#ireaddatarepositorytentity-tkey-interface)
  - [IReadDataRepositoryWithUniqueKey{TEntity, TPrimaryKey, TUniqueKey} Interface](#ireaddatarepositorywithuniquekeytentity-tprimarykey-tuniquekey-interface)
  - [IWriteDataRepository{TEntity} Interface](#iwritedatarepositorytentity-interface)
  - [IDataQuery{TEntity, TCondition} Interface](#idataquerytentity-tcondition-interface)
- [Repositories that use the context](#repositories-that-use-the-context)
  - [IReadDataRepositoryWithContext{TEntity, TKey, TContext} Interface](#ireaddatarepositorywithcontexttentity-tkey-tcontext-interface)
  - [IReadDataRepositoryWithUniqueKeyWithContext{TEntity, TPrimaryKey, TUniqueKey, TContext} Interface](#ireaddatarepositorywithuniquekeywithcontexttentity-tprimarykey-tuniquekey-tcontext-interface)
  - [IWriteDataRepositoryWithContext{TEntity, TContext} Interface](#iwritedatarepositorywithcontexttentity-tcontext-interface)
  - [IDataQueryWithContext{TEntity, TCondition, TContext} Interface](#idataquerywithcontexttentity-tcondition-tcontext-interface)
- [Asynchronous Repositories](#asynchronous-repositories)
  - [IAsyncReadDataRepository{TEntity, TKey} Interface](#iasyncreaddatarepositorytentity-tkey-interface)
  - [IAsyncReadDataRepositoryWithUniqueKey{TEntity, TPrimaryKey, TUniqueKey} Interface](#iasyncreaddatarepositorywithuniquekeytentity-tprimarykey-tuniquekey-interface)
  - [IAsyncWriteDataRepository{TEntity} Interface](#iasyncwritedatarepositorytentity-interface)
  - [IAsyncDataQuery{TEntity, TCondition} Interface](#iasyncdataquerytentity-tcondition-interface)
- [Asynchronous Repositories that use the context](#asynchronous-repositories-that-use-the-context)
  - [IAsyncReadDataRepositoryWithContext{TEntity, TKey, TContext} Interface](#iasyncreaddatarepositorywithcontexttentity-tkey-tcontext-interface)
  - [IAsyncReadDataRepositoryWithUniqueKeyWithContext{TEntity, TPrimaryKey, TUniqueKey, TContext} Interface](#iasyncreaddatarepositorywithuniquekeywithcontexttentity-tprimarykey-tuniquekey-tcontext-interface)
  - [IAsyncWriteDataRepositoryWithContext{TEntity, TContext} Interface](#iasyncwritedatarepositorywithcontexttentity-tcontext-interface)
  - [IAsyncDataQueryWithContext{TEntity, TCondition, TContext} Interface](#iasyncdataquerywithcontexttentity-tcondition-tcontext-interface)

## Basic repositories

### IReadDataRepository{TEntity, TKey} Interface

Repository to retrieve the entity corresponding to the specified key.



## Basic repositories

### IReadDataRepository{TEntity, TKey} Interface

Repository to retrieve the entity corresponding to the specified key.

```c#
bool UseTransactionScope { get; }
TEntity? Get(TKey key);
IEnumerable<TEntity> GetRange(IEnumerable<TKey> keys);
IEnumerable<TEntity> GetAll();
IEnumerable<TKey> GetAllKeys();
```

```c#
internal class SampleEntity
{
    // key
    internal int ID { get; set; }
    internal string? Name { get; set; }
}

internal class SampleReadRepository : IReadDataRepository<SampleEntity, int>
{
    // omission
}

// create a repository.
var repo = new SampleReadRepository();

// get the entity corresponding to the specified id.
var entity = repo.Get(1);

// get the entities corresponding to the specified ids.
var entities = repo.GetRange(new[] { 1, 2, 3 });
```

### IReadDataRepositoryWithUniqueKey{TEntity, TPrimaryKey, TUniqueKey} Interface

Repository to retrieve entities corresponding to the specified primary key or unique key.

```c#
bool UseTransactionScope { get; }
TEntity? GetByPrimaryKey(TPrimaryKey primaryKey);
TEntity? GetByUniqueKey(TUniqueKey uniqueKey);
IEnumerable<TEntity> GetRangeByPrimaryKey(IEnumerable<TPrimaryKey> primaryKeys);
IEnumerable<TEntity> GetRangeByUniqueKey(IEnumerable<TUniqueKey> uniqueKeys);
IEnumerable<TEntity> GetAll();
IEnumerable<TPrimaryKey> GetAllPrimaryKeys();
IEnumerable<TUniqueKey> GetAllUniqueKeys();
```

```c#
internal class SampleEntity
{
    // primary key
    internal int ID { get; set; }
    // unique key
    internal string Code { get; set; }
    internal string? Name { get; set; }
}

internal class SampleReadRepository : IReadDataRepositoryWithUniqueKey<SampleEntity, int, string>
{
    // omission
}

// create a repository.
var repo = new SampleReadRepository();

// get the entity corresponding to the specified id.
var entity = repo.GetByPrimaryKey(1);

// get the entities corresponding to the specified codes.
var entities = repo.GetRangeByUniqueKey(new[] { "001", "002", "003" });
```

### IWriteDataRepository{TEntity} Interface

Repository for updating the specified entity.

```c#
bool UseTransactionScope { get; }
int Insert(TEntity entity);
int InsertRange(IEnumerable<TEntity> entities);
int Update(TEntity entity);
int UpdateRange(IEnumerable<TEntity> entities);
int Delete(TEntity entity);
int DeleteRange(IEnumerable<TEntity> entities);
```

```c#
internal class SampleEntity
{
    internal int ID { get; set; }
    internal string? Name { get; set; }
}

internal class SampleReadRepository : IWriteDataRepository<SampleEntity>
{
    public bool UseTransactionScope => true;
    
    // omission
}

using (TransactionScope scope = new())
{
    // create a repository.
    var repo = new SampleReadRepository();

    // insert the specified entity.
    repo.Insert(new SampleEntity() { ID = 1, Name = "item1" });

    // insert the specified entities.
    repo.InsertRange(new[] {
        new SampleEntity() { ID = 2, Name = "item2" },
        new SampleEntity() { ID = 3, Name = "item3" }
    });

    scope.Complete();
}
```

If you do not use TransactionScope, you will need to pass a transaction object. Using the context described later is one such method.


### IDataQuery{TEntity, TCondition} Interface

Repository for retrieving entities that match the specified condition.

```c#
IEnumerable<TEntity> Query(TCondition condition, int skipCount = 0, int? maximumCount = null);
int GetCount(TCondition condition);
```

```c#
internal class SampleEntity
{
    internal int ID { get; set; }
    internal string Code { get; set; }
    internal string? Name { get; set; }
}

internal class SampleEntityCondition
{
    internal string? MinimumCode { get; set; }
    internal string? MaximumCode { get; set; }
    internal string? NamePattern { get; set; }
}

internal class SampleEntityQuery : IDataQuery<SampleEntity, SampleEntityCondition>
{
    // omission
}

// creates a repository.
var repo = new SampleEntityQuery();

// creates a condition.
var condition = new SampleEntityCondition()
{
    MinimumCode = "001",
    MaximumCode = "100"
};

// find entities that match the specified condition.
var entities = repo.Query(condition);

// get the first 10 entities.
var top10 = repo.Query(condition, maximumCount: 10);

// skip the first 10 entities and get the next 10 entities.
var next10 = repo.Query(condition, skipCount: 10, maximumCount: 10);
```

## Repositories that use the context

The difference from the basic repositories is that a context is added to the arguments of each method, making it suitable for a design that passes objects required for retrieving and updating entities, such as a database connection and transaction.

### IReadDataRepositoryWithContext{TEntity, TKey, TContext} Interface

Repository to retrieve the entity corresponding to the specified key.

```c#
bool UseTransactionScope { get; }
TEntity? GetByPrimaryKey(TPrimaryKey primaryKey, TContext context);
TEntity? GetByUniqueKey(TUniqueKey uniqueKey, TContext context);
IEnumerable<TEntity> GetRangeByPrimaryKey(IEnumerable<TPrimaryKey> primaryKeys, TContext context);
IEnumerable<TEntity> GetRangeByUniqueKey(IEnumerable<TUniqueKey> uniqueKeys, TContext context);
IEnumerable<TEntity> GetAll(TContext context);
IEnumerable<TPrimaryKey> GetAllPrimaryKeys(TContext context);
IEnumerable<TUniqueKey> GetAllUniqueKeys(TContext context);
```

### IReadDataRepositoryWithUniqueKeyWithContext{TEntity, TPrimaryKey, TUniqueKey, TContext} Interface

Repository to retrieve entities corresponding to the specified primary key or unique key.

```c#
bool UseTransactionScope { get; }
TEntity? GetByPrimaryKey(TPrimaryKey primaryKey, TContext context);
TEntity? GetByUniqueKey(TUniqueKey uniqueKey, TContext context);
IEnumerable<TEntity> GetRangeByPrimaryKey(IEnumerable<TPrimaryKey> primaryKeys, TContext context);
IEnumerable<TEntity> GetRangeByUniqueKey(IEnumerable<TUniqueKey> uniqueKeys, TContext context);
IEnumerable<TEntity> GetAll(TContext context);
IEnumerable<TPrimaryKey> GetAllPrimaryKeys(TContext context);
IEnumerable<TUniqueKey> GetAllUniqueKeys(TContext context);
```

### IWriteDataRepositoryWithContext{TEntity, TContext} Interface

Repository for updating the specified entity.

```c#
bool UseTransactionScope { get; }
int Insert(TEntity entity, TContext context);
int InsertRange(IEnumerable<TEntity> entities, TContext context);
int Update(TEntity entity, TContext context);
int UpdateRange(IEnumerable<TEntity> entities, TContext context);
int Delete(TEntity entity, TContext context);
int DeleteRange(IEnumerable<TEntity> entities, TContext context);
```

### IDataQueryWithContext{TEntity, TCondition, TContext} Interface

Repository for retrieving entities that match the specified condition.  

```c#
bool UseTransactionScope { get; }
IEnumerable<TEntity> Query(TCondition condition, TContext context, int skipCount = 0, int? maximumCount = null);
int GetCount(TCondition condition, TContext context);
```

## Asynchronous Repositories

The difference with the base repositories is that each method is an asynchronous method.

### IAsyncReadDataRepository{TEntity, TKey} Interface

Repository to retrieve the entity corresponding to the specified key.

```c#
bool UseTransactionScope { get; }
ValueTask<TEntity?> GetAsync(TKey key);
IAsyncEnumerable<TEntity> GetRangeAsync(IEnumerable<TKey> keys, CancellationToken cancellationToken = default);
IAsyncEnumerable<TEntity> GetRangeAsync(IAsyncEnumerable<TKey> keys, CancellationToken cancellationToken = default);
IAsyncEnumerable<TEntity> GetAllAsync(CancellationToken cancellationToken = default);
IAsyncEnumerable<TKey> GetAllKeysAsync(CancellationToken cancellationToken = default);
```

### IAsyncReadDataRepositoryWithUniqueKey{TEntity, TPrimaryKey, TUniqueKey} Interface

Repository to retrieve entities corresponding to the specified primary key or unique key.

```c#
bool UseTransactionScope { get; }
ValueTask<TEntity?> GetByPrimaryKeyAsync(TPrimaryKey primaryKey);
ValueTask<TEntity?> GetByUniqueKeyAsync(TUniqueKey uniqueKey);
IAsyncEnumerable<TEntity> GetRangeByPrimaryKeyAsync(IEnumerable<TPrimaryKey> primaryKeys, CancellationToken cancellationToken = default);
IAsyncEnumerable<TEntity> GetRangeByPrimaryKeyAsync(IAsyncEnumerable<TPrimaryKey> primaryKeys, CancellationToken cancellationToken = default);
IAsyncEnumerable<TEntity> GetRangeByUniqueKeyAsync(IEnumerable<TUniqueKey> uniqueKeys, CancellationToken cancellationToken = default);
IAsyncEnumerable<TEntity> GetRangeByUniqueKeyAsync(IAsyncEnumerable<TUniqueKey> uniqueKeys, CancellationToken cancellationToken = default);
IAsyncEnumerable<TEntity> GetAllAsync(CancellationToken cancellationToken = default);
IAsyncEnumerable<TPrimaryKey> GetAllPrimaryKeysAsync(CancellationToken cancellationToken = default);
IAsyncEnumerable<TUniqueKey> GetAllUniqueKeysAsync(CancellationToken cancellationToken = default);
```

### IAsyncWriteDataRepository{TEntity} Interface

Repository for updating the specified entity.

```c#
bool UseTransactionScope { get; }
ValueTask<int> InsertAsync(TEntity entity);
ValueTask<int> InsertRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
ValueTask<int> InsertRangeAsync(IAsyncEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
ValueTask<int> UpdateAsync(TEntity entity);
ValueTask<int> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
ValueTask<int> UpdateRangeAsync(IAsyncEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
ValueTask<int> DeleteAsync(TEntity entity);
ValueTask<int> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
ValueTask<int> DeleteRangeAsync(IAsyncEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
```

### IAsyncDataQuery{TEntity, TCondition} Interface

Repository for retrieving entities that match the specified condition.  

```c#
bool UseTransactionScope { get; }
IAsyncEnumerable<TEntity> QueryAsync(TCondition condition, int skipCount = 0, int? maximumCount = null, CancellationToken cancellationToken = default);
ValueTask<int> GetCountAsync(TCondition condition);
```

## Asynchronous Repositories that use the context

The difference with the basic repositories is that each method is asynchronous and takes additional context as an argument, making it suitable for designs that pass in objects required to retrieve and update entities, such as a database connection and transaction.

### IAsyncReadDataRepositoryWithContext{TEntity, TKey, TContext} Interface

Repository to retrieve the entity corresponding to the specified key.

```c#
bool UseTransactionScope { get; }
ValueTask<TEntity?> GetAsync(TKey key, TContext context);
IAsyncEnumerable<TEntity> GetRangeAsync(IEnumerable<TKey> keys, TContext context, CancellationToken cancellationToken = default);
IAsyncEnumerable<TEntity> GetRangeAsync(IAsyncEnumerable<TKey> keys, TContext context, CancellationToken cancellationToken = default);
IAsyncEnumerable<TEntity> GetAllAsync(TContext context, CancellationToken cancellationToken = default);
IAsyncEnumerable<TKey> GetAllKeysAsync(TContext context, CancellationToken cancellationToken = default);
```

### IAsyncReadDataRepositoryWithUniqueKeyWithContext{TEntity, TPrimaryKey, TUniqueKey, TContext} Interface

Repository to retrieve entities corresponding to the specified primary key or unique key.

```c#
bool UseTransactionScope { get; }
ValueTask<TEntity?> GetByPrimaryKeyAsync(TPrimaryKey primaryKey, TContext context);
ValueTask<TEntity?> GetByUniqueKeyAsync(TUniqueKey uniqueKey, TContext context);
IAsyncEnumerable<TEntity> GetRangeByPrimaryKeyAsync(IEnumerable<TPrimaryKey> primaryKeys, TContext context, CancellationToken cancellationToken = default);
IAsyncEnumerable<TEntity> GetRangeByPrimaryKeyAsync(IAsyncEnumerable<TPrimaryKey> primaryKeys, TContext context, CancellationToken cancellationToken = default);
IAsyncEnumerable<TEntity> GetRangeByUniqueKeyAsync(IEnumerable<TUniqueKey> uniqueKeys, TContext context, CancellationToken cancellationToken = default);
IAsyncEnumerable<TEntity> GetRangeByUniqueKeyAsync(IAsyncEnumerable<TUniqueKey> uniqueKeys, TContext context, CancellationToken cancellationToken = default);
IAsyncEnumerable<TEntity> GetAllAsync(TContext context, CancellationToken cancellationToken = default);
IAsyncEnumerable<TPrimaryKey> GetAllPrimaryKeysAsync(TContext context, CancellationToken cancellationToken = default);
IAsyncEnumerable<TUniqueKey> GetAllUniqueKeysAsync(TContext context, CancellationToken cancellationToken = default);
```

### IAsyncWriteDataRepositoryWithContext{TEntity, TContext} Interface

Repository for updating the specified entity.

```c#
bool UseTransactionScope { get; }
ValueTask<int> InsertAsync(TEntity entity, TContext context);
ValueTask<int> InsertRangeAsync(IEnumerable<TEntity> entities, TContext context, CancellationToken cancellationToken = default);
ValueTask<int> InsertRangeAsync(IAsyncEnumerable<TEntity> entities, TContext context, CancellationToken cancellationToken = default);
ValueTask<int> UpdateAsync(TEntity entity, TContext context);
ValueTask<int> UpdateRangeAsync(IEnumerable<TEntity> entities, TContext context, CancellationToken cancellationToken = default);
ValueTask<int> UpdateRangeAsync(IAsyncEnumerable<TEntity> entities, TContext context, CancellationToken cancellationToken = default);
ValueTask<int> DeleteAsync(TEntity entity, TContext context);
ValueTask<int> DeleteRangeAsync(IEnumerable<TEntity> entities, TContext context, CancellationToken cancellationToken = default);
ValueTask<int> DeleteRangeAsync(IAsyncEnumerable<TEntity> entities, TContext context, CancellationToken cancellationToken = default);
```

### IAsyncDataQueryWithContext{TEntity, TCondition, TContext} Interface

Repository for retrieving entities that match the specified condition.  

```c#
bool UseTransactionScope { get; }
IAsyncEnumerable<TEntity> QueryAsync(TCondition condition, TContext context, int skipCount = 0, int? maximumCount = null, CancellationToken cancellationToken = default);
ValueTask<int> GetCountAsync(TCondition condition, TContext context);
```
