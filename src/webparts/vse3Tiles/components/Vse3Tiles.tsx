import * as React from 'react';
import styles from './Vse3Tiles.module.scss';
import { IVse3TilesProps, ITileConfig } from './IVse3TilesProps';
import { escape } from '@microsoft/sp-lodash-subset';

export default class Vse3Tiles extends React.Component<IVse3TilesProps, {}> {
  
  private handleTileClick = (tile: ITileConfig, index: number): void => {
    if (this.props.enableAnalytics) {
      console.log(`Tile clicked: ${tile.title} (Tile ${index + 1})`);
    }
  }

  private renderTile = (tile: ITileConfig, index: number): JSX.Element => {
    const isEmpty = !tile.title && !tile.description && !tile.imageUrl && !tile.linkUrl;
    
    const tileStyle: React.CSSProperties = {
      backgroundColor: tile.backgroundColor || undefined
    };

    const tileContent = (
      <div className={styles.tileContent}>
        {tile.imageUrl && (
          <div className={styles.tileImageContainer}>
            <img 
              src={tile.imageUrl} 
              alt={tile.imageAlt || tile.title || 'Tile image'}
              className={styles.tileImage}
              loading="lazy"
            />
          </div>
        )}
        <div className={styles.tileTextContainer}>
          {tile.title && (
            <h3 className={styles.tileTitle}>
              {escape(tile.title)}
            </h3>
          )}
          {tile.description && (
            <p className={styles.tileDescription}>
              {escape(tile.description)}
            </p>
          )}
        </div>
      </div>
    );

    if (isEmpty) {
      return (
        <div 
          className={`${styles.tile} ${styles.tileEmpty}`}
          style={tileStyle}
          key={index}
        >
          <div className={styles.tilePlaceholder}>
            <span className={styles.placeholderIcon}>⚙️</span>
            <span className={styles.placeholderText}>Configure Tile {index + 1}</span>
          </div>
        </div>
      );
    }

    if (!tile.linkUrl) {
      return (
        <div 
          className={styles.tile}
          style={tileStyle}
          key={index}
        >
          {tileContent}
        </div>
      );
    }

    const linkProps: React.AnchorHTMLAttributes<HTMLAnchorElement> = {
      href: tile.linkUrl,
      className: styles.tile,
      onClick: () => this.handleTileClick(tile, index),
      'aria-label': `${tile.title}${tile.description ? ': ' + tile.description : ''}`,
      role: 'link',
      tabIndex: 0
    };

    if (tile.openInNewTab) {
      linkProps.target = '_blank';
      linkProps.rel = 'noopener noreferrer';
    }

    return (
      <a 
        {...linkProps}
        style={tileStyle}
        key={index}
      >
        {tileContent}
      </a>
    );
  }

  public render(): React.ReactElement<IVse3TilesProps> {
    return (
      <div className={styles.vse3Tiles}>
        <div className={styles.tilesContainer}>
          {this.props.tiles.map((tile, index) => this.renderTile(tile, index))}
        </div>
      </div>
    );
  }
}
