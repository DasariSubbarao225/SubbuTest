# Quick Start Guide

## Get Started in 5 Minutes!

### Prerequisites
- Windows 10/11
- .NET 8.0 SDK ([Download here](https://dotnet.microsoft.com/download/dotnet/8.0))
- SharePoint Online access
- Azure AD App Registration (see [Azure AD Setup Guide](AZURE_AD_SETUP.md))

---

## Step 1: Build the Application

```bash
cd SharePointLargeListApp
dotnet restore
dotnet build
dotnet run
```

Or open in Visual Studio 2022 and press F5.

---

## Step 2: Setup Azure AD App (One-Time)

### Quick Setup (3 minutes):

1. Go to [Azure Portal](https://portal.azure.com)
2. Navigate to **Azure Active Directory** > **App registrations** > **New registration**
3. Settings:
   - Name: `SP List Processor`
   - Account type: **Single tenant**
   - Redirect URI: `Public client` → `http://localhost`
4. Click **Register**, copy:
   - ✅ **Application (client) ID**
   - ✅ **Directory (tenant) ID**
5. Go to **API Permissions**:
   - Add **SharePoint** > **Delegated** > `AllSites.FullControl`
   - Click **Grant admin consent**
6. Go to **Authentication**:
   - Enable **Allow public client flows**

**Done!** You now have your Client ID and Tenant ID.

📖 [Detailed Azure AD Setup Guide](AZURE_AD_SETUP.md)

---

## Step 3: Run Your First Job

### Example Scenario:
You have a list with 15,000 items and want to copy data from a calculated column called "FullName" to a text column called "DisplayName".

### Using the Application:

1. **Connection Settings:**
   ```
   Site URL: https://contoso.sharepoint.com/sites/YourSite
   List Name: Employees
   ```

2. **Authentication (Modern Auth - Recommended):**
   - Select: **Modern Auth (MSAL - Interactive)**
   - Client ID: `[Paste from Step 2]`
   - Tenant ID: `[Paste from Step 2]`
   - Click **Test Connection**
   - Sign in when browser opens

3. **Column Configuration:**
   ```
   Calculated Column: FullName
   Target Column: DisplayName
   ```

4. **Settings (Use Defaults):**
   ```
   Retrieve Batch Size: 2000
   Update Batch Size: 100
   ☐ Use ID Range Processing (check for 50k+ items)
   ```

5. **Start Processing:**
   - Click **Start Processing**
   - Watch the progress bar
   - View logs in real-time

---

## Common Scenarios

### Scenario 1: Small List (<5000 items)
- Use default settings
- Processing time: ~1-2 minutes

### Scenario 2: Medium List (5000-50,000 items)
- Use default settings
- Enable **Use ID Range Processing** for better reliability
- Processing time: ~5-15 minutes

### Scenario 3: Large List (50,000+ items)
- **Must enable**: Use ID Range Processing
- Reduce Retrieve Batch Size to 2000
- Consider running during off-peak hours
- Processing time: ~30-60 minutes

### Scenario 4: Automated/Unattended Processing
- Use **App-Only (Client Secret)** authentication
- Create a client secret in Azure AD
- Schedule via Windows Task Scheduler

---

## Troubleshooting Quick Fixes

### ❌ "List view threshold exceeded"
**Fix:**
```
✅ Enable "Use ID Range Processing"
✅ Reduce Retrieve Batch Size to 2000
```

### ❌ "Authentication failed"
**Fix:**
```
✅ Verify Client ID and Tenant ID
✅ Check "Grant admin consent" was clicked
✅ Wait 5-10 minutes after granting consent
```

### ❌ "Column not found"
**Fix:**
```
✅ Use internal column name (no spaces)
✅ Check case sensitivity
✅ Example: "DisplayName" not "Display Name"
```

### ❌ "Cannot update read-only field"
**Fix:**
```
✅ Target column must not be read-only
✅ Cannot update: Created, Modified, ID, etc.
✅ Check column settings in SharePoint
```

### ❌ "Throttling" or "429 errors"
**Fix:**
```
✅ The app auto-retries with backoff
✅ Reduce batch sizes
✅ Run during off-peak hours (evenings/weekends)
```

---

## Performance Tips

| List Size | Recommended Settings |
|-----------|---------------------|
| < 5k      | Defaults (2000/100) |
| 5k - 50k  | Enable ID Range + 2000/100 |
| 50k - 200k| ID Range + 1500/50 |
| 200k+     | ID Range + 1000/50 + Off-peak hours |

---

## Understanding the Process

```
┌─────────────────┐
│ 1. Connect      │  Authenticate with SharePoint
└────────┬────────┘
         │
┌────────▼────────┐
│ 2. Validate     │  Check columns exist & are accessible
└────────┬────────┘
         │
┌────────▼────────┐
│ 3. Retrieve     │  Get items in batches (pagination)
└────────┬────────┘  Avoids list view threshold
         │
┌────────▼────────┐
│ 4. Process      │  Copy calculated → target column
└────────┬────────┘  Converts data types automatically
         │
┌────────▼────────┐
│ 5. Update       │  Batch updates (100 items at a time)
└────────┬────────┘  Uses SystemUpdate (no workflow trigger)
         │
┌────────▼────────┐
│ 6. Report       │  Show summary + export failures
└─────────────────┘
```

---

## After Processing

### ✅ Success!
- View summary dialog
- Check log file (click **Open Log File**)
- Verify data in SharePoint list

### ⚠️ Some Failures
- Click **Export Failed Items**
- Review CSV file in `Reports` folder
- Common issues:
  - Data type mismatch
  - Read-only items
  - Concurrent modifications

### Re-running
- Safe to re-run (uses SystemUpdate, doesn't duplicate)
- Failed items can be filtered and processed separately

---

## File Locations

```
SharePointLargeListApp/
├── Logs/
│   └── SPLog_YYYYMMDD_HHMMSS.txt    ← Detailed logs
└── Reports/
    └── FailedItems_YYYYMMDD_HHMMSS.csv  ← Export of failures
```

---

## Next Steps

1. ✅ Read [Azure AD Setup Guide](AZURE_AD_SETUP.md) for authentication options
2. ✅ Review [README.md](README.md) for complete documentation
3. ✅ Test with a small list first
4. ✅ Monitor performance and adjust batch sizes
5. ✅ Setup App-Only authentication for automation

---

## Support & Resources

### Documentation
- [Full README](README.md)
- [Azure AD Setup](AZURE_AD_SETUP.md)

### Common Questions

**Q: How long will it take?**  
A: ~100-200 items/second. 10k items ≈ 1-2 minutes, 100k items ≈ 10-20 minutes

**Q: Will it trigger workflows?**  
A: No! Uses `SystemUpdate()` which bypasses workflows and doesn't update Modified date

**Q: Can I run multiple processes?**  
A: Not recommended. Run one at a time to avoid throttling

**Q: Does it work with SharePoint on-premises?**  
A: No, designed for SharePoint Online only

**Q: Can I process multiple columns?**  
A: One column pair at a time. Run multiple times for multiple columns

---

## Safety Features

✅ **Test Connection** - Verify before processing  
✅ **Progress Tracking** - See real-time status  
✅ **Cancel Anytime** - Safe to stop mid-process  
✅ **Comprehensive Logging** - Full audit trail  
✅ **Error Recovery** - Auto-retry with backoff  
✅ **Failed Item Export** - CSV report for troubleshooting  
✅ **SystemUpdate** - No version history bloat  

---

## Happy Processing! 🚀

Need help? Check the logs first, then review the documentation.
