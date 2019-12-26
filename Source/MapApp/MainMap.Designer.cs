namespace Devgis.EagleEye
{
    partial class MainMap
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mapControl1 = new MapInfo.Windows.Controls.MapControl();
            this.mapToolBar1 = new MapInfo.Windows.Controls.MapToolBar();
            this.mapToolBarButtonOpenTable = new MapInfo.Windows.Controls.MapToolBarButton();
            this.mapToolBarButtonLayerControl = new MapInfo.Windows.Controls.MapToolBarButton();
            this.toolBarButtonSeparator = new System.Windows.Forms.ToolBarButton();
            this.mapToolBarButtonSelect = new MapInfo.Windows.Controls.MapToolBarButton();
            this.mapToolBarButtonZoomIn = new MapInfo.Windows.Controls.MapToolBarButton();
            this.mapToolBarButtonZoomOut = new MapInfo.Windows.Controls.MapToolBarButton();
            this.mapToolBarButtonPan = new MapInfo.Windows.Controls.MapToolBarButton();
            this.toolBarButtonSeparator2 = new System.Windows.Forms.ToolBarButton();
            this.mapToolBarButtonSelectRect = new MapInfo.Windows.Controls.MapToolBarButton();
            this.mapToolBarButtonSelectRadius = new MapInfo.Windows.Controls.MapToolBarButton();
            this.toolBarEagle = new System.Windows.Forms.ToolBarButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tslScale = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tslPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mapControl1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.mapControl1, 3);
            this.mapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapControl1.IgnoreLostFocusEvent = false;
            this.mapControl1.Location = new System.Drawing.Point(3, 33);
            this.mapControl1.Name = "mapControl1";
            this.tableLayoutPanel1.SetRowSpan(this.mapControl1, 3);
            this.mapControl1.Size = new System.Drawing.Size(776, 327);
            this.mapControl1.TabIndex = 0;
            this.mapControl1.Text = "mapControl1";
            this.mapControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mapControl1_MouseMove);
            this.mapControl1.Tools.LeftButtonTool = null;
            this.mapControl1.Tools.MiddleButtonTool = null;
            this.mapControl1.Tools.RightButtonTool = null;
            // 
            // mapToolBar1
            // 
            this.mapToolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.mapToolBarButtonOpenTable,
            this.mapToolBarButtonLayerControl,
            this.toolBarButtonSeparator,
            this.mapToolBarButtonSelect,
            this.mapToolBarButtonZoomIn,
            this.mapToolBarButtonZoomOut,
            this.mapToolBarButtonPan,
            this.toolBarButtonSeparator2,
            this.mapToolBarButtonSelectRect,
            this.mapToolBarButtonSelectRadius,
            this.toolBarEagle});
            this.tableLayoutPanel1.SetColumnSpan(this.mapToolBar1, 3);
            this.mapToolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapToolBar1.DropDownArrows = true;
            this.mapToolBar1.Location = new System.Drawing.Point(3, 0);
            this.mapToolBar1.MapControl = this.mapControl1;
            this.mapToolBar1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.mapToolBar1.Name = "mapToolBar1";
            this.mapToolBar1.ShowToolTips = true;
            this.mapToolBar1.Size = new System.Drawing.Size(776, 28);
            this.mapToolBar1.TabIndex = 1;
            // 
            // mapToolBarButtonOpenTable
            // 
            this.mapToolBarButtonOpenTable.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.OpenTable;
            this.mapToolBarButtonOpenTable.Name = "mapToolBarButtonOpenTable";
            this.mapToolBarButtonOpenTable.ToolTipText = "打开表";
            this.mapToolBarButtonOpenTable.Visible = false;
            // 
            // mapToolBarButtonLayerControl
            // 
            this.mapToolBarButtonLayerControl.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.LayerControl;
            this.mapToolBarButtonLayerControl.Name = "mapToolBarButtonLayerControl";
            this.mapToolBarButtonLayerControl.ToolTipText = "图层控制";
            this.mapToolBarButtonLayerControl.Visible = false;
            // 
            // toolBarButtonSeparator
            // 
            this.toolBarButtonSeparator.Name = "toolBarButtonSeparator";
            this.toolBarButtonSeparator.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // mapToolBarButtonSelect
            // 
            this.mapToolBarButtonSelect.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.Select;
            this.mapToolBarButtonSelect.Name = "mapToolBarButtonSelect";
            this.mapToolBarButtonSelect.ToolTipText = "选择";
            // 
            // mapToolBarButtonZoomIn
            // 
            this.mapToolBarButtonZoomIn.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.ZoomIn;
            this.mapToolBarButtonZoomIn.Name = "mapToolBarButtonZoomIn";
            this.mapToolBarButtonZoomIn.ToolTipText = "放大";
            // 
            // mapToolBarButtonZoomOut
            // 
            this.mapToolBarButtonZoomOut.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.ZoomOut;
            this.mapToolBarButtonZoomOut.Name = "mapToolBarButtonZoomOut";
            this.mapToolBarButtonZoomOut.ToolTipText = "缩小";
            // 
            // mapToolBarButtonPan
            // 
            this.mapToolBarButtonPan.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.Pan;
            this.mapToolBarButtonPan.Name = "mapToolBarButtonPan";
            this.mapToolBarButtonPan.ToolTipText = "平移";
            // 
            // toolBarButtonSeparator2
            // 
            this.toolBarButtonSeparator2.Name = "toolBarButtonSeparator2";
            this.toolBarButtonSeparator2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // mapToolBarButtonSelectRect
            // 
            this.mapToolBarButtonSelectRect.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.SelectRectangle;
            this.mapToolBarButtonSelectRect.Name = "mapToolBarButtonSelectRect";
            this.mapToolBarButtonSelectRect.ToolTipText = "矩形选择";
            this.mapToolBarButtonSelectRect.Visible = false;
            // 
            // mapToolBarButtonSelectRadius
            // 
            this.mapToolBarButtonSelectRadius.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.SelectRadius;
            this.mapToolBarButtonSelectRadius.Name = "mapToolBarButtonSelectRadius";
            this.mapToolBarButtonSelectRadius.ToolTipText = "半径选择";
            this.mapToolBarButtonSelectRadius.Visible = false;
            // 
            // toolBarEagle
            // 
            this.toolBarEagle.ImageIndex = 49;
            this.toolBarEagle.Name = "toolBarEagle";
            this.toolBarEagle.ToolTipText = "打开或者关闭鹰眼地图";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslScale,
            this.tslPos});
            this.statusStrip1.Location = new System.Drawing.Point(0, 387);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(782, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tslScale
            // 
            this.tslScale.Name = "tslScale";
            this.tslScale.Size = new System.Drawing.Size(39, 17);
            this.tslScale.Text = "缩放: ";
            this.tslScale.ToolTipText = "地图缩放";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(782, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.mapToolBar1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.mapControl1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(782, 363);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // tslPos
            // 
            this.tslPos.Name = "tslPos";
            this.tslPos.Size = new System.Drawing.Size(98, 17);
            this.tslPos.Text = "位置: X:Na Y:Na";
            // 
            // MainMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 409);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainMap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MapAPP";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainMap_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MapInfo.Windows.Controls.MapControl mapControl1;
        private MapInfo.Windows.Controls.MapToolBar mapToolBar1;
        private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonOpenTable;
        private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonLayerControl;
        private System.Windows.Forms.ToolBarButton toolBarButtonSeparator;
        private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonSelect;
        private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonZoomIn;
        private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonZoomOut;
        private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonPan;
        private System.Windows.Forms.ToolBarButton toolBarButtonSeparator2;
        private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonSelectRect;
        private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonSelectRadius;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tslScale;
        private System.Windows.Forms.ToolBarButton toolBarEagle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tslPos;
    }
}

