using CoffeeShop.Models;
using System.Collections.Generic;

namespace CoffeeShop.Repositories
{
    public interface ICoffeeRepository
    {
        void Add(Coffee coffee);
        void Delete(int coffeeId);
        List<Coffee> GetAll();
        Coffee GetById(int coffeeId);
        void Update(Coffee coffee);
    }
}