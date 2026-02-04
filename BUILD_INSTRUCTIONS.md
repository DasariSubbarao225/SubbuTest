# Build Instructions for vse3TilesWebPart

## Prerequisites

This project uses SharePoint Framework (SPFx) version 1.11.0, which has specific Node.js version requirements.

### Required Tools

- **Node.js**: Version 14.x (LTS) or 12.x
  - Node 16+ is NOT compatible with SPFx 1.11.0 due to node-sass dependencies
  - Use NVM (Node Version Manager) to easily switch between Node versions
- **npm**: Version 6.x (comes with Node 14)
- **gulp**: Installed globally via `npm install -g gulp`
- **Yeoman**: Installed globally via `npm install -g yo`
- **SPFx Yeoman generator**: Installed globally via `npm install -g @microsoft/generator-sharepoint`

## Setup Instructions

### 1. Install Node Version Manager (NVM)

```bash
# Install NVM
curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.0/install.sh | bash

# Load NVM
export NVM_DIR="$HOME/.nvm"
[ -s "$NVM_DIR/nvm.sh" ] && \. "$NVM_DIR/nvm.sh"
```

### 2. Install and Use Node 14

```bash
# Install Node 14
nvm install 14

# Use Node 14
nvm use 14

# Verify Node version
node --version  # Should output v14.x.x
```

### 3. Install Dependencies

```bash
# Install project dependencies
npm install
```

If you encounter errors with `node-sass`, try:

```bash
# Clean install
rm -rf node_modules package-lock.json
npm install --legacy-peer-deps

# Rebuild node-sass
npm rebuild node-sass
```

## Building the Project

### Development Build

```bash
# Bundle the solution
gulp bundle

# Serve locally with workbench
gulp serve
```

### Production Build

```bash
# Create production bundle
gulp bundle --ship

# Package the solution
gulp package-solution --ship
```

The packaged solution will be available in `sharepoint/solution/` directory.

## Common Issues and Solutions

### Issue: "Error: Node Sass does not yet support your current environment"

**Solution**: You're using an incompatible Node.js version. Switch to Node 14:

```bash
nvm use 14
npm rebuild node-sass
```

### Issue: Python errors during node-sass installation

**Solution**: Node-sass requires Python 2.7 for compilation. Either:
- Install Python 2.7
- Use the prebuilt binaries (should work automatically on most systems)
- Switch to a system that has compatible Python

### Issue: "Cannot find module '@microsoft/sp-build-web'"

**Solution**: Dependencies not properly installed:

```bash
rm -rf node_modules
npm install --legacy-peer-deps
```

## Testing

```bash
# Run tests
gulp test
```

## Deployment

1. Build the production package: `gulp bundle --ship && gulp package-solution --ship`
2. Upload the `.sppkg` file from `sharepoint/solution/` to your SharePoint App Catalog
3. Deploy the solution to your site collection
4. Add the web part to a SharePoint page

## Web Part Features

### Configuration
The web part provides a comprehensive property pane with:
- **Three tile configurations** (Tile 1, Tile 2, Tile 3), each with:
  - Title
  - Description (multiline)
  - Image URL
  - Image Alt Text (for accessibility)
  - Link URL (with validation)
  - Open in New Tab toggle
  - Background Color override (optional)
- **Advanced Settings**:
  - Analytics logging toggle

### Responsive Design
- **Desktop** (> 1024px): 3 tiles per row
- **Tablet** (640px - 1024px): 2 tiles per row
- **Mobile** (< 640px): 1 tile per row

### Accessibility Features
- WCAG AA compliant color contrast
- Keyboard navigation support
- ARIA labels for screen readers
- Semantic HTML
- Alt text for images
- Focus indicators

### Theme Support
- Respects SharePoint theme colors
- Dark mode support
- High contrast mode support
- Custom background color overrides

## Development Notes

- TypeScript version: 3.3.x (required by SPFx 1.11.0)
- React version: 16.8.5 (required by SPFx 1.11.0)
- SCSS compilation via node-sass (replaced by Dart Sass in newer versions)

## Upgrading

To upgrade to a newer SPFx version (e.g., 1.18+):
1. Update `package.json` dependencies
2. Replace `node-sass` with `sass` package
3. Update TypeScript and React versions
4. Test thoroughly as breaking changes may occur
