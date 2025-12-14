# Паттерн Мост (Bridge Pattern)

## Описание проекта

Проект демонстрирует реализацию структурного паттерна проектирования "Мост" (Bridge) на языке C#. 
Паттерн Мост разделяет абстракцию и реализацию так, чтобы они могли изменяться независимо друг от друга.

## Структура проекта

### Основные компоненты

#### 1. **Хранилища данных (Storage Layer)**

##### MongoUserStorage
Реализация хранилища пользователей с использованием MongoDB:
- Сохранение пользователей (`SaveUser`) - INSERT/REPLACE операции
- Получение пользователя по ID (`GetUser`) - поиск по фильтру
- Поиск пользователей (`FindUsers`) - фильтрация по условию
- Удаление пользователей (`DeleteUser`) - DELETE операция
- Сохранение результатов подсчета пробелов
- Получение содержимого файлов из БД
- Получение всех результатов подсчета

**Особенности:**
- Использует MongoDB.Driver 3.5.0
- Реализует интерфейс `IUserStorage`
- Поддерживает атрибуты MongoDB для сериализации
- Работает с двумя коллекциями: `Users` и `CreateUsers`
- Асинхронные операции для производительности

##### PostgresUserStorage
Полноценная реализация хранилища для PostgreSQL:
- **Автоматическая инициализация таблиц** при создании экземпляра
- Сохранение пользователей (INSERT/UPDATE) в таблицу `users`
- Получение пользователя по ID через SQL запросы
- Поиск пользователей с фильтрацией
- Удаление пользователей через DELETE
- Сохранение результатов подсчета в таблицу `space_count_results`
- Пакетное сохранение с транзакциями
- Получение всех результатов с сортировкой

**Структура таблиц PostgreSQL:**

```sql
-- Таблица пользователей
CREATE TABLE users (
    id UUID PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    updata VARCHAR(255),
    delete_users VARCHAR(255),
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Таблица результатов подсчета пробелов
CREATE TABLE space_count_results (
    id UUID PRIMARY KEY,
    file_path VARCHAR(500) NOT NULL,
    space_count INTEGER NOT NULL,
    processing_time_ms BIGINT NOT NULL,
    created_at TIMESTAMP NOT NULL
);
```

#### 2. **Модели данных (Models)**

##### User
```csharp
public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    
    [BsonElement("name")]
    public string Name { get; set; }
    
    [BsonElement("updata")]
    public string Updata { get; set; }
    
    [BsonElement("deleteUsers")]
    public string DeleteUsers { get; set; }
    
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }
}
```

##### MongoSpaceCountResult
Модель для хранения результатов подсчета пробелов в файлах:
- `Id` - уникальный идентификатор (UUID)
- `FilePath` - путь к файлу
- `SpaceCount` - количество пробелов
- `ProcessingTimeMs` - время обработки в миллисекундах
- `CreatedAt` - дата создания записи (UTC)

#### 3. **Интерфейсы (Interfaces)**

##### IUserStorage
Главный интерфейс для работы с пользователями:
```csharp
public interface IUserStorage
{
    User GetUser(Guid userId);
    void SaveUser(User user);
    void DeleteUser(string userId);
    List<User> FindUsers(Func<User, bool> predicate);
    Task SaveSpaceCountResultAsync(MongoSpaceCountResult result);
    Task SaveSpaceCountResultsAsync(MongoSpaceCountResult[] results);
    Task<string?> GetFileContentAsync(string filePath);
    Task<List<MongoSpaceCountResult>> GetAllSpaceCountResultsAsync();
}
```

##### IMongoDBService
Интерфейс для работы с MongoDB:
```csharp
public interface IMongoDBService
{
    Task SaveSpaceCountResultAsync(MongoSpaceCountResult result);
}
```

##### IPostgreSQLService
Интерфейс для работы с PostgreSQL:
```csharp
public interface IPostgreSQLService
{
    Task SaveSpaceCountResultAsync(MongoSpaceCountResult result);
    Task SaveSpaceCountResultsAsync(MongoSpaceCountResult[] results);
    Task<string?> GetFileContentAsync(string filePath);
    Task<List<MongoSpaceCountResult>> GetAllSpaceCountResultsAsync();
}
```

##### ISpaceCounterService
Интерфейс для сервисов подсчета пробелов:
```csharp
public interface ISpaceCounterService
{
    Task CountSpacesInRemoteFolder(string remoteDirectory);
}
```

#### 4. **Сервисы (Services)**

