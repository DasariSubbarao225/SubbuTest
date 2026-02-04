/**
 * Interface for a Risk Compliance Task
 * Based on Phase 4.4 API/DATA MODEL from project plan
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
 * Based on Phase 4.3 STATE MANAGEMENT STRUCTURE from project plan
 */
export interface ITaskFilters {
  status: string[];
  priority: string[];
  assignee: string[];
  dueDateRange: { from: Date | null; to: Date | null };
}

/**
 * Interface for sorting configuration
 */
export interface ITaskSorting {
  field: 'dueDate' | 'priority' | 'title';
  direction: 'asc' | 'desc';
}
