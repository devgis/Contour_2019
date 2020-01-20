using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DEVGIS.Common;

namespace DEVGIS.MapAPP
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var main = new MainMap();
            if(DateTime.Now.Date>new DateTime(2020,03,01))
            {
                MessageHelper.ShowError("试用版已过期 请联系 devgis@qq.com!获取授权");
                main.Text = "试用版已过期 请联系 devgis@qq.com!获取授权";
            }
            Application.Run(main);
        }
    }
}