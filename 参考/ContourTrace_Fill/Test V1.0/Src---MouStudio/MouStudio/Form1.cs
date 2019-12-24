using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MouStudio {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        C_Trianglate trianglate = null;
        C_Trianglate2 trianglate2 = null;

        double dMax_X = -999999999;
        double dMax_Y = -999999999;
        double dMin_X = 9999999999;
        double dMin_Y = 9999999999;
        double dMax_Z = -9999999999;
        double dMin_Z = 9999999999;

        
        private void pictureBox1_Move(object sender, EventArgs e) {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            if (trianglate != null)
            {                
                if (conTrace != null && conTrace.list_ContourLine.Count > 0)
                {
                    //------- 等值线填充 -------   
                    if (isFill)
                        conTrace.CTrace_Fill(g);

                    //------- 边标记 -------
                    if(isMarkEdge)
                    conTrace.CTrace_MarkEdge(g);

                    //------- 三角形标记 -------
                    if(isMarkTriangle)
                    conTrace.CTrace_MarkTriangle(g);

                    //------- 等值线绘制 -------
                    if (isDrawConLine)
                    {
                        #region "Draw ConLine"
                        PointF p1 = new PointF(0, 0);
                        PointF p2 = new PointF(0, 0);
                        Contour.Cmou_Point conP1 = new MouStudio.Contour.Cmou_Point();
                        Contour.Cmou_Point conP2 = new MouStudio.Contour.Cmou_Point();
                        foreach (Contour.Cmou_ContourLine conLine in conTrace.list_ContourLine)
                        {
                            //Contour.Cmou_ContourLine conLine = conTrace.list_ContourLine[0];
                            if (conLine.conType == MouStudio.Contour.ContourLineType.Opened)
                            {
                                for (int iP = 0; iP < conLine.list_Point.Count; iP++)
                                {
                                    if (iP == 0)
                                    {
                                        conP1 = conLine.list_Point[iP];
                                        p1.X = (float)conP1.X;
                                        p1.Y = (float)conP1.Y;
                                    }
                                    else
                                    {
                                        conP2 = conLine.list_Point[iP];
                                        p2.X = (float)conP2.X;
                                        p2.Y = (float)conP2.Y;
                                        g.DrawLine(Pens.Black, p1, p2);
                                        p1 = p2;
                                    }
                                }
                            }
                            else
                            {
                                for (int iP = 0; iP < conLine.list_Point.Count; iP++)
                                {
                                    if (iP == 0)
                                    {
                                        conP1 = conLine.list_Point[iP];
                                        p1.X = (float)conP1.X;
                                        p1.Y = (float)conP1.Y;
                                    }
                                    else
                                    {
                                        conP2 = conLine.list_Point[iP];
                                        p2.X = (float)conP2.X;
                                        p2.Y = (float)conP2.Y;
                                        g.DrawLine(Pens.Black, p1, p2);
                                        p1 = p2;
                                    }
                                }

                                conP1 = conLine.list_Point[0];
                                p1.X = (float)conP1.X;
                                p1.Y = (float)conP1.Y;
                                g.DrawLine(Pens.Black, p2, p1);
                            }
                        }
                        #endregion
                    }   
          
                    //------- 标注等值线 -------
                    conTrace.CTrace_MarkContourLine(g);
                }

                //------- 绘制三角形 -------
                if(isDrawTriangle)
                trianglate.drawtriangle(g);

            }
            
        }

        C_ContourTrace conTrace = null;
        private void btn_ConTrace_Click(object sender, EventArgs e)
        {
            conTrace = new C_ContourTrace(trianglate);
            conTrace.d_Max = dMax_Z;
            conTrace.d_Min = dMin_Z;
            conTrace.CTrace_ContourLineTrace();
            pictureBox1.Invalidate();
        }

        bool isDrawConLine = true;
        private void btn_DrawContourLine_Click(object sender, EventArgs e)
        {

            if (isDrawConLine)
            {
                isDrawConLine = false;
            }
            else
            {
                isDrawConLine = true;
            }
            pictureBox1.Invalidate();
        }

        bool isFill = false;
        private void btn_Test_Click(object sender, EventArgs e)
        {
            if (isFill)
            {
                isFill = false;
            } 
            else
            {
                isFill = true;
            }
            pictureBox1.Invalidate();
        }

        bool isMarkEdge = false;
        private void btn_MarkEdge_Click(object sender, EventArgs e)
        {
            if (isMarkEdge)
            {
                isMarkEdge = false;
            }
            else
            {
                isMarkEdge = true;
            }
            pictureBox1.Invalidate();
        }

        bool isMarkTriangle = false;
        private void btn_MarkTriangle_Click(object sender, EventArgs e)
        {
            if (isMarkTriangle)
            {
                isMarkTriangle = false;
            }
            else
            {
                isMarkTriangle = true;
            }
            pictureBox1.Invalidate();
        }

        bool isDrawTriangle = false;
        private void btn_DrawTriangle_Click(object sender, EventArgs e)
        {
            if (isDrawTriangle)
            {
                isDrawTriangle = false;
            }
            else
            {
                isDrawTriangle = true;
            }
            pictureBox1.Invalidate();
        }

        string str_FilePath = null;
        private void btn_OpenFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog open = new OpenFileDialog())
            {
                if (open.ShowDialog() == DialogResult.OK)
                {
                    str_FilePath = open.FileName;
                    ReadReguXFile();

                    btn_ConTrace_Click(sender, e);
                }
            }
        }

        private void ReadReguXFile()
        {
            System.IO.StreamReader sr_ScatterData = new System.IO.StreamReader(str_FilePath);
            sr_ScatterData.ReadLine();
            string temp_str = sr_ScatterData.ReadLine();
            temp_str.TrimStart();
            temp_str.TrimEnd();
            string[] tempData_str = temp_str.Split(' ').ToArray();
            int iNum_Point = int.Parse(tempData_str[1]);
            //iNum_Point++;
            double[] dData_X = new double[iNum_Point];
            double[] dData_Y = new double[iNum_Point];
            double[] dData_Z = new double[iNum_Point];
            PointF[] ps = new PointF[iNum_Point];
            int iCount = 3;

            int i = 0;
            while (!sr_ScatterData.EndOfStream)
            {
                if (iCount == 3)
                {
                    temp_str = sr_ScatterData.ReadLine();
                    temp_str.TrimStart();
                    temp_str.TrimEnd();
                    tempData_str = temp_str.Split(' ').ToArray();
                    dData_X[i] = double.Parse(tempData_str[1]);
                    dData_Y[i] = double.Parse(tempData_str[2]);
                    dData_Z[i] = double.Parse(tempData_str[3]);
                    if (dMax_X < dData_X[i]) dMax_X = dData_X[i];
                    if (dMin_X > dData_X[i]) dMin_X = dData_X[i];
                    if (dMax_Y < dData_Y[i]) dMax_Y = dData_Y[i];
                    if (dMin_Y > dData_Y[i]) dMin_Y = dData_Y[i];
                    if (dMax_Z < dData_Z[i]) dMax_Z = dData_Z[i];
                    if (dMin_Z > dData_Z[i]) dMin_Z = dData_Z[i];
                    i++;
                    iCount = 0;
                }
                else
                {
                    sr_ScatterData.ReadLine();
                    iCount++;
                }
            }
            sr_ScatterData.Close();

            double dSx = (dMax_X - dMin_X) / (pictureBox1.Width - 3);
            double dSy = (dMax_Y - dMin_Y) / (pictureBox1.Height - 3);
            for (i = 0; i < iNum_Point; i++)
            {
                dData_X[i] = Convert.ToSingle((dData_X[i] - dMin_X) / dSx);
                dData_Y[i] = Convert.ToSingle((dData_Y[i] - dMin_Y) / dSy);
                //ps[i].X = (float)dData_X[i];
                //ps[i].Y = (float)dData_Y[i];
            }

            if (iNum_Point > 3)
            {
                trianglate = new C_Trianglate(dData_X, dData_Y, dData_Z);
                int iNum_Tri = trianglate.Triangulate();
                //trianglate2 = new C_Trianglate2(ps);
                //trianglate2.GeneTIN(pictureBox1.CreateGraphics());
                pictureBox1.Invalidate();
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (str_FilePath != null)
            {
                ReadReguXFile();
                btn_ConTrace_Click(sender, e);
            }
        }
    }
}
