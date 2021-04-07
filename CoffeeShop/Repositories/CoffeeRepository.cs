using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using CoffeeShop.Models;

namespace CoffeeShop.Repositories
{
    public class CoffeeRepository : ICoffeeRepository
    {
        private readonly string _connectionString;
        public CoffeeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection Connection
        {
            get { return new SqlConnection(_connectionString); }
        }

        public List<Coffee> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id AS CoffeeId, c.Title, bv.Id As BeanVarietyId, bv.Name, bv.Region, bv.Notes
                                        FROM Coffee c 
                                        JOIN BeanVariety bv 
                                        ON c.BeanVarietyId = bv.Id";

                    var reader = cmd.ExecuteReader();

                    // Create a List to hold the Coffee Objects
                    var allCoffee = new List<Coffee>();

                    // As long as the reader detects another row, continue making 
                    // coffee objects and storing them into the allCoffee List.
                    while (reader.Read())
                    {
                        var coffee = new Coffee()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("CoffeeId")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            BeanVarietyId = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                            BeanVariety = new BeanVariety()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Region = reader.GetString(reader.GetOrdinal("Region"))
                            }
                        };
                        if (!reader.IsDBNull(reader.GetOrdinal("Notes")))
                        {
                            coffee.BeanVariety.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                        }
                        allCoffee.Add(coffee);
                    }
                    reader.Close();
                    return allCoffee;
                }
            }
        }                      

    public Coffee GetById(int coffeeId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id AS CoffeeId, c.Title, bv.Id As BeanVarietyId, bv.Name, bv.Region, bv.Notes
                                        FROM Coffee c 
                                        JOIN BeanVariety bv 
                                        ON c.BeanVarietyId = bv.Id
                                        WHERE c.Id = @coffeeId";

                    cmd.Parameters.AddWithValue("@coffeeId", coffeeId);
                    var reader = cmd.ExecuteReader();

                    Coffee coffee = null;

                    // If the reader detects a row of data to pull, it will overwrite
                    // the empty Coffee object. If there is no data based off the query,
                    // the Coffee object will be returned as null.
                    if (reader.Read())
                    {
                        coffee = new Coffee()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("CoffeeId")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            BeanVarietyId = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                            BeanVariety = new BeanVariety()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Region = reader.GetString(reader.GetOrdinal("Region"))
                            }
                        };
                        if (!reader.IsDBNull(reader.GetOrdinal("Notes")))
                        {
                            coffee.BeanVariety.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                        }          
                    }
                    reader.Close();
                    return coffee;
                }
            }
        }

        public void Add(Coffee coffee)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INPUT INTO Coffee (Title, BeanVarietyId) 
                                        OUTPUT INSERTED.ID
                                        VALUES (@title, @beanVarietyId)";

                    cmd.Parameters.AddWithValue("@title", coffee.Title);
                    cmd.Parameters.AddWithValue("@beanVarietyId", coffee.BeanVarietyId);

                    coffee.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Coffee coffee)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using(var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Coffee
                                        SET Title = @title
                                            BeanVarietyId = @beanVarietyId
                                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@title", coffee.Title);
                    cmd.Parameters.AddWithValue("@beanVarietyId", coffee.BeanVarietyId);
                    cmd.Parameters.AddWithValue("@id", coffee.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int coffeeId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using(var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE 
                                        FROM Coffee
                                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", coffeeId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
