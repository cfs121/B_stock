namespace B_stock
{
    partial class Form_major
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.界面ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.入库端ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.出库端ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.确认端ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.测试ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(0, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1900, 1000);
            this.panel1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.界面ToolStripMenuItem,
            this.数据ToolStripMenuItem,
            this.测试ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1902, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // 界面ToolStripMenuItem
            // 
            this.界面ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.入库端ToolStripMenuItem,
            this.出库端ToolStripMenuItem,
            this.确认端ToolStripMenuItem});
            this.界面ToolStripMenuItem.Name = "界面ToolStripMenuItem";
            this.界面ToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.界面ToolStripMenuItem.Text = "界面";
            // 
            // 入库端ToolStripMenuItem
            // 
            this.入库端ToolStripMenuItem.Name = "入库端ToolStripMenuItem";
            this.入库端ToolStripMenuItem.Size = new System.Drawing.Size(216, 26);
            this.入库端ToolStripMenuItem.Text = "入库端";
            // 
            // 出库端ToolStripMenuItem
            // 
            this.出库端ToolStripMenuItem.Name = "出库端ToolStripMenuItem";
            this.出库端ToolStripMenuItem.Size = new System.Drawing.Size(216, 26);
            this.出库端ToolStripMenuItem.Text = "出库端";
            // 
            // 确认端ToolStripMenuItem
            // 
            this.确认端ToolStripMenuItem.Name = "确认端ToolStripMenuItem";
            this.确认端ToolStripMenuItem.Size = new System.Drawing.Size(216, 26);
            this.确认端ToolStripMenuItem.Text = "确认端";
            // 
            // 数据ToolStripMenuItem
            // 
            this.数据ToolStripMenuItem.Name = "数据ToolStripMenuItem";
            this.数据ToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.数据ToolStripMenuItem.Text = "数据";
            // 
            // 测试ToolStripMenuItem
            // 
            this.测试ToolStripMenuItem.Name = "测试ToolStripMenuItem";
            this.测试ToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.测试ToolStripMenuItem.Text = "测试";
            // 
            // Form_major
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1902, 1033);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form_major";
            this.Text = "主界面";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 界面ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 入库端ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 出库端ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 确认端ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 测试ToolStripMenuItem;
    }
}