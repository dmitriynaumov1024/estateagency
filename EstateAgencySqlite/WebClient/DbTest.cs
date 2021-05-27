using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static WebClient.Program;
using Entities;

namespace WebClient
{
    public static class DbTest
    {
        public static void AddLocations()
        {
            client.Execute(
@"insert into Location (Id, Region, Town, District) values
(0, 'Запорізька', 'Запоріжжя', 'Вознесенівський'),
(1, 'Запорізька', 'Запоріжжя', 'Олександрівський'),
(2, 'Запорізька', 'Запоріжжя', 'Дніпровський'),
(3, 'Запорізька', 'Запоріжжя', 'Шевченківський'),
(4, 'Запорізька', 'Запоріжжя', 'Заводський'),
(5, 'Запорізька', 'Запоріжжя', 'Комунарський'),
(6, 'Запорізька', 'Запоріжжя', 'Хортицький'),
(7, 'Запорізька', 'Біленьке', '');"
            );
        }
        public static void AddPersons()
        {
            string now = $"{DateTime.Now: yyyy-MM-dd}";
            for(int i=3890; i<3999; i++)
            client.Execute (
@$"insert into Person (Name, Surname, Phone, Email, LocationId, Address, RegDate) values
('Іван', 'Петрів', '+37028900{i}', 'ivanpetriv99@ukr.net', 3, 'вулиця Будівельників, 22а, кв.14', '{now}');"
            );
        }
    }
}
