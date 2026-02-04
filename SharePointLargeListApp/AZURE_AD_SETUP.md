# Azure AD App Registration Guide

This guide will help you set up Azure AD app registration for authentication with the SharePoint Large List Processor application.

## Table of Contents
1. [Interactive Authentication (Modern Auth)](#interactive-authentication-modern-auth)
2. [App-Only Authentication (Client Secret)](#app-only-authentication-client-secret)
3. [Required Permissions](#required-permissions)
4. [Common Issues](#common-issues)

---

## Interactive Authentication (Modern Auth)

This method is **recommended for personal use** where you want to authenticate with your own credentials.

### Step 1: Register the Application

1. Go to [Azure Portal](https://portal.azure.com)
2. Navigate to **Azure Active Directory**
3. Select **App registrations** from the left menu
4. Click **+ New registration**

### Step 2: Configure Basic Settings

Fill in the registration form:

- **Name**: `SharePoint List Processor` (or any name you prefer)
- **Supported account types**: Select **Accounts in this organizational directory only**
- **Redirect URI**: 
  - Platform: **Public client/native (mobile & desktop)**
  - URI: `http://localhost`

Click **Register**

### Step 3: Note the IDs

From the **Overview** page, copy these values (you'll need them in the app):

- **Application (client) ID**: e.g., `12345678-1234-1234-1234-123456789abc`
- **Directory (tenant) ID**: e.g., `87654321-4321-4321-4321-cba987654321`

### Step 4: Configure API Permissions

1. Select **API permissions** from the left menu
2. Click **+ Add a permission**
3. Select **SharePoint** (not Microsoft Graph)
4. Choose **Delegated permissions**
5. Select these permissions:
   - ✅ **AllSites.FullControl** - Full control of all site collections
   - OR ✅ **AllSites.Write** - Write to all site collections (if you don't need full control)
6. Click **Add permissions**
7. Click **Grant admin consent for [Your Organization]** (requires admin privileges)
   - If you don't have admin rights, ask your IT administrator to grant consent

### Step 5: Configure Authentication

1. Select **Authentication** from the left menu
2. Under **Advanced settings**:
   - **Allow public client flows**: Set to **Yes**
3. Click **Save**

### Step 6: Use in Application

In the application:
1. Select **Modern Auth (MSAL - Interactive)**
2. Enter the **Client ID** from Step 3
3. Enter the **Tenant ID** from Step 3
4. Click **Test Connection**
5. A browser window will open - sign in with your Microsoft 365 account
6. Grant the requested permissions
7. The browser will redirect and the application will be authenticated

---

## App-Only Authentication (Client Secret)

This method is **recommended for automated/unattended scenarios** where the application runs without user interaction.

### Step 1: Register the Application

Follow Steps 1-3 from Interactive Authentication above.

### Step 2: Configure API Permissions

1. Select **API permissions** from the left menu
2. Click **+ Add a permission**
3. Select **SharePoint** (not Microsoft Graph)
4. Choose **Application permissions** (not delegated)
5. Select:
   - ✅ **Sites.FullControl.All** - Full control of all site collections
   - OR ✅ **Sites.ReadWrite.All** - Read and write items in all site collections
6. Click **Add permissions**
7. Click **Grant admin consent for [Your Organization]** (requires admin privileges)
   - **Important**: Admin consent is REQUIRED for application permissions

### Step 3: Create Client Secret

1. Select **Certificates & secrets** from the left menu
2. Under **Client secrets**, click **+ New client secret**
3. Configure:
   - **Description**: `SharePoint List Processor Secret`
   - **Expires**: Choose expiration (recommended: 6 months or 1 year)
4. Click **Add**
5. **IMPORTANT**: Copy the **Value** immediately (you won't be able to see it again!)
   - This is your **Client Secret**
   - Store it securely (consider using a password manager)

### Step 4: Use in Application

In the application:
1. Select **App-Only (Client Secret)**
2. Enter the **Client ID** from registration
3. Enter the **Client Secret** from Step 3
4. Enter the **Tenant ID** from registration
5. Click **Test Connection**

---

## Required Permissions

### Delegated Permissions (Interactive Auth)
These permissions act on behalf of the signed-in user.

| Permission | Description | Required For |
|-----------|-------------|--------------|
| AllSites.FullControl | Full control of all site collections | Reading and updating list items |
| AllSites.Write | Write access to all site collections | Alternative with less privilege |

### Application Permissions (App-Only)
These permissions allow the app to act independently without a user.

| Permission | Description | Required For |
|-----------|-------------|--------------|
| Sites.FullControl.All | Full control of all site collections | Reading and updating list items |
| Sites.ReadWrite.All | Read/Write access to all sites | Alternative with less privilege |

---

## Permission Scope Comparison

| Scenario | Use | Permissions Type | Requires Admin Consent |
|----------|-----|------------------|------------------------|
| Personal use with your account | Interactive | Delegated | Optional (recommended) |
| Automated/scheduled tasks | App-Only | Application | **Required** |
| Multiple users | Interactive | Delegated | **Required** |

---

## Common Issues

### Issue 1: "AADSTS50194: Application requires admin approval"

**Cause**: Admin consent not granted for required permissions.

**Solution**:
1. Go to App Registration > API permissions
2. Click "Grant admin consent for [Organization]"
3. If you're not an admin, send the consent URL to your admin:
   ```
   https://login.microsoftonline.com/{tenant-id}/adminconsent?client_id={client-id}
   ```

### Issue 2: "The application does not have any delegated permissions"

**Cause**: No permissions configured or permissions not granted.

**Solution**:
1. Go to API permissions
2. Add SharePoint permissions (see steps above)
3. Click "Grant admin consent"

### Issue 3: "AADSTS65001: The user or administrator has not consented"

**Cause**: User hasn't consented to the permissions (for delegated permissions without admin consent).

**Solution**:
- When prompted during sign-in, click "Accept" to consent to the permissions
- Or have an admin grant tenant-wide consent

### Issue 4: "Client secret has expired"

**Cause**: The client secret has reached its expiration date.

**Solution**:
1. Go to Certificates & secrets
2. Delete the expired secret
3. Create a new client secret
4. Update the application with the new secret

### Issue 5: "AADSTS700016: Application not found in directory"

**Cause**: Incorrect Client ID or Tenant ID.

**Solution**:
1. Verify the Client ID from App Registration > Overview
2. Verify the Tenant ID from Azure AD > Overview
3. Ensure there are no extra spaces or characters

### Issue 6: "Access denied" when accessing SharePoint

**Cause**: Insufficient SharePoint permissions or permission not granted.

**Solution**:
1. Verify the correct SharePoint permissions are added (not Graph API)
2. Ensure admin consent is granted
3. For App-Only: Verify application permissions (not delegated)
4. Wait 5-10 minutes after granting consent for changes to propagate

---

## Security Best Practices

### For Interactive Authentication:
1. ✅ Use delegated permissions (not application)
2. ✅ Grant only necessary permissions (prefer Write over FullControl if possible)
3. ✅ Enable MFA for user accounts
4. ✅ Regularly review app permissions

### For App-Only Authentication:
1. ✅ Store client secrets securely (use Azure Key Vault in production)
2. ✅ Set appropriate secret expiration (3-12 months)
3. ✅ Rotate secrets before expiration
4. ✅ Use certificates instead of secrets for higher security (advanced)
5. ✅ Monitor app usage through Azure AD sign-in logs
6. ✅ Implement least-privilege principle

### General:
1. ✅ Don't commit secrets to source control
2. ✅ Don't share app registration across multiple applications
3. ✅ Review and audit app permissions regularly
4. ✅ Disable or delete unused app registrations

---

## Testing Your Setup

### Test Script (PowerShell)
```powershell
# Test Interactive Auth
$clientId = "YOUR_CLIENT_ID"
$tenantId = "YOUR_TENANT_ID"
$siteUrl = "https://yourtenant.sharepoint.com/sites/yoursite"

# This will open a browser window for authentication
# If successful, you should be able to authenticate
```

### Verification Checklist
- [ ] App registration created
- [ ] Client ID copied
- [ ] Tenant ID copied
- [ ] Correct permissions added (SharePoint, not Graph)
- [ ] Correct permission type (Delegated or Application)
- [ ] Admin consent granted (if required)
- [ ] Client secret created (for App-Only)
- [ ] Client secret copied and stored securely
- [ ] Test Connection successful in the application

---

## Additional Resources

- [Microsoft Documentation: Register an app](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app)
- [SharePoint REST API Permissions](https://docs.microsoft.com/en-us/sharepoint/dev/sp-add-ins/add-in-permissions-in-sharepoint)
- [MSAL.NET Documentation](https://docs.microsoft.com/en-us/azure/active-directory/develop/msal-overview)

---

## Support

If you encounter issues not covered here:
1. Check Azure AD sign-in logs for detailed error messages
2. Review the application logs in the `Logs` folder
3. Contact your IT administrator for Azure AD permission issues
