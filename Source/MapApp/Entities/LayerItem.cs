using System;
using System.Collections.Generic;
using System.Text;

namespace DEVGIS.MapAPP.Entities
{
    public class LayerItem
    {
        public string LayerName
        {
            get;
            set;
        }
        public string DisplayName
        {
            get;
            set;
        }

        public List<string> Propertys
        {
            get;
            set;
        }
    }
}
