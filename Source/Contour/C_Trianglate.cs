using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DEVGIS.Contour {
    class C_Trianglate {

        public C_Trianglate(double[] dData_XIN, double[] dData_YIN, double[] dData_ZIN)
        {
            iNum_Vert = dData_XIN.Length;
            for (int i = 1; i <= iNum_Vert;i++ ) {
                vertex[i] = new C_Vertex(dData_XIN[i-1], dData_YIN[i-1],dData_ZIN[i-1]);
            }

            triangle = new C_Triangle[iMax];
        }

        int iNum_Vert = 0;
        public C_Triangle[] triangle ;//= new C_Triangle[5000];
        public C_Vertex[] vertex = new C_Vertex[50000];

        public int HowMany = 0; // the number of triangle
        int iMax = 500000;

        #region Triangulate
        public int Triangulate() {
            int nvert = iNum_Vert;

            for (int ii = 1; ii <= nvert;ii++ ) {
                triangle[ii] = new C_Triangle();
            }
            //Takes as input NVERT vertices in arrays Vertex()
            //Returned is a list of NTRI triangular faces in the array
            //Triangle(). These triangles are arranged in clockwise order.
            bool[] Complete = new bool[iMax];
            int[,] Edges = new int[3, 10*iMax * 3];
            int Nedge;

            //For Super Triangle
            double xmin, xmax, ymin, ymax, xmid, ymid, dx, dy, dmax;

            //General Variables
            int i, j, k, ntri;
            double xc = 0;
            double yc = 0;
            double r = 0;
            bool inc;

            //Find the maximum and minimum vertex bounds.   
            //This is to allow calculation of the bounding triangle
            xmin = vertex[1].x;
            ymin = vertex[1].y;
            xmax = xmin;
            ymax = ymin;
            for (i = 2; i <= nvert; i++) {
                if (vertex[i].x < xmin) {
                    xmin = vertex[i].x;
                }
                if (vertex[i].x > xmax) {
                    xmax = vertex[i].x;
                }
                if (vertex[i].y < ymin) {
                    ymin = vertex[i].y;
                }
                if (vertex[i].y > ymax) {
                    ymax = vertex[i].y;
                }
            }

            dx = xmax - xmin;
            dy = ymax - ymin;
            if (dx > dy) {
                dmax = dx;
            }
            else {
                dmax = dy;
            }
            xmid = (xmax + xmin) / 2;
            ymid = (ymax + ymin) / 2;

            //Set up the supertriangle
            //This is a triangle which encompasses all the sample points.
            //The supertriangle coordinates are added to the end of the
            //vertex list. The supertriangle is the first triangle in the triangle list.

            vertex[nvert + 1] = new C_Vertex();
            vertex[nvert + 2] = new C_Vertex();
            vertex[nvert + 3] = new C_Vertex();

            vertex[nvert + 1].x = xmid - 2 * dmax;
            vertex[nvert + 1].y = ymid - dmax;
            vertex[nvert + 2].x = xmid;
            vertex[nvert + 2].y = ymid + 2 * dmax;
            vertex[nvert + 3].x = xmid + 2 * dmax;
            vertex[nvert + 3].y = ymid - dmax;

            triangle[1].vv0 = nvert + 1;
            triangle[1].vv1 = nvert + 2;
            triangle[1].vv2 = nvert + 3;
            Complete[1] = false;
            ntri = 1;

            //Include each point one at a time into the existing mesh
            for (i = 1; i <= nvert; i++) {
                Nedge = 0;

                //Set up the edge buffer.
                //If the point (Vertex(i).x,Vertex(i).y) lies inside the circumcircle then the
                //three edges of that triangle are added to the edge buffer.

                j = 0;
                do {
                    j += 1;
                    //triangle[j] = new Triangle();
                    
                    if (!Complete[j]) {
                        inc = InCircle(vertex[i].x, vertex[i].y, vertex[triangle[j].vv0].x, vertex[triangle[j].vv0].y,
                            vertex[triangle[j].vv1].x, vertex[triangle[j].vv1].y, vertex[triangle[j].vv2].x, vertex[triangle[j].vv2].y, ref xc, ref yc, ref r);
                        //Include this if points are sorted by X ,If (xc + r) < Vertex(i).x Then,complete(j) = True,Else
                        if (inc) {
                            Edges[1, Nedge + 1] = triangle[j].vv0;
                            Edges[2, Nedge + 1] = triangle[j].vv1;
                            Edges[1, Nedge + 2] = triangle[j].vv1;
                            Edges[2, Nedge + 2] = triangle[j].vv2;
                            Edges[1, Nedge + 3] = triangle[j].vv2;
                            Edges[2, Nedge + 3] = triangle[j].vv0;
                            Nedge = Nedge + 3;
                            triangle[j].vv0 = triangle[ntri].vv0;
                            triangle[j].vv1 = triangle[ntri].vv1;
                            triangle[j].vv2 = triangle[ntri].vv2;
                            Complete[j] = Complete[ntri];
                            j = j - 1;
                            ntri = ntri - 1;
                        }
                    }
                }
                while (j < ntri);

                //Tag multiple edges,Note: if all triangles are specified anticlockwise then all,interior edges are opposite pointing in direction.
                for (j = 1; j < Nedge; j++) {
                    if (Edges[1, j] != 0 && Edges[2, j] != 0) {
                        for (k = j + 1; k <= Nedge; k++) {
                            if (Edges[1, k] != 0 && Edges[2, k] != 0) {
                                if (Edges[1, j] == Edges[2, k]) {
                                    if (Edges[2, j] == Edges[1, k]) {
                                        Edges[1, j] = 0;
                                        Edges[2, j] = 0;
                                        Edges[1, k] = 0;
                                        Edges[2, k] = 0;
                                    }
                                }
                            }
                        }
                    }
                }

                //Form new triangles for the current point,Skipping over any tagged edges.All edges are arranged in clockwise order.
                for (j = 1; j <= Nedge; j++) {
                    if (Edges[1, j] != 0 && Edges[2, j] != 0) {
                        ntri += 1;

                        triangle[ntri] = new C_Triangle();

                        triangle[ntri].vv0 = Edges[1, j];
                        triangle[ntri].vv1 = Edges[2, j];
                        triangle[ntri].vv2 = i;
                        Complete[ntri] = false;
                    }
                }

            }

            //Remove triangles with supertriangle vertices,These are triangles which have a vertex number greater than NVERT

            i = 0;
            do {
                i += 1;
                if (triangle[i].vv0 > nvert || triangle[i].vv1 > nvert || triangle[i].vv2 > nvert) {
                    triangle[i].vv0 = triangle[ntri].vv0;
                    triangle[i].vv1 = triangle[ntri].vv1;
                    triangle[i].vv2 = triangle[ntri].vv2;
                    i = i - 1;
                    ntri = ntri - 1;
                }
            }
            while (i < ntri);

            HowMany = ntri;
            return ntri;
        }
        #endregion

        #region InCircle
        private bool InCircle(double xp, double yp, double x1, double y1, double x2, double y2, double x3, double y3,
            ref double xc, ref double yc, ref double r) {
            //Return TRUE if the point (xp,yp) lies inside the circumcircle;made up by points (x1,y1) (x2,y2) (x3,y3);
            //The circumcircle centre is returned in (xc,yc) and the radius r;NOTE: A point on the edge is inside the circumcircle

            double eps, m1, m2, mx1, mx2, my1, my2, dx, dy, rsqr, drsqr;

            eps = 0.0000001;

            if (Math.Abs(y1 - y2) < eps && Math.Abs(y2 - y3) < eps) {
                //MessageBox.Show("INCIRCUM - F - Points are coincident !!");
                return false;
            }

            if (Math.Abs(y2 - y1) < eps) {
                m2 = -(x3 - x2) / (y3 - y2);
                mx2 = (x2 + x3) / 2;
                my2 = (y2 + y3) / 2;
                xc = (x2 + x1) / 2;
                yc = m2 * (xc - mx2) + my2;
            }
            else if (Math.Abs(y3 - y2) < eps) {
                m1 = -(x2 - x1) / (y2 - y1);
                mx1 = (x1 + x2) / 2;
                my1 = (y1 + y2) / 2;
                xc = (x3 + x2) / 2;
                yc = m1 * (xc - mx1) + my1;
            }
            else {
                m1 = -(x2 - x1) / (y2 - y1);
                m2 = -(x3 - x2) / (y3 - y2);
                mx1 = (x1 + x2) / 2;
                mx2 = (x2 + x3) / 2;
                my1 = (y1 + y2) / 2;
                my2 = (y2 + y3) / 2;
                xc = (m1 * mx1 - m2 * mx2 + my2 - my1) / (m1 - m2);
                yc = m1 * (xc - mx1) + my1;
            }

            dx = x2 - xc;
            dy = y2 - yc;
            rsqr = dx * dx + dy * dy;
            r = Math.Sqrt(rsqr);
            dx = xp - xc;
            dy = yp - yc;
            drsqr = dx * dx + dy * dy;

            if (drsqr <= rsqr) {
                return true;
            }
            else {
                return false;
            }
        }
        #endregion

        #region DrawTriangle
        public void drawtriangle(System.Drawing.Graphics g)
        {
            Pen bPen = new Pen(Color.Green, 1);
            Point p0, p1, p2;

            if (HowMany < 2) return;            
            
            for (int i = 1; i <= HowMany; i++) {
                p0 = new Point(Convert.ToInt32(vertex[triangle[i].vv0].x), Convert.ToInt32(vertex[triangle[i].vv0].y));
                p1 = new Point(Convert.ToInt32(vertex[triangle[i].vv1].x), Convert.ToInt32(vertex[triangle[i].vv1].y));
                p2 = new Point(Convert.ToInt32(vertex[triangle[i].vv2].x), Convert.ToInt32(vertex[triangle[i].vv2].y));

                g.DrawLine(bPen, p0, p1);
                g.DrawLine(bPen, p1, p2);
                g.DrawLine(bPen, p0, p2);
               
            }
        }

        public void filltriangle(System.Drawing.Graphics g,System.Drawing.Color cIN)
        {

            PointF[] ps = new PointF[3];

            //if (HowMany < 2) return;

            for (int i = 1; i <= HowMany; i++)
            {
                ps[0] = new PointF(Convert.ToSingle(vertex[triangle[i].vv0].x), Convert.ToSingle(vertex[triangle[i].vv0].y));
                ps[1] = new PointF(Convert.ToSingle(vertex[triangle[i].vv1].x), Convert.ToSingle(vertex[triangle[i].vv1].y));
                ps[2] = new PointF(Convert.ToSingle(vertex[triangle[i].vv2].x), Convert.ToSingle(vertex[triangle[i].vv2].y));

                g.FillPolygon(new System.Drawing.SolidBrush(cIN), ps);
            }
        }

        #endregion
    }
}
