# AtmMonitor

Веб-приложение для мониторинга транзакций банкоматов с возможностью фильтрации по банкоматам и периоду времени.

## Описание

Приложение позволяет просматривать операции снятия и внесения наличных по всем банкоматам или по выбранным, с фильтрацией по датам.

## Технологии

**Backend:**
- ASP.NET Core 8 Web API
- Entity Framework Core 8
- MS SQL Server (LocalDB)

**Frontend:**
- Vue 3 (Composition API)
- Vite
- Axios
- Bootstrap 5

## Структура проекта
```
AtmMonitor/
├── src/
│   ├── AtmMonitor.API/           # Web API
│   ├── AtmMonitor.Core/          # Domain entities
│   └── AtmMonitor.Infrastructure/ # EF Core, DbContext
└── frontend/                      # Vue 3 приложение
```

## Запуск с Docker

### Команда docker-compose up --build в корне проекта /AtmMonitor
**Откройте приложение:**
   - Frontend: http://localhost:8080
   - API Swagger: http://localhost:5201/swagger

## Функциональность

### Фильтрация транзакций

- **Все банкоматы** - показать транзакции по всем банкоматам
- **Выбор конкретных банкоматов** - фильтрация по одному или нескольким банкоматам
- **Период** - выбор диапазона дат для отображения транзакций

### Отображение данных

- Дата и время операции
- Адрес банкомата
- Тип операции (Снятие/Внесение наличных)
- Сумма транзакции

## API Endpoints

- `GET /api/ATMs` - получить список всех банкоматов
- `GET /api/ATMs/{id}` - получить банкомат по ID
- `GET /api/Transactions` - получить транзакции с фильтрацией
  - Параметры:
    - `ATMIds` (опционально) - список ID банкоматов
    - `DateFrom` (опционально) - дата начала периода
    - `DateTo` (опционально) - дата окончания периода

## База данных

**Тестовые данные:**
- 5 банкоматов (Москва, Санкт-Петербург, Казань)
- 100 транзакций за ноябрь-декабрь 2024

## Скриншоты

### Все транзакции за выьранный период
<img width="1213" height="899" alt="image" src="https://github.com/user-attachments/assets/8e4fbf9a-5a45-44e9-9314-9758e9ebec95" />

### Фильтрация по нескольким банкоматам и с выбранным периодом
<img width="1234" height="936" alt="image" src="https://github.com/user-attachments/assets/47abad14-6ed5-482f-963b-42656576d454" />

### Фильтрация с выбором 1 банкомата и даты начала выборки
<img width="1319" height="941" alt="image" src="https://github.com/user-attachments/assets/992bf399-623e-4bf8-81e2-cd247ea34122" />

### Получение данных в Swagger UI (Банкоматы)
<img width="1455" height="839" alt="image" src="https://github.com/user-attachments/assets/d4d119fc-ad97-4fa1-9360-ad1a5f9aba56" />

### Получение данных в Swagger UI (Транзакции по id банкомата)
<img width="1498" height="716" alt="image" src="https://github.com/user-attachments/assets/b1fa8095-a048-40ec-95e0-a57ae51e73db" />




