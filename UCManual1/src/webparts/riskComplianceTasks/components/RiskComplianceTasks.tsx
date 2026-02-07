import * as React from 'react';
import styles from './RiskComplianceTasks.module.scss';
import { IRiskComplianceTasksProps } from './IRiskComplianceTasksProps';
import { TaskGrid } from './TaskGrid';
import { IComplianceTask } from '../types/IComplianceTask';

/**
 * Main Risk Compliance Tasks Component
 * Displays tasks with loading/error states and summary statistics
 */
const RiskComplianceTasks: React.FC<IRiskComplianceTasksProps> = (props) => {
  const { loading, error, tasks, onTaskClick } = props;

  const handleTaskClick = (task: IComplianceTask): void => {
    if (onTaskClick) {
      onTaskClick(task);
    }
    // Log task click for future analytics
    console.log('Task clicked:', task.title);
  };

  return (
    <div className={styles.riskComplianceTasks}>
      <div className={styles.header}>
        <h2 className={styles.title}>Risk &amp; Compliance Tasks</h2>
        <div className={styles.summary}>
          <span className={styles.summaryItem}>
            <span className={styles.summaryIcon}>📊</span>
            <span className={styles.summaryText}>Total: {tasks.length}</span>
          </span>
          <span className={styles.summaryItem}>
            <span className={styles.summaryIcon}>✅</span>
            <span className={styles.summaryText}>
              Completed: {tasks.filter(t => t.status === 'Completed').length}
            </span>
          </span>
          <span className={styles.summaryItem}>
            <span className={styles.summaryIcon}>⚠️</span>
            <span className={styles.summaryText}>
              Overdue: {tasks.filter(t => t.status === 'Overdue').length}
            </span>
          </span>
        </div>
      </div>

      {loading && (
        <div className={styles.loading}>
          <div className={styles.spinner} />
          <p className={styles.loadingText}>Loading compliance tasks...</p>
        </div>
      )}

      {error && !loading && (
        <div className={styles.error}>
          <div className={styles.errorIcon}>⚠️</div>
          <h3 className={styles.errorTitle}>Error Loading Tasks</h3>
          <p className={styles.errorMessage}>{error}</p>
        </div>
      )}

      {!loading && !error && (
        <TaskGrid 
          tasks={tasks} 
          onTaskClick={handleTaskClick}
        />
      )}
    </div>
  );
};

export default RiskComplianceTasks;
