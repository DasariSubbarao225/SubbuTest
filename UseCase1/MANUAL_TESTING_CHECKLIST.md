# Manual Testing Checklist for vse3TilesWebPart

## Pre-Testing Setup

- [ ] Ensure Node.js 14.x is installed and active
- [ ] Build the solution successfully: `gulp bundle`
- [ ] Package the solution: `gulp package-solution`
- [ ] Deploy to SharePoint App Catalog
- [ ] Add the web part to a test page

## AC1 — Configurable Content

### Tile 1 Configuration
- [ ] Open web part property pane
- [ ] Navigate to "Tile 1 Configuration" section
- [ ] Set title: "Resource Center"
- [ ] Set description: "Access training materials and documentation"
- [ ] Set image URL: (valid image URL)
- [ ] Set image alt text: "Resource center icon"
- [ ] Set link URL: "https://example.com/resources"
- [ ] Toggle "Open in new tab": ON
- [ ] Set background color: "#E3F2FD" (optional)
- [ ] Verify tile updates in real-time preview

### Tile 2 Configuration
- [ ] Navigate to "Tile 2 Configuration" section
- [ ] Configure with different content
- [ ] Verify preview updates

### Tile 3 Configuration
- [ ] Navigate to "Tile 3 Configuration" section
- [ ] Configure with different content
- [ ] Verify preview updates

## AC2 — Responsive Display

### Desktop View (> 1024px)
- [ ] Resize browser to > 1024px width
- [ ] Verify 3 tiles display in a single row
- [ ] Verify consistent spacing between tiles
- [ ] Verify tiles are aligned properly

### Tablet View (640px - 1024px)
- [ ] Resize browser to ~800px width
- [ ] Verify 2 tiles per row
- [ ] Verify third tile wraps to second row
- [ ] Verify spacing remains consistent

### Mobile View (< 640px)
- [ ] Resize browser to < 640px width
- [ ] Verify 1 tile per row (stacked vertically)
- [ ] Verify tiles stack properly
- [ ] Verify no horizontal scroll

## AC3 — Click Behavior

### Same Tab Navigation
- [ ] Configure a tile with "Open in new tab": OFF
- [ ] Click the tile
- [ ] Verify navigation happens in same tab

### New Tab Navigation
- [ ] Configure a tile with "Open in new tab": ON
- [ ] Click the tile
- [ ] Verify link opens in new tab
- [ ] Check browser dev tools: verify `target="_blank"`
- [ ] Verify `rel="noopener noreferrer"` attribute is present

### Tiles Without Links
- [ ] Configure a tile with no link URL
- [ ] Verify tile is not clickable
- [ ] Verify no cursor pointer on hover

## AC4 — Accessibility

### Keyboard Navigation
- [ ] Press Tab key to navigate through page
- [ ] Verify tiles receive focus in logical order
- [ ] Verify focus indicator is clearly visible
- [ ] Press Enter on focused tile
- [ ] Verify tile link activates

### Screen Reader
- [ ] Use screen reader (NVDA, JAWS, or VoiceOver)
- [ ] Verify each tile announces: title + description
- [ ] Verify images have alt text announced
- [ ] Verify "link" role is announced for clickable tiles

### ARIA Labels
- [ ] Inspect tile elements in dev tools
- [ ] Verify `role="link"` attribute
- [ ] Verify `aria-label` contains title and description
- [ ] Verify `tabindex="0"` for keyboard accessibility

### Color Contrast
- [ ] Use browser color contrast checker extension
- [ ] Verify title text meets WCAG AA (4.5:1 minimum)
- [ ] Verify description text meets WCAG AA
- [ ] Test in both light and dark themes

## AC5 — Authoring UX

### Live Preview
- [ ] Open property pane
- [ ] Change tile 1 title
- [ ] Verify preview updates immediately (no save required)
- [ ] Change tile 2 image URL
- [ ] Verify image updates in preview
- [ ] Change tile 3 link URL
- [ ] Verify no visual change (link is internal)

### Empty Tile Placeholders
- [ ] Create new web part instance (all tiles empty)
- [ ] Verify placeholder message: "Configure Tile 1", etc.
- [ ] Verify placeholder icon displayed
- [ ] Configure one tile
- [ ] Verify placeholder replaced with actual content
- [ ] Verify other two tiles still show placeholders

## AC6 — URL Validation

