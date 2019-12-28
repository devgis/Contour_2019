using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DEVGIS.MapAPP.Entities;

namespace DEVGIS.MapAPP
{
    public partial class SelectLayer : Form
    {
        public string LayerName
        {
            get;
            set;
        }

        public string PropertyName
        {
            get;
            set;
        }

        List<LayerItem> layerItems=null;
        public SelectLayer(List<LayerItem> LayerItems )
        {
            InitializeComponent();
            layerItems=LayerItems;
        }
    }
}
