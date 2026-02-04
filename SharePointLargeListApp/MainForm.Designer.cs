namespace SharePointLargeListApp
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.grpConnection = new System.Windows.Forms.GroupBox();
            this.txtSiteUrl = new System.Windows.Forms.TextBox();
            this.lblSiteUrl = new System.Windows.Forms.Label();
            this.txtListName = new System.Windows.Forms.TextBox();
            this.lblListName = new System.Windows.Forms.Label();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.lblConnectionStatus = new System.Windows.Forms.Label();
            
            this.grpAuthentication = new System.Windows.Forms.GroupBox();
            this.rbModernAuth = new System.Windows.Forms.RadioButton();
            this.rbInteractive = new System.Windows.Forms.RadioButton();
            this.rbUsernamePassword = new System.Windows.Forms.RadioButton();
            this.rbClientSecret = new System.Windows.Forms.RadioButton();
            this.pnlUsernamePassword = new System.Windows.Forms.Panel();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.pnlModernAuth = new System.Windows.Forms.Panel();
            this.txtClientId = new System.Windows.Forms.TextBox();
            this.lblClientId = new System.Windows.Forms.Label();
            this.txtTenantId = new System.Windows.Forms.TextBox();
            this.lblTenantId = new System.Windows.Forms.Label();
            this.pnlClientSecret = new System.Windows.Forms.Panel();
            this.txtClientIdSecret = new System.Windows.Forms.TextBox();
            this.lblClientIdSecret = new System.Windows.Forms.Label();
            this.txtClientSecret = new System.Windows.Forms.TextBox();
            this.lblClientSecret = new System.Windows.Forms.Label();
            this.txtTenantIdSecret = new System.Windows.Forms.TextBox();
            this.lblTenantIdSecret = new System.Windows.Forms.Label();

            this.grpColumns = new System.Windows.Forms.GroupBox();
            this.txtCalculatedColumn = new System.Windows.Forms.TextBox();
            this.lblCalculatedColumn = new System.Windows.Forms.Label();
            this.txtTargetColumn = new System.Windows.Forms.TextBox();
            this.lblTargetColumn = new System.Windows.Forms.Label();

            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.numBatchSize = new System.Windows.Forms.NumericUpDown();
            this.lblBatchSize = new System.Windows.Forms.Label();
            this.numUpdateBatchSize = new System.Windows.Forms.NumericUpDown();
            this.lblUpdateBatchSize = new System.Windows.Forms.Label();
            this.chkUseIdRange = new System.Windows.Forms.CheckBox();

            this.grpProgress = new System.Windows.Forms.GroupBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblProgress = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();

            this.btnProcess = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnExportFailures = new System.Windows.Forms.Button();
            this.btnOpenLog = new System.Windows.Forms.Button();

            this.grpConnection.SuspendLayout();
            this.grpAuthentication.SuspendLayout();
            this.pnlUsernamePassword.SuspendLayout();
            this.pnlModernAuth.SuspendLayout();
            this.pnlClientSecret.SuspendLayout();
            this.grpColumns.SuspendLayout();
            this.grpSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBatchSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpdateBatchSize)).BeginInit();
            this.grpProgress.SuspendLayout();
            this.SuspendLayout();

            // 
            // grpConnection
            // 
            this.grpConnection.Controls.Add(this.lblConnectionStatus);
            this.grpConnection.Controls.Add(this.btnTestConnection);
            this.grpConnection.Controls.Add(this.txtListName);
            this.grpConnection.Controls.Add(this.lblListName);
            this.grpConnection.Controls.Add(this.txtSiteUrl);
            this.grpConnection.Controls.Add(this.lblSiteUrl);
            this.grpConnection.Location = new System.Drawing.Point(12, 12);
            this.grpConnection.Name = "grpConnection";
            this.grpConnection.Size = new System.Drawing.Size(760, 100);
            this.grpConnection.TabIndex = 0;
            this.grpConnection.TabStop = false;
            this.grpConnection.Text = "SharePoint Connection";

            // 
            // lblSiteUrl
            // 
            this.lblSiteUrl.AutoSize = true;
            this.lblSiteUrl.Location = new System.Drawing.Point(15, 25);
            this.lblSiteUrl.Name = "lblSiteUrl";
            this.lblSiteUrl.Size = new System.Drawing.Size(55, 15);
            this.lblSiteUrl.TabIndex = 0;
            this.lblSiteUrl.Text = "Site URL:";

            // 
            // txtSiteUrl
            // 
            this.txtSiteUrl.Location = new System.Drawing.Point(120, 22);
            this.txtSiteUrl.Name = "txtSiteUrl";
            this.txtSiteUrl.Size = new System.Drawing.Size(450, 23);
            this.txtSiteUrl.TabIndex = 1;
            this.txtSiteUrl.Text = "https://yourtenant.sharepoint.com/sites/yoursite";

            // 
            // lblListName
            // 
            this.lblListName.AutoSize = true;
            this.lblListName.Location = new System.Drawing.Point(15, 54);
            this.lblListName.Name = "lblListName";
            this.lblListName.Size = new System.Drawing.Size(63, 15);
            this.lblListName.TabIndex = 2;
            this.lblListName.Text = "List Name:";

            // 
            // txtListName
            // 
            this.txtListName.Location = new System.Drawing.Point(120, 51);
            this.txtListName.Name = "txtListName";
            this.txtListName.Size = new System.Drawing.Size(300, 23);
            this.txtListName.TabIndex = 3;

            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Location = new System.Drawing.Point(590, 22);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(150, 50);
            this.btnTestConnection.TabIndex = 4;
            this.btnTestConnection.Text = "Test Connection";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);

            // 
            // lblConnectionStatus
            // 
            this.lblConnectionStatus.AutoSize = true;
            this.lblConnectionStatus.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblConnectionStatus.Location = new System.Drawing.Point(440, 54);
            this.lblConnectionStatus.Name = "lblConnectionStatus";
            this.lblConnectionStatus.Size = new System.Drawing.Size(0, 15);
            this.lblConnectionStatus.TabIndex = 5;

            // 
            // grpAuthentication
            // 
            this.grpAuthentication.Controls.Add(this.pnlClientSecret);
            this.grpAuthentication.Controls.Add(this.pnlModernAuth);
            this.grpAuthentication.Controls.Add(this.pnlUsernamePassword);
            this.grpAuthentication.Controls.Add(this.rbClientSecret);
            this.grpAuthentication.Controls.Add(this.rbUsernamePassword);
            this.grpAuthentication.Controls.Add(this.rbInteractive);
            this.grpAuthentication.Controls.Add(this.rbModernAuth);
            this.grpAuthentication.Location = new System.Drawing.Point(12, 118);
            this.grpAuthentication.Name = "grpAuthentication";
            this.grpAuthentication.Size = new System.Drawing.Size(760, 210);
            this.grpAuthentication.TabIndex = 1;
            this.grpAuthentication.TabStop = false;
            this.grpAuthentication.Text = "Authentication";

            // 
            // rbInteractive
            // 
            this.rbInteractive.AutoSize = true;
            this.rbInteractive.Checked = true;
            this.rbInteractive.Location = new System.Drawing.Point(15, 25);
            this.rbInteractive.Name = "rbInteractive";
            this.rbInteractive.Size = new System.Drawing.Size(280, 19);
            this.rbInteractive.TabIndex = 0;
            this.rbInteractive.TabStop = true;
            this.rbInteractive.Text = "Interactive Browser Login (Recommended - No setup needed)";
            this.rbInteractive.UseVisualStyleBackColor = true;
            this.rbInteractive.CheckedChanged += new System.EventHandler(this.AuthMethod_CheckedChanged);

            // 
            // rbModernAuth
            // 
            this.rbModernAuth.AutoSize = true;
            this.rbModernAuth.Location = new System.Drawing.Point(15, 50);
            this.rbModernAuth.Name = "rbModernAuth";
            this.rbModernAuth.Size = new System.Drawing.Size(250, 19);
            this.rbModernAuth.TabIndex = 1;
            this.rbModernAuth.Text = "Modern Auth (MSAL - Custom Azure App)";
            this.rbModernAuth.UseVisualStyleBackColor = true;
            this.rbModernAuth.CheckedChanged += new System.EventHandler(this.AuthMethod_CheckedChanged);

            // 
            // rbUsernamePassword
            // 
            this.rbUsernamePassword.AutoSize = true;
            this.rbUsernamePassword.Location = new System.Drawing.Point(15, 115);
            this.rbUsernamePassword.Name = "rbUsernamePassword";
            this.rbUsernamePassword.Size = new System.Drawing.Size(197, 19);
            this.rbUsernamePassword.TabIndex = 2;
            this.rbUsernamePassword.Text = "Username/Password (Legacy)";
            this.rbUsernamePassword.UseVisualStyleBackColor = true;
            this.rbUsernamePassword.CheckedChanged += new System.EventHandler(this.AuthMethod_CheckedChanged);

            // 
            // rbClientSecret
            // 
            this.rbClientSecret.AutoSize = true;
            this.rbClientSecret.Location = new System.Drawing.Point(15, 160);
            this.rbClientSecret.Name = "rbClientSecret";
            this.rbClientSecret.Size = new System.Drawing.Size(171, 19);
            this.rbClientSecret.TabIndex = 3;
            this.rbClientSecret.Text = "App-Only (Client Secret)";
            this.rbClientSecret.UseVisualStyleBackColor = true;
            this.rbClientSecret.CheckedChanged += new System.EventHandler(this.AuthMethod_CheckedChanged);

            // 
            // pnlModernAuth
            // 
            this.pnlModernAuth.Controls.Add(this.txtTenantId);
            this.pnlModernAuth.Controls.Add(this.lblTenantId);
            this.pnlModernAuth.Controls.Add(this.txtClientId);
            this.pnlModernAuth.Controls.Add(this.lblClientId);
            this.pnlModernAuth.Location = new System.Drawing.Point(35, 70);
            this.pnlModernAuth.Name = "pnlModernAuth";
            this.pnlModernAuth.Size = new System.Drawing.Size(350, 60);
            this.pnlModernAuth.TabIndex = 3;
            this.pnlModernAuth.Visible = false;

            // 
            // lblClientId
            // 
            this.lblClientId.AutoSize = true;
            this.lblClientId.Location = new System.Drawing.Point(3, 8);
            this.lblClientId.Name = "lblClientId";
            this.lblClientId.Size = new System.Drawing.Size(59, 15);
            this.lblClientId.TabIndex = 0;
            this.lblClientId.Text = "Client ID:";

            // 
            // txtClientId
            // 
            this.txtClientId.Location = new System.Drawing.Point(90, 5);
            this.txtClientId.Name = "txtClientId";
            this.txtClientId.Size = new System.Drawing.Size(250, 23);
            this.txtClientId.TabIndex = 1;

            // 
            // lblTenantId
            // 
            this.lblTenantId.AutoSize = true;
            this.lblTenantId.Location = new System.Drawing.Point(3, 37);
            this.lblTenantId.Name = "lblTenantId";
            this.lblTenantId.Size = new System.Drawing.Size(61, 15);
            this.lblTenantId.TabIndex = 2;
            this.lblTenantId.Text = "Tenant ID:";

            // 
            // txtTenantId
            // 
            this.txtTenantId.Location = new System.Drawing.Point(90, 34);
            this.txtTenantId.Name = "txtTenantId";
            this.txtTenantId.Size = new System.Drawing.Size(250, 23);
            this.txtTenantId.TabIndex = 3;

            // 
            // pnlUsernamePassword
            // 
            this.pnlUsernamePassword.Controls.Add(this.txtPassword);
            this.pnlUsernamePassword.Controls.Add(this.lblPassword);
            this.pnlUsernamePassword.Controls.Add(this.txtUsername);
            this.pnlUsernamePassword.Controls.Add(this.lblUsername);
            this.pnlUsernamePassword.Location = new System.Drawing.Point(35, 135);
            this.pnlUsernamePassword.Name = "pnlUsernamePassword";
            this.pnlUsernamePassword.Size = new System.Drawing.Size(350, 60);
            this.pnlUsernamePassword.TabIndex = 4;
            this.pnlUsernamePassword.Visible = false;

            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(3, 8);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(63, 15);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "Username:";

            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(90, 5);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(250, 23);
            this.txtUsername.TabIndex = 1;

            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(3, 37);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(60, 15);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Password:";

            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(90, 34);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(250, 23);
            this.txtPassword.TabIndex = 3;

            // 
            // pnlClientSecret
            // 
            this.pnlClientSecret.Controls.Add(this.txtTenantIdSecret);
            this.pnlClientSecret.Controls.Add(this.lblTenantIdSecret);
            this.pnlClientSecret.Controls.Add(this.txtClientSecret);
            this.pnlClientSecret.Controls.Add(this.lblClientSecret);
            this.pnlClientSecret.Controls.Add(this.txtClientIdSecret);
            this.pnlClientSecret.Controls.Add(this.lblClientIdSecret);
            this.pnlClientSecret.Location = new System.Drawing.Point(35, 180);
            this.pnlClientSecret.Name = "pnlClientSecret";
            this.pnlClientSecret.Size = new System.Drawing.Size(350, 90);
            this.pnlClientSecret.TabIndex = 5;
            this.pnlClientSecret.Visible = false;

            // 
            // lblClientIdSecret
            // 
            this.lblClientIdSecret.AutoSize = true;
            this.lblClientIdSecret.Location = new System.Drawing.Point(3, 8);
            this.lblClientIdSecret.Name = "lblClientIdSecret";
            this.lblClientIdSecret.Size = new System.Drawing.Size(59, 15);
            this.lblClientIdSecret.TabIndex = 0;
            this.lblClientIdSecret.Text = "Client ID:";

            // 
            // txtClientIdSecret
            // 
            this.txtClientIdSecret.Location = new System.Drawing.Point(90, 5);
            this.txtClientIdSecret.Name = "txtClientIdSecret";
            this.txtClientIdSecret.Size = new System.Drawing.Size(250, 23);
            this.txtClientIdSecret.TabIndex = 1;

            // 
            // lblClientSecret
            // 
            this.lblClientSecret.AutoSize = true;
            this.lblClientSecret.Location = new System.Drawing.Point(3, 37);
            this.lblClientSecret.Name = "lblClientSecret";
            this.lblClientSecret.Size = new System.Drawing.Size(82, 15);
            this.lblClientSecret.TabIndex = 2;
            this.lblClientSecret.Text = "Client Secret:";

            // 
            // txtClientSecret
            // 
            this.txtClientSecret.Location = new System.Drawing.Point(90, 34);
            this.txtClientSecret.Name = "txtClientSecret";
            this.txtClientSecret.PasswordChar = '*';
            this.txtClientSecret.Size = new System.Drawing.Size(250, 23);
            this.txtClientSecret.TabIndex = 3;

            // 
            // lblTenantIdSecret
            // 
            this.lblTenantIdSecret.AutoSize = true;
            this.lblTenantIdSecret.Location = new System.Drawing.Point(3, 66);
            this.lblTenantIdSecret.Name = "lblTenantIdSecret";
            this.lblTenantIdSecret.Size = new System.Drawing.Size(61, 15);
            this.lblTenantIdSecret.TabIndex = 4;
            this.lblTenantIdSecret.Text = "Tenant ID:";

            // 
            // txtTenantIdSecret
            // 
            this.txtTenantIdSecret.Location = new System.Drawing.Point(90, 63);
            this.txtTenantIdSecret.Name = "txtTenantIdSecret";
            this.txtTenantIdSecret.Size = new System.Drawing.Size(250, 23);
            this.txtTenantIdSecret.TabIndex = 5;

            // 
            // grpColumns
            // 
            this.grpColumns.Controls.Add(this.txtTargetColumn);
            this.grpColumns.Controls.Add(this.lblTargetColumn);
            this.grpColumns.Controls.Add(this.txtCalculatedColumn);
            this.grpColumns.Controls.Add(this.lblCalculatedColumn);
            this.grpColumns.Location = new System.Drawing.Point(12, 334);
            this.grpColumns.Name = "grpColumns";
            this.grpColumns.Size = new System.Drawing.Size(760, 90);
            this.grpColumns.TabIndex = 2;
            this.grpColumns.TabStop = false;
            this.grpColumns.Text = "Column Configuration";

            // 
            // lblCalculatedColumn
            // 
            this.lblCalculatedColumn.AutoSize = true;
            this.lblCalculatedColumn.Location = new System.Drawing.Point(15, 25);
            this.lblCalculatedColumn.Name = "lblCalculatedColumn";
            this.lblCalculatedColumn.Size = new System.Drawing.Size(114, 15);
            this.lblCalculatedColumn.TabIndex = 0;
            this.lblCalculatedColumn.Text = "Calculated Column:";

            // 
            // txtCalculatedColumn
            // 
            this.txtCalculatedColumn.Location = new System.Drawing.Point(160, 22);
            this.txtCalculatedColumn.Name = "txtCalculatedColumn";
            this.txtCalculatedColumn.Size = new System.Drawing.Size(250, 23);
            this.txtCalculatedColumn.TabIndex = 1;

            // 
            // lblTargetColumn
            // 
            this.lblTargetColumn.AutoSize = true;
            this.lblTargetColumn.Location = new System.Drawing.Point(15, 54);
            this.lblTargetColumn.Name = "lblTargetColumn";
            this.lblTargetColumn.Size = new System.Drawing.Size(89, 15);
            this.lblTargetColumn.TabIndex = 2;
            this.lblTargetColumn.Text = "Target Column:";

            // 
            // txtTargetColumn
            // 
            this.txtTargetColumn.Location = new System.Drawing.Point(160, 51);
            this.txtTargetColumn.Name = "txtTargetColumn";
            this.txtTargetColumn.Size = new System.Drawing.Size(250, 23);
            this.txtTargetColumn.TabIndex = 3;

            // 
            // grpSettings
            // 
            this.grpSettings.Controls.Add(this.chkUseIdRange);
            this.grpSettings.Controls.Add(this.numUpdateBatchSize);
            this.grpSettings.Controls.Add(this.lblUpdateBatchSize);
            this.grpSettings.Controls.Add(this.numBatchSize);
            this.grpSettings.Controls.Add(this.lblBatchSize);
            this.grpSettings.Location = new System.Drawing.Point(12, 430);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(760, 80);
            this.grpSettings.TabIndex = 3;
            this.grpSettings.TabStop = false;
            this.grpSettings.Text = "Processing Settings";

            // 
            // lblBatchSize
            // 
            this.lblBatchSize.AutoSize = true;
            this.lblBatchSize.Location = new System.Drawing.Point(15, 25);
            this.lblBatchSize.Name = "lblBatchSize";
            this.lblBatchSize.Size = new System.Drawing.Size(130, 15);
            this.lblBatchSize.TabIndex = 0;
            this.lblBatchSize.Text = "Retrieve Batch Size:";

            // 
            // numBatchSize
            // 
            this.numBatchSize.Increment = new decimal(new int[] { 500, 0, 0, 0 });
            this.numBatchSize.Location = new System.Drawing.Point(160, 23);
            this.numBatchSize.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            this.numBatchSize.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            this.numBatchSize.Name = "numBatchSize";
            this.numBatchSize.Size = new System.Drawing.Size(100, 23);
            this.numBatchSize.TabIndex = 1;
            this.numBatchSize.Value = new decimal(new int[] { 2000, 0, 0, 0 });

            // 
            // lblUpdateBatchSize
            // 
            this.lblUpdateBatchSize.AutoSize = true;
            this.lblUpdateBatchSize.Location = new System.Drawing.Point(15, 54);
            this.lblUpdateBatchSize.Name = "lblUpdateBatchSize";
            this.lblUpdateBatchSize.Size = new System.Drawing.Size(121, 15);
            this.lblUpdateBatchSize.TabIndex = 2;
            this.lblUpdateBatchSize.Text = "Update Batch Size:";

            // 
            // numUpdateBatchSize
            // 
            this.numUpdateBatchSize.Increment = new decimal(new int[] { 50, 0, 0, 0 });
            this.numUpdateBatchSize.Location = new System.Drawing.Point(160, 52);
            this.numUpdateBatchSize.Maximum = new decimal(new int[] { 500, 0, 0, 0 });
            this.numUpdateBatchSize.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            this.numUpdateBatchSize.Name = "numUpdateBatchSize";
            this.numUpdateBatchSize.Size = new System.Drawing.Size(100, 23);
            this.numUpdateBatchSize.TabIndex = 3;
            this.numUpdateBatchSize.Value = new decimal(new int[] { 100, 0, 0, 0 });

            // 
            // chkUseIdRange
            // 
            this.chkUseIdRange.AutoSize = true;
            this.chkUseIdRange.Location = new System.Drawing.Point(300, 25);
            this.chkUseIdRange.Name = "chkUseIdRange";
            this.chkUseIdRange.Size = new System.Drawing.Size(250, 19);
            this.chkUseIdRange.TabIndex = 4;
            this.chkUseIdRange.Text = "Use ID Range Processing (Recommended for very large lists)";
            this.chkUseIdRange.UseVisualStyleBackColor = true;

            // 
            // grpProgress
            // 
            this.grpProgress.Controls.Add(this.txtLog);
            this.grpProgress.Controls.Add(this.lblProgress);
            this.grpProgress.Controls.Add(this.progressBar);
            this.grpProgress.Location = new System.Drawing.Point(12, 516);
            this.grpProgress.Name = "grpProgress";
            this.grpProgress.Size = new System.Drawing.Size(760, 230);
            this.grpProgress.TabIndex = 4;
            this.grpProgress.TabStop = false;
            this.grpProgress.Text = "Progress";

            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(15, 25);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(725, 23);
            this.progressBar.TabIndex = 0;

            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(15, 51);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(0, 15);
            this.lblProgress.TabIndex = 1;

            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(15, 70);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(725, 150);
            this.txtLog.TabIndex = 2;

            // 
            // btnProcess
            // 
            this.btnProcess.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnProcess.Location = new System.Drawing.Point(12, 712);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(150, 40);
            this.btnProcess.TabIndex = 5;
            this.btnProcess.Text = "Start Processing";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);

            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(168, 712);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 40);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // 
            // btnExportFailures
            // 
            this.btnExportFailures.Enabled = false;
            this.btnExportFailures.Location = new System.Drawing.Point(480, 712);
            this.btnExportFailures.Name = "btnExportFailures";
            this.btnExportFailures.Size = new System.Drawing.Size(140, 40);
            this.btnExportFailures.TabIndex = 7;
            this.btnExportFailures.Text = "Export Failed Items";
            this.btnExportFailures.UseVisualStyleBackColor = true;
            this.btnExportFailures.Click += new System.EventHandler(this.btnExportFailures_Click);

            // 
            // btnOpenLog
            // 
            this.btnOpenLog.Location = new System.Drawing.Point(626, 712);
            this.btnOpenLog.Name = "btnOpenLog";
            this.btnOpenLog.Size = new System.Drawing.Size(146, 40);
            this.btnOpenLog.TabIndex = 8;
            this.btnOpenLog.Text = "Open Log File";
            this.btnOpenLog.UseVisualStyleBackColor = true;
            this.btnOpenLog.Click += new System.EventHandler(this.btnOpenLog_Click);

            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 761);
            this.Controls.Add(this.btnOpenLog);
            this.Controls.Add(this.btnExportFailures);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.grpProgress);
            this.Controls.Add(this.grpSettings);
            this.Controls.Add(this.grpColumns);
            this.Controls.Add(this.grpAuthentication);
            this.Controls.Add(this.grpConnection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SharePoint Large List Processor";
            this.grpConnection.ResumeLayout(false);
            this.grpConnection.PerformLayout();
            this.grpAuthentication.ResumeLayout(false);
            this.grpAuthentication.PerformLayout();
            this.pnlUsernamePassword.ResumeLayout(false);
            this.pnlUsernamePassword.PerformLayout();
            this.pnlModernAuth.ResumeLayout(false);
            this.pnlModernAuth.PerformLayout();
            this.pnlClientSecret.ResumeLayout(false);
            this.pnlClientSecret.PerformLayout();
            this.grpColumns.ResumeLayout(false);
            this.grpColumns.PerformLayout();
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBatchSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpdateBatchSize)).EndInit();
            this.grpProgress.ResumeLayout(false);
            this.grpProgress.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private GroupBox grpConnection;
        private TextBox txtSiteUrl;
        private Label lblSiteUrl;
        private TextBox txtListName;
        private Label lblListName;
        private Button btnTestConnection;
        private Label lblConnectionStatus;

        private GroupBox grpAuthentication;
        private RadioButton rbModernAuth;
        private RadioButton rbInteractive;
        private RadioButton rbUsernamePassword;
        private RadioButton rbClientSecret;
        private Panel pnlUsernamePassword;
        private TextBox txtUsername;
        private Label lblUsername;
        private TextBox txtPassword;
        private Label lblPassword;
        private Panel pnlModernAuth;
        private TextBox txtClientId;
        private Label lblClientId;
        private TextBox txtTenantId;
        private Label lblTenantId;
        private Panel pnlClientSecret;
        private TextBox txtClientIdSecret;
        private Label lblClientIdSecret;
        private TextBox txtClientSecret;
        private Label lblClientSecret;
        private TextBox txtTenantIdSecret;
        private Label lblTenantIdSecret;

        private GroupBox grpColumns;
        private TextBox txtCalculatedColumn;
        private Label lblCalculatedColumn;
        private TextBox txtTargetColumn;
        private Label lblTargetColumn;

        private GroupBox grpSettings;
        private NumericUpDown numBatchSize;
        private Label lblBatchSize;
        private NumericUpDown numUpdateBatchSize;
        private Label lblUpdateBatchSize;
        private CheckBox chkUseIdRange;

        private GroupBox grpProgress;
        private ProgressBar progressBar;
        private Label lblProgress;
        private TextBox txtLog;

        private Button btnProcess;
        private Button btnCancel;
        private Button btnExportFailures;
        private Button btnOpenLog;
    }
}
