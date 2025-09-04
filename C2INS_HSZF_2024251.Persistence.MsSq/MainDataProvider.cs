using C2INIS_HSZF_2024251.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2INIS_HSZF_2024251.Persistence.MsSql
{
    public class MainDataProvider : IMainDataProvider
    {
        //public AppDbContext ctx { get; set; }
        public IAnimalDataProvider AnimalDataProvider { get; set; }
        public IFoodDataProvider FoodDataProvider { get; set; }
        public IActivityDataProvider ActivityDataProvider { get; set; }

        public bool wasCreated { get { return context.wasCreated; } }

        /*public static DBManager()
        {
            ctx = new AnimalKeepingDbContext();
            ReadFromJSON();
        }*/

        /*static void MakeConnections()
        {
            foreach (var item in ctx.Activities)
            {
                item.Animal = ctx.Animals.FirstOrDefault(x => x.Id == item.AnimalId);
                item.Keeper = ctx.Keepers.FirstOrDefault(x => x.Id == item.KeeperId);
            }
        }*/

        private readonly AppDbContext context;

        public MainDataProvider(AppDbContext context)
        {
            this.context = context;
        }
        public void Init(IAnimalDataProvider animalDataProvider, IFoodDataProvider foodDataProvider, IActivityDataProvider activityDataProvider, bool read)
        {
            //context = new AppDbContext();
            AnimalDataProvider = animalDataProvider;
            FoodDataProvider = foodDataProvider;
            ActivityDataProvider = activityDataProvider;
            if (context.wasCreated && read)
                ReadFromJSON();
            //ezeket átírni valahogy
            //MakeConnections();
        }

        public void Delete()
        {
            context.Delete();
        }




        public void Reset()
        {
            context.Animals.RemoveRange(context.Animals);
            context.SaveChanges();

            context.Reset();
            ReadFromJSON();
            //ctx = new AppDbContext();

            //MakeConnections();
        }

        public void ReadFromJSON()
        {
            AnimalDataProvider.ReadFromJSON();
            FoodDataProvider.ReadFromJSON();
            ActivityDataProvider.ReadFromJSON();

            /*List<Keeper>? keepers = JsonConvert.DeserializeObject<List<Keeper>>(File.ReadAllText("Inputs/Keepers.json"));
            ctx.Keepers.AddRange(keepers);
            ctx.SaveChanges();

            List<Activity>? activites = JsonConvert.DeserializeObject<List<Activity>>(File.ReadAllText("Inputs/Activities.json"));
            ctx.Activities.AddRange(activites);
            ctx.SaveChanges();*/
        }

        //kéne ezeknek egy interfész, és a külön típúsú cuccokat külön classba tenni
    }
}
