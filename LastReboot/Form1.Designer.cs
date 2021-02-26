namespace LastReboot
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.AssetSearch = new System.Windows.Forms.TextBox();
            this.ButtonPCLookup = new System.Windows.Forms.Button();
            this.PCInfoLabel = new System.Windows.Forms.Label();
            this.small_wait_dance = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.small_wait_dance)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // AssetSearch
            // 
            this.AssetSearch.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AssetSearch.Location = new System.Drawing.Point(13, 13);
            this.AssetSearch.Name = "AssetSearch";
            this.AssetSearch.Size = new System.Drawing.Size(137, 26);
            this.AssetSearch.TabIndex = 0;
            this.AssetSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AssetSearch_KeyDown);
            // 
            // ButtonPCLookup
            // 
            this.ButtonPCLookup.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonPCLookup.Location = new System.Drawing.Point(156, 13);
            this.ButtonPCLookup.Name = "ButtonPCLookup";
            this.ButtonPCLookup.Size = new System.Drawing.Size(75, 26);
            this.ButtonPCLookup.TabIndex = 1;
            this.ButtonPCLookup.Text = "PC Lookup";
            this.ButtonPCLookup.UseVisualStyleBackColor = true;
            this.ButtonPCLookup.Click += new System.EventHandler(this.ButtonPCLookup_Click);
            // 
            // PCInfoLabel
            // 
            this.PCInfoLabel.BackColor = System.Drawing.Color.Gainsboro;
            this.PCInfoLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PCInfoLabel.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PCInfoLabel.Location = new System.Drawing.Point(0, 0);
            this.PCInfoLabel.Name = "PCInfoLabel";
            this.PCInfoLabel.Size = new System.Drawing.Size(218, 204);
            this.PCInfoLabel.TabIndex = 3;
            this.PCInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // small_wait_dance
            // 
            this.small_wait_dance.Image = ((System.Drawing.Image)(resources.GetObject("small_wait_dance.Image")));
            this.small_wait_dance.Location = new System.Drawing.Point(94, 65);
            this.small_wait_dance.Name = "small_wait_dance";
            this.small_wait_dance.Size = new System.Drawing.Size(35, 64);
            this.small_wait_dance.TabIndex = 4;
            this.small_wait_dance.TabStop = false;
            this.small_wait_dance.Visible = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.small_wait_dance);
            this.panel1.Controls.Add(this.PCInfoLabel);
            this.panel1.Location = new System.Drawing.Point(13, 42);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(218, 204);
            this.panel1.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(243, 251);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ButtonPCLookup);
            this.Controls.Add(this.AssetSearch);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "PC Info";
            ((System.ComponentModel.ISupportInitialize)(this.small_wait_dance)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox AssetSearch;
        private System.Windows.Forms.Button ButtonPCLookup;
        private System.Windows.Forms.Label PCInfoLabel;
        private System.Windows.Forms.PictureBox small_wait_dance;
        private System.Windows.Forms.Panel panel1;
    }
}

