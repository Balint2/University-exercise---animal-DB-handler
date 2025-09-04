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
    public class ActivityDataProvider : IActivityDataProvider
    {
        private readonly AppDbContext context;

        public ActivityDataProvider(AppDbContext context)
        {
            this.context = context;
        }



        public void ReadFromJSON()
        {
            /*context.Activities.RemoveRange(context.Activities);
            context.SaveChanges();*/

            List<Activity>? activities = JsonConvert.DeserializeObject<List<Activity>>(File.ReadAllText(Path.Combine("Inputs", "Activities.json")));
            context.Activities.AddRange(activities);
            context.SaveChanges();
        }



        public void CreateEntity(Activity activity)
        {
            context.Activities.Add(activity);
            context.SaveChanges();
        }

        public Activity DeleteEntity(Activity activityToDelete)
        {
            context.Activities.Remove(activityToDelete);
            context.SaveChanges();
            return activityToDelete;
        }

        public IEnumerable<Activity> ReadAllEntity()
        {
            return context.Activities;
        }

        public Activity Update(Activity oldActivity, Activity newActivity)
        {

            oldActivity.Date = newActivity.Date;
            oldActivity.Comment = newActivity.Comment;
            oldActivity.Name = newActivity.Name;

            oldActivity.AnimalId = newActivity.AnimalId;
            oldActivity.FoodId = newActivity.FoodId;

            /*oldActivity.Animal = DBManager.ctx.Animals.FirstOrDefault(x => x.Id == newActivity.AnimalId);
            oldActivity.Keeper = DBManager.ctx.Keepers.FirstOrDefault(x => x.Id == newActivity.KeeperId);*/

            context.SaveChanges();

            return oldActivity;
        }



        public IEnumerable<Activity> SearchByName(string keyWord)
        {
            //return DBManager.ActivityDBManager.SearchByKeeper(keyWord);
            return context.Activities.Where(x => x.Name.Contains(keyWord));
        }

        public IEnumerable<Activity> ReOrder<T>(Func<Activity, T> keySelector, bool ascending = true)
        {
            return ascending
                ? context.Activities.OrderBy(keySelector).ToList()
                : context.Activities.OrderByDescending(keySelector).ToList();

        }

        public Activity SearchById(int id)
        {
            return context.Activities.FirstOrDefault(a => a.Id == id);
        }
    }
}
