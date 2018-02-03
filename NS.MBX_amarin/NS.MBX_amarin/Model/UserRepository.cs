using NS.MBX_amarin.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Foundation.ObjectHydrator;

namespace NS.MBX_amarin.Model
{
    public class UserRepository
    {
        public IList<User> Users { get; set; }
        public UserRepository()
        {
            //Hydrator<User> _userHydrator = new Hydrator<User>();
            //Users = _userHydrator.GetList(50);
            Task.Run(async () => Users = await App.Database.GetItemsAsync()).Wait();

        }

        //TODO: Para borrar luego.
        public async Task Delete()
        {
            await App.Database.DeleteItemAsync(Users[0]);
            //Users.Clear();
        }

        public ObservableCollection<Grouping<string, User>> GetAllGrouped()
        {
            IEnumerable<Grouping<string, User>> sorted = new Grouping<string, User>[0];

            if (Users != null)
            {
                sorted =
                   from u in Users
                   orderby u.NroTarjeta
                   group u by u.NroTarjeta[0].ToString()
                   into theGroup
                   select
                   new Grouping<string, User>
                   (theGroup.Key, theGroup);

            }
            return new ObservableCollection<Grouping<string, User>>(sorted);
        }
    }
}
