import * as React from 'react';
import styles from './Tiles.module.scss';
import type { ITilesProps, ITileData } from './ITilesProps';
import { DisplayMode } from '@microsoft/sp-core-library';

export default class Tiles extends React.Component<ITilesProps> {

  private _handleTileClick = (tile: ITileData, tileNumber: number): void => {
    if (this.props.enableAnalytics) {
      console.log(`[Tile Analytics] Tile ${tileNumber} clicked:`, {
        title: tile.title,
        linkUrl: tile.linkUrl,
        timestamp: new Date().toISOString()
      });
    }
  };

  private _renderTile = (tile: ITileData, index: number): JSX.Element => {
    const isEditMode = this.props.displayMode === DisplayMode.Edit;
    const isEmpty = !tile.title && !tile.imageUrl && !tile.description && !tile.linkUrl;
    
    // Show placeholder in edit mode if tile is empty
    if (isEmpty && isEditMode) {
      return (
        <div 
          className={`${styles.tile} ${styles.emptyTile}`}
          role="article"
          aria-label={`Empty tile ${index + 1}. Please configure in the property pane.`}
        >
          <div className={styles.placeholderContent}>
            <div className={styles.placeholderIcon}>📝</div>
            <h3 className={styles.placeholderTitle}>Configure Tile {index + 1}</h3>
            <p className={styles.placeholderText}>
              Click the edit button to add title, image, description, and link.
            </p>
          </div>
        </div>
      );
    }

    const tileStyle: React.CSSProperties = {};
    if (tile.backgroundColor) {
      tileStyle.backgroundColor = tile.backgroundColor;
    }

    const ariaLabel = `${tile.title}. ${tile.description}. ${tile.linkUrl ? 'Click to navigate.' : ''}`;

    // If there's a link, render as a clickable card
    if (tile.linkUrl) {
      return (
        <a
          href={tile.linkUrl}
          className={styles.tile}
          style={tileStyle}
          target={tile.openInNewTab ? '_blank' : '_self'}
          rel={tile.openInNewTab ? 'noopener noreferrer' : undefined}
          role="article"
          aria-label={ariaLabel}
          onClick={() => this._handleTileClick(tile, index + 1)}
          tabIndex={0}
        >
          {this._renderTileContent(tile)}
        </a>
      );
    }

    // No link - render as static card
    return (
      <div
        className={styles.tile}
        style={tileStyle}
        role="article"
        aria-label={ariaLabel}
      >
        {this._renderTileContent(tile)}
      </div>
    );
  };

  private _renderTileContent = (tile: ITileData): JSX.Element => {
    return (
      <>
        {tile.imageUrl && (
          <div className={styles.tileImageContainer}>
            <img
              src={tile.imageUrl}
              alt={tile.altText || tile.title || 'Tile image'}
              className={styles.tileImage}
              loading="lazy"
            />
          </div>
        )}
        <div className={styles.tileContent}>
          {tile.title && (
            <h3 className={styles.tileTitle}>{tile.title}</h3>
          )}
          {tile.description && (
            <p className={styles.tileDescription}>{tile.description}</p>
          )}
        </div>
      </>
    );
  };

  public render(): React.ReactElement<ITilesProps> {
    const { tile1, tile2, tile3, hasTeamsContext } = this.props;

    return (
      <section 
        className={`${styles.tiles} ${hasTeamsContext ? styles.teams : ''}`}
        aria-label="Three tiles section"
      >
        <div className={styles.tilesContainer}>
          {this._renderTile(tile1, 0)}
          {this._renderTile(tile2, 1)}
          {this._renderTile(tile3, 2)}
        </div>
      </section>
    );
  }
}
