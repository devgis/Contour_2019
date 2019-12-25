using System;
using System.Collections.Generic;
using System.Text;

namespace DEVGIS.Contour
{
    public class Cmou_Point
    {
        Int32 id = -1;
        public Int32 ID
        {
            get { return id; }
            set { id = value; }
        }

        double d_X = 0.0;
        public double X
        {
            get { return d_X; }
            set { d_X = value; }
        }

        double d_Y = 0.0;
        public double Y
        {
            get { return d_Y; }
            set { d_Y = value; }
        }

        double d_V = 0.0;
        public double V
        {
            get { return d_V; }
            set { d_V = value; }
        }

        // 所属边的编号
        public List<Int32> list_ID_Edge = new List<Int32>();

        // 所属线的编号
        public List<Int32> list_ID_Line = new List<Int32>();

        // 所属三角形的编号
        public List<Int32> list_ID_Tri = new List<Int32>();
    }
}
