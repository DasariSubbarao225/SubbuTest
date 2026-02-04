import * as React from 'react';
import * as ReactDom from 'react-dom';
import { Version } from '@microsoft/sp-core-library';
import {
  type IPropertyPaneConfiguration,
  PropertyPaneToggle,
  PropertyPaneDropdown,
  type IPropertyPaneDropdownOption
} from '@microsoft/sp-property-pane';
import { BaseClientSideWebPart } from '@microsoft/sp-webpart-base';
import { IReadonlyTheme } from '@microsoft/sp-component-base';

import RiskComplianceTasks from './components/RiskComplianceTasks';
import { IRiskComplianceTasksProps } from './components/IRiskComplianceTasksProps';
import { IComplianceTask } from './types/IComplianceTask';
import { MockDataService } from './services/MockDataService';

export interface IRiskComplianceTasksWebPartProps {
  useMockData: boolean;
  dataSource: string;
}

/**
 * Risk Compliance Tasks Web Part
 * Displays risk and compliance tasks in a tile-based view with priority color coding,
 * status management, and progress tracking.
 */
export default class RiskComplianceTasksWebPart extends BaseClientSideWebPart<IRiskComplianceTasksWebPartProps> {

  private _tasks: IComplianceTask[] = [];
  private _loading: boolean = false;
  private _error: string | undefined = undefined;
  private _isDarkTheme: boolean = false;

  /**
   * Load tasks from the configured data source
   */
  private async _loadTasks(): Promise<void> {
    this._loading = true;
    this._error = undefined;
    this.render();

    try {
      if (this.properties.useMockData) {
        // Use mock data service for testing
        this._tasks = await MockDataService.getMockTasksAsync(800);
      } else {
        // Future: Fetch from SharePoint/API
        // For now, fallback to mock data
        this._tasks = await MockDataService.getMockTasksAsync(800);
      }
      this._loading = false;
      this._error = undefined;
    } catch (err) {
      this._loading = false;
      // Type guard for Error object
      if (err instanceof Error) {
        this._error = err.message;
      } else {
        this._error = 'Failed to load compliance tasks';
      }
      console.error('Error loading tasks:', err);
    }

    this.render();
  }

  protected async onInit(): Promise<void> {
    console.log('[RiskComplianceTasksWebPart] onInit called');
    await this._loadTasks();
    return Promise.resolve();
  }

  public render(): void {
    try {
      console.log('[RiskComplianceTasksWebPart] render() called');

      const element: React.ReactElement<IRiskComplianceTasksProps> = React.createElement(
        RiskComplianceTasks,
        {
          tasks: this._tasks,
          loading: this._loading,
          error: this._error,
          onTaskClick: (task: IComplianceTask) => {
            console.log('Task clicked in web part:', task);
            // Future: Open task details panel or navigate to task page
          },
          isDarkTheme: this._isDarkTheme,
          hasTeamsContext: !!this.context.sdks.microsoftTeams
        }
      );

      ReactDom.render(element, this.domElement);
      console.log('[RiskComplianceTasksWebPart] Component rendered to DOM');
    } catch (error) {
      console.error('[RiskComplianceTasksWebPart] Error in render():', error);
    }
  }

  protected onThemeChanged(currentTheme: IReadonlyTheme | undefined): void {
    if (!currentTheme) {
      return;
    }

    this._isDarkTheme = !!currentTheme.isInverted;
    const { semanticColors } = currentTheme;

    if (semanticColors) {
      this.domElement.style.setProperty('--bodyText', semanticColors.bodyText || null);
      this.domElement.style.setProperty('--link', semanticColors.link || null);
      this.domElement.style.setProperty('--linkHovered', semanticColors.linkHovered || null);
    }

    this.render();
  }

  protected onDispose(): void {
    ReactDom.unmountComponentAtNode(this.domElement);
  }

  protected get dataVersion(): Version {
    return Version.parse('1.0');
  }

  /**
   * Property pane configuration
   */
  protected getPropertyPaneConfiguration(): IPropertyPaneConfiguration {
    const dataSourceOptions: IPropertyPaneDropdownOption[] = [
      { key: 'mockData', text: 'Mock Data (for testing)' },
      { key: 'sharePointList', text: 'SharePoint List (coming soon)' },
      { key: 'externalApi', text: 'External API (coming soon)' }
    ];

    return {
      pages: [
        {
          header: {
            description: 'Configure the Risk Compliance Tasks web part data source and display options.'
          },
          groups: [
            {
              groupName: 'Data Source Settings',
              groupFields: [
                PropertyPaneToggle('useMockData', {
                  label: 'Use Mock Data',
                  onText: 'Enabled (for testing)',
                  offText: 'Disabled'
                }),
                PropertyPaneDropdown('dataSource', {
                  label: 'Data Source',
                  options: dataSourceOptions,
                  selectedKey: 'mockData',
                  disabled: !this.properties.useMockData
                })
              ]
            }
          ]
        }
      ]
    };
  }

  protected onPropertyPaneFieldChanged(propertyPath: string, oldValue: unknown, newValue: unknown): void {
    super.onPropertyPaneFieldChanged(propertyPath, oldValue, newValue);
    
    // Reload tasks when data source changes
    if (propertyPath === 'useMockData' || propertyPath === 'dataSource') {
      this._loadTasks().catch(console.error);
    }
  }
}
