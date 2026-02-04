declare interface IVse3TilesWebPartStrings {
  PropertyPaneDescription: string;
  
  Tile1GroupName: string;
  Tile2GroupName: string;
  Tile3GroupName: string;
  SettingsGroupName: string;
  
  TitleFieldLabel: string;
  TitlePlaceholder: string;
  DescriptionFieldLabel: string;
  DescriptionPlaceholder: string;
  ImageUrlFieldLabel: string;
  ImageUrlPlaceholder: string;
  ImageAltFieldLabel: string;
  ImageAltPlaceholder: string;
  LinkUrlFieldLabel: string;
  LinkUrlPlaceholder: string;
  OpenInNewTabFieldLabel: string;
  OpenInNewTabOnText: string;
  OpenInNewTabOffText: string;
  BackgroundColorFieldLabel: string;
  BackgroundColorPlaceholder: string;
  EnableAnalyticsFieldLabel: string;
  EnableAnalyticsOnText: string;
  EnableAnalyticsOffText: string;
  
  InvalidUrlMessage: string;
}

declare module 'Vse3TilesWebPartStrings' {
  const strings: IVse3TilesWebPartStrings;
  export = strings;
}
