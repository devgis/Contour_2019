using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DEVGIS.Common;
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

        public int Level
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

        private void SelectLayer_Load(object sender, EventArgs e)
        {
            cbLayers.DataSource = layerItems;
            cbLayers.DisplayMember = "DisplayName";
            cbLayers.ValueMember = "LayerName";
        }

        private void cbLayers_SelectedValueChanged(object sender, EventArgs e)
        {
            cbPropertis.Items.Clear();
            if (cbLayers.SelectedItem != null&&cbLayers.SelectedItem is LayerItem)
            {
                var list = (cbLayers.SelectedItem as LayerItem).Propertys;
                if (list != null && list.Count > 0)
                {
                    foreach (var prop in list)
                    {
                        cbPropertis.Items.Add(prop);
                    }
                }
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            if (cbLayers.SelectedValue != null)
            {
                LayerName = cbLayers.SelectedValue.ToString();
            }
            else
            {
                MessageHelper.ShowError("请选择图层！");
                cbLayers.Focus();
                return;
            }

            if (cbPropertis.Text != null)
            {
                PropertyName = cbPropertis.Text;
            }
            else
            {
                MessageHelper.ShowError("请选择属性！");
                cbPropertis.Focus();
                return;
            }

            Level = (int)nuLevel.Value;
            if (Level < 10 || Level > 100)
            {
                MessageHelper.ShowError("Level为10~100之间的整数！");
                nuLevel.Focus();
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}
