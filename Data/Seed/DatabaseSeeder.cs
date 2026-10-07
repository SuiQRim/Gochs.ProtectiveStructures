using Gochs.ProtectiveStructures.Entities;
using Gochs.ProtectiveStructures.Enums;
using Microsoft.EntityFrameworkCore;

namespace Gochs.ProtectiveStructures.Data.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.ProtectiveStructures.AnyAsync())
            return;

        var shelter = new ProtectiveStructure
        {
            RegistrationNumber = "ZS-001",
            Name = "Убежище административного корпуса",
            Type = ProtectiveStructureType.Shelter,
            Address = "ул. Центральная, 10",
            Capacity = 8,
            Condition = ProtectiveStructureCondition.Ready,
            ResponsiblePerson = "Иванов Сергей Петрович",
            Phone = "+7 900 100-10-01",
            Notes = "Основное защитное сооружение административного корпуса"
        };

        var antiRadiationShelter = new ProtectiveStructure
        {
            RegistrationNumber = "ZS-002",
            Name = "Противорадиационное укрытие производственного корпуса",
            Type = ProtectiveStructureType.AntiRadiationShelter,
            Address = "Промышленный проезд, 5",
            Capacity = 6,
            Condition = ProtectiveStructureCondition.RequiresAttention,
            ResponsiblePerson = "Петров Алексей Викторович",
            Phone = "+7 900 100-10-02",
            Notes = "Требуется устранить замечания последней проверки"
        };

        var simpleCover = new ProtectiveStructure
        {
            RegistrationNumber = "ZS-003",
            Name = "Укрытие складского комплекса",
            Type = ProtectiveStructureType.SimpleCover,
            Address = "Складская ул., 3",
            Capacity = 5,
            Condition = ProtectiveStructureCondition.NotReady,
            ResponsiblePerson = "Соколова Мария Андреевна",
            Phone = "+7 900 100-10-03"
        };

        context.ProtectiveStructures.AddRange(shelter, antiRadiationShelter, simpleCover);

        context.Employees.AddRange(
            new Employee { PersonnelNumber = "EMP-001", FullName = "Александров Кирилл Сергеевич", Department = "Администрация", Position = "Специалист", ProtectiveStructure = shelter },
            new Employee { PersonnelNumber = "EMP-002", FullName = "Белов Максим Андреевич", Department = "Администрация", Position = "Инженер", ProtectiveStructure = shelter },
            new Employee { PersonnelNumber = "EMP-003", FullName = "Васильева Ирина Олеговна", Department = "Бухгалтерия", Position = "Бухгалтер", ProtectiveStructure = shelter },
            new Employee { PersonnelNumber = "EMP-004", FullName = "Громов Илья Денисович", Department = "Производство", Position = "Мастер", ProtectiveStructure = antiRadiationShelter },
            new Employee { PersonnelNumber = "EMP-005", FullName = "Давыдов Артём Сергеевич", Department = "Производство", Position = "Механик", ProtectiveStructure = antiRadiationShelter },
            new Employee { PersonnelNumber = "EMP-006", FullName = "Ермакова Анна Павловна", Department = "Производство", Position = "Технолог" },
            new Employee { PersonnelNumber = "EMP-007", FullName = "Жуков Николай Павлович", Department = "Логистика", Position = "Кладовщик" },
            new Employee { PersonnelNumber = "EMP-008", FullName = "Зайцева Ольга Андреевна", Department = "Логистика", Position = "Специалист по снабжению" });

        context.Inspections.AddRange(
            new Inspection
            {
                ProtectiveStructure = shelter,
                InspectionDate = new DateOnly(2026, 9, 15),
                InspectorName = "Комиссия по ГО",
                ResultCondition = ProtectiveStructureCondition.Ready,
                Findings = "Существенных замечаний не выявлено.",
                NextInspectionDate = new DateOnly(2027, 3, 15)
            },
            new Inspection
            {
                ProtectiveStructure = antiRadiationShelter,
                InspectionDate = new DateOnly(2026, 8, 20),
                InspectorName = "Комиссия по ГО",
                ResultCondition = ProtectiveStructureCondition.RequiresAttention,
                Findings = "Требуется обслуживание вентиляционной системы.",
                RequiredActions = "Провести техническое обслуживание и повторную проверку.",
                NextInspectionDate = new DateOnly(2026, 11, 20)
            },
            new Inspection
            {
                ProtectiveStructure = simpleCover,
                InspectionDate = new DateOnly(2026, 7, 10),
                InspectorName = "Ответственный по ГО",
                ResultCondition = ProtectiveStructureCondition.NotReady,
                Findings = "Не обеспечена готовность помещения к приёму людей.",
                RequiredActions = "Освободить проходы и восстановить аварийное освещение.",
                NextInspectionDate = new DateOnly(2026, 9, 10)
            });

        await context.SaveChangesAsync();
    }
}
