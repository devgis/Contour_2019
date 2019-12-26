using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MapInfo.Mapping;
using MapInfo.Engine;
using MapInfo.Data;
using MapInfo.Geometry;
using MapInfo.Styles;
using System.IO;

namespace Devgis.EagleEye
{
    public partial class MainMap : Form
    {
        FeatureLayer eagleEye;
        Feature fRec;

        public MainMap()
        {
            InitializeComponent();
            mapControl1.Map.ViewChangedEvent += new MapInfo.Mapping.ViewChangedEventHandler(Map_ViewChangedEvent);
            Map_ViewChangedEvent(this, null);
        }

        void Map_ViewChangedEvent(object sender, MapInfo.Mapping.ViewChangedEventArgs e)
        {
            
            // Display the zoom level
            Double dblZoom = System.Convert.ToDouble(String.Format("{0:E2}", mapControl1.Map.Zoom.Value));
            tslScale.Text = "缩放: " + dblZoom.ToString() + " " + MapInfo.Geometry.CoordSys.DistanceUnitAbbreviation(mapControl1.Map.Zoom.Unit);
           
            Table tblTemp = Session.Current.Catalog.GetTable("EagleEyeTemp");
            try
            {
                (tblTemp as ITableFeatureCollection).Clear();
            }
            catch (Exception)
            { }
        }

        private void mapControl2_MouseClick(object sender, MouseEventArgs e)
        {
            //鹰眼地图点击时切换主地图到该点中兴
            DPoint pt = new DPoint();
            mapControl1.Map.DisplayTransform.FromDisplay(e.Location, out pt);
            mapControl1.Map.Center = pt;

        }

        private void MainMap_Load(object sender, EventArgs e)
        {
            this.LoadMap();
        }

        private void LoadMap()
        {
            string MapPath = Path.Combine(Application.StartupPath, @"Data\map.mws");
            MapWorkSpaceLoader mwsLoader = new MapWorkSpaceLoader(MapPath);
            mapControl1.Map.Load(mwsLoader);
            mapControl1.Map.Zoom = new MapInfo.Geometry.Distance(
                                      CoordSys.ConvertDistanceUnits(
                                      DistanceUnit.Meter,
                                      mapControl1.Map.Zoom.Value,
                                      mapControl1.Map.Zoom.Unit),
                                      DistanceUnit.Meter);
        }

        private void loadEagleLayer()
        {
            //TableInfoMemTable ti = new TableInfoMemTable("EagleEyeTemp");
            //ti.Temporary = true;
            //Column column;
            //column = new GeometryColumn(mapControl2.Map.GetDisplayCoordSys());
            //column.Alias = "MI_Geometry ";
            //column.DataType = MIDbType.FeatureGeometry;
            //ti.Columns.Add(column);

            //column = new Column();
            //column.Alias = "MI_Style ";
            //column.DataType = MIDbType.Style;
            //ti.Columns.Add(column);
            //Table table;
            //try
            //{
            //    table = Session.Current.Catalog.CreateTable(ti);

            //}
            //catch (Exception ex)
            //{
            //    table = Session.Current.Catalog.GetTable("EagleEyeTemp");
            //}
            //if (mapControl2.Map.Layers["MyEagleEye"] != null)
            //    mapControl2.Map.Layers.Remove(eagleEye);
            //eagleEye = new FeatureLayer(table, "EagleEye ", "MyEagleEye");
            //mapControl2.Map.Layers.Insert(0, eagleEye);
            //mapControl1.Refresh();
        }

        private void mapControl1_MouseMove(object sender, MouseEventArgs e)
        {
            DPoint pt = new DPoint();
            mapControl1.Map.DisplayTransform.FromDisplay(e.Location, out pt);
            tslPos.Text = $"位置: X:{pt.x.ToString("0.0000")} Y:{pt.y.ToString("0.0000")}";
        }
    }
}