using System;
using System.Collections.Generic;
using System.Text;

namespace NS.MBX_amarin.Model.Gplaces
{
    public class Components
    {
        private string components;

        /// <summary>
        /// Initialises a new instance of the <see cref="T:DurianCode.PlacesSearchBar.Components"/> class
        /// </summary>
        /// <param name="components">A components string as per the google places API (eg. contry:au|country=nz)</param>
        public Components(string components)
        {
            this.components = components;
        }

        public override string ToString()
        {
            return $"&components={components}";
        }
    }
}
