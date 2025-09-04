using C2INIS_HSZF_2024251.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2INIS_HSZF_2024251.Persistence.MsSql
{
    public class FoodDataProvider : IFoodDataProvider
    {

        private readonly AppDbContext context;

        public FoodDataProvider(AppDbContext context)
        {
            this.context = context;
        }

        public void ReadFromJSON()
        {
            /*context.Keepers.RemoveRange(context.Keepers);
            context.SaveChanges();*/

            List<Food>? keepers = JsonConvert.DeserializeObject<List<Food>>(File.ReadAllText(Path.Combine("Inputs", "Foods.json")));
            context.Keepers.AddRange(keepers);
            context.SaveChanges();
        }



        public void CreateEntity(Food keeper)
        {
            context.Keepers.Add(keeper);
            context.SaveChanges();
        }

        public Food DeleteEntity(Food keeperToDelete)
        {
            context.Keepers.Remove(keeperToDelete);
            context.SaveChanges();
            return keeperToDelete;
        }

        public IEnumerable<Food> ReadAllEntity()
        {
            return context.Keepers;
        }

        public Food Update(Food oldKeeper, Food newKkeper)
        {

            oldKeeper.Name = newKkeper.Name;
            oldKeeper.Quantity = newKkeper.Quantity;

            context.SaveChanges();

            return oldKeeper;
        }


        public void Reduce(Food food)
        {
            if (food.Quantity > 0)
            {
                food.Quantity--;
                context.SaveChanges();
            }
            else
            {
                throw new Exception("No more");
            }
        }

        public Food SearchById(int id)
        {
            return context.Keepers.FirstOrDefault(a => a.Id == id);
        }
    }
}