##### MongoDBService
Реализация сервиса для работы с MongoDB:
- Подключение к базе данных MongoDB
- Сохранение результатов подсчета пробелов
- Использует `InsertOneAsync` для асинхронного сохранения

##### PostgreSQLService
Реализация сервиса для работы с PostgreSQL:
- Подключение к базе данных PostgreSQL
- Сохранение результатов с использованием Npgsql
- Поддержка транзакций для пакетных операций
- Обработка rollback при ошибках

##### MongoSpaceCounterService
**Независимый** сервис для подсчета пробелов с сохранением в MongoDB:
- Реализует `ISpaceCounterService`
- Использует `IMongoDBService` для сохранения
- Полностью независим от PostgreSQL
- Подсчет пробелов в удаленных директориях

##### PostgresSpaceCounterService
**Независимый** сервис для подсчета пробелов с сохранением в PostgreSQL:
- Реализует `ISpaceCounterService`
- Использует `IPostgreSQLService` для сохранения
- Полностью независим от MongoDB
- Подсчет пробелов в удаленных директориях

## Применение паттерна Мост

Паттерн Мост используется для разделения абстракции (работа с пользователями и результатами) и реализации (конкретные хранилища данных):

```
Абстракция (IUserStorage / ISpaceCounterService)
    ?
Реализация 1 (MongoUserStorage / MongoSpaceCounterService) ? MongoDB (NoSQL)
Реализация 2 (PostgresUserStorage / PostgresSpaceCounterService) ? PostgreSQL (SQL)
```

### Преимущества:

1. **Независимость изменений**: Можно изменять реализацию хранилища, не затрагивая клиентский код
2. **Расширяемость**: Легко добавить новые хранилища (Redis, SQL Server, MySQL и т.д.)
3. **Переключение на лету**: Возможность менять хранилище в runtime
4. **Тестируемость**: Легко создавать mock-объекты для тестирования
5. **Единый интерфейс**: Одинаковая работа с разными типами БД (SQL и NoSQL)
6. **Полная независимость**: MongoDB и PostgreSQL работают полностью независимо друг от друга

## Пример использования

### Работа с MongoDB

```csharp
// Создание подключения к MongoDB
string connectionString = "mongodb://admin:password@host:port/?authSource=admin";
IUserStorage mongoStorage = new MongoUserStorage(connectionString, "database", "collection");

// Создание пользователя
var user = new User
{
    Id = Guid.NewGuid(),
    Name = "Иван Иванов",
    Updata = "Создан",
    DeleteUsers = "Нет"
};

// Сохранение в MongoDB
mongoStorage.SaveUser(user);

// Получение пользователя
var foundUser = mongoStorage.GetUser(user.Id);

// Обновление пользователя
user.Name = "Иван Иванович Иванов";
mongoStorage.SaveUser(user);

// Удаление пользователя
mongoStorage.DeleteUser(user.Id.ToString());
```

### Работа с PostgreSQL

```csharp
// Создание подключения к PostgreSQL
string postgresConnectionString = "Host=localhost;Port=5432;Database=mydb;Username=user;Password=pass";
IUserStorage postgresStorage = new PostgresUserStorage(postgresConnectionString);

// Таблицы создаются автоматически при инициализации

// Создание пользователя
var user = new User
{
    Id = Guid.NewGuid(),
    Name = "Петр Петров",
    Updata = "Создан",
    DeleteUsers = "Нет"
};

// Сохранение в PostgreSQL
postgresStorage.SaveUser(user);

// Получение пользователя
var foundUser = postgresStorage.GetUser(user.Id);

// Поиск пользователей
var users = postgresStorage.FindUsers(u => u.Name.Contains("Петр"));
```

### Независимое использование сервисов подсчета

```csharp
// Создание независимых сервисов
IMongoDBService mongoDBService = new MongoDBService(mongoConnectionString, "database", "collection");
IPostgreSQLService postgreSQLService = new PostgreSQLService(postgresConnectionString);

// Сервис для MongoDB - работает независимо
ISpaceCounterService mongoCounter = new MongoSpaceCounterService(mongoDBService);
await mongoCounter.CountSpacesInRemoteFolder("path/to/mongo/files");

// Сервис для PostgreSQL - работает независимо
ISpaceCounterService postgresCounter = new PostgresSpaceCounterService(postgreSQLService);
await postgresCounter.CountSpacesInRemoteFolder("path/to/postgres/files");

// Оба сервиса работают полностью независимо друг от друга
```

### Демонстрация паттерна Мост

