namespace PluginInstaller
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.buttonInstall = new System.Windows.Forms.Button();
            this.flowLayoutPanelPlugins = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // buttonInstall
            // 
            this.buttonInstall.Location = new System.Drawing.Point(415, 265);
            this.buttonInstall.Name = "buttonInstall";
            this.buttonInstall.Size = new System.Drawing.Size(75, 23);
            this.buttonInstall.TabIndex = 1;
            this.buttonInstall.Text = "Установить";
            this.buttonInstall.UseVisualStyleBackColor = true;
            this.buttonInstall.Click += new System.EventHandler(this.buttonInstall_Click);
            // 
            // flowLayoutPanelPlugins
            // 
            this.flowLayoutPanelPlugins.AutoScroll = true;
            this.flowLayoutPanelPlugins.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanelPlugins.Name = "flowLayoutPanelPlugins";
            this.flowLayoutPanelPlugins.Size = new System.Drawing.Size(478, 245);
            this.flowLayoutPanelPlugins.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 300);
            this.Controls.Add(this.flowLayoutPanelPlugins);
            this.Controls.Add(this.buttonInstall);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle; 
            this.MaximizeBox = false; 
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Установщик плагинов";
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button buttonInstall;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelPlugins;
    }
}