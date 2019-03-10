import { IUrlInfo } from "../facades/applicationInfo";

export interface IUrlService {
  getAbsoluteUrl(relativeUrl: string): string;
}

class UrlService implements IUrlService {
  private appUrl: string;
  private siteUrl: string;

  constructor(urlInfo: IUrlInfo) {
    this.appUrl = UrlService.trimSlashes(urlInfo.appUrl || "");
    this.siteUrl = UrlService.trimSlashes(urlInfo.siteUrl || "");
  }

  getAbsoluteUrl(relativeUrl: string): string {
    if (relativeUrl.indexOf("http") === 0) return relativeUrl;
    return [this.siteUrl, this.appUrl, UrlService.trimSlashes(relativeUrl)]
      .filter(_ => _)
      .join('/');
  }

  static trimSlashes(string: string): string {
    return string.replace(/^\/+|\/+$/g, '');
  }
}

export default UrlService;