```csharp
// Создание пользователя
var testUser = new User
{
    Id = Guid.NewGuid(),
    Name = "Тестовый Пользователь"
};

// Переключение между хранилищами через один интерфейс
IUserStorage storage;

// Сохранение в MongoDB
storage = mongoStorage;
storage.SaveUser(testUser);

// Сохранение в PostgreSQL
storage = postgresStorage;
storage.SaveUser(testUser);

// Переключение между сервисами подсчета
ISpaceCounterService counter;

// Работа с MongoDB
counter = mongoCounter;
await counter.CountSpacesInRemoteFolder("files/mongo");

// Работа с PostgreSQL
counter = postgresCounter;
await counter.CountSpacesInRemoteFolder("files/postgres");
```

## Технологии

- **.NET 8.0**
- **MongoDB.Driver 3.5.0** - драйвер для работы с MongoDB
- **Npgsql 9.0.4** - драйвер для работы с PostgreSQL

## Конфигурация баз данных

### MongoDB
- База данных: `space_counter`
- Коллекция пользователей: `Users`
- Коллекция результатов: `CreateUsers`
- Строка подключения: `mongodb://admin:password@host:port/?authSource=admin`

### PostgreSQL
- База данных: `postgres`
- Таблицы: `users`, `space_count_results`
- Строка подключения: `Host=host;Port=port;Database=postgres;Username=user;Password=pass`
- **Автоматическое создание таблиц** при первом запуске

## Особенности реализации

### Независимость сервисов
**Ключевая особенность архитектуры**: 
- `MongoSpaceCounterService` работает **только** с MongoDB через `IMongoDBService`
- `PostgresSpaceCounterService` работает **только** с PostgreSQL через `IPostgreSQLService`
- Сервисы **полностью независимы** друг от друга
- Нет перекрестных зависимостей между MongoDB и PostgreSQL

### Без блоков try-catch
Код намеренно упрощен и не содержит избыточных блоков обработки исключений для демонстрации чистоты паттерна.

### Атрибуты MongoDB
Модель `User` использует атрибуты MongoDB для корректной сериализации:
- `[BsonId]` - помечает поле как идентификатор документа
- `[BsonRepresentation]` - указывает способ представления в BSON
- `[BsonElement]` - задает имя поля в документе MongoDB
- `[BsonDateTimeOptions]` - настраивает работу с датами

### Инициализация PostgreSQL
`PostgresUserStorage` автоматически создает необходимые таблицы при первом запуске:
- Использует `CREATE TABLE IF NOT EXISTS`
- Выполняется синхронно в конструкторе
- Не требует дополнительной настройки

### Асинхронные операции
Все операции с базами данных выполняются асинхронно для повышения производительности:
- MongoDB: `InsertOneAsync`, `FindAsync`, `ReplaceOneAsync`, `DeleteOneAsync`
- PostgreSQL: `OpenAsync`, `ExecuteNonQueryAsync`, `ExecuteReaderAsync`

### Транзакции в PostgreSQL
Пакетные операции используют транзакции для обеспечения целостности данных:
```csharp
using var transaction = await connection.BeginTransactionAsync();
// ... операции ...
await transaction.CommitAsync();
```

## Запуск проекта

1. Убедитесь, что MongoDB и PostgreSQL запущены и доступны
2. Обновите строки подключения в `Program.cs`
3. Запустите проект

```bash
dotnet run
```

### Вывод программы

```
=== Работа с пользователями в MongoDB ===
Пользователь Иван Иванов (MongoDB) (Id: ...) сохранен в MongoDB
Пользователь Петр Петров (MongoDB) (Id: ...) сохранен в MongoDB
Найден пользователь в MongoDB: Иван Иванов (MongoDB)
Пользователь Иван Иванович Иванов (MongoDB) (Id: ...) обновлен в MongoDB
Пользователь ... удален из MongoDB

Таблицы PostgreSQL инициализированы
=== Работа с пользователями в PostgreSQL ===
Пользователь Алексей Сидоров (PostgreSQL) (Id: ...) сохранен в PostgreSQL
Пользователь Мария Петрова (PostgreSQL) (Id: ...) сохранена в PostgreSQL
Найден пользователь в PostgreSQL: Алексей Сидоров (PostgreSQL)
Пользователь Алексей Александрович Сидоров (PostgreSQL) (Id: ...) обновлен в PostgreSQL
Пользователь ... удален из PostgreSQL

=== Демонстрация паттерна Мост ===
Сохранение одного пользователя в обе базы данных:
Пользователь Тестовый Пользователь (Id: ...) сохранен в MongoDB
Пользователь Тестовый Пользователь (Id: ...) сохранен в PostgreSQL

=== Независимое сохранение результатов подсчета ===
Подсчет пробелов и сохранение в MongoDB:
Результат успешно сохранен в MongoDB: TextFile/MongoDB

Подсчет пробелов и сохранение в PostgreSQL:
Результат успешно сохранен в PostgreSQL: TextFile/PostgreSQL

Сохранение одного результата в обе базы данных:
Результат подсчета пробелов для файла 'shared/example.txt' сохранен в MongoDB
Результат подсчета пробелов для файла 'shared/example.txt' сохранен в PostgreSQL

=== Получение всех результатов из обеих БД ===
Получено X результатов подсчета пробелов из MongoDB
Результатов в MongoDB: X
Получено Y результатов подсчета пробелов из PostgreSQL
Результатов в PostgreSQL: Y
```

