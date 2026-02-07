import { IComplianceTask } from '../types/IComplianceTask';

/**
 * Props interface for the main Risk Compliance Tasks component
 */
export interface IRiskComplianceTasksProps {
  tasks: IComplianceTask[];
  loading: boolean;
  error: string | undefined;
  onTaskClick?: (task: IComplianceTask) => void;
  isDarkTheme: boolean;
  hasTeamsContext: boolean;
}
