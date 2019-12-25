using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Collections;

namespace DEVGIS.Contour {
    class C_Trianglate2 {
        public C_Trianglate2(PointF[] arrDotsIN)
        {
            arrDots = arrDotsIN;
        }
        public PointF[] arrDots;
        private ArrayList arrEdges = new ArrayList();
        private ArrayList arrTris = new ArrayList();
        
        private bool doshow = false;

        public class Edge {
            public int Start;//边的起点
            public int End;//边的终点
            public int LeftTri = -1;//左三角形索引
            public int RightTri = -1;//右三角形索引
        }
        public class Tri {
            public int NodeA;
            public int NodeB;
            public int NodeC;
            public int AdjTriA = -1;
            public int AdjTriB = -1;
            public int AdjTriC = -1;
        }

        public void GeneTIN(Graphics g) {
            arrEdges.Clear();
            arrTris.Clear();
            int i, idxStart = 0, endTemp, ptindex;
            bool isExist;
            double angMax, angMin, angTemp, angRcdMax, angRcdTmp, lenMin, lenCur, lenTmp1, lenTmp2;
            Edge edge = new Edge();
            //this.picTIN.Paint -= new PaintEventHandler(picTIN_DrawTIN);
            //this.picTIN.Paint += new PaintEventHandler(picTIN_DrawTIN);


            //找到边界---（删除不需要的点，从X最小的地方开始找，直至回到起始点）
            PointF dirCur = new PointF();
            PointF dirTmp1 = new PointF();
            PointF dirTmp2 = new PointF();
            PointF ptStart = new PointF();

            for (i = 1; i < arrDots.Length; i++) {
                if (arrDots[i].X < arrDots[idxStart].X) {
                    idxStart = i;
                }
            }
            endTemp = idxStart - 1;
            ptStart.X = arrDots[idxStart].X;
            ptStart.Y = arrDots[idxStart].Y;
            edge.Start = idxStart;
            angMin = Math.PI;
            dirCur.X = 0;
            dirCur.Y = 500;
            while (endTemp != idxStart) {
                lenCur = Math.Sqrt(dirCur.X * dirCur.X + dirCur.Y * dirCur.Y);
                lenMin = 1000;
                for (i = 0; i < arrDots.Length; i++)//找边界
				{

                    if (i != edge.Start) {
                        dirTmp1.X = arrDots[i].X - ptStart.X;
                        dirTmp1.Y = arrDots[i].Y - ptStart.Y;
                        lenTmp1 = Math.Sqrt(dirTmp1.X * dirTmp1.X + dirTmp1.Y * dirTmp1.Y);
                        angTemp = Math.Acos((dirCur.X * dirTmp1.X + dirCur.Y * dirTmp1.Y) / (lenTmp1 * lenCur));
                        if (angTemp < angMin) {
                            angMin = angTemp;
                            edge.End = i;
                            lenMin = lenTmp1;
                        }
                        else if (angTemp == angMin && lenTmp1 < lenMin) {
                            edge.End = i;
                            lenMin = lenTmp1;
                        }
                    }
                }
                arrEdges.Add(edge);
                if (doshow == true) {
                    System.Threading.Thread.Sleep(500);
                    //this.picTIN.Refresh();
                }
                endTemp = edge.End;
                edge = new Edge();
                angMin = Math.PI;
                dirCur.X = arrDots[endTemp].X - ptStart.X;
                dirCur.Y = arrDots[endTemp].Y - ptStart.Y;
                ptStart = arrDots[endTemp];
                edge.Start = endTemp;
            }
            if (doshow == true) {
                System.Threading.Thread.Sleep(500);
                //this.picTIN.Refresh();
            }
            //以下为自动生成TIN
            //从第一条边开始，按照先左后右的顺序寻找，找到则加入三角形数组和边数组，没有则继续下一边，直到边到达最后
            //注意边可能有两种顺序存储。
            for (i = 0; i < arrEdges.Count; i++) {
                //取出一条边
                edge = new Edge();
                edge = (Edge)arrEdges[i];
                //先左后右计算扩展点-判断三角形是否存在过（若本边的左三角已存在，则计算右三角）？？
                if (edge.LeftTri == -1) {
                    ptindex = -1;//选中的点的index
                    dirCur.X = arrDots[edge.End].X - arrDots[edge.Start].X;
                    dirCur.Y = arrDots[edge.End].Y - arrDots[edge.Start].Y;
                    angRcdMax = 0;//与该边夹角最大值
                    angMax = 0;//最大圆内接角
                    for (int j = 0; j < arrDots.Length; j++) {
                        if (j != edge.Start && j != edge.End)//排除边的端点
						{
                            dirTmp1.X = arrDots[j].X - arrDots[edge.Start].X;
                            dirTmp1.Y = arrDots[j].Y - arrDots[edge.Start].Y;
                            if (dirCur.X * dirTmp1.Y - dirCur.Y * dirTmp1.X < 0)//如果该点在左边，则计算
							{
                                //找角度最大的
                                lenCur = Math.Sqrt(dirCur.X * dirCur.X + dirCur.Y * dirCur.Y);//当前向量长度
                                lenTmp1 = Math.Sqrt(dirTmp1.X * dirTmp1.X + dirTmp1.Y * dirTmp1.Y);

                                dirTmp2.X = arrDots[j].X - arrDots[edge.End].X;
                                dirTmp2.Y = arrDots[j].Y - arrDots[edge.End].Y;
                                lenTmp2 = Math.Sqrt(dirTmp2.X * dirTmp2.X + dirTmp2.Y * dirTmp2.Y);
                                angRcdTmp = Math.Acos((dirCur.X * dirTmp1.X + dirCur.Y * dirTmp1.Y) / (lenTmp1 * lenCur));
                                angTemp = Math.Acos((dirTmp2.X * dirTmp1.X + dirTmp2.Y * dirTmp1.Y) / (lenTmp1 * lenTmp2));
                                if (angTemp > angMax) {
                                    angMax = angTemp;
                                    angRcdMax = angRcdTmp;
                                    ptindex = j;
                                }
                                else if (angTemp == angMax && angRcdMax < angRcdTmp)//相等取最左
								{
                                    angRcdMax = angRcdTmp;
                                    ptindex = j;
                                }

                            }
                        }
                    }
                    if (ptindex != -1)//选择有点
					{
                        //记录三角形
                        Tri tri = new Tri();
                        tri.NodeA = edge.Start;
                        tri.NodeB = edge.End;
                        tri.NodeC = ptindex;
                        edge.LeftTri = arrTris.Count;


                        isExist = false;
                        //记录边1-需要检索是否存在过这条边-由于每条边都先有左三角形，如有三角形加入，必定为右三角形
                        for (int k = 0; k < arrEdges.Count; k++) {
                            Edge e = (Edge)arrEdges[k];
                            if (e.Start == edge.Start && e.End == ptindex)//如果存在过这条边，则记录其右三角形
							{
                                e.RightTri = arrTris.Count;
                                tri.AdjTriB = e.LeftTri;
                                isExist = true;
                                break;
                            }
                            else if (e.Start == ptindex && e.End == edge.Start) {
                                e.LeftTri = arrTris.Count;
                                tri.AdjTriB = e.RightTri;
                                isExist = true;
                                break;
                            }
                        }
                        if (isExist == false)//如果不存在这条边，则新建一条边
						{
                            Edge edgeadd = new Edge();
                            edgeadd.Start = ptindex;
                            edgeadd.End = edge.Start;
                            edgeadd.LeftTri = arrTris.Count;
                            arrEdges.Add(edgeadd);
                            if (doshow == true) {
                                System.Threading.Thread.Sleep(500);
                                //this.picTIN.Refresh();
                            }

                        }


                        isExist = false;
                        //记录边2
                        for (int k = 0; k < arrEdges.Count; k++) {
                            Edge e = (Edge)arrEdges[k];
                            if (e.Start == ptindex && e.End == edge.End)//如果存在过这条边，则记录其右三角形
							{
                                e.RightTri = arrTris.Count;
                                tri.AdjTriA = e.LeftTri;
                                isExist = true;
                                break;
                            }
                            else if (e.Start == edge.End && e.End == ptindex) {
                                e.LeftTri = arrTris.Count;
                                tri.AdjTriA = e.RightTri;
                                isExist = true;
                                break;
                            }
                        }
                        if (isExist == false)//如果不存在这条边，则新建一条边
						{
                            Edge edgeadd = new Edge();
                            edgeadd.Start = edge.End;
                            edgeadd.End = ptindex;
                            edgeadd.LeftTri = arrTris.Count;
                            arrEdges.Add(edgeadd);
                            if (doshow == true) {
                                System.Threading.Thread.Sleep(500);
                                //this.picTIN.Refresh();
                            }

                        }
                        tri.AdjTriC = edge.RightTri;//如果edge的右三角形不存在，由if进来可见左三角也不存在，这只能是边界，从而tri.AdjTriC=-1合理
                        arrTris.Add(tri);//add the tri to the arraylist
                    }
                }
                else if (edge.RightTri == -1)//由于最开始的那部分都是边界，只有一个三角形；以后的边都已存在一个三角形，也仅剩余一个，故可以else if
				{
                    //仅在右边找
                    ptindex = -1;//选中的点的index
                    dirCur.X = arrDots[edge.End].X - arrDots[edge.Start].X;
                    dirCur.Y = arrDots[edge.End].Y - arrDots[edge.Start].Y;
                    angMax = 0;//最大角度
                    angRcdMax = 0;//与该边夹角最大值
                    for (int j = 0; j < arrDots.Length; j++) {
                        if (j != edge.Start && j != edge.End)//排除边的端点
						{
                            lenCur = Math.Sqrt(dirCur.X * dirCur.X + dirCur.Y * dirCur.Y);//当前向量长度
                            dirTmp1.X = arrDots[j].X - arrDots[edge.Start].X;
                            dirTmp1.Y = arrDots[j].Y - arrDots[edge.Start].Y;
                            if (dirCur.X * dirTmp1.Y - dirCur.Y * dirTmp1.X > 0)//如果该点在右边，则计算
							{
                                //找角度最大的
                                lenTmp1 = Math.Sqrt(dirTmp1.X * dirTmp1.X + dirTmp1.Y * dirTmp1.Y);

                                dirTmp2.X = arrDots[j].X - arrDots[edge.End].X;
                                dirTmp2.Y = arrDots[j].Y - arrDots[edge.End].Y;
                                lenTmp2 = Math.Sqrt(dirTmp2.X * dirTmp2.X + dirTmp2.Y * dirTmp2.Y);
                                angRcdTmp = Math.Acos((dirCur.X * dirTmp1.X + dirCur.Y * dirTmp1.Y) / (lenTmp1 * lenCur));
                                angTemp = Math.Acos((dirTmp2.X * dirTmp1.X + dirTmp2.Y * dirTmp1.Y) / (lenTmp1 * lenTmp2));
                                if (angTemp > angMax) {
                                    angMax = angTemp;
                                    angRcdMax = angRcdTmp;
                                    ptindex = j;
                                }
                                else if (angTemp == angMax && angRcdTmp > angRcdTmp)//相等取最左
								{
                                    angRcdTmp = angRcdTmp;
                                    ptindex = j;
                                }

                            }
                        }
                    }
                    if (ptindex != -1)//选择有点
					{
                        //记录三角形
                        //记录三角形
                        Tri tri = new Tri();
                        tri.NodeA = edge.Start;
                        tri.NodeB = edge.End;
                        tri.NodeC = ptindex;
                        edge.RightTri = arrTris.Count;


                        isExist = false;
                        //记录边1-需要检索是否存在过这条边-由于每条边都先有左三角形，如有三角形加入，必定为右三角形
                        for (int k = 0; k < arrEdges.Count; k++) {
                            Edge e = (Edge)arrEdges[k];
                            if (e.Start == ptindex && e.End == edge.Start)//如果存在过这条边，则记录其右三角形
							{
                                e.RightTri = arrTris.Count;
                                tri.AdjTriB = e.LeftTri;
                                isExist = true;
                                break;
                            }
                            else if (e.Start == edge.Start && e.End == ptindex) {
                                e.LeftTri = arrTris.Count;
                                tri.AdjTriB = e.RightTri;
                                isExist = true;
                                break;

                            }
                        }
                        if (isExist == false)//如果不存在这条边，则新建一条边
						{
                            Edge edgeadd = new Edge();
                            edgeadd.Start = edge.Start;
                            edgeadd.End = ptindex;
                            edgeadd.LeftTri = arrTris.Count;
                            arrEdges.Add(edgeadd);
                            if (doshow == true) {
                                System.Threading.Thread.Sleep(500);
                                //this.picTIN.Refresh();
                            }

                        }
                        isExist = false;
                        //记录边2
                        for (int k = 0; k < arrEdges.Count; k++) {
                            Edge e = (Edge)arrEdges[k];
                            if (e.Start == edge.End && e.End == ptindex)//如果存在过这条边，则记录其右三角形
							{
                                e.RightTri = arrTris.Count;
                                tri.AdjTriA = e.LeftTri;
                                isExist = true;
                                break;
                            }
                            else if (e.Start == ptindex && e.End == edge.End) {
                                e.LeftTri = arrTris.Count;
                                tri.AdjTriA = e.RightTri;
                                isExist = true;
                                break;
                            }
                        }
                        if (isExist == false)//如果不存在这条边，则新建一条边
						{
                            Edge edgeadd = new Edge();
                            edgeadd.Start = ptindex;
                            edgeadd.End = edge.End;
                            edgeadd.LeftTri = arrTris.Count;
                            arrEdges.Add(edgeadd);
                            if (doshow == true) {
                                System.Threading.Thread.Sleep(500);
                                //this.picTIN.Refresh();
                            }
                        }
                        tri.AdjTriC = edge.LeftTri;//如果edge的左三角形不存在，由if进来可见右三角也不存在，这只能是边界，从而tri.AdjTriC=-1合理
                        arrTris.Add(tri);//add the tri to the arraylist	
                    }

                }

            }

            //------- 绘制三角网 -------
            for (int iTri = 0; iTri < arrEdges.Count; iTri++) {
                Edge eg = (Edge)arrEdges[iTri];
                PointF pt1, pt2;
                pt1 = arrDots[eg.Start];
                pt2 = arrDots[eg.End];
                g.DrawLine(Pens.Black, pt1, pt2);
            }
        }
    }


}
