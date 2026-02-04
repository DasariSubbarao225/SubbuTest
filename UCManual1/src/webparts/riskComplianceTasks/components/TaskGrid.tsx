import * as React from 'react';
import { IComplianceTask } from '../types/IComplianceTask';
import { TaskTile } from './TaskTile';
import styles from './TaskGrid.module.scss';

export interface ITaskGridProps {
  tasks: IComplianceTask[];
  onTaskClick?: (task: IComplianceTask) => void;
}

/**
 * TaskGrid Component
 * Arranges task tiles in a responsive grid layout
 * - 3 columns on desktop (>1024px)
 * - 2 columns on tablet (640px-1024px)
 * - 1 column on mobile (<640px)
 */
export const TaskGrid: React.FC<ITaskGridProps> = ({ tasks, onTaskClick }) => {
  
  if (tasks.length === 0) {
    return (
      <div className={styles.emptyState}>
        <div className={styles.emptyIcon}>📋</div>
        <h3 className={styles.emptyTitle}>No Tasks Found</h3>
        <p className={styles.emptyDescription}>
          There are no compliance tasks to display at this time.
        </p>
      </div>
    );
  }

  return (
    <div className={styles.taskGrid}>
      {tasks.map((task) => (
        <TaskTile
          key={task.id}
          task={task}
          onClick={onTaskClick}
        />
      ))}
    </div>
  );
};
