# SharePoint Large List Processor

A Windows desktop application to process SharePoint lists with more than 10,000 items without hitting the list view threshold, using CSOM (Client-Side Object Model).

## Features

✅ **List View Threshold Bypass** - Handles lists with 10,000+ items using pagination and ID-based filtering  
✅ **Multiple Authentication Methods**  
   - Modern Authentication (MSAL) - Interactive  
   - Username/Password (Legacy)  
   - App-Only Authentication (Client Secret)  
✅ **Retry Logic with Exponential Backoff** - Handles transient errors and throttling  
✅ **Column Type Handling** - Supports Text, Number, DateTime, Boolean, Lookup, URL, and more  
✅ **Batch Processing** - Configurable batch sizes for optimal performance  
✅ **Progress Tracking** - Real-time progress updates with detailed logging  
✅ **Error Handling** - Failed items are tracked and can be exported to CSV  
✅ **Logging** - Comprehensive logging to file for troubleshooting  

## Requirements

- .NET 8.0 or later
- Windows OS
- SharePoint Online subscription
- Appropriate SharePoint permissions (Read/Write on the list)

## NuGet Packages Used

```xml
<PackageReference Include="Microsoft.SharePointOnline.CSOM" Version="16.1.24706.12000" />
<PackageReference Include="Microsoft.Identity.Client" Version="4.61.3" />
<PackageReference Include="CsvHelper" Version="30.0.1" />
```

## Setup

### 1. Clone or Download the Project

```bash
git clone <repository-url>
cd SharePointLargeListApp
```

### 2. Restore NuGet Packages

```bash
dotnet restore
```

### 3. Azure AD App Registration (For Modern Auth)

For **Modern Authentication** or **Client Secret** methods, you need to register an app in Azure AD:

