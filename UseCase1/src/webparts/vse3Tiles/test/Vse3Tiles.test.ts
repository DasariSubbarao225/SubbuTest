import * as React from 'react';
import * as ReactDom from 'react-dom';
import { assert } from 'chai';

import Vse3Tiles from '../components/Vse3Tiles';
import { IVse3TilesProps, ITileConfig } from '../components/IVse3TilesProps';

describe('Vse3Tiles Component', () => {
  let container: HTMLDivElement;

  beforeEach(() => {
    container = document.createElement('div');
    document.body.appendChild(container);
  });

  afterEach(() => {
    ReactDom.unmountComponentAtNode(container);
    document.body.removeChild(container);
  });

  it('should render three tiles', () => {
    const tiles: ITileConfig[] = [
      {
        title: 'Tile 1',
        description: 'Description 1',
        imageUrl: 'https://example.com/image1.jpg',
        imageAlt: 'Alt 1',
        linkUrl: 'https://example.com/link1',
        openInNewTab: false,
        backgroundColor: ''
      },
      {
        title: 'Tile 2',
        description: 'Description 2',
        imageUrl: 'https://example.com/image2.jpg',
        imageAlt: 'Alt 2',
        linkUrl: 'https://example.com/link2',
        openInNewTab: true,
        backgroundColor: ''
      },
      {
        title: 'Tile 3',
        description: 'Description 3',
        imageUrl: 'https://example.com/image3.jpg',
        imageAlt: 'Alt 3',
        linkUrl: 'https://example.com/link3',
        openInNewTab: false,
        backgroundColor: ''
      }
    ];

    const props: IVse3TilesProps = {
      tiles: tiles,
      enableAnalytics: false,
      themeVariant: null
    };

    const element: React.ReactElement<IVse3TilesProps> = React.createElement(
      Vse3Tiles,
      props
    );

    ReactDom.render(element, container);

    const tileElements = container.querySelectorAll('a.tile');
    assert.equal(tileElements.length, 3, 'Should render three tiles');
  });

  it('should show placeholder for empty tiles', () => {
    const tiles: ITileConfig[] = [
      {
        title: '',
        description: '',
        imageUrl: '',
        imageAlt: '',
        linkUrl: '',
        openInNewTab: false,
        backgroundColor: ''
      },
      {
        title: '',
        description: '',
        imageUrl: '',
        imageAlt: '',
        linkUrl: '',
        openInNewTab: false,
        backgroundColor: ''
      },
      {
        title: '',
        description: '',
        imageUrl: '',
        imageAlt: '',
        linkUrl: '',
        openInNewTab: false,
        backgroundColor: ''
      }
    ];

    const props: IVse3TilesProps = {
      tiles: tiles,
      enableAnalytics: false,
      themeVariant: null
    };

    const element: React.ReactElement<IVse3TilesProps> = React.createElement(
      Vse3Tiles,
      props
    );

    ReactDom.render(element, container);

    const placeholders = container.querySelectorAll('.tilePlaceholder');
    assert.equal(placeholders.length, 3, 'Should render three placeholders');
  });

  it('should render tiles with correct accessibility attributes', () => {
    const tiles: ITileConfig[] = [
      {
        title: 'Accessible Tile',
        description: 'This is an accessible tile',
        imageUrl: 'https://example.com/image.jpg',
        imageAlt: 'Test image',
        linkUrl: 'https://example.com',
        openInNewTab: false,
        backgroundColor: ''
      },
      {
        title: '',
        description: '',
        imageUrl: '',
        imageAlt: '',
        linkUrl: '',
        openInNewTab: false,
        backgroundColor: ''
      },
      {
        title: '',
        description: '',
        imageUrl: '',
        imageAlt: '',
        linkUrl: '',
        openInNewTab: false,
        backgroundColor: ''
      }
    ];

    const props: IVse3TilesProps = {
      tiles: tiles,
      enableAnalytics: false,
      themeVariant: null
    };

    const element: React.ReactElement<IVse3TilesProps> = React.createElement(
      Vse3Tiles,
      props
    );

    ReactDom.render(element, container);

    const link = container.querySelector('a.tile');
    assert.isNotNull(link, 'Link should be rendered');
    assert.equal(link.getAttribute('role'), 'link', 'Should have role="link"');
    assert.equal(link.getAttribute('tabindex'), '0', 'Should have tabindex="0"');
    
    const ariaLabel = link.getAttribute('aria-label');
    assert.include(ariaLabel, 'Accessible Tile', 'ARIA label should include title');
    assert.include(ariaLabel, 'This is an accessible tile', 'ARIA label should include description');
  });

  it('should open links in new tab when configured', () => {
    const tiles: ITileConfig[] = [
      {
        title: 'New Tab Tile',
        description: 'Opens in new tab',
        imageUrl: '',
        imageAlt: '',
        linkUrl: 'https://example.com',
        openInNewTab: true,
        backgroundColor: ''
      },
      {
        title: '',
        description: '',
        imageUrl: '',
        imageAlt: '',
        linkUrl: '',
        openInNewTab: false,
        backgroundColor: ''
      },
      {
        title: '',
        description: '',
        imageUrl: '',
        imageAlt: '',
        linkUrl: '',
        openInNewTab: false,
        backgroundColor: ''
      }
    ];

    const props: IVse3TilesProps = {
      tiles: tiles,
      enableAnalytics: false,
      themeVariant: null
    };

    const element: React.ReactElement<IVse3TilesProps> = React.createElement(
      Vse3Tiles,
      props
    );

    ReactDom.render(element, container);

    const link = container.querySelector('a.tile') as HTMLAnchorElement;
    assert.equal(link.getAttribute('target'), '_blank', 'Should have target="_blank"');
    assert.equal(link.getAttribute('rel'), 'noopener noreferrer', 'Should have rel="noopener noreferrer"');
  });

  it('should render images with lazy loading', () => {
    const tiles: ITileConfig[] = [
      {
        title: 'Image Tile',
        description: 'Has an image',
        imageUrl: 'https://example.com/image.jpg',
        imageAlt: 'Test image alt',
        linkUrl: 'https://example.com',
        openInNewTab: false,
        backgroundColor: ''
      },
      {
        title: '',
        description: '',
        imageUrl: '',
        imageAlt: '',
        linkUrl: '',
        openInNewTab: false,
        backgroundColor: ''
      },
      {
        title: '',
        description: '',
        imageUrl: '',
        imageAlt: '',
        linkUrl: '',
        openInNewTab: false,
        backgroundColor: ''
      }
    ];

    const props: IVse3TilesProps = {
      tiles: tiles,
      enableAnalytics: false,
      themeVariant: null
    };

    const element: React.ReactElement<IVse3TilesProps> = React.createElement(
      Vse3Tiles,
      props
    );

    ReactDom.render(element, container);

    const img = container.querySelector('img') as HTMLImageElement;
    assert.isNotNull(img, 'Image should be rendered');
    assert.equal(img.getAttribute('loading'), 'lazy', 'Should have loading="lazy"');
    assert.equal(img.getAttribute('alt'), 'Test image alt', 'Should have correct alt text');
  });

  it('should apply custom background colors', () => {
    const customColor = '#ff0000';
    const tiles: ITileConfig[] = [
      {
        title: 'Colored Tile',
        description: 'Has custom background',
        imageUrl: '',
        imageAlt: '',
        linkUrl: 'https://example.com',
        openInNewTab: false,
        backgroundColor: customColor
      },
      {
        title: '',
        description: '',
        imageUrl: '',
        imageAlt: '',
        linkUrl: '',
        openInNewTab: false,
        backgroundColor: ''
      },
      {
        title: '',
        description: '',
        imageUrl: '',
        imageAlt: '',
        linkUrl: '',
        openInNewTab: false,
        backgroundColor: ''
      }
    ];

    const props: IVse3TilesProps = {
      tiles: tiles,
      enableAnalytics: false,
      themeVariant: null
    };

    const element: React.ReactElement<IVse3TilesProps> = React.createElement(
      Vse3Tiles,
      props
    );

    ReactDom.render(element, container);

    const tile = container.querySelector('a.tile') as HTMLElement;
    assert.equal(tile.style.backgroundColor, customColor, 'Should have custom background color');
  });
});