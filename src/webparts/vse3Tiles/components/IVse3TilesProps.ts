export interface ITileConfig {
  title: string;
  description: string;
  imageUrl: string;
  imageAlt: string;
  linkUrl: string;
  openInNewTab: boolean;
  backgroundColor: string;
}

export interface IVse3TilesProps {
  tiles: ITileConfig[];
  enableAnalytics: boolean;
  themeVariant: any;
}
