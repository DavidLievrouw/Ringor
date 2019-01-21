class UrlService {
  constructor(urlInfo) {
    this.appUrl = UrlService.trimSlashes(urlInfo.appUrl || "");
    this.siteUrl = UrlService.trimSlashes(urlInfo.siteUrl || "");
  }

  getAbsoluteUrl(relativeUrl) {
    if(relativeUrl.indexOf("http") === 0) return relativeUrl;
    return [this.siteUrl, this.appUrl, UrlService.trimSlashes(relativeUrl)]
      .filter(_ => _)
      .join('/');
  }

  static trimSlashes(string) {
    return string.replace(/^\/+|\/+$/g, '');
  }
}

export default UrlService;