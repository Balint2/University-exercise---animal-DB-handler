using Azure;
using C2INIS_HSZF_2024251.Application;
using C2INIS_HSZF_2024251.Model;
using C2INIS_HSZF_2024251.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace C2INIS_HSZF_2024251.Console
{
    public class MenuHandler
    {
        //DBMethods dBReacher;
        string keyWord = "";
        Action actFunction;
        bool orderDate = false;
        bool descend = false;
        public event Action<Food, string> LittleFoodAction;


        IHost? host;
        IAnimalService animalService;
        IFoodService foodService;
        IActivityService activityService;
        IMainService mainDataService;



        #region startMethods
        public MenuHandler()
        {
            LittleFoodAction += warningMessage;

            host = Host.CreateDefaultBuilder().ConfigureLogging(logging =>
            {
                logging.ClearProviders(); 
                logging.AddDebug();
            }).ConfigureServices((hostContext, services) =>
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
            mainDataService.Init(animalService, foodService, activityService, host.Services.GetRequiredService<IAnimalDataProvider>(), host.Services.GetRequiredService<IFoodDataProvider>(), host.Services.GetRequiredService<IActivityDataProvider>(), true);

            //dBReacher = new DBMethods(true);
            MainMenu();
        }


        void MainMenu()
        {
            Menu menu = new Menu("Main menu", "", new MenuItem[7] { new MenuItem("List", ListMenu), new MenuItem("Add something", AddMenu), new MenuItem("Delete something", DeleteMenu), new MenuItem("Update properties", UpdateMenu), new MenuItem("Make riports", RiportMenu), new MenuItem("Reset the database", Reset), new MenuItem("Quit", Exit) });
            menu.Loop().Function();
        }
        #endregion



        #region MainFunctions
        void ListMenu()
        {
            Menu menu = new Menu("Listing menu", "Which one do you want to list?", ListTypes(ListAnimals, ListKeepers, ListActivities));
            menu.Loop().Function();
        }
        void AddMenu()
        {
            Menu menu = new Menu("Adding menu", "What do you want to add?", ListTypes(AddAnimal, AddKeeper, AddActivity));
            menu.Loop().Function();
        }
        void DeleteMenu()
        {
            Menu menu = new Menu("Deleting menu", "What do you want to delete?", ListTypes(DeleteAnimal, DeleteKeeper, DeleteActivity));
            menu.Loop().Function();
        }
        void UpdateMenu()
        {
            Menu menu = new Menu("Updating menu", "What do you want to update?", ListTypes(UpdateAnimal, UpdateKeeper, UpdateActivity));
            menu.Loop().Function();
        }
        void RiportMenu()
        {
            Menu menu = new Menu("Riporting menu", "What do you want to make a riport about?", new MenuItem[3] { new MenuItem("Animals", RiportAnimal), new MenuItem("Foods", RiportKeeper), new MenuItem("Cancel", MainMenu) });
            menu.Loop().Function();
        }
        void Reset()
        {
            bool answer = AskConfirm("Do you really want to reset the whole database?","","You successfully reseted the whole database!");
            if (answer == true)
            {
                mainDataService.Reset();
            }
            MainMenu();
        }


        #endregion





        #region helperFunctions


        MenuItem[] ListTypes(Action animalFunction, Action keeperFunction, Action activityFunction)
        {
            //meg kell csinálni, hogy a függvényeket megkapja paraméterül
            keyWord = "";
            orderDate = false;
            descend = false;
            return new MenuItem[4] { new MenuItem("Animals", animalFunction), new MenuItem("Foods", keeperFunction), new MenuItem("Activities", activityFunction), new MenuItem("Cancel", MainMenu) };
        }
        MenuItem[] ConvertToItems(IEnumerable<Model.Entity> source, Action action)
        {
            //meg kell csinálni, hogy a függvényeket megkapja paraméterül
            MenuItem[] menuItems = new MenuItem[source.Count()];
            int i = 0;
            foreach (var item in source)
            {
                menuItems[i] = new MenuItem(item.ToString(), action, item.Id);
                i++;
            }
            return menuItems;
        }

        void WriteSuccess(Model.Entity entity, string welcomeString)
        {
            System.Console.Clear();

            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine(welcomeString + " Press enter to continue!");
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.WriteLine(entity.ToString());

            ConsoleKeyInfo consoleKey;
            do
            {
                consoleKey = System.Console.ReadKey(true);
            } while (consoleKey.Key != ConsoleKey.Enter);
        }


        bool AskConfirm(string question, string entityDatas, string confirmString)
        {
            bool agree = false;
            System.Console.Clear();

            Menu menu = new Menu("Confirm menu", $"{question}\n{entityDatas}", new MenuItem[2] { new MenuItem("Yes", Empty), new MenuItem("No", Empty) });
            if (menu.Loop().Text == "Yes")
            {
                agree = true;
                System.Console.Clear();
                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine(confirmString + " Press enter to continue!");
                System.Console.ForegroundColor = ConsoleColor.White;

                System.Console.WriteLine(entityDatas);

                ConsoleKeyInfo consoleKey;
                do
                {
                    consoleKey = System.Console.ReadKey(true);
                } while (consoleKey.Key != ConsoleKey.Enter);

            }
            return agree;
        }

        void warningMessage(Food food, string warningMesage)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine(warningMesage);
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.WriteLine("Press enter to continue!");

            ConsoleKeyInfo consoleKey;
            do
            {
                consoleKey = System.Console.ReadKey(true);
            } while (consoleKey.Key != ConsoleKey.Enter);
        }


        IEnumerable<Model.Activity> ActivitySequence()
        {
            IEnumerable<Model.Activity> List;
            if (keyWord == "")
            {
                if (orderDate)
                {
                    if (descend)
                    {
                        List = activityService.ReOrder(x => x.Date, false);
                    }
                    else
                    {
                        List = activityService.ReOrder(x => x.Date, true);
                    }
                }
                else
                {
                    if (descend)
                    {
                        List = activityService.ReOrder(x => x.Name, false);
                    }
                    else
                    {
                        List = activityService.ReOrder(x => x.Name, true);
                    }
                }
            }
            else
                List = activityService.SearchByName(keyWord);
            return List;
        }
        #endregion

        #region inputEntities

        Animal InputAnimal(string oldString = null)
        {

            System.Console.Clear();
            if (oldString != null)
            {
                System.Console.WriteLine(oldString);
                System.Console.WriteLine();
            }

            System.Console.WriteLine("New animal: ");
            string inputString;
            bool correctData;

            System.Console.Write("-Name: ");
            string name = System.Console.ReadLine();

            Genders gender;
            do
            {
                System.Console.Write("-Gender (male/female): ");
                inputString = System.Console.ReadLine();
                correctData = Enum.TryParse<Genders>(inputString, out gender);
                if (!correctData)
                {
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine("Wrong format");
                    System.Console.ForegroundColor = ConsoleColor.White;
                }
            } while (!correctData);




            int age;
            do
            {
                System.Console.Write("-Age (whole number): ");
                inputString = System.Console.ReadLine();
                correctData = int.TryParse(inputString, out age);
                if (!correctData)
                {
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine("Wrong format");
                    System.Console.ForegroundColor = ConsoleColor.White;
                }
            } while (!correctData);

            System.Console.Write("-Species: ");
            string species = System.Console.ReadLine();

            Animal newAnimal = new Animal(name, gender, age, species);
            return newAnimal;
        }

        Food InputFood(string oldString = null)
        {
            System.Console.Clear();
            if (oldString != null)
            {
                System.Console.WriteLine(oldString);
                System.Console.WriteLine();
            }
            System.Console.WriteLine("New food: ");
            string inputString;

            System.Console.Write("-Name: ");
            string name = System.Console.ReadLine();

            bool correctData;

            int quantity;
            do
            {
                System.Console.Write("-Quantity (whole number): ");
                inputString = System.Console.ReadLine();
                correctData = int.TryParse(inputString, out quantity);
                if (!correctData)
                {
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine("Wrong format");
                    System.Console.ForegroundColor = ConsoleColor.White;
                }
            } while (!correctData);

            Food newKeeper = new Food(name, quantity);
            return newKeeper;
        }

        Model.Activity InputActivity(string oldString = null)
        {
            System.Console.Clear();
            if (oldString != null)
            {
                System.Console.WriteLine(oldString);
                System.Console.WriteLine();
            }
            System.Console.WriteLine("New activity: ");
            string inputString;
            bool correctData;


            System.Console.Write("-Type: ");
            string name = System.Console.ReadLine();

            DateTime date;



            do
            {
                System.Console.Write("-Date (YYYY.MM.DD): ");
                inputString = System.Console.ReadLine();
                correctData = DateTime.TryParse(inputString, out date);
                if (!correctData)
                {
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine("Wrong format");
                    System.Console.ForegroundColor = ConsoleColor.White;
                }
            } while (!correctData);

            System.Console.Write("-Comment: ");
            string comment = System.Console.ReadLine();


            //actFunction = ListAnimals;
            var AnimalList = animalService.ListEntities();
            ListingMenu animalMenu = new ListingMenu("Select an animal", "Select an animal to add to the activity", ConvertToItems(AnimalList, Empty), new MenuItem[0]);
            int animalId = animalMenu.Loop().Id;

            Model.Activity newActivity;
            if (name == "Feeding")
            {
                bool success = false;
                int keeperId;
                do
                {
                    var KeeperList = foodService.ListEntities();
                    ListingMenu keeperMenu = new ListingMenu("Select a food", "Select a food to add to the activity", ConvertToItems(KeeperList, Empty), new MenuItem[0]);
                    keeperId = keeperMenu.Loop().Id;

                    Food newFood = foodService.SearchById(keeperId);
                    try
                    {
                        foodService.Reduce(newFood);
                        if (newFood.Quantity < 3)
                        {
                            LittleFoodAction?.Invoke(newFood, $"There are very little foods from {newFood.Name}!");
                        }
                        success = true;
                    }
                    catch
                    {
                        LittleFoodAction?.Invoke(newFood, $"There are no foods from {newFood.Name}! Please select another food!");
                        success = false;
                    }
                } while (!success);




                newActivity = new Model.Activity(date, comment, name, animalId, keeperId);
            }
            else
            {
                newActivity = new Model.Activity(date, comment, name, animalId);
            }

            return newActivity;
        }
        #endregion

        #region ListMethods
        void ListAnimals()
        {
            actFunction = ListAnimals;
            var List = animalService.ListEntities();
            ListingMenu menu = new ListingMenu("Listing animals", "You can turn the page by left and right arrows", ConvertToItems(List, ListAnimals), new MenuItem[1] { new MenuItem("Cancel", ListMenu) });
            menu.Loop().Function();
        }
        void ListKeepers()
        {
            actFunction = ListKeepers;
            var List = foodService.ListEntities();
            ListingMenu menu = new ListingMenu("Listing foods", "You can turn the page by left and right arrows", ConvertToItems(List, ListKeepers), new MenuItem[1] { new MenuItem("Cancel", ListMenu) });
            menu.Loop().Function();
        }
        void ListActivities()
        {
            actFunction = ListActivities;

            IEnumerable<Model.Activity> List = ActivitySequence();



            ListingMenu menu = new ListingMenu("Listing activities", "You can turn the page by left and right arrows", ConvertToItems(List, ListActivities), new MenuItem[3] { new MenuItem("Cancel", ListMenu), new MenuItem("Search", SearchByName), new MenuItem("Order", ActivityOrderMenu) }, 4);
            menu.Loop().Function();
        }

        #endregion

        #region addMethods
        void AddAnimal()
        {
            Animal newAnimal = InputAnimal();

            animalService.CreateEntity(newAnimal);
            bool answer = AskConfirm("Do you really want to add the animal?", newAnimal.ToString(), "You successfully added the animal!");
            if (answer == false)
            {
                animalService.DeleteEntity(newAnimal);
            }

            AddMenu();
        }

        void AddKeeper()
        {
            Food newKeeper = InputFood();
            foodService.CreateEntity(newKeeper);
            bool answer = AskConfirm("Do you really want to add the food?", newKeeper.ToString(), "You successfully added the food!");
            if (answer == false)
            {
                foodService.DeleteEntity(newKeeper);
            }

            AddMenu();
        }

        void AddActivity()
        {

            Model.Activity newActivity = InputActivity();


            activityService.CreateEntity(newActivity);
            bool answer = AskConfirm("Do you really want to add the activity?", newActivity.ToString(), "You successfully added the activity!");
            if (answer == false)
            {
                activityService.DeleteEntity(newActivity);
            }
            /*else
            {
                if (activityService.LessThanThree(newActivity))
                {
                    tooLittleFoodAction?.Invoke();
                }
            }*/
            
            AddMenu();
        }

        #endregion


        #region DeleteMethods

        void DeleteAnimal()
        {
            var AnimalList = animalService.ListEntities();
            ListingMenu animalMenu = new ListingMenu("Select an animal", "Select an animal that you want to delete!", ConvertToItems(AnimalList, Empty), new MenuItem[1] { new MenuItem("Cancel", Empty) });
            int animalId = animalMenu.Loop().Id;
            if (animalId != null && animalId != 0) // is not cancel
            {
                Animal deletedAnimal = animalService.SearchById(animalId);
                bool answer = AskConfirm("Do you really want to delete the animal?",deletedAnimal.ToString(),"You successfully deleted the animal!");
                if (answer == true)
                {
                    animalService.DeleteEntity(deletedAnimal);
                }
            }

            DeleteMenu();
        }

        void DeleteKeeper()
        {
            var KeeperList = foodService.ListEntities();


            ListingMenu keeperMenu = new ListingMenu("Select a food", "Select a food that you want to delete!", ConvertToItems(KeeperList, Empty /* ez nem fut le*/), new MenuItem[1] { new MenuItem("Cancel", Empty) });
            int keeperId = keeperMenu.Loop().Id;
            if (keeperId != null && keeperId != 0) //is not cancel
            {
                Food deletedKeeper = foodService.SearchById(keeperId);
                bool answer = AskConfirm("Do you really want to delete the food?", deletedKeeper.ToString(), "You successfully deleted the food!");
                if (answer == true)
                {
                    foodService.DeleteEntity(deletedKeeper);
                }
            }

            DeleteMenu();
        }

        void DeleteActivity()
        {
            var ActivityList = ActivitySequence();
            actFunction = DeleteActivity;



            ListingMenu activityMenu = new ListingMenu("Select an activity", "Select an activity that you want to delete!", ConvertToItems(ActivityList, Empty /* ez nem fut le*/), new MenuItem[3] { new MenuItem("Cancel", ListMenu), new MenuItem("Search", SearchByName), new MenuItem("Order", ActivityOrderMenu) }, 4);

            MenuItem selecetedActivity = activityMenu.Loop();
            int activityId = selecetedActivity.Id;
            
            if (activityId != null && activityId != 0) //is not cancel
            {
                Model.Activity deletedActivity = activityService.SearchById(activityId);
                bool answer = AskConfirm("Do you really want to delete the activity?", deletedActivity.ToString(), "You successfully deleted the activity!");
                if (answer == true)
                {
                    activityService.DeleteEntity(deletedActivity);
                }
            }
            else if (selecetedActivity.Function == SearchByName || selecetedActivity.Function == ActivityOrderMenu)
            {
                selecetedActivity.Function();
            }
            DeleteMenu();


        }

        #endregion


        #region UpdateMethods

        void UpdateAnimal()
        {
            var AnimalList = animalService.ListEntities();
            ListingMenu animalMenu = new ListingMenu("Select an animal", "Select an animal that you want to update!", ConvertToItems(AnimalList, Empty), new MenuItem[1] { new MenuItem("Cancel", Empty) });
            int animalId = animalMenu.Loop().Id;



            if (animalId != null && animalId != 0)
            {
                Animal oldAnimal = animalService.SearchById(animalId);
                string oldString = oldAnimal.ToString();

                /*System.Console.Clear();
                System.Console.WriteLine(oldString);
                System.Console.WriteLine();
                System.Console.WriteLine("New datas: ");
                string inputString;
                bool correctData;

                System.Console.Write("-Name: ");
                string name = System.Console.ReadLine();

                Genders gender;
                do
                {
                    System.Console.Write("-Gender (male/female): ");
                    inputString = System.Console.ReadLine();
                    correctData = Enum.TryParse<Genders>(inputString, out gender);
                    if (!correctData)
                    {
                        System.Console.ForegroundColor = ConsoleColor.Red;
                        System.Console.WriteLine("Wrong format");
                        System.Console.ForegroundColor = ConsoleColor.White;
                    }
                } while (!correctData);




                int age;
                do
                {
                    System.Console.Write("-Age (whole number): ");
                    inputString = System.Console.ReadLine();
                    correctData = int.TryParse(inputString, out age);
                    if (!correctData)
                    {
                        System.Console.ForegroundColor = ConsoleColor.Red;
                        System.Console.WriteLine("Wrong format");
                        System.Console.ForegroundColor = ConsoleColor.White;
                    }
                } while (!correctData);

                System.Console.Write("-Species: ");
                string species = System.Console.ReadLine();*/




                Animal newAnimal = InputAnimal(oldString);
                newAnimal.Id = oldAnimal.Id;


                bool answer = AskConfirm("Do you really want to update the animal?", $"From:\n{oldString}\nTo:\n{newAnimal.ToString()}", "You successfully updated the animal!");
                if (answer == true)
                {
                    animalService.UpdateEntity(oldAnimal, newAnimal);
                }

            }

            UpdateMenu();
        }

        void UpdateKeeper()
        {
            var KeeperList = foodService.ListEntities();
            ListingMenu keeperMenu = new ListingMenu("Select a food", "Select a food that you want to update!", ConvertToItems(KeeperList, Empty), new MenuItem[1] { new MenuItem("Cancel", Empty) });
            int keeperId = keeperMenu.Loop().Id;



            if (keeperId != null && keeperId != 0)
            {
                Food oldKeeper = foodService.SearchById(keeperId);
                string oldString = oldKeeper.ToString();

                /*System.Console.Clear();
                System.Console.WriteLine(oldString);
                System.Console.WriteLine();
                System.Console.WriteLine("New datas: ");
                string inputString;
                bool correctData;

                System.Console.Write("-Name: ");
                string name = System.Console.ReadLine();*/



                Food newKeeper = InputFood(oldString);
                newKeeper.Id = oldKeeper.Id;


                bool answer = AskConfirm("Do you really want to update the food?", $"From:\n{oldString}\nTo:\n{newKeeper.ToString()}", "You successfully updated the food!");
                if (answer == true)
                {
                    foodService.UpdateEntity(oldKeeper, newKeeper);
                }

            }

            UpdateMenu();
        }



        void UpdateActivity()
        {
            var ActivityList = ActivitySequence();
            actFunction = UpdateActivity;


            ListingMenu activityMenu = new ListingMenu("Select an activity", "Select an activity that you want to update!", ConvertToItems(ActivityList, Empty), new MenuItem[3] { new MenuItem("Cancel", ListMenu), new MenuItem("Search", SearchByName), new MenuItem("Order", ActivityOrderMenu) }, 4);


            MenuItem selecetedActivity = activityMenu.Loop();
            int activityId = selecetedActivity.Id;



            if (activityId != null && activityId != 0)
            {
                Model.Activity oldActivity = activityService.SearchById(activityId);
                string oldString = oldActivity.ToString();

                /*System.Console.Clear();
                System.Console.WriteLine(oldString);
                System.Console.WriteLine();
                System.Console.WriteLine("New datas: ");
                string inputString;
                bool correctData;


                System.Console.Write("-Name: ");
                string name = System.Console.ReadLine();

                DateTime date;



                do
                {
                    System.Console.Write("-Date (YYYY.MM.DD): ");
                    inputString = System.Console.ReadLine();
                    correctData = DateTime.TryParse(inputString, out date);
                    if (!correctData)
                    {
                        System.Console.ForegroundColor = ConsoleColor.Red;
                        System.Console.WriteLine("Wrong format");
                        System.Console.ForegroundColor = ConsoleColor.White;
                    }
                } while (!correctData);

                System.Console.Write("-Comment: ");
                string comment = System.Console.ReadLine();


                //actFunction = ListAnimals;
                var AnimalList = animalService.ListEntities();
                ListingMenu animalMenu = new ListingMenu("Select an animal", $"Select an animal to update to the activity\n{oldString}", ConvertToItems(AnimalList, Empty), new MenuItem[0]);
                int animalId = animalMenu.Loop().Id;


                var KeeperList = foodService.ListEntities();
                ListingMenu keeperMenu = new ListingMenu("Select a food", $"Select a food to update to the activity\n{oldString}", ConvertToItems(KeeperList, Empty), new MenuItem[0]);
                int keeperId = keeperMenu.Loop().Id;*/


                Model.Activity newActivity = InputActivity(oldString);
                newActivity.Id = oldActivity.Id;


                activityService.UpdateEntity(oldActivity, newActivity);
                bool answer = AskConfirm("Do you really want to update the activity?", $"From:\n{oldString}\nTo:\n{oldActivity.ToString()}", "You successfully updated the activity!");
                if (answer == false)
                {
                    activityService.UpdateEntity(newActivity, oldActivity);
                }
                /*else
                {
                    if (activityService.LessThanThree(oldActivity))
                    {
                        //tooLittleFoodAction?.Invoke();
                    }
                }*/

            }
            else if (selecetedActivity.Function == SearchByName || selecetedActivity.Function == ActivityOrderMenu)
            {
                selecetedActivity.Function();
            }

            UpdateMenu();
        }
        #endregion

        #region RiportMethods

        void RiportAnimal()
        {
            var AnimalList = animalService.ListEntities();
            ListingMenu animalMenu = new ListingMenu("Select an animal", "Select an animal that you want to make a riport about!", ConvertToItems(AnimalList, Empty), new MenuItem[1] { new MenuItem("Cancel", Empty) });
            int animalId = animalMenu.Loop().Id;
            if (animalId != null && animalId != 0) // is not cancel
            {
                Animal reportedAnimal = animalService.SearchById(animalId);
                bool answer = AskConfirm("Do you really want to make a riport about the animal?", reportedAnimal.ToString(), "You successfully made a riport about the animal!");
                if (answer == true)
                {
                    animalService.MakeRiport(reportedAnimal);
                }
            }

            RiportMenu();
        }

        void RiportKeeper()
        {
            //Food reportedKeeper = foodService.SearchById(keeperId);
            bool answer = AskConfirm("Do you really want to make a riport about all food?", "", "You successfully made a riport about all the food!");
            if (answer == true)
            {
                foodService.MakeRiport();
            }
            RiportMenu();
            /*var KeeperList = foodService.ListEntities();
            ListingMenu animalMenu = new ListingMenu("Select a food", "Select a food that you want to make a riport about!", ConvertToItems(KeeperList, Empty), new MenuItem[1] { new MenuItem("Cancel", Empty) });
            int keeperId = animalMenu.Loop().Id;
            if (keeperId != null && keeperId != 0) // is not cancel
            {

            }*/


        }


        #endregion

        #region SearchAndOrder
        void SearchByName()
        {
            System.Console.ForegroundColor = ConsoleColor.Magenta;
            System.Console.WriteLine();
            System.Console.Write("Type: ");
            keyWord = System.Console.ReadLine();
            System.Console.ForegroundColor = ConsoleColor.White;
            actFunction();
        }
        void ActivityOrderMenu()
        {
            Menu menu = new Menu("Order menu", "According to what you want to arrange?", new MenuItem[4] { new MenuItem("Ascending by date", OrderAcendByDate), new MenuItem("Descending by date", OrderDescendByDate), new MenuItem("Ascending by type", OrderAcendByType), new MenuItem("Descending by type", OrderDescendByType) });
            menu.Loop().Function();
        }
        void OrderAcendByDate()
        {
            descend = false;
            orderDate = true;
            actFunction();
        }
        void OrderDescendByDate()
        {
            descend = true;
            orderDate = true;
            actFunction();
        }
        void OrderAcendByType()
        {
            descend = false;
            orderDate = false;
            actFunction();
        }
        void OrderDescendByType()
        {
            descend = true;
            orderDate = false;
            actFunction();
        }
        #endregion




        #region Exit
        void Empty()
        {
            System.Console.WriteLine("mit nyomogatsz?");
        }
        void Exit()
        {
            //System.Diagnostics.Process.GetCurrentProcess().Kill();
            Environment.Exit(0);
            //exitFromMenu = true;
        }
        #endregion
    }
}
