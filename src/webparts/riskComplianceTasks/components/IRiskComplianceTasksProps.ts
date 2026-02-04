import { IComplianceTask } from '../types/IComplianceTask';

/**
 * Props interface for the main Risk Compliance Tasks component
 */
export interface IRiskComplianceTasksProps {
  tasks: IComplianceTask[];
  loading: boolean;
  error: string | null;
  onTaskClick?: (task: IComplianceTask) => void;
  themeVariant: any; // IReadonlyTheme from SPFx - using any for SPFx 1.11.0 compatibility
}
