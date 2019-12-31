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
    public partial class SelectProperties : Form
    {
        public string LayerName
        {
            get;
            set;
        }

        public List<string> PropertyNames
        {
            get;
            set;
        }

        List<LayerItem> layerItems=null;
        public SelectProperties(List<LayerItem> LayerItems )
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
            dgvProperties.Rows.Clear();
            if (cbLayers.SelectedItem != null&&cbLayers.SelectedItem is LayerItem)
            {
                var list = (cbLayers.SelectedItem as LayerItem).Propertys;
                if (list != null && list.Count > 0)
                {
                    foreach (var prop in list)
                    {
                        int index=dgvProperties.Rows.Add();
                        dgvProperties.Rows[index].Cells["cSelect"].Value = true;
                        dgvProperties.Rows[index].Cells["cName"].Value = prop;
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
                return;
            }


            PropertyNames = new List<string>();
            foreach (DataGridViewRow row in dgvProperties.Rows)
            {
                if (row.Cells["cSelect"].Value != null 
                    && (bool)row.Cells["cSelect"].Value)
                {
                    PropertyNames.Add(row.Cells["cName"].Value.ToString());
                }
            }
            if (PropertyNames.Count <= 0)
            {
                MessageHelper.ShowError("请至少选择一个属性！");
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
