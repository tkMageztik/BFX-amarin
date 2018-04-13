using NS.MBX_amarin.Model.Gplaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace NS.MBX_amarin.Services
{
    public interface IGPlacesService
    {
        Task<ObservableCollection<AutoCompletePrediction>> GetPlaces(string newTextValue);
    }
}
