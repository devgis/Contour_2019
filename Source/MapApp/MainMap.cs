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
using DEVGIS.Contour;
using DEVGIS.Common;
using Newtonsoft.Json;
using DEVGIS.MapAPP.Entities;
using DEVGIS.MapAPP;

namespace DEVGIS.MapAPP
{
    public partial class MainMap : Form
    {
        double dMax_X = double.MinValue;
        double dMax_Y = double.MinValue;
        double dMin_X = double.MaxValue;
        double dMin_Y = double.MaxValue;
        double dMax_Z = double.MinValue;
        double dMin_Z = double.MaxValue;
        const string fileext = "ctr";
        const string layername = "ContourLayer";
        FeatureLayer flContourLayer = null;

        public MainMap()
        {
            InitializeComponent();
            mapControl1.Map.ViewChangedEvent += new MapInfo.Mapping.ViewChangedEventHandler(Map_ViewChangedEvent);
            Map_ViewChangedEvent(this, null);
        }

        #region 菜单事件
        void Map_ViewChangedEvent(object sender, MapInfo.Mapping.ViewChangedEventArgs e)
        {
            mapControl1.Map.Zoom = new MapInfo.Geometry.Distance(
                                      CoordSys.ConvertDistanceUnits(
                                      DistanceUnit.Meter,
                                      mapControl1.Map.Zoom.Value,
                                      mapControl1.Map.Zoom.Unit),
                                      DistanceUnit.Meter);
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

        private void tsmiOpen_Click(object sender, EventArgs e)
        {
            try
            {
                OpenData();
            }
            catch (Exception ex)
            {
                Loger.WriteLog(ex);
                MessageHelper.ShowInfo("出现未知异常:" + ex.Message);
            }
        }

        private void tsmiExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveData();
            }
            catch (Exception ex)
            {
                Loger.WriteLog(ex);
                MessageHelper.ShowInfo("出现未知异常:" + ex.Message);
            }
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        private void tsmiGenerateContour_Click(object sender, EventArgs e)
        {
            var layerinfos = GetLayerItems();
            SelectLayer selectLayer = new SelectLayer(layerinfos);
            if (selectLayer.ShowDialog() == DialogResult.OK)
            {
                DrowContour(selectLayer.LayerName, selectLayer.PropertyName);
            }
        }

        private void tsmiHelper_Click(object sender, EventArgs e)
        {
            MessageHelper.ShowInfo("暂无帮助！");
        }

        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            MessageHelper.ShowInfo(string.Format("{0} {1}",this.Text,System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));
        }

        private void tsmiAverage_Click(object sender, EventArgs e)
        {
            try
            {
                CalculateAverage();
            }
            catch(Exception ex)
            {
                Loger.WriteLog(ex);
                MessageHelper.ShowInfo("出现未知异常:"+ ex.Message);
            }
        }
        #endregion

        #region 私有方法
        private void LoadMap()
        {
            string MapPath = Path.Combine(Application.StartupPath, @"Data\map.mws");
            MapWorkSpaceLoader mwsLoader = new MapWorkSpaceLoader(MapPath);
            mapControl1.Map.Load(mwsLoader);
            CreateTableLayer();//加载用于绘制等值线的图

            mapControl1.Tools.LeftButtonTool = "Pan";
        }

        private void CreateTableLayer()
        {
            TableInfoMemTable ti = new TableInfoMemTable("ContourLayerTemp");
            ti.Temporary = true;
            ti.Columns.Add(ColumnFactory.CreateFeatureGeometryColumn(mapControl1.Map.GetDisplayCoordSys()));
            ti.Columns.Add(ColumnFactory.CreateStyleColumn());
            ti.Columns.Add(ColumnFactory.CreateStringColumn("CValue", 100));

            Table table;
            try
            {
                table = Session.Current.Catalog.CreateTable(ti);

            }
            catch (Exception ex)
            {
                Loger.WriteLog(ex);
                table = Session.Current.Catalog.GetTable("ContourLayerTemp");
            }

            flContourLayer = new FeatureLayer(table, "ContourLayer ", "ContourLayer");

            //标签图层
            LabelLayer lbLayer = new LabelLayer();
            LabelSource source = new LabelSource(table);
            source.DefaultLabelProperties.Caption = "CValue";
            source.DefaultLabelProperties.Style.Font.ForeColor = System.Drawing.Color.Blue;   //字体颜色
            source.DefaultLabelProperties.Style.Font.BackColor =System.Drawing.Color.PowderBlue ;   //字体背景
            source.DefaultLabelProperties.Style.Font.TextEffect = MapInfo.Styles.TextEffect.Box;  //边缘效果
            source.DefaultLabelProperties.Style.Font.FontWeight = MapInfo.Styles.FontWeight.Bold; //粗体
            source.DefaultLabelProperties.Layout.Alignment = MapInfo.Text.Alignment.CenterRight;  //相对位置
            source.DefaultLabelProperties.Layout.Offset = 2;
            lbLayer.Sources.Append(source);

            mapControl1.Map.Layers.Insert(0, flContourLayer);
            mapControl1.Map.Layers.Insert(1, lbLayer);
            mapControl1.Refresh();
        }

