# Gochs.ProtectiveStructures — учёт защитных сооружений ГО

Учебный web-продукт для учёта защитных сооружений гражданской обороны, их вместимости и состояния, проведения проверок и распределения сотрудников.

## Возможности

- реестр защитных сооружений ГО;
- типы: убежище, ПРУ, укрытие;
- учёт вместимости, занятых и свободных мест;
- состояния Ready / RequiresAttention / NotReady;
- ответственные лица и контакты;
- реестр сотрудников организации;
- распределение и снятие распределения сотрудников;
- контроль превышения вместимости;
- история проверок;
- замечания и необходимые мероприятия;
- дата следующей проверки;
- автоматическое обновление состояния сооружения по результату новой проверки;
- Dashboard;
- карточка защитного сооружения;
- фильтры сотрудников и проверок;
- REST API и Swagger;
- демонстрационные данные в Development.

## Технологии

- .NET 8 / ASP.NET Core Web API
- Entity Framework Core 8
- SQLite
- Swagger / Swashbuckle
- Bootstrap 5
- Vanilla JavaScript

## Архитектура

HTTP → Controllers → Services → Repositories → AppDbContext → SQLite

Один ASP.NET Core проект без CQRS, MediatR и микросервисного усложнения.

## Запуск

```bash
dotnet restore
dotnet build
dotnet run
```

В Development приложение автоматически применяет существующие EF Core migrations и заполняет пустую БД демонстрационными данными.

Web UI:

```text
http://localhost:<port>/
```

Swagger:

```text
http://localhost:<port>/swagger
```

## Бизнес-правила

- RegistrationNumber защитного сооружения уникален;
- PersonnelNumber сотрудника уникален;
- Capacity должна быть больше 0;
- сотрудник может быть распределён максимум в одно сооружение;
- сотрудника можно снять с распределения;
- нельзя назначить сотрудника в сооружение NotReady;
- нельзя превышать вместимость сооружения;
- нельзя уменьшить Capacity ниже количества уже распределённых сотрудников;
- нельзя удалить сооружение с распределёнными сотрудниками;
- нельзя удалить сооружение с историей проверок;
- InspectionDate не может быть в будущем;
- NextInspectionDate, если указана, должна быть позже InspectionDate;
- новая проверка меняет текущее Condition сооружения на ResultCondition.

## Основные API endpoints

- `GET/POST /api/protective-structures`
- `GET/PUT/DELETE /api/protective-structures/{id}`
- `GET /api/protective-structures/{id}/employees`
- `GET /api/protective-structures/{id}/inspections`
- `GET/POST /api/employees`
- `GET/PUT/DELETE /api/employees/{id}`
- `PATCH /api/employees/{id}/assignment`
- `GET/POST /api/inspections`
- `GET /api/inspections/{id}`
- `GET /api/dashboard`

Enum передаются в JSON строковыми значениями.

## Dashboard

Dashboard показывает:

- количество защитных сооружений;
- общую вместимость;
- количество распределённых и нераспределённых сотрудников;
- количество свободных мест;
- число Ready / RequiresAttention / NotReady;
- количество сооружений с просроченной следующей проверкой.

Просроченной считается только явно заданная NextInspectionDate последней проверки сооружения.

## Граница задания

Проект не является кадровой системой и не моделирует нормативную документацию ГО целиком. Реестр Employee в этом продукте нужен для сценария распределения сотрудников по защитным сооружениям и в будущем может быть заменён общим кадровым источником.
