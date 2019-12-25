using System;
using System.Collections.Generic;
using System.Text;

namespace DEVGIS.Contour
{
    public class Cmou_Edge
    {
        private Int32 id = -1;
        public Int32 ID
        {
            get { return id; }
            set { id = value; }
        }


        // 边界的两个数据点的编号
        //public Int32[] iPointIndex = new Int32[2];
        private Int32 id_StartPoint = -1;
        public Int32 ID_StartPoint
        {
            get { return id_StartPoint; }
            set { id_StartPoint = value; }
        }

        private Int32 id_EndPoint = -1;
        public Int32 ID_EndPoint 
        {
            get { return id_EndPoint; }
            set { id_EndPoint = value; }
        }

        // 被引用次数，即表示有多少个三角形共用此边
        // 最少0个，最多2个
        public Int32 iNum_TriRef = 0;

        // 所属三角形的编号
        public List<Int32> list_ID_Tri = new List<Int32>();
    }
}
