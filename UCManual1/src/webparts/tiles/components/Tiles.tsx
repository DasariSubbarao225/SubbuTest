import * as React from 'react';
import styles from './Tiles.module.scss';
import type { ITilesProps, ITileData } from './ITilesProps';

export default class Tiles extends React.Component<ITilesProps> {

  constructor(props: ITilesProps) {
    super(props);
    try {
      console.log('[Tiles] Constructor called');
      console.log('[Tiles] Props received:', JSON.stringify(props, null, 2));
      console.log('[Tiles] DisplayMode:', props.displayMode);
      console.log('[Tiles] Tile1:', props.tile1);
      console.log('[Tiles] Tile2:', props.tile2);
      console.log('[Tiles] Tile3:', props.tile3);
    } catch (e) {
      console.error('[Tiles] Error in constructor:', e);
    }
  }

  componentDidMount(): void {
    console.log('[Tiles] componentDidMount - component mounted to DOM');
  }

  componentDidUpdate(prevProps: ITilesProps): void {
    console.log('[Tiles] componentDidUpdate - props changed');
    if (prevProps !== this.props) {
      console.log('[Tiles] Props changed from:', prevProps);
      console.log('[Tiles] Props changed to:', this.props);
    }
  }

  componentDidCatch(error: Error, errorInfo: React.ErrorInfo): void {
    console.error('[Tiles] ===== COMPONENT ERROR CAUGHT =====');
    console.error('[Tiles] Error:', error);
    console.error('[Tiles] Error message:', error.message);
    console.error('[Tiles] Error stack:', error.stack);
    console.error('[Tiles] Error info:', errorInfo);
    console.error('[Tiles] Error info componentStack:', errorInfo.componentStack);
    console.error('[Tiles] ===== END ERROR =====');
  }

  private _handleTileClick = (tile: ITileData, tileNumber: number): void => {
    try {
      if (this.props.enableAnalytics) {
        console.log(`[Tile Analytics] Tile ${tileNumber} clicked:`, {
          title: tile?.title,
          linkUrl: tile?.linkUrl,
          timestamp: new Date().toISOString()
        });
      }
    } catch (error) {
      console.error('[Tiles] Error in click handler:', error);
    }
  };

  private _renderTile = (tile: ITileData, index: number): JSX.Element => {
    try {
      console.log(`[Tiles] _renderTile called for index ${index}`, tile);

      if (!tile) {
        console.warn(`[Tiles] Tile ${index} is null/undefined`);
        return <div className={styles.tile}><p>Tile {index} data not available</p></div>;
      }

      const isEditMode = this.props.displayMode === 1; // DisplayMode.Edit = 1
      const isEmpty = !tile.title && !tile.imageUrl && !tile.description && !tile.linkUrl;
      
      console.log(`[Tiles] Tile ${index} - isEditMode: ${isEditMode}, isEmpty: ${isEmpty}`);
      
      // Show placeholder in edit mode if tile is empty
      if (isEmpty && isEditMode) {
        console.log(`[Tiles] Rendering placeholder for tile ${index}`);
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

      const ariaLabel = `${tile.title || ''}. ${tile.description || ''}. ${tile.linkUrl ? 'Click to navigate.' : ''}`;

      console.log(`[Tiles] Rendering tile ${index} with linkUrl: ${tile.linkUrl}`);

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
      console.log(`[Tiles] Rendering tile ${index} as static card`);
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
    } catch (error) {
      console.error(`[Tiles] ===== ERROR RENDERING TILE ${index} =====`);
      console.error('[Tiles] Error:', error);
      console.error('[Tiles] Error message:', (error as Error).message);
      console.error('[Tiles] Error stack:', (error as Error).stack);
      console.error('[Tiles] Tile data:', tile);
      console.error('[Tiles] ===== END ERROR =====');
      return <div className={styles.tile}><p>Error rendering tile {index}</p></div>;
    }
  };

  private _renderTileContent = (tile: ITileData): JSX.Element => {
    try {
      console.log('[Tiles] _renderTileContent called for tile:', tile);
      
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
    } catch (error) {
      console.error('[Tiles] Error in _renderTileContent:', error);
      return <><p>Error rendering content</p></>;
    }
  };

  public render(): React.ReactElement<ITilesProps> {
    try {
      console.log('[Tiles] render() method called');
      console.log('[Tiles] this.props:', this.props);
      
      const { tile1, tile2, tile3, hasTeamsContext, displayMode } = this.props;

      console.log('[Tiles] Destructured props - tile1:', tile1);
      console.log('[Tiles] Destructured props - tile2:', tile2);
      console.log('[Tiles] Destructured props - tile3:', tile3);
      console.log('[Tiles] Destructured props - displayMode:', displayMode);

      // Ensure tiles exist with defaults
      const safeTile1 = tile1 || { title: '', imageUrl: '', altText: '', description: '', linkUrl: '', openInNewTab: false };
      const safeTile2 = tile2 || { title: '', imageUrl: '', altText: '', description: '', linkUrl: '', openInNewTab: false };
      const safeTile3 = tile3 || { title: '', imageUrl: '', altText: '', description: '', linkUrl: '', openInNewTab: false };

      console.log('[Tiles] Safe tiles created');
      console.log('[Tiles] About to render JSX');

      const jsx = (
        <section 
          className={`${styles.tiles} ${hasTeamsContext ? styles.teams : ''}`}
          aria-label="Three tiles section"
        >
          <div className={styles.tilesContainer}>
            {this._renderTile(safeTile1, 0)}
            {this._renderTile(safeTile2, 1)}
            {this._renderTile(safeTile3, 2)}
          </div>
        </section>
      );

      console.log('[Tiles] JSX created successfully');
      return jsx;
    } catch (error) {
      console.error('[Tiles] ===== CRITICAL RENDER ERROR =====');
      console.error('[Tiles] Error:', error);
      console.error('[Tiles] Error message:', (error as Error).message);
      console.error('[Tiles] Error stack:', (error as Error).stack);
      console.error('[Tiles] ===== END ERROR =====');
      
      return (
        <div style={{ padding: '20px', color: 'red' }}>
          <h3>Error loading tiles</h3>
          <p>Error: {(error as Error).message}</p>
          <p>Check the browser console for full details.</p>
        </div>
      );
    }
  }
}
