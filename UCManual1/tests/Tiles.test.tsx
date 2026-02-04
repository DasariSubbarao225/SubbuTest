import * as React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import Tiles from '../components/Tiles';
import { ITilesProps, ITileData } from '../components/ITilesProps';
import { DisplayMode } from '@microsoft/sp-core-library';

describe('Tiles Component', () => {
  const mockTile: ITileData = {
    title: 'Test Tile',
    imageUrl: 'https://example.com/image.jpg',
    altText: 'Test image',
    description: 'Test description',
    linkUrl: 'https://example.com',
    openInNewTab: false,
    backgroundColor: ''
  };

  const defaultProps: ITilesProps = {
    tile1: mockTile,
    tile2: { ...mockTile, title: 'Tile 2' },
    tile3: { ...mockTile, title: 'Tile 3' },
    isDarkTheme: false,
    hasTeamsContext: false,
    semanticColors: {},
    enableAnalytics: false,
    displayMode: DisplayMode.Read
  };

  beforeEach(() => {
    // Clear console logs before each test
    jest.spyOn(console, 'log').mockImplementation();
  });

  afterEach(() => {
    jest.restoreAllMocks();
  });

  // AC2 - Display
  test('renders three tiles', () => {
    render(<Tiles {...defaultProps} />);
    expect(screen.getByText('Test Tile')).toBeInTheDocument();
    expect(screen.getByText('Tile 2')).toBeInTheDocument();
    expect(screen.getByText('Tile 3')).toBeInTheDocument();
  });

  test('renders tile content correctly', () => {
    render(<Tiles {...defaultProps} />);
    expect(screen.getByText('Test Tile')).toBeInTheDocument();
    expect(screen.getByText('Test description')).toBeInTheDocument();
    expect(screen.getByAltText('Test image')).toBeInTheDocument();
  });

  // AC3 - Click behavior
  test('renders tiles as links when linkUrl is provided', () => {
    render(<Tiles {...defaultProps} />);
    const links = screen.getAllByRole('article');
    expect(links[0].tagName).toBe('A');
  });

  test('opens in same tab when openInNewTab is false', () => {
    render(<Tiles {...defaultProps} />);
    const link = screen.getAllByRole('article')[0] as HTMLAnchorElement;
    expect(link.target).toBe('_self');
    expect(link.rel).toBe('');
  });

  test('opens in new tab with security attributes when openInNewTab is true', () => {
    const props = {
      ...defaultProps,
      tile1: { ...mockTile, openInNewTab: true }
    };
    render(<Tiles {...props} />);
    const link = screen.getAllByRole('article')[0] as HTMLAnchorElement;
    expect(link.target).toBe('_blank');
    expect(link.rel).toBe('noopener noreferrer');
  });

  // AC4 - Accessibility
  test('has proper ARIA labels', () => {
    render(<Tiles {...defaultProps} />);
    const tiles = screen.getAllByRole('article');
    expect(tiles[0]).toHaveAttribute('aria-label', 'Test Tile. Test description. Click to navigate.');
  });

  test('images have alt text', () => {
    render(<Tiles {...defaultProps} />);
    const images = screen.getAllByRole('img');
    expect(images[0]).toHaveAttribute('alt', 'Test image');
  });

  test('images use title as fallback alt text when altText is empty', () => {
    const props = {
      ...defaultProps,
      tile1: { ...mockTile, altText: '' }
    };
    render(<Tiles {...props} />);
    const image = screen.getAllByRole('img')[0];
    expect(image).toHaveAttribute('alt', 'Test Tile');
  });

  test('tiles are keyboard focusable', () => {
    render(<Tiles {...defaultProps} />);
    const tiles = screen.getAllByRole('article');
    expect(tiles[0]).toHaveAttribute('tabIndex', '0');
  });

  // AC5 - Authoring UX
  test('shows placeholder in edit mode when tile is empty', () => {
    const emptyTile: ITileData = {
      title: '',
      imageUrl: '',
      altText: '',
      description: '',
      linkUrl: '',
      openInNewTab: false,
      backgroundColor: ''
    };
    const props = {
      ...defaultProps,
      tile1: emptyTile,
      displayMode: DisplayMode.Edit
    };
    render(<Tiles {...props} />);
    expect(screen.getByText('Configure Tile 1')).toBeInTheDocument();
    expect(screen.getByText(/Click the edit button to add title, image/)).toBeInTheDocument();
  });

  test('does not show placeholder in read mode when tile is empty', () => {
    const emptyTile: ITileData = {
      title: '',
      imageUrl: '',
      altText: '',
      description: '',
      linkUrl: '',
      openInNewTab: false,
      backgroundColor: ''
    };
    const props = {
      ...defaultProps,
      tile1: emptyTile,
      displayMode: DisplayMode.Read
    };
    render(<Tiles {...props} />);
    expect(screen.queryByText('Configure Tile 1')).not.toBeInTheDocument();
  });

  // AC7 - Performance
  test('images use lazy loading', () => {
    render(<Tiles {...defaultProps} />);
    const images = screen.getAllByRole('img');
    expect(images[0]).toHaveAttribute('loading', 'lazy');
  });

  // AC8 - Theming
  test('applies custom background color when provided', () => {
    const props = {
      ...defaultProps,
      tile1: { ...mockTile, backgroundColor: '#ff0000' }
    };
    render(<Tiles {...props} />);
    const tile = screen.getAllByRole('article')[0];
    expect(tile).toHaveStyle({ backgroundColor: '#ff0000' });
  });

  // AC9 - Analytics
  test('logs analytics when enabled and tile is clicked', () => {
    const consoleSpy = jest.spyOn(console, 'log');
    const props = {
      ...defaultProps,
      enableAnalytics: true
    };
    render(<Tiles {...props} />);
    const tile = screen.getAllByRole('article')[0];
    fireEvent.click(tile);
    
    expect(consoleSpy).toHaveBeenCalledWith(
      '[Tile Analytics] Tile 1 clicked:',
      expect.objectContaining({
        title: 'Test Tile',
        linkUrl: 'https://example.com',
        timestamp: expect.any(String)
      })
    );
  });

  test('does not log analytics when disabled', () => {
    const consoleSpy = jest.spyOn(console, 'log');
    const props = {
      ...defaultProps,
      enableAnalytics: false
    };
    render(<Tiles {...props} />);
    const tile = screen.getAllByRole('article')[0];
    fireEvent.click(tile);
    
    expect(consoleSpy).not.toHaveBeenCalled();
  });

  // Edge cases
  test('renders static div when no linkUrl is provided', () => {
    const props = {
      ...defaultProps,
      tile1: { ...mockTile, linkUrl: '' }
    };
    render(<Tiles {...props} />);
    const tiles = screen.getAllByRole('article');
    expect(tiles[0].tagName).toBe('DIV');
  });

  test('handles missing image gracefully', () => {
    const props = {
      ...defaultProps,
      tile1: { ...mockTile, imageUrl: '' }
    };
    render(<Tiles {...props} />);
    expect(screen.queryByRole('img')).not.toBeInTheDocument();
    expect(screen.getByText('Test Tile')).toBeInTheDocument();
  });

  test('handles missing title and description', () => {
    const props = {
      ...defaultProps,
      tile1: { ...mockTile, title: '', description: '' }
    };
    render(<Tiles {...props} />);
    const tile = screen.getAllByRole('article')[0];
    expect(tile).toBeInTheDocument();
    expect(screen.queryByText('Test Tile')).not.toBeInTheDocument();
  });
});