        public void ClearTableLayer()
        {
            SearchInfo si = MapInfo.Data.SearchInfoFactory.SearchWhere("");
            IResultSetFeatureCollection ifs;
            MapInfo.Data.Table table=flContourLayer.Table;
            if (table != null) //Table exists close it
            {
                si = MapInfo.Data.SearchInfoFactory.SearchWhere("1==1");
                ifs = MapInfo.Engine.Session.Current.Catalog.Search(table, si);
                foreach (Feature ft in ifs)
                {
                    table.DeleteFeature(ft);                           //删除所有该图层上的图元
                }

            }
        }


        private void mapControl1_MouseMove(object sender, MouseEventArgs e)
        {
            DPoint pt = new DPoint();
            mapControl1.Map.DisplayTransform.FromDisplay(e.Location, out pt);
            tslPos.Text = string.Format("位置: X:{0} Y:{1}", pt.x.ToString("0.0000"), pt.y.ToString("0.0000"));
        }

        private void SaveData()
        {
            List<Cmou_ContourLine> lines = new List<Cmou_ContourLine>();
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = string.Format("{0}文件|{0}", fileext);
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(sfd.FileName, JsonConvert.SerializeObject(lines));
                }
                catch (Exception ex)
                {
                    Loger.WriteLog(ex);
                    MessageHelper.ShowError("保存数据出错：" + ex.Message);
                }
            }
        }

        private void OpenData()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = string.Format("{0}文件|{0}", fileext);
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    List<Cmou_ContourLine> lines = JsonConvert.DeserializeObject<List<Cmou_ContourLine>>(File.ReadAllText(ofd.FileName));
                    DrawContour(lines);
                }
                catch (Exception ex)
                {
                    Loger.WriteLog(ex);
                    MessageHelper.ShowError("保存数据出错：" + ex.Message);
                }
            }
        }

        private void DrowContour(string LayerName, string PropertyName)
        {
            var conTrace = new C_ContourTrace(GetData(LayerName, PropertyName));
            conTrace.d_Max = dMax_Z;
            conTrace.d_Min = dMin_Z;
            conTrace.CTrace_ContourLineTrace();
            PointF p1 = new PointF(0, 0);
            PointF p2 = new PointF(0, 0);
            Cmou_Point conP1 = new Cmou_Point();
            Cmou_Point conP2 = new Cmou_Point();
            DrawContour(conTrace.list_ContourLine);
        }

        private void DrawContour(List<Cmou_ContourLine> lines)
        {
            //先清空原有数据
            ClearTableLayer();
            foreach (Cmou_ContourLine conLine in lines)
            {
                List<DPoint> dPoints = new List<DPoint>();
                for (int iP = 0; iP < conLine.list_Point.Count; iP++)
                {
                    dPoints.Add(new DPoint(conLine.list_Point[iP].X, conLine.list_Point[iP].Y));
                }
                if (conLine.conType == ContourLineType.Closed)
                {
                    dPoints.Add(new DPoint(conLine.list_Point[0].X, conLine.list_Point[0].Y));
                }
                DrawSegment(dPoints, conLine.d_Value.ToString());
            }
        }

        private void DrawSegment(List<DPoint> DPoints,string Value)
        {
            if (DPoints == null || DPoints.Count < 2)
            {
                return;
            }
            MultiCurve multiCurve = new MultiCurve(flContourLayer.CoordSys, CurveSegmentType.Linear, DPoints.ToArray());
            

            //FeatureGeometry pgLine = MultiCurve.CreateLine(flContourLayer.CoordSys, new DPoint(p1.X,p1.Y), new DPoint(p2.X,p2.Y));
            MapInfo.Styles.SimpleLineStyle slsLine = new MapInfo.Styles.SimpleLineStyle(new LineWidth(3, LineWidthUnit.Pixel), 2, System.Drawing.Color.OrangeRed);
            MapInfo.Styles.CompositeStyle csLine = new MapInfo.Styles.CompositeStyle(slsLine);
            MapInfo.Data.Feature ptLine = new MapInfo.Data.Feature(flContourLayer.Table.TableInfo.Columns);
            ptLine.Geometry = multiCurve;
            ptLine.Style = csLine;
            ptLine["CValue"] = Value;
            flContourLayer.Table.InsertFeature(ptLine);
        }

        private C_Trianglate GetData(string LayerName, string PropertyName)
        {
            //要给以下变量赋值
            //double dMax_X = -999999999;
            //double dMax_Y = -999999999;
            //double dMin_X = 9999999999;
            //double dMin_Y = 9999999999;
            //double dMax_Z = -9999999999;
            //double dMin_Z = 9999999999;
            FeatureLayer flLayer = mapControl1.Map.Layers["LayerName"] as FeatureLayer;
            List<TData> datas = new List<TData>();
            if (flLayer != null)
            {
                foreach (Feature feature in flLayer.Table)
                {
                    try
                    {
                        TData data = new TData();
                        data.X = feature.Geometry.Centroid.x;
                        data.Y = feature.Geometry.Centroid.y;
                        data.Z = Convert.ToDouble(feature[PropertyName]);
                        datas.Add(data);
                    }
                    catch (Exception ex)
                    {
                        Loger.WriteLog(ex);
                    }
                }
            }

            //
            int iNum_Point = datas.Count;
            double[] dData_X = new double[iNum_Point];
            double[] dData_Y = new double[iNum_Point];
            double[] dData_Z = new double[iNum_Point];
            for (int i = 0; i < iNum_Point; i++)
            {
                dData_X[i] = datas[i].X;
                dData_Y[i] = datas[i].Y;
                dData_Z[i] = datas[i].Z;

                if (datas[i].X > dMax_X)
                {
                    dMax_X = datas[i].X;
                }

                if (datas[i].X < dMin_X)
                {
                    dMin_X = datas[i].X;
                }

                if (datas[i].Y > dMax_Y)
                {
                    dMax_Y = datas[i].Y;
                }

                if (datas[i].Y < dMin_Y)
                {
                    dMin_Y = datas[i].Y;
                }

                if (datas[i].Z > dMax_Z)
                {
                    dMax_Z = datas[i].Z;
                }

                if (datas[i].Z < dMin_Z)
                {
                    dMin_Z = datas[i].Z;
                }
            }

            C_Trianglate trianglate = new C_Trianglate(dData_X, dData_Y, dData_Z);
            int iNum_Tri = trianglate.Triangulate();
            return trianglate;
        }

        private List<LayerItem> GetLayerItems(bool numeric=false)
        {
            List<LayerItem> items = new List<LayerItem>();
            foreach (var layer in mapControl1.Map.Layers)
            {
                if (layer is FeatureLayer)
                {
                    FeatureLayer flayer = layer as FeatureLayer;
                    LayerItem item = new LayerItem();
                    item.DisplayName = flayer.Alias;
                    item.LayerName = flayer.Name;
                    item.Propertys = new List<string>();
                    if (numeric)
                    {
                        foreach (object prop in flayer.CustomProperties.Keys)
                        {
                            if(flayer.Table.TableInfo.Columns[prop.ToString()].DataType==MIDbType.dBaseDecimal
                                || flayer.Table.TableInfo.Columns[prop.ToString()].DataType == MIDbType.Double
                                || flayer.Table.TableInfo.Columns[prop.ToString()].DataType == MIDbType.Int
                                || flayer.Table.TableInfo.Columns[prop.ToString()].DataType == MIDbType.SmallInt)
                            {
                                item.Propertys.Add(prop.ToString());
                            }
                        }
                    }
                    else
                    {
                        foreach (var prop in flayer.CustomProperties.Keys)
                        {

                            item.Propertys.Add(prop.ToString());
                        }
                    }
                    
                }
            }
            return items;
        }

        private List<LayerItem> GetLayerItems()
        {
            List<LayerItem> items = new List<LayerItem>();
            foreach (var layer in mapControl1.Map.Layers)
            {
                if (layer is FeatureLayer)
                {
                    FeatureLayer flayer = layer as FeatureLayer;
                    LayerItem item = new LayerItem();
                    item.DisplayName = flayer.Alias;
                    item.LayerName = flayer.Name;
                    item.Propertys = new List<string>();
                    foreach (var prop in flayer.CustomProperties.Keys)
                    {

                        item.Propertys.Add(prop.ToString());
                    }
                }
            }
            return items;
        }

        private void CalculateAverage()
        {
            var layerinfos = GetLayerItems(true);
            SelectProperties selectLayer = new SelectProperties(layerinfos);
            if (selectLayer.ShowDialog() == DialogResult.OK)
            {
                FeatureLayer flavglayer = mapControl1.Map.Layers[selectLayer.LayerName] as FeatureLayer;
                Dictionary<string, double> dictValues = new Dictionary<string, double>();
                foreach (string key in selectLayer.PropertyNames)
                {
                    int count = 0;
                    dictValues.Add(key, 0);
                    foreach (Feature ft in flavglayer.Table)
                    {
                        count++;
                        dictValues[key] += Convert.ToDouble(ft["key"]); //求和
                    }
                    dictValues[key] = dictValues[key] / count;//求平均数
                }
                StringBuilder sb = new StringBuilder();
                foreach (string key in dictValues.Keys)
                {
                    sb.AppendLine(string.Format("属性[{0}] 平均值{1}", key, dictValues[key]));
                    MessageHelper.ShowInfo(sb.ToString());
                    return;
                }
                MessageHelper.ShowError("计算项！");
            }
        }
        #endregion
    }
}