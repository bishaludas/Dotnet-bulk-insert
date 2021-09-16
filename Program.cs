using System;
using System.Data.SqlClient;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace learn
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                DateTime StartDate = DateTime.Now;
                System.Console.WriteLine(StartDate);
                // Build connection string
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "localhost";   // update me
                builder.UserID = "sa";              // update me
                builder.Password = "wolfmatrix@123";      // update me
                builder.InitialCatalog = "master";

                // prepare fake data of 50 employees with 5 hobbies
                var empList = new List<Employee>();
                for (int i = 0; i < 53; i++)
                {
                    var empName=$"Bishal {i}";
                    var empLocation=$"Lalitpur{i}";
                    var empHobby = new List<Hobby>();

                    for (int j = 0; j < 5; j++)
                    {
                        var hobby=$"Code {i}{j}";
                        var details=$"Its fun and you learn something new {i}{j}";
                        var hobbyObj = new Hobby(hobby, details);
                        empHobby.Add(hobbyObj);
                    }

                    var emp = new Employee(empName, empLocation, empHobby);
                    empList.Add(emp);
                }


                // System.Console.WriteLine(JsonConvert.SerializeObject(empList));

                // Connect to SQL
                Console.Write("Connecting to SQL Server ... ");
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    Console.WriteLine("Done.");
                    String sql;

                    // Function to create DB and tables
                    // var programObj = new Program();
                    // programObj.CreateDatabaseSchema(connection);
                    
                    System.Console.WriteLine("===Inserting Employees===");
                    StringBuilder sb = new StringBuilder();
                    foreach (var emp in empList){
                        int EmpId;        
                        
                        sb.Clear();
                        sb.Append($@"USE SampleDB
                            declare @newId int; select @newId = isnull(max(Id), 0) + 1 from Employees
                            INSERT INTO Employees (Id,Name, Location) VALUES
                            (@newId,'{emp.Name}', '{emp.Location}')
                            
                            select @newId;
                        ");
                        sql = sb.ToString();
                        // System.Console.WriteLine(sql);
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            EmpId = (int)command.ExecuteScalar();
                            Console.WriteLine("Employee added.");
                        }
                        
                        foreach (var hobby in emp.EmpHobby)
                        {
                            hobby.Eid = EmpId;
                        }
                    }
                    System.Console.WriteLine("===Inserting Hobbies===");

                    sb.Clear();
                    sb.Append("INSERT into Hobby (EmpId, Hobbyname, Details) VALUES ");
                    int chunkSizelimit = 500;
                    int chunk = 0;
                    for (int i = 0; i < empList.Count; i++)
                    {
                        foreach (var item in empList[i].EmpHobby)
                        {
                            sb.Append($"({item.Eid}, '{item.Details}', '{item.Hobbyname}'),");
                            chunk ++;
                            
                            if (chunk >= chunkSizelimit)
                            {
                                sql = sb.ToString();
                                sql = sql.Remove(sql.Length -1, 1) + ";";

                                using (SqlCommand command = new SqlCommand(sql, connection))
                                {
                                    int rowsAffected = command.ExecuteNonQuery();
                                    Console.WriteLine(rowsAffected + " row(s) inserted");
                                }

                                chunk = 0;
                                sb.Clear();
                                sb.Append("INSERT into Hobby (EmpId, Hobbyname, Details) VALUES ");    
                            }
                        }
                    }

                    if (sb.ToString() != "INSERT into Hobby (EmpId, Hobbyname, Details) VALUES ")
                    {
                        sql = sb.ToString();
                        sql = sql.Remove(sql.Length -1, 1) + ";";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            Console.WriteLine(rowsAffected + " row(s) inserted");
                        }
                    }
                    sb.Clear();

                DateTime EndDate = DateTime.Now;
                System.Console.WriteLine(EndDate);



                    // System.Console.WriteLine(JsonConvert.SerializeObject(empList));

                /*
                    // INSERT demo
                    Console.Write("Inserting a new row into table, press any key to continue...");
                    Console.ReadKey(true);
                    sb.Clear();
                    sb.Append("INSERT Employees (Name, Location) ");
                    sb.Append("VALUES (@name, @location);");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", "Jake");
                        command.Parameters.AddWithValue("@location", "United States");
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected + " row(s) inserted");
                    }

                    // UPDATE demo
                    String userToUpdate = "Nikita";
                    Console.Write("Updating 'Location' for user '" + userToUpdate + "', press any key to continue...");
                    Console.ReadKey(true);
                    sb.Clear();
                    sb.Append("UPDATE Employees SET Location = N'United States' WHERE Name = @name");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", userToUpdate);
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected + " row(s) updated");
                    }

                    // DELETE demo
                    String userToDelete = "Jared";
                    Console.Write("Deleting user '" + userToDelete + "', press any key to continue...");
                    Console.ReadKey(true);
                    sb.Clear();
                    sb.Append("DELETE FROM Employees WHERE Name = @name;");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", userToDelete);
                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine(rowsAffected + " row(s) deleted");
                    }

                    // READ demo
                    System.Console.WriteLine("Reading data from table, press any key to continue...");
                    sql = "SELECT Id, Name, Location FROM Employees;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("{0} {1} {2}", reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                            }
                        }
                    }
                */
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

        }


        public void CreateDatabaseSchema(SqlConnection connection){
            // Create a sample database
            Console.Write("Dropping and creating database 'SampleDB' ... ");
            String sql = "DROP DATABASE IF EXISTS [SampleDB]; CREATE DATABASE [SampleDB]";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Done.");
            }

            // Create a Table and insert some sample data
            StringBuilder sb = new StringBuilder();
            sb.Append("USE SampleDB; ");
            sb.Append("CREATE TABLE Employees ( ");
            sb.Append(" Id INT NOT NULL PRIMARY KEY, ");
            sb.Append(" Name NVARCHAR(50), ");
            sb.Append(" Location NVARCHAR(50) ");
            sb.Append("); ");

            sb.Append("CREATE TABLE Hobby ( ");
            sb.Append(" EmpId INT NOT NULL, ");
            sb.Append(" Hobbyname NVARCHAR(50), ");
            sb.Append(" Details NVARCHAR(50) ");
            sb.Append("); ");

            sb.Append("INSERT INTO Employees (Id,Name, Location) VALUES ");
            sb.Append("(1,'Jared', 'Australia'), ");
            sb.Append("(2,'Nikita', 'India'), ");
            sb.Append("(3,'Tom', 'Germany'); ");
            sql = sb.ToString();
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
                Console.WriteLine("Tables created.");
            }
        }
    }
}
