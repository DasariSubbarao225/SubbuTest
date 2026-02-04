namespace SharePointListCopyTool
{
    partial class MainForm
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
            this.grpConnection = new System.Windows.Forms.GroupBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtSiteUrl = new System.Windows.Forms.TextBox();
            this.lblSiteUrl = new System.Windows.Forms.Label();
            this.grpOperation = new System.Windows.Forms.GroupBox();
            this.chkSkipEmpty = new System.Windows.Forms.CheckBox();
            this.cmbDestinationColumn = new System.Windows.Forms.ComboBox();
            this.lblDestinationColumn = new System.Windows.Forms.Label();
            this.cmbSourceColumn = new System.Windows.Forms.ComboBox();
            this.lblSourceColumn = new System.Windows.Forms.Label();
            this.cmbLists = new System.Windows.Forms.ComboBox();
            this.lblList = new System.Windows.Forms.Label();
            this.grpProgress = new System.Windows.Forms.GroupBox();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.grpConnection.SuspendLayout();
            this.grpOperation.SuspendLayout();
            this.grpProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpConnection
            // 
            this.grpConnection.Controls.Add(this.btnConnect);
            this.grpConnection.Controls.Add(this.txtSiteUrl);
            this.grpConnection.Controls.Add(this.lblSiteUrl);
            this.grpConnection.Location = new System.Drawing.Point(12, 12);
            this.grpConnection.Name = "grpConnection";
            this.grpConnection.Size = new System.Drawing.Size(760, 80);
            this.grpConnection.TabIndex = 0;
            this.grpConnection.TabStop = false;
            this.grpConnection.Text = "SharePoint Connection";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(650, 37);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(100, 23);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtSiteUrl
            // 
            this.txtSiteUrl.Location = new System.Drawing.Point(90, 39);
            this.txtSiteUrl.Name = "txtSiteUrl";
            this.txtSiteUrl.Size = new System.Drawing.Size(550, 20);
            this.txtSiteUrl.TabIndex = 1;
            this.txtSiteUrl.Text = "https://yourtenant.sharepoint.com/sites/yoursite";
            // 
            // lblSiteUrl
            // 
            this.lblSiteUrl.AutoSize = true;
            this.lblSiteUrl.Location = new System.Drawing.Point(15, 42);
            this.lblSiteUrl.Name = "lblSiteUrl";
            this.lblSiteUrl.Size = new System.Drawing.Size(49, 13);
            this.lblSiteUrl.TabIndex = 0;
            this.lblSiteUrl.Text = "Site URL:";
            // 
            // grpOperation
            // 
            this.grpOperation.Controls.Add(this.chkSkipEmpty);
            this.grpOperation.Controls.Add(this.cmbDestinationColumn);
            this.grpOperation.Controls.Add(this.lblDestinationColumn);
            this.grpOperation.Controls.Add(this.cmbSourceColumn);
            this.grpOperation.Controls.Add(this.lblSourceColumn);
            this.grpOperation.Controls.Add(this.cmbLists);
            this.grpOperation.Controls.Add(this.lblList);
            this.grpOperation.Location = new System.Drawing.Point(12, 98);
            this.grpOperation.Name = "grpOperation";
            this.grpOperation.Size = new System.Drawing.Size(760, 140);
            this.grpOperation.TabIndex = 1;
            this.grpOperation.TabStop = false;
            this.grpOperation.Text = "Copy Operation";
            // 
            // chkSkipEmpty
            // 
            this.chkSkipEmpty.AutoSize = true;
            this.chkSkipEmpty.Checked = true;
            this.chkSkipEmpty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSkipEmpty.Location = new System.Drawing.Point(90, 110);
            this.chkSkipEmpty.Name = "chkSkipEmpty";
            this.chkSkipEmpty.Size = new System.Drawing.Size(208, 17);
            this.chkSkipEmpty.TabIndex = 6;
            this.chkSkipEmpty.Text = "Skip items with empty source columns";
            this.chkSkipEmpty.UseVisualStyleBackColor = true;
            // 
            // cmbDestinationColumn
            // 
            this.cmbDestinationColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDestinationColumn.FormattingEnabled = true;
            this.cmbDestinationColumn.Location = new System.Drawing.Point(130, 78);
            this.cmbDestinationColumn.Name = "cmbDestinationColumn";
            this.cmbDestinationColumn.Size = new System.Drawing.Size(300, 21);
            this.cmbDestinationColumn.TabIndex = 5;
            // 
            // lblDestinationColumn
            // 
            this.lblDestinationColumn.AutoSize = true;
            this.lblDestinationColumn.Location = new System.Drawing.Point(15, 81);
            this.lblDestinationColumn.Name = "lblDestinationColumn";
            this.lblDestinationColumn.Size = new System.Drawing.Size(101, 13);
            this.lblDestinationColumn.TabIndex = 4;
            this.lblDestinationColumn.Text = "Destination Column:";
            // 
            // cmbSourceColumn
            // 
            this.cmbSourceColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSourceColumn.FormattingEnabled = true;
            this.cmbSourceColumn.Location = new System.Drawing.Point(130, 51);
            this.cmbSourceColumn.Name = "cmbSourceColumn";
            this.cmbSourceColumn.Size = new System.Drawing.Size(300, 21);
            this.cmbSourceColumn.TabIndex = 3;
            // 
            // lblSourceColumn
            // 
            this.lblSourceColumn.AutoSize = true;
            this.lblSourceColumn.Location = new System.Drawing.Point(15, 54);
            this.lblSourceColumn.Name = "lblSourceColumn";
            this.lblSourceColumn.Size = new System.Drawing.Size(82, 13);
            this.lblSourceColumn.TabIndex = 2;
            this.lblSourceColumn.Text = "Source Column:";
            // 
            // cmbLists
            // 
            this.cmbLists.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLists.FormattingEnabled = true;
            this.cmbLists.Location = new System.Drawing.Point(130, 24);
            this.cmbLists.Name = "cmbLists";
            this.cmbLists.Size = new System.Drawing.Size(300, 21);
            this.cmbLists.TabIndex = 1;
            this.cmbLists.SelectedIndexChanged += new System.EventHandler(this.cmbLists_SelectedIndexChanged);
            // 
            // lblList
            // 
            this.lblList.AutoSize = true;
            this.lblList.Location = new System.Drawing.Point(15, 27);
            this.lblList.Name = "lblList";
            this.lblList.Size = new System.Drawing.Size(25, 13);
            this.lblList.TabIndex = 0;
            this.lblList.Text = "List:";
            // 
            // grpProgress
            // 
            this.grpProgress.Controls.Add(this.btnClearLog);
            this.grpProgress.Controls.Add(this.txtLog);
            this.grpProgress.Controls.Add(this.progressBar);
            this.grpProgress.Location = new System.Drawing.Point(12, 244);
            this.grpProgress.Name = "grpProgress";
            this.grpProgress.Size = new System.Drawing.Size(760, 250);
            this.grpProgress.TabIndex = 2;
            this.grpProgress.TabStop = false;
            this.grpProgress.Text = "Progress and Logs";
            // 
            // btnClearLog
            // 
            this.btnClearLog.Location = new System.Drawing.Point(650, 19);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(100, 23);
            this.btnClearLog.TabIndex = 2;
            this.btnClearLog.Text = "Clear Log";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.SystemColors.Window;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.Location = new System.Drawing.Point(15, 48);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(735, 190);
            this.txtLog.TabIndex = 1;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(15, 19);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(625, 23);
            this.progressBar.TabIndex = 0;
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(532, 500);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(120, 35);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "Start Copy";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Location = new System.Drawing.Point(658, 500);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(120, 35);
            this.btnStop.TabIndex = 4;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 547);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.grpProgress);
            this.Controls.Add(this.grpOperation);
            this.Controls.Add(this.grpConnection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SharePoint List Column Copy Tool";
            this.grpConnection.ResumeLayout(false);
            this.grpConnection.PerformLayout();
            this.grpOperation.ResumeLayout(false);
            this.grpOperation.PerformLayout();
            this.grpProgress.ResumeLayout(false);
            this.grpProgress.ResumeLayout(false);
            this.grpProgress.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpConnection;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtSiteUrl;
        private System.Windows.Forms.Label lblSiteUrl;
        private System.Windows.Forms.GroupBox grpOperation;
        private System.Windows.Forms.ComboBox cmbDestinationColumn;
        private System.Windows.Forms.Label lblDestinationColumn;
        private System.Windows.Forms.ComboBox cmbSourceColumn;
        private System.Windows.Forms.Label lblSourceColumn;
        private System.Windows.Forms.ComboBox cmbLists;
        private System.Windows.Forms.Label lblList;
        private System.Windows.Forms.GroupBox grpProgress;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.CheckBox chkSkipEmpty;
        private System.Windows.Forms.Button btnClearLog;
    }
}
