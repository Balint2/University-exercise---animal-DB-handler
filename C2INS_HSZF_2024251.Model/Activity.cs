using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace C2INIS_HSZF_2024251.Model
{
    public class Activity : Entity
    {
        /*static public List<Animal> Animals { get; set; }
        static public List<Keeper> Keepers { get; set; }*/


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        [StringLength(500)]
        public string Comment { get; set; }
        [StringLength(100)]
        public string Name { get; set; }

        public int AnimalId { get; set; }
        public virtual Animal Animal { get; set; }
        public int? FoodId { get; set; }
        public virtual Food? Food { get; set; }

        public Activity(DateTime date, string comment, string name, int animalId, int keeperId)
        {
            Date = date;
            Comment = comment;
            Name = name;
            AnimalId = animalId;
            if (name == "Feeding")
                FoodId = keeperId;
        }

        public Activity(DateTime date, string comment, string name, int animalId)
        {
            Date = date;
            Comment = comment;
            Name = name;
            AnimalId = animalId;
        }

        public Activity()
        {
        }

        public override string ToString()
        {
            //az adatbázisból kéne valahogy kikeresni
            if (Name == "Feeding")
                return $"Id: {Id}\tType: {Name}\tDate: {Date}\tComment: {Comment}\n---Animal: ({Animal?.ToString()})\n---Food: ({Food?.ToString()})\n";
            else
                return $"Id: {Id}\tType: {Name}\tDate: {Date}\tComment: {Comment}\n---Animal: ({Animal?.ToString()})\n\n";
        }
    }
}
