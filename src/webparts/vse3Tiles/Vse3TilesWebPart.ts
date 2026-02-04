import * as React from 'react';
import * as ReactDom from 'react-dom';
import { Version } from '@microsoft/sp-core-library';
import {
  IPropertyPaneConfiguration,
  PropertyPaneTextField,
  PropertyPaneToggle,
  IPropertyPaneDropdownOption
} from '@microsoft/sp-property-pane';
import { BaseClientSideWebPart } from '@microsoft/sp-webpart-base';

import * as strings from 'Vse3TilesWebPartStrings';
import Vse3Tiles from './components/Vse3Tiles';
import { IVse3TilesProps, ITileConfig } from './components/IVse3TilesProps';

export interface IVse3TilesWebPartProps {
  tile1Title: string;
  tile1Description: string;
  tile1ImageUrl: string;
  tile1ImageAlt: string;
  tile1LinkUrl: string;
  tile1OpenInNewTab: boolean;
  tile1BackgroundColor: string;
  
  tile2Title: string;
  tile2Description: string;
  tile2ImageUrl: string;
  tile2ImageAlt: string;
  tile2LinkUrl: string;
  tile2OpenInNewTab: boolean;
  tile2BackgroundColor: string;
  
  tile3Title: string;
  tile3Description: string;
  tile3ImageUrl: string;
  tile3ImageAlt: string;
  tile3LinkUrl: string;
  tile3OpenInNewTab: boolean;
  tile3BackgroundColor: string;
  
  enableAnalytics: boolean;
}

export default class Vse3TilesWebPart extends BaseClientSideWebPart<IVse3TilesWebPartProps> {

  public render(): void {
    const tiles: ITileConfig[] = [
      {
        title: this.properties.tile1Title || '',
        description: this.properties.tile1Description || '',
        imageUrl: this.properties.tile1ImageUrl || '',
        imageAlt: this.properties.tile1ImageAlt || '',
        linkUrl: this.properties.tile1LinkUrl || '',
        openInNewTab: this.properties.tile1OpenInNewTab || false,
        backgroundColor: this.properties.tile1BackgroundColor || ''
      },
      {
        title: this.properties.tile2Title || '',
        description: this.properties.tile2Description || '',
        imageUrl: this.properties.tile2ImageUrl || '',
        imageAlt: this.properties.tile2ImageAlt || '',
        linkUrl: this.properties.tile2LinkUrl || '',
        openInNewTab: this.properties.tile2OpenInNewTab || false,
        backgroundColor: this.properties.tile2BackgroundColor || ''
      },
      {
        title: this.properties.tile3Title || '',
        description: this.properties.tile3Description || '',
        imageUrl: this.properties.tile3ImageUrl || '',
        imageAlt: this.properties.tile3ImageAlt || '',
        linkUrl: this.properties.tile3LinkUrl || '',
        openInNewTab: this.properties.tile3OpenInNewTab || false,
        backgroundColor: this.properties.tile3BackgroundColor || ''
      }
    ];

    const element: React.ReactElement<IVse3TilesProps> = React.createElement(
      Vse3Tiles,
      {
        tiles: tiles,
        enableAnalytics: this.properties.enableAnalytics || false,
        themeVariant: this.context.pageContext.legacyPageContext.theme
      }
    );

    ReactDom.render(element, this.domElement);
  }

  protected onDispose(): void {
    ReactDom.unmountComponentAtNode(this.domElement);
  }

  protected get dataVersion(): Version {
    return Version.parse('1.0');
  }

  private validateUrl(value: string): string {
    if (!value) {
      return '';
    }
    
    // Use URL constructor for proper validation
    try {
      new URL(value);
      return '';
    } catch {
      // If URL constructor fails, it's not a valid URL
      return strings.InvalidUrlMessage;
    }
  }

