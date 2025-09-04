using C2INIS_HSZF_2024251.Model;
using C2INIS_HSZF_2024251.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2INIS_HSZF_2024251.Application
{
    public class AnimalService : IAnimalService
    {
        string path;
        private readonly IAnimalDataProvider animalDataProvider;
        public AnimalService(IAnimalDataProvider animalDataProvider)
        {
            this.animalDataProvider = animalDataProvider;
        }


        public void Init(string path)
        {
            this.path = path;
        }




        public Animal CreateEntity(Animal newAnimal)
        {
            animalDataProvider.CreateEntity(newAnimal);
            Directory.CreateDirectory(Path.Combine(path, newAnimal.Id + "_" + newAnimal.Name));
            return newAnimal;
        }
        public Animal DeleteEntity(Animal animal)
        {
            return animalDataProvider.DeleteEntity(animal);
        }
        public Animal UpdateEntity(Animal oldAnimal, Animal newAnimal)
        {
            //Animal newAnimal = new Animal(name, gender, age, species);
            Directory.CreateDirectory(Path.Combine(path, oldAnimal.Id + "_" + newAnimal.Name));
            return animalDataProvider.Update(oldAnimal, newAnimal);
        }
        public IEnumerable<Animal> ListEntities()
        {
            return animalDataProvider.ReadAllEntity();
        }

        public void MakeRiport(Animal animal)
        {
            StreamWriter writer = new StreamWriter(Path.Combine(path, animal.Id + "_" + animal.Name, $"{animal.Name}_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.txt"));

            writer.WriteLine("Animal data: ");
            writer.WriteLine(animal.ToString());


            writer.WriteLine("\nRelated foods: ");
            foreach (var item in animal.Activities.Select(x => x.Food).Distinct())
            {
                if (item != null)
                    writer.WriteLine(item.ToString());
            }

            writer.WriteLine("\nRelated activities: ");
            foreach (var item in animal.Activities)
            {
                if (item != null)
                    writer.WriteLine(item.ToString());
            }

            writer.Close();
        }



        public Animal SearchById(int id)
        {
            return animalDataProvider.SearchById(id);
        }
    }
}
