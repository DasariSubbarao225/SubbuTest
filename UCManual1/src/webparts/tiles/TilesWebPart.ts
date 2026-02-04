import * as React from 'react';
import * as ReactDom from 'react-dom';
import { Version } from '@microsoft/sp-core-library';
import {
  type IPropertyPaneConfiguration,
  PropertyPaneTextField,
  PropertyPaneToggle
} from '@microsoft/sp-property-pane';
import { BaseClientSideWebPart } from '@microsoft/sp-webpart-base';
import { IReadonlyTheme } from '@microsoft/sp-component-base';

import Tiles from './components/Tiles';
import { ITilesProps, ITileData } from './components/ITilesProps';

export interface ITilesWebPartProps {
  // Tile 1
  tile1Title: string;
  tile1ImageUrl: string;
  tile1AltText: string;
  tile1Description: string;
  tile1LinkUrl: string;
  tile1OpenInNewTab: boolean;
  tile1BackgroundColor: string;
  
  // Tile 2
  tile2Title: string;
  tile2ImageUrl: string;
  tile2AltText: string;
  tile2Description: string;
  tile2LinkUrl: string;
  tile2OpenInNewTab: boolean;
  tile2BackgroundColor: string;
  
  // Tile 3
  tile3Title: string;
  tile3ImageUrl: string;
  tile3AltText: string;
  tile3Description: string;
  tile3LinkUrl: string;
  tile3OpenInNewTab: boolean;
  tile3BackgroundColor: string;
  
  // Global settings
  enableAnalytics: boolean;
}

export default class TilesWebPart extends BaseClientSideWebPart<ITilesWebPartProps> {

  private _isDarkTheme: boolean = false;
  private _semanticColors: any = {};

  protected onInit(): Promise<void> {
    console.log('[TilesWebPart] onInit called');
    console.log('[TilesWebPart] properties:', this.properties);
    console.log('[TilesWebPart] displayMode:', this.displayMode);
    return Promise.resolve();
  }

  private _validateUrl(value: string): string {
    if (!value) {
      return '';
    }
    
    // Basic URL validation
    const urlPattern = /^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$/;
    const relativePattern = /^\/[^\/].*$/; // Starts with single slash
    
    if (!urlPattern.test(value) && !relativePattern.test(value)) {
      return 'Please enter a valid URL (e.g., https://example.com or /sites/mysite)';
    }
    
    return '';
  }

  public render(): void {
    try {
      console.log('[TilesWebPart] render() called');
      console.log('[TilesWebPart] Current properties:', this.properties);
      console.log('[TilesWebPart] DisplayMode value:', this.displayMode);
      console.log('[TilesWebPart] isDarkTheme:', this._isDarkTheme);

      // Safely build tile objects
      const tile1: ITileData = {
        title: this.properties.tile1Title,
        imageUrl: this.properties.tile1ImageUrl,
        altText: this.properties.tile1AltText,
        description: this.properties.tile1Description,
        linkUrl: this.properties.tile1LinkUrl,
        openInNewTab: this.properties.tile1OpenInNewTab,
        backgroundColor: this.properties.tile1BackgroundColor
      };

      const tile2: ITileData = {
        title: this.properties.tile2Title,
        imageUrl: this.properties.tile2ImageUrl,
        altText: this.properties.tile2AltText,
        description: this.properties.tile2Description,
        linkUrl: this.properties.tile2LinkUrl,
        openInNewTab: this.properties.tile2OpenInNewTab,
        backgroundColor: this.properties.tile2BackgroundColor
      };

      const tile3: ITileData = {
        title: this.properties.tile3Title,
        imageUrl: this.properties.tile3ImageUrl,
        altText: this.properties.tile3AltText,
        description: this.properties.tile3Description,
        linkUrl: this.properties.tile3LinkUrl,
        openInNewTab: this.properties.tile3OpenInNewTab,
        backgroundColor: this.properties.tile3BackgroundColor
      };

      console.log('[TilesWebPart] Tile 1:', tile1);
      console.log('[TilesWebPart] Tile 2:', tile2);
      console.log('[TilesWebPart] Tile 3:', tile3);

      const propsToPass: ITilesProps = {
        tile1: tile1,
        tile2: tile2,
        tile3: tile3,
        isDarkTheme: this._isDarkTheme,
        hasTeamsContext: !!this.context.sdks.microsoftTeams,
        semanticColors: this._semanticColors,
        enableAnalytics: this.properties.enableAnalytics,
        displayMode: this.displayMode || 2
      };

      console.log('[TilesWebPart] Props to pass to component:', propsToPass);

      const element: React.ReactElement<ITilesProps> = React.createElement(
        Tiles,
        propsToPass
      );

      console.log('[TilesWebPart] React element created successfully');

      ReactDom.render(element, this.domElement);
      
      console.log('[TilesWebPart] Component rendered to DOM');
    } catch (error) {
      console.error('[TilesWebPart] CRITICAL ERROR in render():', error);
      console.error('[TilesWebPart] Error stack:', (error as Error).stack);
      const message = 'Critical Error. Check console for details.';
      (this.domElement as HTMLElement).textContent = message;
    }
  }

