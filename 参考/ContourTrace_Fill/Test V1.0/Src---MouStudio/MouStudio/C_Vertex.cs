using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MouStudio {
    public class C_Vertex {
        public C_Vertex(double d_xIN,double d_yIN,double d_zIN)
        {
            x = d_xIN;
            y = d_yIN;
            z = d_zIN;
        }
        public C_Vertex(){}

        public double x;
        public double y;
        public double z;
    }
}
