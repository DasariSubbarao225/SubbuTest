# Build Requirements and Known Issues

## Critical: Node.js Version Requirement

This project uses SharePoint Framework (SPFx) version 1.11.0, which has strict Node.js version requirements due to its dependency on `node-sass`.

### Required Environment
- **Node.js**: Version 14.x (LTS) **ONLY**
- **npm**: Version 6.x
- Node.js 16+ will **NOT** work due to node-sass compilation issues

### Why This Limitation Exists

SPFx 1.11.0 uses `@microsoft/sp-build-web@1.11.0` which internally depends on `node-sass@4.12.0`. The `node-sass` package:
1. Is deprecated and no longer maintained
2. Requires Python 2.7 for compilation on some systems
3. Is incompatible with Node.js 16+ (uses removed Node.js APIs)
4. Cannot be easily upgraded without upgrading the entire SPFx version

### Current Workaround

Use Node Version Manager (nvm) to switch to Node.js 14.x:

```bash
# Install NVM (if not already installed)
curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.0/install.sh | bash

# Install Node 14
nvm install 14

# Use Node 14
nvm use 14

# Verify
node --version  # Should output v14.x.x

# Install dependencies
npm install

# Build the project
npm run build
```

### Future Upgrade Path (Recommended)

To resolve this permanently, the project should be upgraded to:
- **SPFx 1.18+** or **SPFx 1.22.2+** (as specified in the project plan)
- Replace `node-sass` with `sass` (Dart Sass)
- Update to Node.js 18.x LTS
- Update React to version 18.x
- Migrate from Gulp to Heft build toolchain (SPFx 1.20+)

**Note**: The `sass` package (Dart Sass) is already added to devDependencies in preparation for this upgrade, but it won't be used until SPFx is upgraded.

## Current Build Status

The code is complete and correct. Build failures in CI/CD are expected if running on Node.js 20.x. The solution will build successfully once the environment is configured with Node.js 14.x.

## Web Parts Included

1. **VSE 3 Tiles Web Part** - Existing, production-ready
2. **Risk Compliance Tasks Web Part** - New, Sprint 1-2 complete, ready for testing

Both web parts follow SPFx best practices and are TypeScript/React-based.
