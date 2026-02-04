/**
 * Interface for a Risk Compliance Task
 */
export interface IComplianceTask {
  id: string;
  title: string;
  description: string;
  priority: 'High' | 'Medium' | 'Low';
  status: 'Not Started' | 'In Progress' | 'Completed' | 'Overdue' | 'At Risk';
  dueDate: string; // ISO format
  assignee: string;
  assigneeEmail: string;
  completionPercentage: number;
  tags: string[];
}

/**
 * Interface for filter state
 */
export interface ITaskFilters {
  status: string[];
  priority: string[];
  assignee: string[];
  dueDateRange: { from: Date | undefined; to: Date | undefined };
}

/**
 * Interface for sorting configuration
 */
export interface ITaskSorting {
  field: 'dueDate' | 'priority' | 'title';
  direction: 'asc' | 'desc';
}
