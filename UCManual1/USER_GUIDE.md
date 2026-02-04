# vse3TilesWebPart

## Summary

A fully accessible, responsive SharePoint Framework (SPFx) web part that displays three configurable tiles. Perfect for highlighting important resources, quick links, or featured content on any SharePoint page.

## Features

✅ **Three Configurable Tiles** - Each with title, image, description, and link  
✅ **Responsive Design** - 3 columns (desktop), 2 columns (tablet), 1 column (mobile)  
✅ **Accessibility First** - WCAG AA compliant with keyboard navigation and ARIA labels  
✅ **Theme Aware** - Automatically adapts to light/dark themes  
✅ **Smart Validation** - Real-time URL validation with helpful error messages  
✅ **Performance Optimized** - Lazy-loaded images for fast page loads  
✅ **Analytics Ready** - Optional click tracking for insights  

## Used SharePoint Framework Version

![version](https://img.shields.io/badge/version-1.21.1-green.svg)

## Applies to

- [SharePoint Framework](https://aka.ms/spfx)
- [Microsoft 365 tenant](https://docs.microsoft.com/en-us/sharepoint/dev/spfx/set-up-your-developer-tenant)

## Prerequisites

- Node.js v16 or v18 (LTS)
- gulp-cli installed globally: `npm install -g gulp-cli`
- SharePoint Online tenant

## Solution

| Solution    | Author(s)                                               |
| ----------- | ------------------------------------------------------- |
| UCManual1 | Your Organization |

## Version history

| Version | Date             | Comments        |
| ------- | ---------------- | --------------- |
| 1.0     | February 4, 2026 | Initial release |

---

## Quick Start

### Build and Test

```bash
# Clone the repository
cd UCManual1

# Install dependencies
npm install

# Start local workbench
gulp serve
```

### Deploy to SharePoint

```bash
# Bundle the solution
gulp bundle --ship

# Package the solution
gulp package-solution --ship

# Upload the .sppkg file from sharepoint/solution/ to your App Catalog
```

## Configuration

### Adding the Web Part

1. Edit a SharePoint page
2. Click **+** to add a web part
3. Search for **"Tiles"**
4. Select the web part to add it to the page

### Configuring Tiles

1. Click the **Edit** (pencil) icon on the web part
2. Configure each of the three tiles:

#### Tile Properties

| Property | Description | Required |
|----------|-------------|----------|
| **Title** | Heading text for the tile | Recommended |
| **Image URL** | Full URL or relative path to image | Optional |
| **Alt Text** | Accessible description for image | Recommended if image used |
| **Description** | Brief description of the tile content | Recommended |
| **Link URL** | Destination URL when tile is clicked | Optional |
| **Open in new tab** | Toggle to open link in new tab | Optional |
| **Background Color** | CSS color value (e.g., `#f3f2f1`) | Optional |

#### Advanced Settings

- **Enable analytics logging**: Toggle to log tile clicks to browser console

### Example Configurations

**News & Announcements**
- Title: "Latest News"
- Image: Company logo or news icon
- Description: "Stay updated with company announcements"
- Link: `/sites/intranet/SitePages/news.aspx`

**Employee Resources**
- Title: "HR Portal"
- Image: Icon representing HR
- Description: "Access benefits, policies, and forms"
- Link: `https://hr.company.com`
- Open in new tab: Yes

**Quick Actions**
- Title: "Submit Request"
- Image: Action icon
- Description: "Submit IT support tickets"
- Link: `/sites/helpdesk`

## Responsive Behavior

| Screen Size | Layout | Breakpoint |
|-------------|--------|------------|
| **Desktop** | 3 tiles per row | > 1024px |
| **Tablet** | 2 tiles per row | 641px - 1024px |
| **Mobile** | 1 tile per row | ≤ 640px |

## Accessibility Features

- ✅ Keyboard navigable (Tab, Enter, Space)
- ✅ ARIA labels for screen readers
- ✅ Alt text for images
- ✅ Focus indicators
- ✅ High contrast mode support
- ✅ WCAG AA color contrast compliance

## Customization

### Styling

Modify [Tiles.module.scss](src/webparts/tiles/components/Tiles.module.scss) to customize:
- Colors
- Spacing
- Border radius
- Hover effects
- Breakpoints

### Analytics Integration

Replace console logging in [Tiles.tsx](src/webparts/tiles/components/Tiles.tsx) with your analytics SDK:

```typescript
private _handleTileClick = (tile: ITileData, tileNumber: number): void => {
  if (this.props.enableAnalytics) {
    // Replace with Application Insights, Google Analytics, etc.
    window.appInsights?.trackEvent('TileClick', {
      tileNumber,
      title: tile.title,
      linkUrl: tile.linkUrl
    });
  }
};
```

## Testing

Refer to [TESTING_CHECKLIST.md](TESTING_CHECKLIST.md) for comprehensive test scenarios.

### Unit Tests

```bash
# Install test dependencies (if not already installed)
npm install --save-dev @testing-library/react @testing-library/jest-dom

# Run tests
npm test
```

## Troubleshooting

### Images not loading
- Verify image URLs are accessible
- Check CORS settings for external images
- Use relative paths for SharePoint images

### Tiles not responsive
- Clear browser cache
- Check if custom CSS is overriding styles
- Verify page width settings

### Validation errors not showing
- Wait 500ms after typing (debounce delay)
- Check browser console for errors

## References

- [SharePoint Framework Overview](https://aka.ms/spfx)
- [Build your first web part](https://docs.microsoft.com/en-us/sharepoint/dev/spfx/web-parts/get-started/build-a-hello-world-web-part)
- [WCAG 2.1 Guidelines](https://www.w3.org/WAI/WCAG21/quickref/)
