using System;
using System.Collections.Generic;
using System.Text;

namespace DEVGIS.Contour
{
    public class Cmou_Line
    {
        // 点集
        public List<Cmou_Point> list_Points = new List<Cmou_Point>();

        // 端点---起点
        Cmou_Point point_Start = new Cmou_Point();
        public Cmou_Point CLine_Start
        {
            get { return list_Points[0]; }
            set { list_Points[0] = value; }
        }

        // 端点---终点
        Cmou_Point point_End = new Cmou_Point();
        public Cmou_Point CLine_End
        {
            get { return list_Points[list_Points.Count-1]; }
            set { list_Points[list_Points.Count-1] = value; }
        }

        // 插入一个点,并排序
        // 
        public void insertPoint(Cmou_Point pointIN)
        {
            double d_DisSE = Math.Sqrt(CLine_End.X - CLine_Start.X) + Math.Sqrt(CLine_End.Y - CLine_Start.Y);
            for (int iP = 0; iP < list_Points.Count;iP++ )
            {

            }
        }
    }
}
