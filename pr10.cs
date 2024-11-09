using System;
using System.Collections.Generic;

class RoomBookingSystem
{
    public bool CheckAvailability(string roomType)
    {
        Console.WriteLine($"Проверка доступности для номера типа: {roomType}");
        return true;
    }

    public void BookRoom(string roomType, int nights)
    {
        Console.WriteLine($"Номер типа {roomType} забронирован на {nights} ночей.");
    }

    public void CancelBooking(string roomType)
    {
        Console.WriteLine($"Бронирование номера типа {roomType} отменено.");
    }
}

class RestaurantSystem
{
    public void BookTable(int seats)
    {
        Console.WriteLine($"Стол на {seats} человек забронирован в ресторане.");
    }

    public void OrderFood(string[] dishes)
    {
        Console.WriteLine("Заказаны следующие блюда:");
        foreach (var dish in dishes)
        {
            Console.WriteLine($"- {dish}");
        }
    }
}

class EventManagementSystem
{
    public void BookEventHall(string hallName)
    {
        Console.WriteLine($"Конференц-зал '{hallName}' забронирован для мероприятия.");
    }

    public void ArrangeEquipment(string equipment)
    {
        Console.WriteLine($"Оборудование '{equipment}' подготовлено для мероприятия.");
    }
}

class CleaningService
{
    public void ScheduleCleaning(string roomType, string time)
    {
        Console.WriteLine($"Уборка для номера типа {roomType} запланирована на {time}.");
    }

    public void RequestCleaningNow(string roomType)
    {
        Console.WriteLine($"Уборка для номера типа {roomType} выполняется немедленно.");
    }
}
class HotelFacade
{
    private RoomBookingSystem roomBooking;
    private RestaurantSystem restaurant;
    private EventManagementSystem eventManagement;
    private CleaningService cleaning;

    public HotelFacade()
    {
        roomBooking = new RoomBookingSystem();
        restaurant = new RestaurantSystem();
        eventManagement = new EventManagementSystem();
        cleaning = new CleaningService();
    }

    public void BookRoomWithServices(string roomType, int nights, string[] dishes)
    {
        Console.WriteLine("\n--- Бронирование номера с услугами ---");
        
        if (roomBooking.CheckAvailability(roomType))
        {
            roomBooking.BookRoom(roomType, nights);
            restaurant.OrderFood(dishes);
            cleaning.ScheduleCleaning(roomType, "9:00 утра");
        }
        else
        {
            Console.WriteLine($"Извините, номер типа {roomType} недоступен.");
        }
    }

    public void OrganizeEventWithRoomAndEquipment(string hallName, string equipment, string roomType, int roomsNeeded)
    {
        Console.WriteLine("\n--- Организация мероприятия ---");

        eventManagement.BookEventHall(hallName);
        eventManagement.ArrangeEquipment(equipment);

        for (int i = 0; i < roomsNeeded; i++)
        {
            if (roomBooking.CheckAvailability(roomType))
            {
                roomBooking.BookRoom(roomType, 1);
            }
        }
    }

    public void BookTableWithTaxi(int seats)
    {
        Console.WriteLine("\n--- Бронирование стола и вызов такси ---");

        restaurant.BookTable(seats);
        Console.WriteLine("Такси вызвано и ожидает возле отеля.");
    }

    public void CancelRoomBooking(string roomType)
    {
        Console.WriteLine("\n--- Отмена бронирования номера ---");
        roomBooking.CancelBooking(roomType);
    }

    public void RequestImmediateCleaning(string roomType)
    {
        Console.WriteLine("\n--- Запрос немедленной уборки ---");
        cleaning.RequestCleaningNow(roomType);
    }
}





class Program
{
    static void Main(string[] args)
    {
        // Создание фасада
        HotelFacade hotelFacade = new HotelFacade();

        hotelFacade.BookRoomWithServices("Люкс", 3, new string[] { "Салат Цезарь", "Стейк", "Тирамису" });

        hotelFacade.OrganizeEventWithRoomAndEquipment("Конференц-зал A", "Проектор и микрофон", "Стандарт", 5);

        hotelFacade.BookTableWithTaxi(4);

        hotelFacade.CancelRoomBooking("Люкс");
        hotelFacade.RequestImmediateCleaning("Люкс");
    }
}

