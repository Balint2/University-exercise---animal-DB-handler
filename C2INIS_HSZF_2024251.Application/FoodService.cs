using C2INIS_HSZF_2024251.Model;
using C2INIS_HSZF_2024251.Persistence.MsSql;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2INIS_HSZF_2024251.Application
{
    public class FoodService : IFoodService
    {
        string path;
        private readonly IFoodDataProvider foodDataProvider;
        public FoodService(IFoodDataProvider keeperDataProvider)
        {           
            this.foodDataProvider = keeperDataProvider;
        }
        public void Init(string path)
        {
            this.path = path;
        }

        public Food CreateEntity(Food newKeeper)
        {
            foodDataProvider.CreateEntity(newKeeper);
            return newKeeper;
        }
        public Food DeleteEntity(Food keeper)
        {
            return foodDataProvider.DeleteEntity(keeper);
        }
        public Food UpdateEntity(Food oldKeeper, Food newKeeper)
        {
            return foodDataProvider.Update(oldKeeper, newKeeper);
        }
        public IEnumerable<Food> ListEntities()
        {
            return foodDataProvider.ReadAllEntity();
        }

        public Food SearchById(int id)
        {
            return foodDataProvider.SearchById(id);
        }

        public void Reduce(Food food)
        {
            try
            {
                foodDataProvider.Reduce(food);
            }
            catch
            {
                throw;
            }
        }


        public void MakeRiport()
        {
            StreamWriter writer = new StreamWriter(Path.Combine(path, $"Foods_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.txt"));

            var foods = ListEntities();
            foreach (var food in foods)
            {
                writer.WriteLine("Food: ");
                writer.WriteLine(food.ToString());
                var connectedAnimals = food.Activities.Select(x => x.Animal).Distinct();
                writer.WriteLine($"Eat: {connectedAnimals.Sum(x => x.Activities.Where(x => x.Food == food).Count())}");
                foreach (var animal in connectedAnimals)
                {
                    writer.WriteLine("\tAnimal: ");
                    writer.WriteLine("\t" + animal.ToString());
                    writer.WriteLine($"\tEat: {animal.Activities.Where(x => x.Food == food).Count(/*x => x.Food.Quantity*/)}");
                    writer.WriteLine();

                    //writer.WriteLine("\t\t" + foods.Where(x => x.Activities.).Sum(x => x.Quantity));
                }

                writer.WriteLine();
            }

            writer.Close();
            /*
                        writer.WriteLine(keeper.ToString());

                        var activities = keeper.Activities;

                        var byDay = activities.GroupBy(x => x.Date.Date);

                        writer.WriteLine("\nActivity quantity: ");
                        foreach (var daily in byDay)
                        {
                            writer.WriteLine("\tDate: "+daily.Key.ToString("yyyy-MM-dd"));
                            writer.WriteLine("\tQuantity: " + daily.Count());
                            var byAnimal = daily.GroupBy(x => x.Animal);
                            foreach (var animally in byAnimal)
                            {
                                writer.WriteLine("\t\tAnimal: " + animally.Key.ToString());
                                writer.WriteLine("\t\tQuantiy: " + animally.Count());
                                writer.WriteLine();
                            }
                        }
                        writer.Close();*/
        }

    }
}