### Valid URLs
- [ ] Enter valid URL: "https://example.com"
- [ ] Verify no error message
- [ ] Enter valid URL: "http://test.org/page"
- [ ] Verify no error message

### Invalid URLs
- [ ] Enter invalid URL: "not a url"
- [ ] Verify inline error message displayed
- [ ] Verify error text: "Please enter a valid URL"
- [ ] Verify web part still saves (warning, not blocking)
- [ ] Clear invalid URL
- [ ] Verify error message clears

### Empty URLs
- [ ] Leave URL field empty
- [ ] Verify no error (empty is valid)
- [ ] Verify tile renders without link

## AC7 — Performance

### Image Loading
- [ ] Configure tiles with large images (> 1MB)
- [ ] Open browser Network tab
- [ ] Reload page
- [ ] Verify page loads before images finish loading
- [ ] Scroll to tiles
- [ ] Verify images load as they come into view (lazy loading)
- [ ] Check img element: verify `loading="lazy"` attribute

### Page Load Time
- [ ] Use browser performance profiling
- [ ] Measure page load time
- [ ] Verify web part doesn't significantly block page load
- [ ] Verify no JavaScript errors in console

## AC8 — Theming

### Light Theme
- [ ] Apply SharePoint light theme to site
- [ ] Reload page
- [ ] Verify tiles use light theme colors
- [ ] Verify text is readable

### Dark Theme
- [ ] Apply SharePoint dark theme to site
- [ ] Reload page
- [ ] Verify tiles adapt to dark theme
- [ ] Verify text is readable with good contrast
- [ ] Check @media (prefers-color-scheme: dark) styles apply

### Custom Background Colors
- [ ] Set tile 1 background color: "#FFEBEE"
- [ ] Verify tile uses custom color
- [ ] Verify theme doesn't override custom color
- [ ] Set tile 2 background color: "rgb(232, 245, 233)"
- [ ] Verify RGB color format works

### High Contrast Mode
- [ ] Enable Windows High Contrast mode
- [ ] Reload page
- [ ] Verify tile borders are visible
- [ ] Verify focus indicators are enhanced

## AC9 — Analytics (Optional)

### Analytics Disabled (Default)
- [ ] Open browser console
- [ ] Click tile
- [ ] Verify no analytics message logged

### Analytics Enabled
- [ ] Open property pane
- [ ] Navigate to "Advanced Settings"
- [ ] Toggle "Enable analytics logging": ON
- [ ] Open browser console
- [ ] Click tile 1
- [ ] Verify console log: "Tile clicked: [title] (Tile 1)"
- [ ] Click tile 2
- [ ] Verify console log: "Tile clicked: [title] (Tile 2)"
- [ ] Click tile 3
- [ ] Verify console log: "Tile clicked: [title] (Tile 3)"

## Cross-Browser Testing

### Chrome
- [ ] Test all features in latest Chrome
- [ ] Verify responsive breakpoints work

### Firefox
- [ ] Test all features in latest Firefox
- [ ] Verify responsive breakpoints work

### Edge
- [ ] Test all features in latest Edge
- [ ] Verify responsive breakpoints work

### Safari (if available)
- [ ] Test all features in latest Safari
- [ ] Verify responsive breakpoints work

## Edge Cases

### Very Long Text
- [ ] Set title: 100+ characters
- [ ] Verify text wraps appropriately
- [ ] Verify tile height adjusts

### Special Characters
- [ ] Set title with emoji: "Resources 🎓📚"
- [ ] Verify emoji renders correctly
- [ ] Set description with HTML entities
- [ ] Verify text is escaped (no XSS)

### Multiple Web Part Instances
- [ ] Add 3 instances of vse3TilesWebPart to same page
- [ ] Configure each differently
- [ ] Verify no CSS class conflicts
- [ ] Verify no JavaScript errors

### Page Edit vs View Mode
- [ ] Configure web part in edit mode
- [ ] Save page
- [ ] Switch to view mode
- [ ] Verify tiles display correctly
- [ ] Verify links are clickable in view mode

## Final Checklist

- [ ] All acceptance criteria (AC1-AC9) passed
- [ ] No console errors
- [ ] No accessibility violations
- [ ] Responsive design works on all breakpoints
- [ ] Theme support verified
- [ ] Documentation is complete
- [ ] Unit tests pass

## Sign-Off

- Tester Name: _________________
- Date: _________________
- Result: Pass / Fail
- Notes: _________________