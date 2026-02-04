import * as React from 'react';
import * as ReactDom from 'react-dom';
import { Version } from '@microsoft/sp-core-library';
import {
  IPropertyPaneConfiguration,
  PropertyPaneToggle,
  PropertyPaneDropdown,
  IPropertyPaneDropdownOption
} from '@microsoft/sp-property-pane';
import { BaseClientSideWebPart } from '@microsoft/sp-webpart-base';

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
 * Implementation based on PROJECT PLAN for Risk Compliance Tasks SharePoint SPFx Web Application
 * 
 * Phase 1: Project Setup & Architecture
 * Phase 2: Feature Design & Specifications
 * Phase 3: Development (Sprint 1 & 2)
 */
export default class RiskComplianceTasksWebPart extends BaseClientSideWebPart<IRiskComplianceTasksWebPartProps> {

  private tasks: IComplianceTask[] = [];
  private loading: boolean = false;
  private error: string | null = null;

  /**
   * Phase 3.2 - Implement data fetching with proper error handling
   */
  private async loadTasks(): Promise<void> {
    this.loading = true;
    this.error = null;
    this.render();

    try {
      if (this.properties.useMockData) {
        // Phase 3.1 - Use mock data service for testing
        this.tasks = await MockDataService.getMockTasksAsync(800);
      } else {
        // Future: Phase 3.2 - Create ComplianceService to fetch from SharePoint/API
        // For now, fallback to mock data
        this.tasks = await MockDataService.getMockTasksAsync(800);
      }
      this.loading = false;
      this.error = null;
    } catch (err) {
      this.loading = false;
      this.error = err.message || 'Failed to load compliance tasks';
      console.error('Error loading tasks:', err);
    }

    this.render();
  }

  protected async onInit(): Promise<void> {
    await super.onInit();
    await this.loadTasks();
  }

  public render(): void {
    const element: React.ReactElement<IRiskComplianceTasksProps> = React.createElement(
      RiskComplianceTasks,
      {
        tasks: this.tasks,
        loading: this.loading,
        error: this.error,
        onTaskClick: (task: IComplianceTask) => {
          console.log('Task clicked in web part:', task);
          // Future: Phase 3.3 - Add quick action buttons
        },
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

  /**
   * Property pane configuration
   * Phase 1.2 - Configure web part property pane (if needed)
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

  protected onPropertyPaneFieldChanged(propertyPath: string, oldValue: any, newValue: any): void {
    super.onPropertyPaneFieldChanged(propertyPath, oldValue, newValue);
    
    // Reload tasks when data source changes
    if (propertyPath === 'useMockData' || propertyPath === 'dataSource') {
      this.loadTasks();
    }
  }
}
