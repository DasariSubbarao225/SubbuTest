export interface ITileData {
  title: string;
  imageUrl: string;
  altText: string;
  description: string;
  linkUrl: string;
  openInNewTab: boolean;
  backgroundColor?: string;
}

export interface ITilesProps {
  tile1: ITileData;
  tile2: ITileData;
  tile3: ITileData;
  isDarkTheme: boolean;
  hasTeamsContext: boolean;
  semanticColors: any;
  enableAnalytics: boolean;
  displayMode: number;
}
