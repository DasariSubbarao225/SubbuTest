# vse-3-tiles-webpart

## Summary

A modern SharePoint Framework (SPFx) web part that displays three configurable tiles with images, titles, descriptions, and links. Perfect for highlighting important resources or actions on your SharePoint page with full accessibility support and responsive design.

## Features

- ✅ **Three Configurable Tiles**: Each with title, description, image, link, and custom styling
- ✅ **Responsive Design**: 3 columns on desktop, 2 on tablet, 1 on mobile
- ✅ **Accessibility**: WCAG AA compliant with keyboard navigation and ARIA labels
- ✅ **URL Validation**: Inline validation with helpful error messages
- ✅ **Lazy Loading**: Images load as needed for optimal performance
- ✅ **Theme Support**: Adapts to SharePoint light/dark themes
- ✅ **Custom Colors**: Optional background color overrides per tile
- ✅ **Analytics**: Optional click tracking for insights

## Used SharePoint Framework Version

![version](https://img.shields.io/badge/version-1.11.0-green.svg)

## Applies to

- [SharePoint Framework](https://aka.ms/spfx)
- [Microsoft 365 tenant](https://docs.microsoft.com/en-us/sharepoint/dev/spfx/set-up-your-developer-tenant)
- SharePoint Online
- SharePoint 2019 (with SPFx support)

> Get your own free development tenant by subscribing to [Microsoft 365 developer program](http://aka.ms/o365devprogram)

## Prerequisites

- Node.js 14.x (LTS) or 12.x - **Node 16+ is NOT compatible**
- npm 6.x
- SharePoint Online tenant or SharePoint 2019
- See [BUILD_INSTRUCTIONS.md](./BUILD_INSTRUCTIONS.md) for detailed requirements

## Solution

Solution|Author(s)
--------|---------
vse-3-tiles-webpart | Copilot Agent

## Version history

Version|Date|Comments
-------|----|--------
1.0|February 4, 2026|Initial release with full feature set

## Disclaimer

**THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.**

---

## Minimal Path to Awesome

⚠️ **Important**: This project requires Node.js 14.x due to SPFx 1.11.0 dependencies.

```bash
# Install and use Node 14
nvm install 14
nvm use 14

# Clone this repository
git clone https://github.com/DasariSubbarao225/SubbuTest.git
cd SubbuTest

# Install dependencies
npm install

# Serve locally with SharePoint Workbench
gulp serve

# Build for production
gulp bundle --ship
gulp package-solution --ship
```

The packaged solution (`.sppkg` file) will be in the `sharepoint/solution/` folder.

## Configuration

The web part provides a comprehensive property pane with:

### Per Tile Configuration (3 tiles)
- **Title**: Main heading for the tile
- **Description**: Supporting text (multiline)
- **Image URL**: Path to tile image
- **Image Alt Text**: Accessibility description for screen readers
- **Link URL**: Target URL (validated automatically)
- **Open in New Tab**: Toggle for link target behavior
- **Background Color**: Optional custom color (hex, RGB, or color name)

### Advanced Settings
- **Enable Analytics**: Toggle console logging of tile clicks for tracking

## Features

This web part demonstrates the following SPFx capabilities:

- **React Components**: Modern React 16.8 with hooks-compatible structure
- **Responsive CSS Grid**: Mobile-first design with CSS Grid
- **Property Pane Validation**: Real-time URL validation with user-friendly error messages
- **Accessibility**: Full WCAG AA compliance with ARIA labels and keyboard navigation
- **Theme Integration**: Respects SharePoint themes (light/dark/high-contrast)
- **Performance Optimization**: Lazy loading images for faster page load
- **TypeScript**: Strongly typed for better development experience
- **SCSS Modules**: Scoped styling to prevent conflicts

## Accessibility Features

- Keyboard navigable (Tab, Enter keys)
- ARIA labels combining title and description
- Alt text for all images (configurable)
- WCAG AA color contrast ratios
- Focus indicators for keyboard users
- Semantic HTML structure
- Screen reader tested

## Browser Support

- Microsoft Edge (latest)
- Google Chrome (latest)
- Mozilla Firefox (latest)
- Safari (latest)

## Testing

See [MANUAL_TESTING_CHECKLIST.md](./MANUAL_TESTING_CHECKLIST.md) for comprehensive testing procedures covering all acceptance criteria.

```bash
# Run unit tests
gulp test
```

## Documentation

- [BUILD_INSTRUCTIONS.md](./BUILD_INSTRUCTIONS.md) - Complete setup and build guide
- [MANUAL_TESTING_CHECKLIST.md](./MANUAL_TESTING_CHECKLIST.md) - QA testing procedures
- [DEVELOPMENT_PLAN.md](./DEVELOPMENT_PLAN.md) - Original project plan

## References

- [Getting started with SharePoint Framework](https://docs.microsoft.com/en-us/sharepoint/dev/spfx/set-up-your-developer-tenant)
- [Building for Microsoft teams](https://docs.microsoft.com/en-us/sharepoint/dev/spfx/build-for-teams-overview)
- [SharePoint Framework v1.11.0 release notes](https://docs.microsoft.com/en-us/sharepoint/dev/spfx/release-1.11.0)
- [Fluent UI React Components](https://developer.microsoft.com/en-us/fluentui)
- [WCAG 2.1 Guidelines](https://www.w3.org/WAI/WCAG21/quickref/)
- [Microsoft 365 Patterns and Practices](https://aka.ms/m365pnp)
