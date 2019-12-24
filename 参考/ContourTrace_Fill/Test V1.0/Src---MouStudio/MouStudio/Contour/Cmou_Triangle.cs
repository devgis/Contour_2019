using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MouStudio.Contour
{
    public class Cmou_Triangle
    {
        private Int32 id = -1;
        public Int32 ID
        {
            get { return id; }
            set { id = value; }
        }

        // 三角形所包含的边界编号
        public Int32[] iEdgeIndex = new Int32[3];

        // 三角形所包含的顶点编号
        public int[] iPointIndex = new int[3];

    }
}
