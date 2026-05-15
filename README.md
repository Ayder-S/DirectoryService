# DirectoryService

Сервис каталога организационной структуры (локации, отделы, должности).

## Структура

```
.
├── backend/    — .NET 10 (Clean Architecture)
└── frontend/   — пока пусто
```

## Backend

Стек: .NET 9, EF Core, PostgreSQL, Scalar (OpenAPI).

### Запуск

```bash
cd backend
docker compose up -d              # PostgreSQL на :5434
dotnet run --project src/DS.WebApi
```

API: `http://localhost:5266`
OpenAPI (Scalar): `http://localhost:5266/scalar/v1`

### Миграции

```bash
cd backend
dotnet ef database update --project src/DS.Infrastructure.Postgresql --startup-project src/DS.WebApi
```

### Проекты

| Проект | Назначение |
|---|---|
| `DS.Domain` | Сущности, value objects |
| `DS.Application` | Use cases, обработчики команд |
| `DS.Infrastructure.Postgresql` | EF Core, репозитории, миграции |
| `DS.Presenters` | Контроллеры |
| `DS.WebApi` | Хост, DI, middleware |
| `DS.Contracts` | DTO для входящих запросов |
| `Shared` | Общие типы (Result, Envelope, константы) |
