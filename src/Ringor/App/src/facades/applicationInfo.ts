export interface IWindow extends Window {
  applicationInfo: IApplicationInfo;
}
declare var window: IWindow;

export interface IUrlInfo {
  siteUrl: string;
  appUrl: string;
}

export interface IApplicationInfo {
  company: string;
  product: string;
  version: string;
  urlInfo: IUrlInfo;
}

const applicationInfo = (typeof window !== "undefined" && window.applicationInfo || {}) as IApplicationInfo;

export default applicationInfo;