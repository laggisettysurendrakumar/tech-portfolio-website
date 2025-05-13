export type AlertType = 'success' | 'error' | 'warning';

export interface Notification {
  message: string;
  type: AlertType;
}
