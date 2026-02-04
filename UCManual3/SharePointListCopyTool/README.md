# SharePoint List Column Copy Tool

A Windows Forms application built with .NET Framework 4.8 that uses CSOM to copy data from one column to another in SharePoint Online lists, with support for large lists.

## Features

- **OfficeDevPnP Authentication**: Uses GetWebLoginClientContext for easy browser-based authentication
- **Large List Support**: Handles lists with more than 5000 items using pagination
- **Batch Processing**: Processes items in configurable batches (default: 500 items)
- **Exception Handling**: Comprehensive error handling with retry logic for throttling
- **Logging**: File-based logging using NLog with automatic log rotation
- **Progress Tracking**: Real-time progress bar and detailed logging in UI
- **Cancellation Support**: Ability to stop the operation at any time

## Prerequisites

### Required NuGet Packages

Install the following NuGet packages:

```
Install-Package Microsoft.SharePointOnline.CSOM
Install-Package SharePointPnPCoreOnline
Install-Package NLog
Install-Package NLog.Config
```

## Configuration

Edit `App.config` to adjust settings:

- **BatchSize**: Number of items to process per batch (default: 500)
- **RetryAttempts**: Number of retry attempts for failed operations (default: 3)
- **RetryDelaySeconds**: Delay between retries in seconds (default: 5)
- **RequestTimeout**: Request timeout in milliseconds (default: 180000)

## Usage

1. **Connect to SharePoint**:
   - Enter your SharePoint site URL
   - Click "Connect" button
   - A browser window will open for authentication

2. **Select List and Columns**:
   - Choose the target list from the dropdown
   - Select the source column
   - Select the destination column
   - Optionally check "Skip items with empty source columns"

3. **Start Copy Operation**:
   - Click "Start Copy" button
   - Monitor progress in the progress bar and log window
   - Click "Stop" to cancel the operation if needed

## Error Handling

The application handles various error scenarios:

- Authentication failures
- Network/connection errors
- Permission errors
- List or column not found
- Throttling (429 errors) with automatic retry
- Item-level update failures (logs failed items and continues)

## Logging

Logs are stored in the `Logs` folder with the following naming pattern:
- Current log: `SharePointCopyTool_[date].log`
- Archives: Automatically rotated daily, keeps last 30 days

## Architecture

```
SharePointListCopyTool/
├── Models/
│   ├── AppConfig.cs          # Configuration settings
│   └── CopyResult.cs         # Result tracking
├── Services/
│   ├── SharePointService.cs  # CSOM operations
│   └── LoggingService.cs     # Logging wrapper
├── Helpers/
│   └── ExceptionHandler.cs   # Error handling
└── MainForm.cs               # UI and orchestration
```

## Key Features Implementation

### Large List Handling
Uses CAML queries with `ListItemCollectionPosition` for pagination to handle lists exceeding the 5000 item threshold.

### Throttling Management
Automatically detects throttling (429 errors) and retries with exponential backoff.

### Async Operations
All SharePoint operations run asynchronously to keep the UI responsive.

## Troubleshooting

1. **Authentication Issues**: Ensure you're using a valid SharePoint Online site URL
2. **Permission Errors**: Verify you have edit permissions on the target list
3. **Throttling**: Reduce batch size in App.config if throttling occurs frequently
4. **Column Type Mismatches**: Ensure source and destination columns are compatible types

## License

Free to use and modify as needed.
