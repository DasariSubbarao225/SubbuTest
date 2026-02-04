import * as React from 'react';
import { IComplianceTask } from '../types/IComplianceTask';
import styles from './TaskTile.module.scss';
import { escape } from '@microsoft/sp-lodash-subset';

export interface ITaskTileProps {
  task: IComplianceTask;
  onClick?: (task: IComplianceTask) => void;
}

/**
 * TaskTile Component
 * Displays individual task information in a tile format with color-coded priority and status
 */
export const TaskTile: React.FC<ITaskTileProps> = ({ task, onClick }) => {
  
  /**
   * Get color class based on priority and status
   * Color Coding: Red (High priority/Overdue), Yellow (Medium), Green (Low/Completed)
   */
  const getPriorityColorClass = (): string => {
    if (task.status === 'Overdue') {
      return styles.priorityHigh; // Red for overdue
    }
    if (task.status === 'Completed') {
      return styles.priorityLow; // Green for completed
    }
    
    switch (task.priority) {
      case 'High':
        return styles.priorityHigh;
      case 'Medium':
        return styles.priorityMedium;
      case 'Low':
        return styles.priorityLow;
      default:
        return '';
    }
  };

  /**
   * Get status class based on task status
   */
  const getStatusClass = (): string => {
    switch (task.status) {
      case 'Not Started':
        return styles.statusNotStarted;
      case 'In Progress':
        return styles.statusInProgress;
      case 'Completed':
        return styles.statusCompleted;
      case 'Overdue':
        return styles.statusOverdue;
      case 'At Risk':
        return styles.statusAtRisk;
      default:
        return '';
    }
  };

  /**
   * Format due date for display with smart relative formatting
   */
  const formatDueDate = (dateString: string): string => {
    const date = new Date(dateString);
    const now = new Date();
    const diffTime = date.getTime() - now.getTime();
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

    if (diffDays < 0) {
      return `${Math.abs(diffDays)} days overdue`;
    } else if (diffDays === 0) {
      return 'Due today';
    } else if (diffDays === 1) {
      return 'Due tomorrow';
    } else if (diffDays <= 7) {
      return `Due in ${diffDays} days`;
    } else {
      return date.toLocaleDateString();
    }
  };

  const handleClick = (): void => {
    if (onClick) {
      onClick(task);
    }
  };

  const handleKeyDown = (event: React.KeyboardEvent): void => {
    if (event.key === 'Enter' || event.key === ' ') {
      handleClick();
      event.preventDefault();
    }
  };

  return (
    <div 
      className={`${styles.taskTile} ${getPriorityColorClass()}`}
      onClick={handleClick}
      onKeyDown={handleKeyDown}
      role="button"
      tabIndex={0}
      aria-label={`Task: ${task.title}, Priority: ${task.priority}, Status: ${task.status}`}
    >
      <div className={styles.tileHeader}>
        <span className={styles.taskId}>#{task.id}</span>
        <span className={`${styles.priorityBadge} ${getPriorityColorClass()}`}>
          {task.priority}
        </span>
      </div>
      
      <h3 className={styles.taskTitle}>{escape(task.title)}</h3>
      
      <p className={styles.taskDescription}>{escape(task.description)}</p>
      
      <div className={styles.statusRow}>
        <span className={`${styles.statusBadge} ${getStatusClass()}`}>
          {task.status}
        </span>
      </div>

      <div className={styles.progressRow}>
        <div className={styles.progressBar}>
          <div 
            className={styles.progressFill} 
            style={{ width: `${task.completionPercentage}%` }}
          />
        </div>
        <span className={styles.progressText}>{task.completionPercentage}%</span>
      </div>
      
      <div className={styles.tileFooter}>
        <div className={styles.assigneeInfo}>
          <span className={styles.assigneeIcon}>👤</span>
          <span className={styles.assigneeName}>{escape(task.assignee)}</span>
        </div>
        <div className={styles.dueDateInfo}>
          <span className={styles.dueDateIcon}>📅</span>
          <span className={styles.dueDateText}>{formatDueDate(task.dueDate)}</span>
        </div>
      </div>
    </div>
  );
};
