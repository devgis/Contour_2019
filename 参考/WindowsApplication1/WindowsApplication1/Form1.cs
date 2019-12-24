using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;


using System.Runtime.InteropServices;

/*
[StructLayout(LayoutKind.Sequential)]
class  POINT
{
    public long  x=0;
    public long  y=0;
}

class Apis
{
    [DllImport("USER32.DLL", EntryPoint = "GetDC")]
    public static extern IntPtr GetDC(IntPtr hwnd);

    [DllImport("USER32.DLL", EntryPoint = "ReleaseDC")]
    public static extern IntPtr ReleaseDC(IntPtr hwnd, IntPtr hdc);

    [DllImport("GDI32.DLL", EntryPoint = "MoveToEx")]
    public static extern IntPtr MoveToEx(IntPtr hdc, int x, int y,  POINT p);
    [DllImport("GDI32.DLL", EntryPoint = "TextOut")]
    public static extern IntPtr TextOut(IntPtr hdc,
        int nXStart,       // x-coordinate of starting position
       int nYStart,       // y-coordinate of starting position
      char[]  lpString,  // character string
     int cbString      // number of characters
);

}*/
namespace WindowsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        void SetContourLinesColor()
        {
            ;
            // change color from red to green,then from green to blue
            ContourOCX1.ClearColorClass();
            ContourOCX1.AddNewElementToColorClass(0xff0000);
            ContourOCX1.AddNewElementToColorClass(0x00ff00);
            ContourOCX1.AddNewElementToColorClass(0x0000ff);

            //ContourOCX1.SetDefaultZValueRange 1, 10
            ContourOCX1.ResetContoursColor();
            /*				int colorInt;
            char* color=(char*)&colorInt;
            color[3]=0;color[2]=255;color[1]=ValidateData(value * 25);color[0]=ValidateData(255 -value * 25);
            axContourOCX1->SetLineColor(i, colorInt);
            } 
            */
        }
        void SetContourPolysColor()
        {
            ;
            // change color from red to green,then from green to blue
            ContourOCX1.ClearColorClass();
            ContourOCX1.AddNewElementToColorClass(0xff0000);
            ContourOCX1.AddNewElementToColorClass(0x00ff00);
            ContourOCX1.AddNewElementToColorClass(0x0000ff);

            //ContourOCX1.SetDefaultZValueRange 1, 10
            ContourOCX1.ResetPolygonsColor(-1);
            /*				int colorInt;
            char* color=(char*)&colorInt;
            color[3]=0;color[2]=255;color[1]=ValidateData(value * 25);color[0]=ValidateData(255 -value * 25);
            axContourOCX1->SetLineColor(i, colorInt);
            } 
            */
        }
        private void button1_Click(object sender, EventArgs e)
        {

            /*   IntPtr hdc = Apis.GetDC(this.Handle); ;
                POINT p=new POINT ();
                int x = 0, y = 0;
                Apis.MoveToEx(hdc, x, y,p);


                char[] s =  {'t','e','s','t'};
            Apis.TextOut(hdc,100,100,s,s.Length);
            Apis.ReleaseDC(this.Handle, hdc);*/


            pictureBox1.Refresh(); 
            ContourOCX1.FreeData();
            ContourOCX1.SetDefaultPositionValus(pictureBox1.Width, pictureBox1.Height, 300, 600, 100, 600);

       

            //Smooth Parameter
            int Smooth = 2;
            //line step
            float linestep = 1;
            ContourOCX1.InitialRandomCFWAI(-1, linestep, Smooth);
            Random random = new Random();

            int PointCount;
            //PointCount=int::Parse(this->CountTextBox->Text) ;
       

            double X, Y, value;
           /* for (int i = 0; i < 100; i++)
            {
                X = random.NextDouble() * 300;
                Y = random.NextDouble() * 300;// 
                value = random.NextDouble() * 10;
                ContourOCX1.AddPointRandom(X, Y, value);
                //point position and value
            }*/


            string path=System.Windows.Forms.Application.StartupPath + "\\data.txt";
            PointCount=ContourOCX1.AddRandomPointsFromFile(ref path);



            ContourOCX1.CalculateRandom();

            //draw

            SetContourLinesColor();
            //set position values
            ContourOCX1.ResetContourPosition();

            this.pictureBox1.Refresh();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int suc = new int();
            ContourOCX1.SetPolygonsTransparent((float)0.2);
            
            ContourOCX1.ConvertToPolygon(ref suc);
            ContourOCX1.ResetContourPosition();
            ContourOCX1.ResetPolyPosition();
//            Graphics graphic = Graphics.FromHwnd(pictureBox1.Handle);
  //          IntPtr dc = graphic.GetHdc();
            SetContourPolysColor();
          //  ContourOCX1.DrawAllPolygons(dc.ToInt32());
           // graphic.ReleaseHdc();
            this.pictureBox1.Refresh();
        }
        private void DrawLabels(PaintEventArgs e)
        {
            int lineCount=new int();
            int pointCount=new int();
            double x=new double ();
            double  y=new double();
            double value = new double();
            int scrX=new int();
            int  scrY=new int();
            ContourOCX1.GetLineCount(ref lineCount);
     
            for (int i = 0; i < lineCount; i++)
            {
                ContourOCX1.GetCtrlPointCount(i, ref pointCount);
                ContourOCX1.GetCtrlPoint(i, pointCount / 2, ref x, ref y, ref value);
                ContourOCX1.GetScrPosFromRealPos((float)x,(float) y, ref scrX, ref scrY);
                e.Graphics.DrawString(value.ToString(),new Font("Verdana",10),  new SolidBrush(Color.Tomato),scrX-2,scrY-8);
            }

        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
             DrawLabels(e);
            
            IntPtr ptr=e.Graphics.GetHdc();
            if (radioButton1.Checked)
            {
                ContourOCX1.DrawContours(ptr.ToInt32());
               
            }
            else
                 ContourOCX1.DrawAllPolygons(ptr.ToInt32());

                ;
            e.Graphics.ReleaseHdc();
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}