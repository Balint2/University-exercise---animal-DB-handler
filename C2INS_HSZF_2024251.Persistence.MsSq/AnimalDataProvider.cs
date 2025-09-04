using C2INIS_HSZF_2024251.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2INIS_HSZF_2024251.Persistence.MsSql
{
    public class AnimalDataProvider : IAnimalDataProvider
    {
        private readonly AppDbContext context;

        public AnimalDataProvider(AppDbContext context)
        {
            this.context = context;
        }



        public void ReadFromJSON()
        {

            /*context.Animals.RemoveRange(context.Animals);
            context.SaveChanges();*/


            //List<Animal>? tesztlista = context.Animals.ToList();
            List<Animal>? animals = JsonConvert.DeserializeObject<List<Animal>>(File.ReadAllText(Path.Combine("Inputs", "Animals.json")));
            context.Animals.AddRange(animals);
            context.SaveChanges();

        }



        public void CreateEntity(Animal animal)
        {
            context.Animals.Add(animal);
            context.SaveChanges();
        }

        public Animal DeleteEntity(Animal animalToDelete)
        {
            context.Animals.Remove(animalToDelete);
            context.SaveChanges();
            return animalToDelete;
        }

        public IEnumerable<Animal> ReadAllEntity()
        {
            return context.Animals;
        }

        public Animal Update(Animal oldAnimal, Animal newAnimal)
        {

            oldAnimal.Name = newAnimal.Name;
            oldAnimal.Gender = newAnimal.Gender;
            oldAnimal.Age = newAnimal.Age;
            oldAnimal.Species = newAnimal.Species;


            context.SaveChanges();

            return oldAnimal;
        }

        public Animal SearchById(int id)
        {
            return context.Animals.FirstOrDefault(a => a.Id == id);
        }
    }
}
