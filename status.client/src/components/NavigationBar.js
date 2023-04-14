import styles from '../styles/NavigationBar.module.scss';

export default function NavigationBar() {
  return (
    <div className={styles.navigationBar}>
      <h1 className={styles.title}>Status</h1>
      <button className={styles.menu}>=</button>
    </div>
  );
}