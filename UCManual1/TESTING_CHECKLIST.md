# vse3TilesWebPart - Testing Checklist

## Manual Testing Guide

### AC1 — Configurable Content
- [ ] Add web part to a page in edit mode
- [ ] Open property pane and verify all fields are present for each tile:
  - [ ] Title field
  - [ ] Image URL field
  - [ ] Alt Text field
  - [ ] Description field (multiline)
  - [ ] Link URL field
  - [ ] Open in new tab toggle
  - [ ] Background color field (optional)
- [ ] Configure all three tiles with different content
- [ ] Verify property pane changes update live preview

### AC2 — Display & Responsive Layout
- [ ] **Desktop (>1024px)**: Verify 3 tiles display in a single row
- [ ] **Tablet (641px-1024px)**: Verify 2 tiles per row (third wraps to second row)
- [ ] **Mobile (≤640px)**: Verify 1 tile per row (stacked vertically)
- [ ] Check consistent spacing and alignment across breakpoints
- [ ] Verify tiles maintain aspect ratio and don't overflow

### AC3 — Click Behavior
- [ ] Configure a tile with a link and "Open in same tab" = No
- [ ] Click the tile - verify it opens in the same tab
- [ ] Configure a tile with "Open in new tab" = Yes
- [ ] Click the tile - verify it opens in a new tab
- [ ] Inspect the link element - verify it has `rel="noopener noreferrer"`
- [ ] Verify clicking works on all parts of the tile (image, title, description)

### AC4 — Accessibility
- [ ] **Keyboard Navigation**:
  - [ ] Tab through all tiles - verify focus indicators are visible
  - [ ] Press Enter/Space on a focused tile - verify navigation works
- [ ] **ARIA Labels**:
  - [ ] Use screen reader or inspect HTML - verify each tile has descriptive aria-label
  - [ ] Verify aria-label includes title + description
- [ ] **Images**:
  - [ ] Verify all images have alt text (from Alt Text field or title fallback)
  - [ ] Test with images turned off - verify alt text appears
- [ ] **Color Contrast**:
  - [ ] Use browser extension (e.g., axe DevTools, WAVE) to check contrast
  - [ ] Verify text/background meets WCAG AA (4.5:1 for normal text)

### AC5 — Authoring UX
- [ ] Open web part in edit mode with empty tiles
- [ ] Verify placeholder content shows for unconfigured tiles:
  - [ ] Shows "Configure Tile X" message
  - [ ] Shows icon and instruction text
  - [ ] Has dashed border
- [ ] Start filling in a tile's properties
- [ ] Verify preview updates immediately (no page refresh needed)
- [ ] Add title only - verify it displays
- [ ] Add image - verify it renders
- [ ] Publish page and verify placeholders don't show in read mode

### AC6 — Validation
- [ ] Enter invalid URL (e.g., "not a url") in Link URL field
- [ ] Verify inline validation message appears below field
- [ ] Verify message says something like "Please enter a valid URL"
- [ ] Verify you can still save (warning, not blocking)
- [ ] Enter valid URLs:
  - [ ] Full URL: `https://example.com`
  - [ ] Relative URL: `/sites/mysite`
  - [ ] SharePoint URL: `https://tenant.sharepoint.com/sites/site`
- [ ] Verify validation passes for all valid formats

### AC7 — Performance
- [ ] Open browser DevTools Network tab
- [ ] Load page with web part containing 3 tiles with images
- [ ] Verify images have `loading="lazy"` attribute in HTML
- [ ] Scroll page and verify images load on demand
- [ ] Check image sizes are reasonable (not loading 10MB files)
- [ ] Use Lighthouse or PageSpeed Insights:
  - [ ] Check Performance score
  - [ ] Verify no layout shift warnings
  - [ ] Verify images don't block page load

### AC8 — Theming
- [ ] **Light Theme**:
  - [ ] View web part in default light theme
  - [ ] Verify text is readable, colors are appropriate
- [ ] **Dark Theme**:
  - [ ] Switch site to dark theme (Site settings > Change the look > Theme)
  - [ ] Verify web part adapts (text, borders, backgrounds)
  - [ ] Check tile cards use proper dark theme colors
- [ ] **Custom Tile Colors**:
  - [ ] Set custom background color for a tile (e.g., `#e3f2fd`)
  - [ ] Verify tile uses custom color
  - [ ] Verify text is still readable
- [ ] **High Contrast Mode**:
  - [ ] Enable Windows High Contrast mode
  - [ ] Verify borders are thicker (2px)
  - [ ] Verify focus outlines are visible

### AC9 — Analytics (Optional)
- [ ] Enable "Analytics logging" in property pane
- [ ] Open browser console (F12)
- [ ] Click on Tile 1
- [ ] Verify console log appears with format:
  ```
  [Tile Analytics] Tile 1 clicked: {title: "...", linkUrl: "...", timestamp: "..."}
  ```
- [ ] Click other tiles and verify logs for each
- [ ] Disable analytics - verify no logs appear

## Browser Testing
Test in:
- [ ] Chrome/Edge (latest)
- [ ] Firefox (latest)
- [ ] Safari (if available)
- [ ] Mobile browsers (iOS Safari, Android Chrome)

## Integration Testing
- [ ] Add multiple instances of the web part to same page
- [ ] Verify each instance works independently
- [ ] Test in different page layouts (one column, two column, three column)
- [ ] Test in Teams tab (if applicable)
- [ ] Test in SharePoint mobile app

## Edge Cases
- [ ] Very long tile titles (50+ characters) - verify they don't break layout
- [ ] Very long descriptions (200+ characters) - verify scrolling/truncation
- [ ] Missing images (404 URL) - verify graceful fallback
- [ ] Special characters in text fields - verify they render correctly
- [ ] Empty tiles in read mode - verify they're hidden or show placeholder

## Performance Benchmarks
- [ ] Page load time with 3 tiles: _____ ms
- [ ] First Contentful Paint: _____ ms
- [ ] Largest Contentful Paint: _____ ms
- [ ] Cumulative Layout Shift: _____
- [ ] Time to Interactive: _____ ms

## Notes & Issues Found
(Document any bugs, inconsistencies, or improvements during testing)

---

**Tester Name**: _________________  
**Date**: _________________  
**Browser/Device**: _________________  
**Build Version**: _________________
