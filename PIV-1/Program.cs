using System;
using System.Data.SqlClient;

//Stwórz prostą aplikację konsolową. 
//Podłącz do niej dowolną bazę danych, (np. Northwind) przy pomocy
//ADO.NET i dodaj funkcjonalność CRUD na dowolnej tabeli.
//Najpierw wyświetl listę danych z tej tabeli, następnie pozwól dodać nowy wpis, 
//pozwól go edytować a na koniec usuń go.

namespace PIV_1
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnectionStringBuilder connectionString = new SqlConnectionStringBuilder
            {
                DataSource = @"HAL\MSSERVER",
                InitialCatalog = "ZNorthwind",
                IntegratedSecurity = true,
                ConnectTimeout = 30,
                Encrypt = false,
                TrustServerCertificate = false,
                ApplicationIntent = 0,
                MultiSubnetFailover = false
            };

            //utworzenie obiektu odpowiedzialnego za polaczenie z baza

            using var connection = new SqlConnection(connectionString.ConnectionString);

            connection.Open();//otwarcie polaczenia z baza

            var inquiry = "SELECT * FROM dbo.Pracownicy"; //sformulowanie zapytania

            var command = new SqlCommand(inquiry, connection);

            var count = ShowTable(command);
            //sformulowanie nonquery 
            var insert = "INSERT INTO dbo.Pracownicy (IDpracownika, Nazwisko, Imię, Stanowisko) VALUES (@IDpracownika, @Nazwisko, @Imie, @Stanowisko)";
            var insertCommand = new SqlCommand(insert, connection);

            //inicjalizacja zmiennych:
            Console.WriteLine("Podaj imie nowego pracownika: ");
            var fname = Console.ReadLine();
            Console.WriteLine("Podaj nazwisko pracownika: ");
            var lname = Console.ReadLine();
            Console.WriteLine("Podaj stanowisko pracownika: ");
            var position = Console.ReadLine();

            //inicjalizacja parametrow:
            insertCommand.Parameters.Add(new SqlParameter("IDpracownika", count + 1));
            insertCommand.Parameters.Add(new SqlParameter("Imie", fname));
            insertCommand.Parameters.Add(new SqlParameter("Nazwisko", lname));
            insertCommand.Parameters.Add(new SqlParameter("Stanowisko", position));

            //wykonanie polecenia na bazie:
            Console.WriteLine("Liczba zmienionych wierszy: " +
                insertCommand.ExecuteNonQuery());
            Console.WriteLine();
            //wyswietlenie zmodyfikowanej tabeli dbo.Pracownicy 
            ShowTable(command);

            //edytowanie zawartosci wiersza w oparciu o nazwisko 
            var edit = "UPDATE dbo.Pracownicy SET Stanowisko = @stanowisko WHERE Nazwisko = @nazwisko";
            var editCommand = new SqlCommand(edit, connection);

            Console.WriteLine("Podaj nazwisko pracownika ktorego stanowisko chcesz zmienic: ");
            lname = Console.ReadLine();
            Console.WriteLine("Podaj jego nowe stanowisko pracy: ");
            position = Console.ReadLine();
            editCommand.Parameters.Add(new SqlParameter("stanowisko", position));
            editCommand.Parameters.Add(new SqlParameter("nazwisko", lname));
            //wykonanie polecenia na bazie:
            Console.WriteLine("Liczba zmienionych wierszy: " +
                editCommand.ExecuteNonQuery());


            //wyswietlenie zmodyfikowanej tabeli dbo.Pracownicy 
            ShowTable(command);

            //usuwanie wiersze w oparciu o imie pracownika
            var delete = "DELETE FROM dbo.Pracownicy WHERE Nazwisko = @nazwisko";
            var deleteCommand = new SqlCommand(delete, connection);

            Console.WriteLine("Podaj nazwisko pracownika do elimacji: ");
            lname = Console.ReadLine();
            deleteCommand.Parameters.Add(new SqlParameter("nazwisko", lname));

            //wykonanie polecenia na bazie:
            Console.WriteLine("Liczba zmienionych wierszy: " +
                deleteCommand.ExecuteNonQuery());

            //wyswietlenie zmodyfikowanej tabeli dbo.Pracownicy 
            ShowTable(command);
        }
        static int ShowTable(SqlCommand command)
        {
            int count = 0;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader.GetInt32(0) + "\t" + reader.GetString(2) + " " +
                    reader.GetString(1) + "\t" + reader.GetString(3));
                count++;
            }
            reader.Close();
            return count;
        }
    }
}