  protected onThemeChanged(currentTheme: IReadonlyTheme | undefined): void {
    console.log('[TilesWebPart] onThemeChanged called with theme:', currentTheme);
    
    if (!currentTheme) {
      console.warn('[TilesWebPart] currentTheme is undefined, returning');
      return;
    }

    try {
      this._isDarkTheme = !!currentTheme.isInverted;
      const { semanticColors } = currentTheme;

      console.log('[TilesWebPart] isDarkTheme set to:', this._isDarkTheme);
      console.log('[TilesWebPart] semanticColors:', semanticColors);

      if (semanticColors) {
        this._semanticColors = semanticColors;
        this.domElement.style.setProperty('--bodyText', semanticColors.bodyText || null);
        this.domElement.style.setProperty('--link', semanticColors.link || null);
        this.domElement.style.setProperty('--linkHovered', semanticColors.linkHovered || null);
        console.log('[TilesWebPart] Theme CSS variables set');
      }

      this.render();
      console.log('[TilesWebPart] onThemeChanged completed successfully');
    } catch (error) {
      console.error('[TilesWebPart] Error in onThemeChanged:', error);
    }
  }

  protected onDispose(): void {
    ReactDom.unmountComponentAtNode(this.domElement);
  }

  protected get dataVersion(): Version {
    return Version.parse('1.0');
  }

  protected getPropertyPaneConfiguration(): IPropertyPaneConfiguration {
    return {
      pages: [
        {
          header: {
            description: 'Configure three tiles to highlight important resources or actions'
          },
          groups: [
            {
              groupName: 'Tile 1',
              groupFields: [
                PropertyPaneTextField('tile1Title', {
                  label: 'Title',
                  placeholder: 'Enter tile title'
                }),
                PropertyPaneTextField('tile1ImageUrl', {
                  label: 'Image URL',
                  placeholder: 'https://example.com/image.jpg or /sites/images/icon.png'
                }),
                PropertyPaneTextField('tile1AltText', {
                  label: 'Image Alt Text',
                  placeholder: 'Descriptive text for the image'
                }),
                PropertyPaneTextField('tile1Description', {
                  label: 'Description',
                  placeholder: 'Brief description of the tile',
                  multiline: true
                }),
                PropertyPaneTextField('tile1LinkUrl', {
                  label: 'Link URL',
                  placeholder: 'https://example.com or /sites/mysite',
                  onGetErrorMessage: this._validateUrl.bind(this),
                  deferredValidationTime: 500
                }),
                PropertyPaneToggle('tile1OpenInNewTab', {
                  label: 'Open in new tab',
                  onText: 'Yes',
                  offText: 'No'
                }),
                PropertyPaneTextField('tile1BackgroundColor', {
                  label: 'Background Color (optional)',
                  placeholder: '#f3f2f1 or transparent'
                })
              ]
            },
            {
              groupName: 'Tile 2',
              groupFields: [
                PropertyPaneTextField('tile2Title', {
                  label: 'Title',
                  placeholder: 'Enter tile title'
                }),
                PropertyPaneTextField('tile2ImageUrl', {
                  label: 'Image URL',
                  placeholder: 'https://example.com/image.jpg or /sites/images/icon.png'
                }),
                PropertyPaneTextField('tile2AltText', {
                  label: 'Image Alt Text',
                  placeholder: 'Descriptive text for the image'
                }),
                PropertyPaneTextField('tile2Description', {
                  label: 'Description',
                  placeholder: 'Brief description of the tile',
                  multiline: true
                }),
                PropertyPaneTextField('tile2LinkUrl', {
                  label: 'Link URL',
                  placeholder: 'https://example.com or /sites/mysite',
                  onGetErrorMessage: this._validateUrl.bind(this),
                  deferredValidationTime: 500
                }),
                PropertyPaneToggle('tile2OpenInNewTab', {
                  label: 'Open in new tab',
                  onText: 'Yes',
                  offText: 'No'
                }),
                PropertyPaneTextField('tile2BackgroundColor', {
                  label: 'Background Color (optional)',
                  placeholder: '#f3f2f1 or transparent'
                })
              ]
            },
            {
              groupName: 'Tile 3',
              groupFields: [
                PropertyPaneTextField('tile3Title', {
                  label: 'Title',
                  placeholder: 'Enter tile title'
                }),
                PropertyPaneTextField('tile3ImageUrl', {
                  label: 'Image URL',
                  placeholder: 'https://example.com/image.jpg or /sites/images/icon.png'
                }),
                PropertyPaneTextField('tile3AltText', {
                  label: 'Image Alt Text',
                  placeholder: 'Descriptive text for the image'
                }),
                PropertyPaneTextField('tile3Description', {
                  label: 'Description',
                  placeholder: 'Brief description of the tile',
                  multiline: true
                }),
                PropertyPaneTextField('tile3LinkUrl', {
                  label: 'Link URL',
                  placeholder: 'https://example.com or /sites/mysite',
                  onGetErrorMessage: this._validateUrl.bind(this),
                  deferredValidationTime: 500
                }),
                PropertyPaneToggle('tile3OpenInNewTab', {
                  label: 'Open in new tab',
                  onText: 'Yes',
                  offText: 'No'
                }),
                PropertyPaneTextField('tile3BackgroundColor', {
                  label: 'Background Color (optional)',
                  placeholder: '#f3f2f1 or transparent'
                })
              ]
            },
            {
              groupName: 'Advanced Settings',
              groupFields: [
                PropertyPaneToggle('enableAnalytics', {
                  label: 'Enable analytics logging',
                  onText: 'Enabled',
                  offText: 'Disabled'
                })
              ]
            }
          ]
        }
      ]
    };
  }
}
