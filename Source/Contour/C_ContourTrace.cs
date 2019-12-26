using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DEVGIS.Contour
{
    public class C_ContourTrace
    {
        //======= 构造函数，传递三角网信息 ====================================
        public C_ContourTrace(C_Trianglate trianglateIN)
        {
            trianglate = trianglateIN;
            triangle = trianglateIN.triangle;
            vertex = trianglateIN.vertex;

            CTrace_Initial();
        }

        //------- 相关属性操作 -------
        int    iNum_ContourLine = 10; // 等值线的数目
        public double d_Max;                 // 等值线最大值
        public double d_Min;                 // 等值线最小值
        double[] dData_CLineValue;    // 等值线具体值构成的数组
        // 等值线对应的颜色
        List<System.Drawing.Color> list_Color = new List<System.Drawing.Color>();

        C_Trianglate trianglate = null; // 三角网信息
        public C_Triangle[] triangle;//= new C_Triangle[5000];
        public C_Vertex[] vertex = new C_Vertex[1000];

        Cmou_Triangle[] arr_Triangle;

        // 所有数据点集
        List<Cmou_Point> list_Point = new List<Cmou_Point>();

        // 临时三角形链表，存放具有等值点的三角形
        //List<Cmou_Triangle> list_TriTemp = new List<Cmou_Triangle>();
        List<Int32> list_ID_TriCheck = new List<Int32>();

        // 存放包含边界的三角形
        //List<Cmou_Triangle> list_TriBand = new List<Cmou_Triangle>();
        List<Int32> list_ID_TriBand = new List<int>();

        // 存放所有三角形
        List<Cmou_Triangle> list_TriTotal = new List<Cmou_Triangle>();

        // 存放所有边界
        List<Cmou_Edge> list_Edge = new List<Cmou_Edge>();

        // 边界边
        List<int> list_ID_EdgeBound = new List<int>();

        // 等值线集合
        public List<Cmou_ContourLine> list_ContourLine = new List<Cmou_ContourLine>();

        //======= 三角形数据结构初始化 ========================================
        // 将由散乱点数据构成的三角网，加以数据结构的处理，使其具有
        // 相应的拓扑关系，方便后面的追踪和填充
        private void CTrace_Initial()
        {
            int iNum_Triangle = trianglate.HowMany;
            if (iNum_Triangle < 2) return;
            arr_Triangle = new Cmou_Triangle[iNum_Triangle];

            for (int iTri = 1; iTri <= iNum_Triangle; iTri++)
            {
                
                // 点处理
                Cmou_Point pTemp0 = new Cmou_Point();                
                pTemp0.X = vertex[triangle[iTri].vv0].x;
                pTemp0.Y = vertex[triangle[iTri].vv0].y;
                pTemp0.V = vertex[triangle[iTri].vv0].z;
                if (CTrace_AssertPoint(ref pTemp0))
                {
                    pTemp0.ID = list_Point.Count;
                    list_Point.Add(pTemp0);
                }
                

                Cmou_Point pTemp1 = new Cmou_Point();
                pTemp1.X = vertex[triangle[iTri].vv1].x;
                pTemp1.Y = vertex[triangle[iTri].vv1].y;
                pTemp1.V = vertex[triangle[iTri].vv1].z;
                if (CTrace_AssertPoint(ref pTemp1))
                {
                    pTemp1.ID = list_Point.Count;
                    list_Point.Add(pTemp1);
                }

                Cmou_Point pTemp2 = new Cmou_Point();                
                pTemp2.X = vertex[triangle[iTri].vv2].x;
                pTemp2.Y = vertex[triangle[iTri].vv2].y;
                pTemp2.V = vertex[triangle[iTri].vv2].z;
                if (CTrace_AssertPoint(ref pTemp2))
                {
                    pTemp2.ID = list_Point.Count;
                    list_Point.Add(pTemp2);
                }

                // 边处理
                Cmou_Edge eTemp0 = new Cmou_Edge();                
                eTemp0.ID_StartPoint = pTemp0.ID;
                eTemp0.ID_EndPoint = pTemp1.ID;
                if(CTrace_AssertEdge(ref eTemp0))
                {
                    eTemp0.ID = list_Edge.Count;
                    list_Edge.Add(eTemp0);
                }
                pTemp0.list_ID_Edge.Add(eTemp0.ID);
                pTemp1.list_ID_Edge.Add(eTemp0.ID);

                Cmou_Edge eTemp1 = new Cmou_Edge();                
                eTemp1.ID_StartPoint = pTemp1.ID;
                eTemp1.ID_EndPoint = pTemp2.ID;                
                if (CTrace_AssertEdge(ref eTemp1))
                {
                    eTemp1.ID = list_Edge.Count;
                    list_Edge.Add(eTemp1);
                }
                pTemp2.list_ID_Edge.Add(eTemp1.ID);
                pTemp1.list_ID_Edge.Add(eTemp1.ID);

                Cmou_Edge eTemp2 = new Cmou_Edge();                
                eTemp2.ID_StartPoint = pTemp2.ID;
                eTemp2.ID_EndPoint = pTemp0.ID;                
                if (CTrace_AssertEdge(ref eTemp2))
                {
                    eTemp2.ID = list_Edge.Count;
                    list_Edge.Add(eTemp2);
                }
                pTemp0.list_ID_Edge.Add(eTemp2.ID);
                pTemp2.list_ID_Edge.Add(eTemp2.ID);

                // 三角形处理
                Cmou_Triangle triTemp = new Cmou_Triangle();
                triTemp.ID = list_TriTotal.Count;
                triTemp.iEdgeIndex[0] = eTemp0.ID;
                triTemp.iEdgeIndex[1] = eTemp1.ID;
                triTemp.iEdgeIndex[2] = eTemp2.ID;
                triTemp.iPointIndex[0] = pTemp0.ID;
                triTemp.iPointIndex[1] = pTemp1.ID;
                triTemp.iPointIndex[2] = pTemp2.ID;
                list_TriTotal.Add(triTemp);

                // 后续处理，双向链接信息
                pTemp0.list_ID_Tri.Add(triTemp.ID);
                pTemp1.list_ID_Tri.Add(triTemp.ID);
                pTemp2.list_ID_Tri.Add(triTemp.ID);
                eTemp0.list_ID_Tri.Add(triTemp.ID);
                eTemp1.list_ID_Tri.Add(triTemp.ID);
                eTemp2.list_ID_Tri.Add(triTemp.ID);
                eTemp0.iNum_TriRef++; // 共边记录增加1
                eTemp1.iNum_TriRef++;
                eTemp2.iNum_TriRef++;

            }

            // 识别出边界上的三角形
            for (int iEdge = 0; iEdge < list_Edge.Count;iEdge++ )
            {
                if (list_Edge[iEdge].iNum_TriRef < 2)
                {
                    //list_ID_TriBand.Add(list_Edge[iEdge].ID);
                    list_ID_EdgeBound.Add(list_Edge[iEdge].ID);
                }
            }

        }

        //======= 重复性处理 ==================================================
        private bool CTrace_AssertPoint(ref Cmou_Point pointIN)
        {
            bool isNewPoint = true;
            for (int iP = 0; iP < list_Point.Count;iP++ )
            {
                if (pointIN.X == list_Point[iP].X && pointIN.Y == list_Point[iP].Y)
                {
                    pointIN = list_Point[iP];
                    isNewPoint = false;
                    break;
                }
            }
            return isNewPoint;
        }

        private bool CTrace_AssertEdge(ref Cmou_Edge edgeIN)
        {
            bool isNewEdge = true;

            for (int iEdge = 0; iEdge < list_Edge.Count;iEdge++ )
            {
                if ((edgeIN.ID_EndPoint ==list_Edge[iEdge].ID_EndPoint && edgeIN.ID_StartPoint == list_Edge[iEdge].ID_StartPoint)
                    ||(edgeIN.ID_EndPoint ==list_Edge[iEdge].ID_StartPoint && edgeIN.ID_StartPoint == list_Edge[iEdge].ID_EndPoint))

                {
                    edgeIN = list_Edge[iEdge];
                    isNewEdge = false;
                    break;
                }
            }
            return isNewEdge;
        }

        //======= 计算等值线的具体数值 ========================================
        // INPUT:
        // iNum_ConLineIN -------------等值线划分的数目
        // OUTPUT:
        // double[]  ------------------等值线具体数值构成的数组
        private void CTrace_ContourLineCal()
        {
            list_Color.Clear();
            int iR = 20;
            int iG = 10;
            int iB = 250;
            System.Drawing.Color temp_C;

            dData_CLineValue = new double[iNum_ContourLine+1];
            for (int i = 0; i <= iNum_ContourLine; i++)
            {
                dData_CLineValue[i] = (d_Max - d_Min) / (double)iNum_ContourLine * (double)i + d_Min;

                temp_C = new System.Drawing.Color();
                temp_C = System.Drawing.Color.FromArgb(iR + i * 20, iG + i * 15, iB - i * 10);  
                list_Color.Add(temp_C);
            }
        }

        //======= 追踪等值线 ==================================================
       
        public void CTrace_ContourLineTrace()
        {
            // 计算出初始的等值线追踪值
            CTrace_ContourLineCal();

            for (int iV = 0; iV < dData_CLineValue.Length;iV++ )
            {
                // 等值线值
                double temp_ConValue = dData_CLineValue[iV];

                //------- 寻找包含有当前值的所有三角形 -------
                list_ID_TriCheck.Clear();
                for (int iTri = 0; iTri < list_TriTotal.Count; iTri++)
                {
                    double temp_V0 = list_Point[list_TriTotal[iTri].iPointIndex[0]].V;
                    double temp_V1 = list_Point[list_TriTotal[iTri].iPointIndex[1]].V;
                    double temp_V2 = list_Point[list_TriTotal[iTri].iPointIndex[2]].V;

                    double temp_0 = (temp_V0 - dData_CLineValue[iV]) * (temp_V1 - dData_CLineValue[iV]);
                    if (temp_0 <= 0) { list_ID_TriCheck.Add(list_TriTotal[iTri].ID); continue; }
                    double temp_1 = (temp_V1 - dData_CLineValue[iV]) * (temp_V2 - dData_CLineValue[iV]);
                    if (temp_1 <= 0) { list_ID_TriCheck.Add(list_TriTotal[iTri].ID); continue; }
                    double temp_2 = (temp_V2 - dData_CLineValue[iV]) * (temp_V0 - dData_CLineValue[iV]);
                    if (temp_2 <= 0) { list_ID_TriCheck.Add(list_TriTotal[iTri].ID); continue; }
                }
                if (list_ID_TriCheck.Count < 2)
                {
                    continue; // 等值边不足两条，放弃搜索
                }

                //------- 追踪非闭合等值线 -------
                for (int iB = 0; iB < list_ID_EdgeBound.Count;iB++ )
                {
                    int temp_IndexEdge = list_ID_EdgeBound[iB];
                    Cmou_ContourLine temp_ConLine = new Cmou_ContourLine(temp_ConValue);
                    // 寻找等值线起点
                    if (CTrace_IsContourEdge(temp_ConValue, temp_IndexEdge))
                    {
                        // 计算等值点
                        Cmou_Point temp_ConPoint = CTrace_ContourPointCal(temp_ConValue,
                            list_Edge[temp_IndexEdge].ID_StartPoint, list_Edge[temp_IndexEdge].ID_EndPoint);
                        temp_ConPoint.list_ID_Edge.Add(temp_IndexEdge);
                        temp_ConLine.list_Point.Add(temp_ConPoint); // 记录等值点

                        while(list_ID_TriCheck.Count > 0)
                        {
                            // 寻找当前等值边对应的三角形
                            Cmou_Triangle temp_NewTri = null;
                            int temp_IndexTri = -1;
                            if (CTrace_FindTriangle(temp_IndexEdge, ref temp_IndexTri))
                            {
                                temp_NewTri = list_TriTotal[temp_IndexTri];
                            }
                            else
                            {
                                // 没有找到对应的三角形，退出
                                break;
                            }

                            // 在新三角形中追踪等值线 
                            int temp_IndexNewEdge = -1;
                            if (CTrace_TraceInTriangle(temp_ConValue, temp_IndexTri, temp_IndexEdge, ref temp_IndexNewEdge))
                            {
                                temp_IndexEdge = temp_IndexNewEdge;
                                if (CTrace_IsContourEdge(temp_ConValue, temp_IndexNewEdge))
                                {
                                    Cmou_Point temp_ConPoint2 = CTrace_ContourPointCal(temp_ConValue,
                                     list_Edge[temp_IndexNewEdge].ID_StartPoint, list_Edge[temp_IndexNewEdge].ID_EndPoint);

                                    temp_ConPoint2.list_ID_Edge.Add(temp_IndexNewEdge);
                                    temp_ConLine.list_Point.Add(temp_ConPoint2); // 记录等值点
                                }
                                Cmou_Edge temp_Edge = list_Edge[temp_IndexNewEdge];
                                if (temp_Edge.iNum_TriRef < 2) // 为边界边，代表等开曲线值线追踪结束
                                {
                                    temp_ConLine.conType = ContourLineType.Opened;
                                    list_ContourLine.Add(temp_ConLine);
                                    temp_ConLine = new Cmou_ContourLine(temp_ConValue);
                                    break; // 退出当前等值线追踪
                                }
                            }
                        }
                        // 只有一个点的情况下，不能执行追踪算法
                        if (list_ID_TriCheck.Count < 1)
                        {
                            break;
                        }
                    }                  
                }

                //------- 追踪封闭的等值线 -------
                ///*
                if (list_ID_TriCheck.Count > 2)
                {
                    int temp_IndexEdge = -1;
                    while (list_ID_TriCheck.Count > 0)
                    {
                        // 将第一个三角形作为搜索的第一个三角形
                        Cmou_Triangle temp_Tri = list_TriTotal[list_ID_TriCheck[0]];
                        list_ID_TriCheck.RemoveAt(0);
                        Cmou_ContourLine temp_ConLine = new Cmou_ContourLine(temp_ConValue);

                        for (int iEdge = 0; iEdge < temp_Tri.iEdgeIndex.Length; iEdge++)
                        {
                            if (CTrace_IsContourEdge(temp_ConValue, temp_Tri.iEdgeIndex[iEdge]))
                            {
                                temp_IndexEdge = temp_Tri.iEdgeIndex[iEdge];

                                Cmou_Point temp_ConPoint2 = CTrace_ContourPointCal(temp_ConValue,
                                     list_Edge[temp_IndexEdge].ID_StartPoint, list_Edge[temp_IndexEdge].ID_EndPoint);

                                temp_ConPoint2.list_ID_Edge.Add(temp_IndexEdge);
                                temp_ConLine.list_Point.Add(temp_ConPoint2); // 记录等值点

                                break;
                            }
                        }
                       

                        while (list_ID_TriCheck.Count > 0)
                        {

                            // 寻找当前等值边对应的三角形
                            Cmou_Triangle temp_NewTri = null;
                            int temp_IndexTri = -1;
                            if (CTrace_FindTriangle(temp_IndexEdge, ref temp_IndexTri))
                            {
                                temp_NewTri = list_TriTotal[temp_IndexTri];
                            }
                            else
                            {
                                // 没有找到对应的三角形，退出
                                temp_ConLine.conType = ContourLineType.Closed;
                                list_ContourLine.Add(temp_ConLine);
                                temp_ConLine = new Cmou_ContourLine(temp_ConValue);
                                break; // 退出当前等值线追踪
                            }

                            // 在新三角形中追踪等值线 
                            int temp_IndexNewEdge = -1;
                            if (CTrace_TraceInTriangle(temp_ConValue, temp_IndexTri, temp_IndexEdge, ref temp_IndexNewEdge))
                            {
                                temp_IndexEdge = temp_IndexNewEdge;
                                if (CTrace_IsContourEdge(temp_ConValue, temp_IndexNewEdge))
                                {
                                    Cmou_Point temp_ConPoint2 = CTrace_ContourPointCal(temp_ConValue,
                                     list_Edge[temp_IndexNewEdge].ID_StartPoint, list_Edge[temp_IndexNewEdge].ID_EndPoint);

                                    temp_ConPoint2.list_ID_Edge.Add(temp_IndexNewEdge);
                                    temp_ConLine.list_Point.Add(temp_ConPoint2); // 记录等值点
                                }
                            }

                            if (list_ID_TriCheck.Count == 0)
                            {
                                temp_ConLine.conType = ContourLineType.Closed;
                                list_ContourLine.Add(temp_ConLine);
                                temp_ConLine = new Cmou_ContourLine(temp_ConValue);
                                break; // 退出当前等值线追踪
                            }
                        }
                    }
                  
                }
                //*/

            }
            

            
        }

        //======= 判断边上是否有等值点 ========================================
        // 判断边上是否存在等值点，如有则返回真，否则为假
        //---------------------------------------------------------------------
        //INPUT:
        //     d_ConValueIN  ---------- 等值线的值
        //     iIndexEdgeIN  ---------- 搜索边的编号
        //OUTPUT:
        //     bool  ------------------ 真假结果，是否存在等值点
        //=====================================================================
        private bool CTrace_IsContourEdge(double d_ConValueIN,int iIndexEdgeIN)
        {
            bool isConPointExist = false;
            Cmou_Edge temp_Edge = list_Edge[iIndexEdgeIN];
            Cmou_Point temp_P1 = list_Point[temp_Edge.ID_StartPoint];
            Cmou_Point temp_P2 = list_Point[temp_Edge.ID_EndPoint];

            double temp_0 = (temp_P1.V - d_ConValueIN) * (temp_P2.V - d_ConValueIN);
            if (temp_0 == 0)
            {
                // 将所有点的值为d_ConValueIN的点进行微调
                double d_Offset = 0.0001;
                for (int iP = 0; iP < list_Point.Count;iP++ )
                {
                    if (Math.Abs(list_Point[iP].V - d_ConValueIN) < 0.00000001)
                    {
                        list_Point[iP].V += d_Offset; 
                    }
                }
               
                isConPointExist = true;
                return isConPointExist;
            }
            else if (temp_0 < 0)
            {
                isConPointExist = true;
                return isConPointExist;
            } 
            else
            {
                return isConPointExist;
            }

           
        }

        //======= 计算等值点坐标 ==============================================
        //
        //---------------------------------------------------------------------
        //INPUT:
        // d_CPointValueIN ---------- 等值点对应的属性值
        // iDPointStartIN ----------- 等值边的起点
        // iDPointEndIN ------------- 等值边的终点
        //OUTPUT:
        // Cmou_Point --------------- 等值点（X,Y,V）
        private Cmou_Point CTrace_ContourPointCal(double d_CPointValueIN,int iDPointStartIN,int iDPointEndIN)
        {
            Cmou_Point temp_Point = new Cmou_Point();
            temp_Point.V = d_CPointValueIN;

            double temp_V0 = list_Point[iDPointStartIN].V;
            double temp_V1 = list_Point[iDPointEndIN].V;
            double temp_0 = (temp_V0 - d_CPointValueIN) * (temp_V1 - d_CPointValueIN);

            double temp = (d_CPointValueIN - temp_V0) / (temp_V1 - temp_V0);

            temp_Point.X = list_Point[iDPointStartIN].X +
                temp * (list_Point[iDPointEndIN].X -
                list_Point[iDPointStartIN].X);

            temp_Point.Y = list_Point[iDPointStartIN].Y +
                temp * (list_Point[iDPointEndIN].Y -
                list_Point[iDPointStartIN].Y);

            return temp_Point;
        }


        //======= 查找边所对应的三角形 ========================================
        // 由给定的边查找与之共边的三角形
        //---------------------------------------------------------------------
        //INPUT:
        // iEdgeIndexIN ------- 当前的共边编号
        //OUTPUT:
        // iTriIndexOUT ------- 返回的另一共边三角形编号
        // bool --------------- 有存在iTriIndexOUT则为真，否则返回假
        //=====================================================================
        private bool CTrace_FindTriangle(int iEdgeIndexIN,ref int iTriIndexOUT)
        {
            bool isExistTri = false;
            Cmou_Edge temp_Edge = list_Edge[iEdgeIndexIN];
            if (temp_Edge.iNum_TriRef < 2)
            {
                iTriIndexOUT = temp_Edge.list_ID_Tri[0];

                // 根据三角形编号删除已经找到的三角形
                for (int iTri = 0; iTri < list_ID_TriCheck.Count;iTri++ )
                {
                    if (list_TriTotal[iTriIndexOUT].ID == list_ID_TriCheck[iTri])
                    {
                        list_ID_TriCheck.RemoveAt(iTri); break;
                    }
                }
                isExistTri = true;
                return isExistTri;
            }
            else
            {
                foreach(int iIndex in temp_Edge.list_ID_Tri)
                {
                    foreach(int iDTri in list_ID_TriCheck)
                    {
                        if (iIndex == iDTri)
                        {
                            iTriIndexOUT = iIndex;
                            // 根据三角形编号删除已经找到的三角形
                            for (int iTri = 0; iTri < list_ID_TriCheck.Count; iTri++)
                            {
                                if (list_TriTotal[iTriIndexOUT].ID == list_ID_TriCheck[iTri])
                                {
                                    list_ID_TriCheck.RemoveAt(iTri); break;
                                }
                            }
                            isExistTri = true;
                            return isExistTri;
                        }
                    }    
                }
            }
            return isExistTri;
        }

        //======= 在三角形中追踪等值线 ========================================
        private bool CTrace_TraceInTriangle(double d_ConValueIN, int iIndexTriIN,int iIndexEdgeIN,ref int iEdgeIndexOUT)
        {
            bool isEdgeExist = false;
            Cmou_Triangle temp_Tri = list_TriTotal[iIndexTriIN];
            List<Cmou_Edge> list_TempEdge = new List<Cmou_Edge>();
            list_TempEdge.Clear();
            list_TempEdge.Add(list_Edge[temp_Tri.iEdgeIndex[0]]);
            list_TempEdge.Add(list_Edge[temp_Tri.iEdgeIndex[1]]);
            list_TempEdge.Add(list_Edge[temp_Tri.iEdgeIndex[2]]);

            foreach (Cmou_Edge tempEdge in list_TempEdge)
            {
                if (tempEdge.ID != iIndexEdgeIN)
                {
                    double temp_0 = (list_Point[tempEdge.ID_StartPoint].V - d_ConValueIN) *
                       (list_Point[tempEdge.ID_EndPoint].V - d_ConValueIN);
                    if (temp_0 <= 0) 
                    { 
                        iEdgeIndexOUT = tempEdge.ID; isEdgeExist = true; 
                        return isEdgeExist; 
                    }
                }
            }

            return isEdgeExist;
        }

        
        //======= 填充等值线 ==================================================
        List<System.Drawing.PointF> list_Pss = new List<System.Drawing.PointF>();
        System.Drawing.PointF[] ps = null;
        System.Drawing.PointF p;
        public void CTrace_Fill(System.Drawing.Graphics g)
        {
            List<Cmou_Point> list_ConPoint = new List<Cmou_Point>();
            foreach (Cmou_Triangle temp_Tri in list_TriTotal)
            {
                // 找出当前三角形中所有的等值点
                list_ConPoint.Clear();
                foreach (int indexEdge in temp_Tri.iEdgeIndex)
                {
                    foreach (Cmou_ContourLine temp_ConLine in list_ContourLine)
                    {
                    //Cmou_ContourLine temp_ConLine = list_ContourLine[0];
                        foreach (Cmou_Point temp_Point in temp_ConLine.list_Point)
                        {
                            if (temp_Point.list_ID_Edge[0] == indexEdge)
                            {
                                list_ConPoint.Add(temp_Point); 
                            }
                            if (temp_Point.list_ID_Edge[0] ==-1)
                            {
                                MessageBox.Show(temp_Tri.ID.ToString());
                            }
                        }
                    }
                }

                // 填充
                if (list_ConPoint.Count > 0)
                {
                    foreach (int indexPoint in temp_Tri.iPointIndex)
                    {
                        list_ConPoint.Add(list_Point[indexPoint]);
                    }
                    // 从小到大排序
                    for (int iP = 0; iP < list_ConPoint.Count; iP++)
                    {
                        for (int iP2 = iP; iP2 < list_ConPoint.Count; iP2++)
                        {
                            Cmou_Point temp_P = null;
                            if (list_ConPoint[iP].V > list_ConPoint[iP2].V)
                            {
                                temp_P = list_ConPoint[iP2];
                                list_ConPoint.RemoveAt(iP2);
                                list_ConPoint.Insert(iP2, list_ConPoint[iP]);
                                list_ConPoint[iP] = temp_P;
                            }
                        }
                    }

                    List<Cmou_ContourLine> list_Ps = new List<Cmou_ContourLine>();
                    List<int> list_IndexRemove = new List<int>();
                    while (list_ConPoint.Count>0)
                    {
                        list_IndexRemove.Clear();
                        Cmou_ContourLine temp_Ps = new Cmou_ContourLine(0);
                        int temp_IndexStart = -1;
                        double temp_ValueStart = 0;
                        double temp_ValueEnd = 0;
                        for (int i = 0; i < list_ConPoint.Count;i++ )
                        {
                            if (i == 0)
                            {
                                temp_IndexStart = CTrace_AssertValue(list_ConPoint[i].V);
                                temp_Ps.list_Point.Add(list_ConPoint[i]);
                                temp_ValueStart = dData_CLineValue[temp_IndexStart];
                                if (temp_IndexStart < dData_CLineValue.Length-1)
                                {
                                    temp_ValueEnd = dData_CLineValue[temp_IndexStart + 1];
                                } 
                                else
                                {
                                    temp_ValueEnd = dData_CLineValue[dData_CLineValue.Length- 1]*2.0;
                                }
                                
                                list_IndexRemove.Add(i);
                            }                           
                            else
                            {
                                if(list_ConPoint[i].V <= temp_ValueEnd && list_ConPoint[i].V >= temp_ValueStart)
                                {
                                    temp_Ps.list_Point.Add(list_ConPoint[i]);

                                    if (list_ConPoint[i].V < temp_ValueEnd)
                                        list_IndexRemove.Add(i);
                                }
                                else
                                {                                   
                                    break;
                                }                               
                            }
                        }

                        list_Ps.Add(temp_Ps);
                        //temp_Ps = new Cmou_ContourLine(0);
                        //删除前一个级别的等值点
                        for (int j = 0; j < list_IndexRemove.Count; j++)
                        {
                            list_ConPoint.RemoveAt(list_IndexRemove[0]);
                        }

                        Cmou_ContourLine temp_Region = temp_Ps;
                        if (temp_Region.list_Point.Count == 3)
                        {
                            list_Pss.Clear();
                            ps = new System.Drawing.PointF[temp_Region.list_Point.Count];
                            for (int ip = 0; ip < temp_Region.list_Point.Count; ip++)
                            {
                                ps[ip].X = Convert.ToSingle(temp_Region.list_Point[ip].X);
                                ps[ip].Y = Convert.ToSingle(temp_Region.list_Point[ip].Y);
                                System.Drawing.PointF p = new System.Drawing.PointF(ps[ip].X, ps[ip].Y);
                                list_Pss.Add(p);
                            }
                            g.FillPolygon(new System.Drawing.SolidBrush(list_Color[temp_IndexStart]), (System.Drawing.PointF[])list_Pss.ToArray());
                        } 
                        else
                        {
                            double[] temp_Data_X = new double[temp_Region.list_Point.Count];
                            double[] temp_Data_Y = new double[temp_Region.list_Point.Count];
                            double[] temp_Data_V = new double[temp_Region.list_Point.Count];

                            for (int ip = 0; ip < temp_Region.list_Point.Count; ip++)
                            {
                                temp_Data_X[ip] = temp_Region.list_Point[ip].X;
                                temp_Data_Y[ip] = temp_Region.list_Point[ip].Y;
                                temp_Data_V[ip] = temp_Region.list_Point[ip].V;
                            }

                            C_Trianglate temp_Trianglate = new C_Trianglate(temp_Data_X, temp_Data_Y, temp_Data_V);
                            temp_Trianglate.Triangulate();
                            temp_Trianglate.filltriangle(g,list_Color[temp_IndexStart]);
                        }

                        temp_Ps = new Cmou_ContourLine(0);
                    }

                    // 填充三角形                   
                    foreach (Cmou_ContourLine temp_Region in list_Ps)
                    {
                        
                    }
                }
                else 
                {
                    list_Pss.Clear();
                    ps = new System.Drawing.PointF[3];
                    foreach (int indexPoint in temp_Tri.iPointIndex)
                    {
                        System.Drawing.PointF p = new System.Drawing.PointF(Convert.ToSingle(list_Point[indexPoint].X),
                            Convert.ToSingle(list_Point[indexPoint].Y));
                        list_Pss.Add(p);                    
                    }
                    int temp_Index = CTrace_AssertValue(list_Point[temp_Tri.iPointIndex[0]].V);
                    g.FillPolygon(new System.Drawing.SolidBrush(
                        list_Color[temp_Index]), 
                        (System.Drawing.PointF[])list_Pss.ToArray());
                }
            }
        }

        //======= 判断等值线之属性值 ==========================================
        private int CTrace_AssertValue(double d_ValueIN)
        {
            int index = -1;
            for(int i=0;i<dData_CLineValue.Length;i++)            
            {
                double temp_V = dData_CLineValue[i];
                if (Math.Abs(d_ValueIN - temp_V)<0.00000001)
                {
                    index = i;
                    break;
                }
                else if (d_ValueIN < temp_V)
                {
                    index = i-1;
                    break;
                }               
            }
            return index;
        }

        //======= 标注边
        public void CTrace_MarkEdge(System.Drawing.Graphics g)
        {
            p = new System.Drawing.PointF();
            foreach (Cmou_Edge e in list_Edge)
            {
                
                p.X = (float)(list_Point[e.ID_StartPoint].X + list_Point[e.ID_EndPoint].X) / 2.0f;
                p.X = (float)(list_Point[e.ID_StartPoint].Y + list_Point[e.ID_EndPoint].Y) / 2.0f;
                g.DrawString(e.ID.ToString(), new System.Drawing.Font("Times New Roman", 7),
                    System.Drawing.Brushes.Yellow, 
                    new System.Drawing.PointF((float)(list_Point[e.ID_StartPoint].X + list_Point[e.ID_EndPoint].X) / 2.0f - 3,
                (float)(list_Point[e.ID_StartPoint].Y + list_Point[e.ID_EndPoint].Y) / 2.0f - 5));
            }
        }

        //======= 标注三角形
        public void CTrace_MarkTriangle(System.Drawing.Graphics g)
        {
            p = new System.Drawing.PointF();
            foreach (Cmou_Triangle t in list_TriTotal)
            {
                Cmou_Edge e = list_Edge[t.iEdgeIndex[0]];
                p.X = (float)(list_Point[e.ID_StartPoint].X + list_Point[e.ID_EndPoint].X) / 2.0f;
                p.X = (float)(list_Point[e.ID_StartPoint].Y + list_Point[e.ID_EndPoint].Y) / 2.0f;
                g.DrawString(t.ID.ToString(), new System.Drawing.Font("Times New Roman", 7),
                    System.Drawing.Brushes.Red,
                    new System.Drawing.PointF((float)(list_Point[e.ID_StartPoint].X + list_Point[e.ID_EndPoint].X) / 2.0f,
                (float)(list_Point[e.ID_StartPoint].Y + list_Point[e.ID_EndPoint].Y) / 2.0f+4));
            }
        }

        //======= 标注等值线值 ================================================
        // 直接标注在等值线中间
        //---------------------------------------------------------------------
        public void CTrace_MarkContourLine(System.Drawing.Graphics g)
        {
            foreach (Cmou_ContourLine temp_ConLine in list_ContourLine)
            {
                for (int i = 0; i < temp_ConLine.list_Point.Count;i++ )
                {
                    if (i == temp_ConLine.list_Point.Count/2)
                    {
                        g.DrawString(
                            temp_ConLine.d_Value.ToString("0.0"),
                            new System.Drawing.Font("Times New Roman", 9),
                            System.Drawing.Brushes.White,
                            new System.Drawing.PointF(
                                (float)temp_ConLine.list_Point[i].X-5, 
                                (float)temp_ConLine.list_Point[i].Y-7)
                            );
                    }
                }
            }
        }


    }
}
