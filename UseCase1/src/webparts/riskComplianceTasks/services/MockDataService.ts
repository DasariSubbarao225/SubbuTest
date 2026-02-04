import { IComplianceTask } from '../types/IComplianceTask';

/**
 * Mock Data Service for testing
 * Phase 3.1 - Create mock data service for testing
 */
export class MockDataService {
  
  /**
   * Get mock compliance tasks
   * Returns sample data for development and testing
   */
  public static getMockTasks(): IComplianceTask[] {
    const today = new Date();
    const tomorrow = new Date(today);
    tomorrow.setDate(tomorrow.getDate() + 1);
    const nextWeek = new Date(today);
    nextWeek.setDate(nextWeek.getDate() + 7);
    const lastWeek = new Date(today);
    lastWeek.setDate(lastWeek.getDate() - 7);

    return [
      {
        id: '1',
        title: 'Annual Security Audit',
        description: 'Complete the annual security audit for Q1 2026',
        priority: 'High',
        status: 'In Progress',
        dueDate: nextWeek.toISOString(),
        assignee: 'John Smith',
        assigneeEmail: 'john.smith@contoso.com',
        completionPercentage: 45,
        tags: ['security', 'audit', 'Q1']
      },
      {
        id: '2',
        title: 'GDPR Compliance Review',
        description: 'Review and update GDPR compliance documentation',
        priority: 'High',
        status: 'Not Started',
        dueDate: tomorrow.toISOString(),
        assignee: 'Sarah Johnson',
        assigneeEmail: 'sarah.johnson@contoso.com',
        completionPercentage: 0,
        tags: ['GDPR', 'compliance', 'privacy']
      },
      {
        id: '3',
        title: 'Risk Assessment Report',
        description: 'Prepare quarterly risk assessment report',
        priority: 'Medium',
        status: 'In Progress',
        dueDate: nextWeek.toISOString(),
        assignee: 'Michael Brown',
        assigneeEmail: 'michael.brown@contoso.com',
        completionPercentage: 60,
        tags: ['risk', 'assessment', 'quarterly']
      },
      {
        id: '4',
        title: 'Employee Training Completion',
        description: 'Ensure all employees complete compliance training',
        priority: 'Low',
        status: 'Completed',
        dueDate: lastWeek.toISOString(),
        assignee: 'Emily Davis',
        assigneeEmail: 'emily.davis@contoso.com',
        completionPercentage: 100,
        tags: ['training', 'compliance']
      },
      {
        id: '5',
        title: 'Vendor Risk Assessment',
        description: 'Assess compliance risks for new vendor partnerships',
        priority: 'High',
        status: 'Overdue',
        dueDate: lastWeek.toISOString(),
        assignee: 'David Wilson',
        assigneeEmail: 'david.wilson@contoso.com',
        completionPercentage: 25,
        tags: ['vendor', 'risk', 'assessment']
      },
      {
        id: '6',
        title: 'Policy Update Review',
        description: 'Review and update company compliance policies',
        priority: 'Medium',
        status: 'At Risk',
        dueDate: tomorrow.toISOString(),
        assignee: 'Jennifer Martinez',
        assigneeEmail: 'jennifer.martinez@contoso.com',
        completionPercentage: 30,
        tags: ['policy', 'update']
      }
    ];
  }

  /**
   * Simulate async data fetch
   * @param delay Delay in milliseconds (default 500ms)
   */
  public static async getMockTasksAsync(delay: number = 500): Promise<IComplianceTask[]> {
    return new Promise((resolve) => {
      setTimeout(() => {
        resolve(this.getMockTasks());
      }, delay);
    });
  }
}
