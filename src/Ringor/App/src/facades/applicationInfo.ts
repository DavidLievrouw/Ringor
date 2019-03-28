export interface IWindow extends Window {
  applicationInfo: IApplicationInfo;
}
declare var window: IWindow;

export interface IUrlInfo {
  siteUrl: string;
  appUrl: string;
}

export interface IMsalConfig {
  authority: string;
  tenant: string;
  clientId: string;
  scopes: string[];
}

export interface IApplicationInfo {
  company: string;
  product: string;
  email: string;
  version: string;
  urlInfo: IUrlInfo;
  environment: string;
  msalConfig: IMsalConfig;
}

const applicationInfo = (typeof window !== "undefined" && window.applicationInfo || {}) as IApplicationInfo;

export default applicationInfo;