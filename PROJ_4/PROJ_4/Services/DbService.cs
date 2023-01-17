using System;
using System.Data.SqlClient;
using PROJ_4.Models;


namespace PROJ_4.Services
{
    public class DBService : IDBService
    {
       
        private readonly string connectionString = @"Data Source=db-mssql;Initial Catalog=s17943;Integrated Security=True";

        private bool IsExecuted(int rows)
        {
            if (rows >= 1)
                return true;
            else return false;
        }

        public void CreateAnimal(Animal animal)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = $"INSERT INTO Animal(Name, Description, Category, Area) VALUES(@name, @description, @category, @area)";
                    command.Parameters.AddWithValue("name", animal.Name);
                    command.Parameters.AddWithValue("description", animal.Description);
                    command.Parameters.AddWithValue("category", animal.Category);
                    command.Parameters.AddWithValue("area", animal.Area);
                    connection.Open();
                    int rowsInserted = command.ExecuteNonQuery();
                    
                    connection.Close();
                }
            }
        }

        public void ChangeAnimal(int idAnimal, Animal animal)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE Animal SET Name = @name, Description = @description, Category = @category, Area = @area WHERE idAnimal = @idAnimal";
                    command.Parameters.AddWithValue("name", animal.Name);
                    command.Parameters.AddWithValue("description", animal.Description);
                    command.Parameters.AddWithValue("category", animal.Category);
                    command.Parameters.AddWithValue("area", animal.Area);
                    command.Parameters.AddWithValue("idAnimal", idAnimal);
                    connection.Open();
                    int rowsChanged = command.ExecuteNonQuery();
                    
                    connection.Close();
                }
            }
        }

        public void DeleteAnimal(int idAnimal)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM Animal WHERE idAnimal = @idAnimal";
                    command.Parameters.AddWithValue("idAnimal", idAnimal);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    
                    connection.Close();
                }
            }
        }

        public List<Animal> GetAnimals(string orderBy)
        {
            var animals = new List<Animal>();
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    string[] columnsNames = { "name", "description", "category", "area" };
                    bool isMatched = false;

                    if (!string.IsNullOrEmpty(orderBy))
                    {
                        foreach (var columnName in columnsNames)
                            if (orderBy.ToLower().Equals(columnName))
                                isMatched = true;

                        if (isMatched)
                            command.CommandText = $"SELECT * FROM Animal ORDER BY {orderBy} ASC";
                        
                    }
                    else command.CommandText = "SELECT * FROM Animal ORDER BY Name ASC";

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                        animals.Add(new Animal
                        {
                            IdAnimal = int.Parse(reader["IdAnimal"].ToString()),
                            Name = reader["Name"].ToString(),
                            Description = reader["Description"].ToString(),
                            Category = reader["Category"].ToString(),
                            Area = reader["Area"].ToString()
                        });
                    connection.Close();
                }
            }
            return animals;
        }
    }
}