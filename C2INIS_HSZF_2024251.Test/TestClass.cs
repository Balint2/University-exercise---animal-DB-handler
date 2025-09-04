using C2INIS_HSZF_2024251.Application;
using C2INIS_HSZF_2024251.Model;
using C2INIS_HSZF_2024251.Persistence.MsSql;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace C2INIS_HSZF_2024251.Test
{

    [TestFixture]
    public class TestClass
    {

        /*Animal testAnimal;
        Keeper testKeeper;
        Model.Activity testActivity;*/

        void TestInit(ref IAnimalService? animalService, ref IFoodService? foodService, ref IActivityService? activityService, ref IMainService? mainDataService)
        {
            var host = Host.CreateDefaultBuilder().ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<AppDbContext>();
                services.AddSingleton<IAnimalDataProvider, AnimalDataProvider>();
                services.AddSingleton<IAnimalService, AnimalService>();
                services.AddSingleton<IFoodDataProvider, FoodDataProvider>();
                services.AddSingleton<IFoodService, FoodService>();
                services.AddSingleton<IActivityDataProvider, ActivityDataProvider>();
                services.AddSingleton<IActivityService, ActivityService>();
                services.AddSingleton<IMainDataProvider, MainDataProvider>();
                services.AddSingleton<IMainService, MainService>();

            }).Build();

            host.Start();
            using IServiceScope serviceScope = host.Services.CreateScope();

            animalService = host.Services.GetRequiredService<IAnimalService>();
            foodService = host.Services.GetRequiredService<IFoodService>();
            activityService = host.Services.GetRequiredService<IActivityService>();
            mainDataService = host.Services.GetRequiredService<IMainService>();

            animalService.Init("Riports");
            foodService.Init("Riports");
            mainDataService.Init(animalService, foodService, activityService, host.Services.GetRequiredService<IAnimalDataProvider>(), host.Services.GetRequiredService<IFoodDataProvider>(), host.Services.GetRequiredService<IActivityDataProvider>(), false);
        }


        [Test]
        public void AnimalFolderTest()
        {
            IAnimalService? animalService = null;
            IFoodService? foodService = null;
            IActivityService? activityService = null;
            IMainService? mainDataService = null;
            TestInit(ref animalService, ref foodService, ref activityService, ref mainDataService);

            Assert.That(Directory.Exists(Path.Combine("Riports")), Is.EqualTo(true));
            if (mainDataService.wasCreated)
            {
                mainDataService.DeleteAll();
            }
        }


        [Test]
        public void AnimalSubFolderTest()
        {
            IAnimalService? animalService = null;
            IFoodService? foodService = null;
            IActivityService? activityService = null;
            IMainService? mainDataService = null;
            TestInit(ref animalService, ref foodService, ref activityService, ref mainDataService);

            Animal testAnimal = new Animal("Jancsi", Genders.male, 2, "sas");
            mainDataService.AnimalService.CreateEntity(testAnimal);
            mainDataService.AnimalService.MakeRiport(testAnimal);
            Assert.That(Directory.Exists(Path.Combine("Riports", $"{testAnimal.Id}_Jancsi")), Is.EqualTo(true));
            mainDataService.AnimalService.DeleteEntity(testAnimal);
            if (mainDataService.wasCreated)
            {
                mainDataService.DeleteAll();
            }
        }


        [Test]
        public void AddAnimalTest()
        {
            /*AnimalKeepingDbContext ctx = new AnimalKeepingDbContext();
            ctx.Delete();*/
            IAnimalService? animalService = null;
            IFoodService? foodService = null;
            IActivityService? activityService = null;
            IMainService? mainDataService = null;
            TestInit(ref animalService, ref foodService, ref activityService, ref mainDataService);

            Animal testAnimal = new Animal("Jancsi",Genders.male, 2, "sas");
            mainDataService.AnimalService.CreateEntity(testAnimal);
            Assert.That(mainDataService.AnimalService.ListEntities().Contains(testAnimal), Is.EqualTo(true));
            mainDataService.AnimalService.DeleteEntity(testAnimal);
            if (mainDataService.wasCreated)
            {
                mainDataService.DeleteAll();
            }
        }

        [Test]
        public void AddFoodTest()
        {
            IAnimalService? animalService = null;
            IFoodService? foodService = null;
            IActivityService? activityService = null;
            IMainService? mainDataService = null;
            TestInit(ref animalService, ref foodService, ref activityService, ref mainDataService);

            Food testKeeper = new Food("Jancsi",2);
            mainDataService.FoodService.CreateEntity(testKeeper);
            Assert.That(mainDataService.FoodService.ListEntities().Contains(testKeeper), Is.EqualTo(true));
            mainDataService.FoodService.DeleteEntity(testKeeper);
            if (mainDataService.wasCreated)
            {
                mainDataService.DeleteAll();
            }
        }

        
        [Test]
        public void AddActivityTest()
        {
            IAnimalService? animalService = null;
            IFoodService? foodService = null;
            IActivityService? activityService = null;
            IMainService? mainDataService = null;
            TestInit(ref animalService, ref foodService, ref activityService, ref mainDataService);

            Animal testAnimal = new Animal("Jancsi", Genders.male, 2, "sas");
            mainDataService.AnimalService.CreateEntity(testAnimal);
            Food testKeeper = new Food("Jancsi", 2);
            mainDataService.FoodService.CreateEntity(testKeeper);

            Activity testActivity = new Activity(new DateTime(2020, 10, 10),"Feed the animal","Feeding", testAnimal.Id, testKeeper.Id);
            mainDataService.ActivityService.CreateEntity(testActivity);

            Assert.That(mainDataService.ActivityService.ListEntities().Contains(testActivity), Is.EqualTo(true));

            mainDataService.ActivityService.DeleteEntity(testActivity);
            mainDataService.AnimalService.DeleteEntity(testAnimal);
            mainDataService.FoodService.DeleteEntity(testKeeper);

            if (mainDataService.wasCreated)
            {
                mainDataService.DeleteAll();
            }

        }


        [Test]
        public void DeleteActivityTest()
        {
            IAnimalService? animalService = null;
            IFoodService? foodService = null;
            IActivityService? activityService = null;
            IMainService? mainDataService = null;
            TestInit(ref animalService, ref foodService, ref activityService, ref mainDataService);

            Animal testAnimal = new Animal("Jancsi", Genders.male, 2, "sas");
            mainDataService.AnimalService.CreateEntity(testAnimal);
            Food testKeeper = new Food("Jancsi", 2);
            mainDataService.FoodService.CreateEntity(testKeeper);

            Activity testActivity = new Activity(new DateTime(2020, 10, 10), "Feed the animal", "Feeding", testAnimal.Id, testKeeper.Id);
            mainDataService.ActivityService.CreateEntity(testActivity);


            mainDataService.ActivityService.DeleteEntity(testActivity);
            Assert.That(mainDataService.ActivityService.ListEntities().Contains(testActivity), Is.EqualTo(false));



            mainDataService.AnimalService.DeleteEntity(testAnimal);
            mainDataService.FoodService.DeleteEntity(testKeeper);

            if (mainDataService.wasCreated)
            {
                mainDataService.DeleteAll();
            }

        }

        [Test]
        public void DeleteAnimalTest()
        {
            IAnimalService? animalService = null;
            IFoodService? foodService = null;
            IActivityService? activityService = null;
            IMainService? mainDataService = null;
            TestInit(ref animalService, ref foodService, ref activityService, ref mainDataService);

            Animal testAnimal = new Animal("Jancsi", Genders.male, 2, "sas");
            mainDataService.AnimalService.CreateEntity(testAnimal);
            //Animal testAnimal = mainDataService.AnimalMethods.ListEntities().FirstOrDefault(x => (x.Name == "Jancsi" && x.Gender == Genders.male && x.Age == 2 && x.Species == "sas"));
            mainDataService.AnimalService.DeleteEntity(testAnimal);
            Assert.That(mainDataService.AnimalService.ListEntities().Contains(testAnimal), Is.EqualTo(false));

            if (mainDataService.wasCreated)
            {
                mainDataService.DeleteAll();
            }
        }

        [Test]
        public void DeleteFoodTest()
        {
            IAnimalService? animalService = null;
            IFoodService? foodService = null;
            IActivityService? activityService = null;
            IMainService? mainDataService = null;
            TestInit(ref animalService, ref foodService, ref activityService, ref mainDataService);

            Food testKeeper = new Food("Jancsi", 2);
            mainDataService.FoodService.CreateEntity(testKeeper);
            mainDataService.FoodService.DeleteEntity(testKeeper);
            Assert.That(mainDataService.FoodService.ListEntities().Contains(testKeeper), Is.EqualTo(false));

            if (mainDataService.wasCreated)
            {
                mainDataService.DeleteAll();
            }
        }

        [Test]
        public void UpdateAnimalTest()
        {
            IAnimalService? animalService = null;
            IFoodService? foodService = null;
            IActivityService? activityService = null;
            IMainService? mainDataService = null;
            TestInit(ref animalService, ref foodService, ref activityService, ref mainDataService);

            Animal testAnimal = new Animal("Jancsi", Genders.male, 2, "sas");
            mainDataService.AnimalService.CreateEntity(testAnimal);

            Animal newAnimal = new Animal("Joska", Genders.male, 2, "sas");
            mainDataService.AnimalService.UpdateEntity(testAnimal, newAnimal);

            Assert.That(mainDataService.AnimalService.SearchById(testAnimal.Id).Name, Is.EqualTo("Joska"));

            mainDataService.AnimalService.DeleteEntity(testAnimal);

            if (mainDataService.wasCreated)
            {
                mainDataService.DeleteAll();
            }
        }

        [Test]
        public void UpdateFoodTest()
        {
            IAnimalService? animalService = null;
            IFoodService? foodService = null;
            IActivityService? activityService = null;
            IMainService? mainDataService = null;
            TestInit(ref animalService, ref foodService, ref activityService, ref mainDataService);

            Food testKeeper = new Food("Jancsi", 2);
            mainDataService.FoodService.CreateEntity(testKeeper);

            Food newKeeper = new Food("Joska", 2);
            mainDataService.FoodService.UpdateEntity(testKeeper, newKeeper);

            Assert.That(mainDataService.FoodService.SearchById(testKeeper.Id).Name, Is.EqualTo("Joska"));

            mainDataService.FoodService.DeleteEntity(testKeeper);

            if (mainDataService.wasCreated)
            {
                mainDataService.DeleteAll();
            }
        }
    }
}