  protected getPropertyPaneConfiguration(): IPropertyPaneConfiguration {
    return {
      pages: [
        {
          header: {
            description: strings.PropertyPaneDescription
          },
          groups: [
            {
              groupName: strings.Tile1GroupName,
              groupFields: [
                PropertyPaneTextField('tile1Title', {
                  label: strings.TitleFieldLabel,
                  placeholder: strings.TitlePlaceholder
                }),
                PropertyPaneTextField('tile1Description', {
                  label: strings.DescriptionFieldLabel,
                  placeholder: strings.DescriptionPlaceholder,
                  multiline: true,
                  rows: 3
                }),
                PropertyPaneTextField('tile1ImageUrl', {
                  label: strings.ImageUrlFieldLabel,
                  placeholder: strings.ImageUrlPlaceholder
                }),
                PropertyPaneTextField('tile1ImageAlt', {
                  label: strings.ImageAltFieldLabel,
                  placeholder: strings.ImageAltPlaceholder
                }),
                PropertyPaneTextField('tile1LinkUrl', {
                  label: strings.LinkUrlFieldLabel,
                  placeholder: strings.LinkUrlPlaceholder,
                  onGetErrorMessage: this.validateUrl.bind(this)
                }),
                PropertyPaneToggle('tile1OpenInNewTab', {
                  label: strings.OpenInNewTabFieldLabel,
                  onText: strings.OpenInNewTabOnText,
                  offText: strings.OpenInNewTabOffText
                }),
                PropertyPaneTextField('tile1BackgroundColor', {
                  label: strings.BackgroundColorFieldLabel,
                  placeholder: strings.BackgroundColorPlaceholder
                })
              ]
            },
            {
              groupName: strings.Tile2GroupName,
              groupFields: [
                PropertyPaneTextField('tile2Title', {
                  label: strings.TitleFieldLabel,
                  placeholder: strings.TitlePlaceholder
                }),
                PropertyPaneTextField('tile2Description', {
                  label: strings.DescriptionFieldLabel,
                  placeholder: strings.DescriptionPlaceholder,
                  multiline: true,
                  rows: 3
                }),
                PropertyPaneTextField('tile2ImageUrl', {
                  label: strings.ImageUrlFieldLabel,
                  placeholder: strings.ImageUrlPlaceholder
                }),
                PropertyPaneTextField('tile2ImageAlt', {
                  label: strings.ImageAltFieldLabel,
                  placeholder: strings.ImageAltPlaceholder
                }),
                PropertyPaneTextField('tile2LinkUrl', {
                  label: strings.LinkUrlFieldLabel,
                  placeholder: strings.LinkUrlPlaceholder,
                  onGetErrorMessage: this.validateUrl.bind(this)
                }),
                PropertyPaneToggle('tile2OpenInNewTab', {
                  label: strings.OpenInNewTabFieldLabel,
                  onText: strings.OpenInNewTabOnText,
                  offText: strings.OpenInNewTabOffText
                }),
                PropertyPaneTextField('tile2BackgroundColor', {
                  label: strings.BackgroundColorFieldLabel,
                  placeholder: strings.BackgroundColorPlaceholder
                })
              ]
            },
            {
              groupName: strings.Tile3GroupName,
              groupFields: [
                PropertyPaneTextField('tile3Title', {
                  label: strings.TitleFieldLabel,
                  placeholder: strings.TitlePlaceholder
                }),
                PropertyPaneTextField('tile3Description', {
                  label: strings.DescriptionFieldLabel,
                  placeholder: strings.DescriptionPlaceholder,
                  multiline: true,
                  rows: 3
                }),
                PropertyPaneTextField('tile3ImageUrl', {
                  label: strings.ImageUrlFieldLabel,
                  placeholder: strings.ImageUrlPlaceholder
                }),
                PropertyPaneTextField('tile3ImageAlt', {
                  label: strings.ImageAltFieldLabel,
                  placeholder: strings.ImageAltPlaceholder
                }),
                PropertyPaneTextField('tile3LinkUrl', {
                  label: strings.LinkUrlFieldLabel,
                  placeholder: strings.LinkUrlPlaceholder,
                  onGetErrorMessage: this.validateUrl.bind(this)
                }),
                PropertyPaneToggle('tile3OpenInNewTab', {
                  label: strings.OpenInNewTabFieldLabel,
                  onText: strings.OpenInNewTabOnText,
                  offText: strings.OpenInNewTabOffText
                }),
                PropertyPaneTextField('tile3BackgroundColor', {
                  label: strings.BackgroundColorFieldLabel,
                  placeholder: strings.BackgroundColorPlaceholder
                })
              ]
            },
            {
              groupName: strings.SettingsGroupName,
              groupFields: [
                PropertyPaneToggle('enableAnalytics', {
                  label: strings.EnableAnalyticsFieldLabel,
                  onText: strings.EnableAnalyticsOnText,
                  offText: strings.EnableAnalyticsOffText
                })
              ]
            }
          ]
        }
      ]
    };
  }
}
