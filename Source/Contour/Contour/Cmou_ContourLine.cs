using System;
using System.Collections.Generic;
using System.Text;

namespace DEVGIS.Contour
{
    class Cmou_ContourLine
    {
        public Cmou_ContourLine(double d_ContourValue)
        {
            d_Value = d_ContourValue;
        }

        public List<Cmou_Point> list_Point = new List<Cmou_Point>();

        // 等值线所对应的属性值
        public double d_Value;

        // 等值线类型
        public ContourLineType conType;
    }

    public enum ContourLineType
    {
        Closed,
        Opened
    }
}