/////////////////////////////////////////////////////////////////////////////////////


using System;
using System.Collections.Generic;

abstract class OrganizationComponent
{
    public string Name { get; set; }

    protected OrganizationComponent(string name)
    {
        Name = name;
    }

    public abstract int GetEmployeeCount();
    public abstract decimal GetBudget();
    public abstract void DisplayHierarchy(int depth = 0);
    public virtual void Add(OrganizationComponent component) { }
    public virtual void Remove(OrganizationComponent component) { }
}
class Employee : OrganizationComponent
{
    public string Position { get; set; }
    public decimal Salary { get; set; }
    public bool IsContractor { get; set; } // Временный сотрудник

    public Employee(string name, string position, decimal salary, bool isContractor = false) 
        : base(name)
    {
        Position = position;
        Salary = salary;
        IsContractor = isContractor;
    }

    public override int GetEmployeeCount() => 1;

    public override decimal GetBudget() => IsContractor ? 0 : Salary;

    public override void DisplayHierarchy(int depth = 0)
    {
        Console.WriteLine($"{new string('-', depth)} {Position}: {Name} (Зарплата: {Salary:C}, Контрактор: {IsContractor})");
    }
}
class Department : OrganizationComponent
{
    private readonly List<OrganizationComponent> _components = new List<OrganizationComponent>();

    public Department(string name) : base(name) { }

    public override void Add(OrganizationComponent component)
    {
        _components.Add(component);
    }

    public override void Remove(OrganizationComponent component)
    {
        _components.Remove(component);
    }

    public override int GetEmployeeCount()
    {
        int count = 0;
        foreach (var component in _components)
        {
            count += component.GetEmployeeCount();
        }
        return count;
    }

    public override decimal GetBudget()
    {
        decimal budget = 0;
        foreach (var component in _components)
        {
            budget += component.GetBudget();
        }
        return budget;
    }

    public override void DisplayHierarchy(int depth = 0)
    {
        Console.WriteLine($"{new string('-', depth)} Отдел: {Name}");
        foreach (var component in _components)
        {
            component.DisplayHierarchy(depth + 2);
        }
    }
    public OrganizationComponent FindEmployeeByName(string name)
    {
        foreach (var component in _components)
        {
            if (component is Employee employee && employee.Name == name)
            {
                return employee;
            }
            else if (component is Department department)
            {
                var result = department.FindEmployeeByName(name);
                if (result != null)
                {
                    return result;
                }
            }
        }
        return null;
    }
    public void DisplayAllEmployees()
    {
        Console.WriteLine($"\nСотрудники отдела {Name}:");
        foreach (var component in _components)
        {
            if (component is Employee employee)
            {
                employee.DisplayHierarchy();
            }
            else if (component is Department department)
            {
                department.DisplayAllEmployees();
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        var emp1 = new Employee("Алексей Иванов", "Разработчик", 80000);
        var emp2 = new Employee("Мария Петрова", "Дизайнер", 75000);
        var emp3 = new Employee("Сергей Смирнов", "Аналитик", 70000);
        var emp4 = new Employee("Анна Кузнецова", "Тестировщик", 65000, isContractor: true);
        
        var devDept = new Department("Отдел Разработки");
        devDept.Add(emp1);
        devDept.Add(emp2);

        var qaDept = new Department("Отдел Тестирования");
        qaDept.Add(emp4);

        var itDept = new Department("IT Департамент");
        itDept.Add(devDept);
        itDept.Add(qaDept);
        itDept.Add(emp3);

        Console.WriteLine("Иерархия организации:");
        itDept.DisplayHierarchy();

        Console.WriteLine($"\nОбщее количество сотрудников: {itDept.GetEmployeeCount()}");
        Console.WriteLine($"Общий бюджет IT Департамента: {itDept.GetBudget():C}");

        string searchName = "Мария Петрова";
        var foundEmployee = itDept.FindEmployeeByName(searchName);
        if (foundEmployee != null)
        {
            Console.WriteLine($"\nНайден сотрудник по имени {searchName}:");
            foundEmployee.DisplayHierarchy();
        }
        else
        {
            Console.WriteLine($"\nСотрудник с именем {searchName} не найден.");
        }
        itDept.DisplayAllEmployees();
    }
}
