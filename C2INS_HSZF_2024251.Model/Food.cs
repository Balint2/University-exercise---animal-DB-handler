using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace C2INIS_HSZF_2024251.Model
{
    public class Food : Entity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(100)]

        public string Name { get; set; }

        public int Quantity { get; set; }

        public virtual ICollection<Activity> Activities { get; set; }

        public Food(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
            Activities = new HashSet<Activity>();

        }
        public Food()
        {
            Activities = new HashSet<Activity>();
        }
        public override string ToString()
        {
            return $"Id: {Id}\tName: {Name}\tQuantity: {Quantity}";
        }
    }
}
