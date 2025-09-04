using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public enum Genders
{
    male,
    female
}


namespace C2INIS_HSZF_2024251.Model
{
    public class Animal : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }
        public Genders Gender { get; set; }
        public int Age { get; set; }

        [StringLength(100)]
        public string Species { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }


        public Animal(string name, Genders gender, int age, string species)
        {
            Name = name;
            Gender = gender;
            Age = age;
            Species = species;
            Activities = new HashSet<Activity>();
        }
        public Animal()
        {
            Activities = new HashSet<Activity>();
        }
        public override string ToString()
        {
            return $"Id: {Id}\tName: {Name}\tGender: {Gender}\tAge: {Age}\tSpecies: {Species}";
        }


    }
}