## Расширение проекта

Для добавления нового хранилища:

1. Создайте класс, реализующий `IUserStorage`
2. Создайте интерфейс сервиса (например, `IRedisService`)
3. Создайте реализацию сервиса
4. Создайте сервис подсчета, реализующий `ISpaceCounterService`
5. Используйте новое хранилище через интерфейсы

Пример для Redis:
```csharp
// Интерфейс
public interface IRedisService
{
    Task SaveSpaceCountResultAsync(MongoSpaceCountResult result);
}

// Хранилище
public class RedisUserStorage : IUserStorage
{
    private readonly IDatabase _redis;
    
    public RedisUserStorage(string connectionString)
    {
        var connection = ConnectionMultiplexer.Connect(connectionString);
        _redis = connection.GetDatabase();
    }
    
    public void SaveUser(User user)
    {
        var json = JsonSerializer.Serialize(user);
        _redis.StringSet($"user:{user.Id}", json);
    }
    
    // ... остальные методы
}

// Сервис подсчета
public class RedisSpaceCounterService : ISpaceCounterService
{
    private readonly IRedisService _redisService;
    
    public RedisSpaceCounterService(IRedisService redisService)
    {
        _redisService = redisService;
    }
    
    public async Task CountSpacesInRemoteFolder(string remoteDirectory)
    {
        var result = new MongoSpaceCountResult { /* ... */ };
        await _redisService.SaveSpaceCountResultAsync(result);
    }
}
```

## Архитектура независимости

```
Program.cs
    ?
?????????????????????????????????????????????????????
?   MongoDB Branch        ?   PostgreSQL Branch     ?
?????????????????????????????????????????????????????
? IMongoDBService         ? IPostgreSQLService      ?
?    ?                    ?    ?                    ?
? MongoDBService          ? PostgreSQLService       ?
?    ?                    ?    ?                    ?
? MongoSpaceCounterService? PostgresSpaceCounter... ?
?    ?                    ?    ?                    ?
? MongoDB Database        ? PostgreSQL Database     ?
?????????????????????????????????????????????????????

Обе ветки полностью независимы и не знают друг о друге!
```

## Сравнение хранилищ

| Особенность | MongoDB | PostgreSQL |
|------------|---------|------------|
| Тип БД | NoSQL (документная) | SQL (реляционная) |
| Схема данных | Гибкая, без схемы | Строгая схема |
| Запросы | BSON фильтры | SQL запросы |
| Транзакции | Поддерживаются | Полная поддержка |
| Инициализация | Коллекции создаются автоматически | Требуется CREATE TABLE |
| Производительность | Быстрее для чтения | Быстрее для сложных JOIN |
| Масштабирование | Горизонтальное | Вертикальное |
| Зависимости | Независима от PostgreSQL | Независима от MongoDB |

## Когда использовать каждое хранилище

### MongoDB
- Гибкая структура данных
- Быстрое чтение больших объемов
- Документо-ориентированные данные
- Горизонтальное масштабирование
- Независимая работа от SQL БД

### PostgreSQL
- Строгая структура данных
- Сложные запросы с JOIN
- Транзакционная целостность
- Соответствие ACID
- Независимая работа от NoSQL БД

## Заключение

Проект демонстрирует классическое применение паттерна Мост для работы с разными типами баз данных (MongoDB и PostgreSQL) через единый интерфейс. **Ключевая особенность** - полная независимость хранилищ: MongoDB и PostgreSQL работают изолированно, каждое со своим сервисом подсчета. Это позволяет:

- Легко переключаться между хранилищами
- Добавлять новые реализации
- Тестировать код независимо от конкретной БД
- Использовать обе БД одновременно без взаимного влияния
- Гарантировать, что сбой в одной БД не повлияет на другую
