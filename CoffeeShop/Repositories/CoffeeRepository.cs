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

                    while (reader.Read())
                    {
                        var coffee = new Coffee()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("CoffeeId")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            BeanVarietyId = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                            BeanVariety = new BeanVariety()
                            {
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
                    if (reader.Read())
                    {
                        coffee = new Coffee()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("CoffeeId")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            BeanVarietyId = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                            BeanVariety = new BeanVariety()
                            {
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

            }
        }

        public void Update(Coffee coffee)
        {
            using (var conn = Connection)
            {

            }
        }

        public void Delete(int coffeeId)
        {
            using (var conn = Connection)
            {

            }
        }
    }
}