1. Go to [Azure Portal](https://portal.azure.com)
2. Navigate to **Azure Active Directory** > **App registrations** > **New registration**
3. Configure:
   - Name: `SharePoint List Processor`
   - Supported account types: **Accounts in this organizational directory only**
   - Redirect URI: `Public client/native (mobile & desktop)` - `http://localhost`

#### For Interactive (Modern Auth):
4. Go to **API permissions**
   - Add **SharePoint** > **AllSites.FullControl** (Delegated)
   - Grant admin consent

#### For App-Only (Client Secret):
4. Go to **API permissions**
   - Add **SharePoint** > **Sites.FullControl.All** (Application)
   - Grant admin consent
5. Go to **Certificates & secrets**
   - Create a new client secret
   - Copy the secret value immediately

6. Copy the **Application (client) ID** and **Directory (tenant) ID** from the Overview page

## Usage

### 1. Launch the Application

```bash
dotnet run
```

### 2. Configure Connection

1. **Site URL**: Enter your SharePoint site URL (e.g., `https://contoso.sharepoint.com/sites/yoursite`)
2. **List Name**: Enter the name of the list to process

### 3. Choose Authentication Method

#### Option A: Modern Authentication (Recommended)
- Select **Modern Auth (MSAL - Interactive)**
- Enter **Client ID** and **Tenant ID** from Azure AD app registration
- Click **Test Connection** - a browser window will open for authentication

#### Option B: Username/Password (Legacy - No MFA)
- Select **Username/Password**
- Enter your SharePoint credentials
- Click **Test Connection**

#### Option C: App-Only with Client Secret
- Select **App-Only (Client Secret)**
- Enter **Client ID**, **Client Secret**, and **Tenant ID**
- Click **Test Connection**

### 4. Configure Columns

1. **Calculated Column**: Name of the source calculated column
2. **Target Column**: Name of the destination column to update

### 5. Configure Settings

- **Retrieve Batch Size**: Number of items to retrieve per batch (default: 2000)
  - Max: 5000 (approaching threshold limit)
  - For very large lists, use 2000-3000

- **Update Batch Size**: Number of items to update per batch (default: 100)
  - Recommended: 50-200 for optimal performance

- **Use ID Range Processing**: Check this for very large lists
  - Uses ID-based filtering (most reliable for threshold avoidance)
  - Recommended for lists with 50,000+ items

### 6. Start Processing

1. Click **Start Processing**
2. Monitor progress in real-time
3. View log messages in the log window
4. If needed, click **Cancel** to stop processing

### 7. Handle Failed Items

If any items fail to update:
1. Click **Export Failed Items** button
2. A CSV file will be generated in the `Reports` folder
3. Review and address the errors
4. Re-run the process if needed

### 8. View Logs

Click **Open Log File** to view the detailed log file in the `Logs` folder.

## How It Handles List View Threshold

The application uses multiple strategies to avoid the 5000-item list view threshold:

### 1. Pagination-Based Approach (Default)
```csharp
// Uses ListItemCollectionPosition for pagination
// Each batch returns <= BatchSize items
// No single query exceeds threshold
```

### 2. ID Range-Based Approach (For Very Large Lists)
```csharp
// Filters on ID column (always indexed)
// Query: WHERE ID >= startId AND ID < endId
// Each range query returns < 5000 items
```

### 3. Retry Logic
- Automatically retries on transient errors
- Exponential backoff (1s, 2s, 4s, 8s...)
- Handles SharePoint throttling (429 errors)

### 4. Batch Updates
- Uses `SystemUpdate()` to avoid triggering workflows
- Doesn't update Modified/ModifiedBy fields
- Processes in small batches to avoid timeouts

## Column Type Support

The application automatically converts calculated column values to appropriate target column types:

| Target Column Type | Conversion |
|-------------------|------------|
| Text | String conversion |
| Number/Currency | Numeric parsing |
| DateTime | Date parsing |
| Boolean | Boolean conversion |
| Integer | Integer parsing |
| Lookup/User | Lookup value extraction |
| URL | URL formatting |
| Choice | String conversion |

## Architecture

```
SharePointLargeListApp/
├── Models/
│   ├── SharePointConfig.cs       # Configuration model
│   ├── ProcessResult.cs          # Processing results
│   └── ProgressEventArgs.cs      # Progress tracking
├── Services/
│   ├── AuthenticationService.cs  # Authentication methods
│   ├── SharePointService.cs      # CSOM operations
│   ├── ListProcessor.cs          # Main processing logic
│   └── ColumnTypeHandler.cs      # Column type conversions
├── Utilities/
│   ├── Logger.cs                 # File logging
│   ├── RetryHelper.cs            # Retry with backoff
│   └── CsvExporter.cs            # CSV export
└── MainForm.cs                   # Windows Forms UI
```

## Performance Tips

1. **Batch Sizes**:
   - Retrieve: 2000-3000 (balance between speed and threshold)
   - Update: 100-200 (avoid timeouts)

2. **Processing Method**:
   - Use pagination for lists < 50,000 items
   - Use ID Range for lists > 50,000 items

3. **Off-Peak Hours**:
   - Run during off-peak hours for better performance
   - SharePoint has hourly limits on API calls

4. **Network**:
   - Use wired connection for stability
   - Avoid VPN if possible

## Troubleshooting

### "List view threshold exceeded"
- Enable "Use ID Range Processing"
- Reduce Retrieve Batch Size to 2000
- Ensure ID column is indexed (it always is by default)

### "Authentication failed"
- For Modern Auth: Check Client ID and Tenant ID
- For Username/Password: Ensure account doesn't have MFA
- For Client Secret: Verify secret hasn't expired

### "Throttling" or "429 errors"
- The app automatically retries with exponential backoff
- If persistent, reduce batch sizes or wait for throttling to clear

### "Column not found"
- Use internal column name, not display name
- Check spelling and case sensitivity

### "Cannot update read-only field"
- Target column might be read-only
- System columns (Created, Modified) cannot be updated

## Limitations

- Maximum batch size: 5000 (SharePoint threshold)
- Throttling limits apply (varies by tenant)
- Cannot update read-only or system columns
- Username/Password authentication doesn't work with MFA-enabled accounts

## License

[Your License Here]

## Support

For issues or questions, please [create an issue](link-to-issues).

## Contributing

Contributions are welcome! Please read the contributing guidelines first.
