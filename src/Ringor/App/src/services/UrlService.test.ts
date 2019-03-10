import UrlService, { IUrlService } from './UrlService';
import { IUrlInfo } from '../facades/applicationInfo';

let urlService: IUrlService;
let urlInfo: IUrlInfo;

describe('UrlService', () => {
  describe('getAbsoluteUrl', () => {
    beforeEach(() => {
      urlInfo = {siteUrl: "http://dalion.eu", appUrl: "/RingorApi"};
      urlService = new UrlService(urlInfo);
    });

    test("should build the correct absolute URL for a simple resource", () => {
      const url = urlService.getAbsoluteUrl("test");
      expect(url).toBe("http://dalion.eu/RingorApi/test");
    });

    test("should build the correct absolute URL with a deeply nested resource", () => {
      const url = urlService.getAbsoluteUrl("test/1/2/3/four");
      expect(url).toBe("http://dalion.eu/RingorApi/test/1/2/3/four");
    });

    test("should build the correct absolute URL with a deeply nested resource and query parameters", () => {
      const url = urlService.getAbsoluteUrl("test/1/2/3/four?what=goingon");
      expect(url).toBe("http://dalion.eu/RingorApi/test/1/2/3/four?what=goingon");
    });

    test("should not generate URLs with double forward slashes between the site url and the app url", () => {
      urlInfo.siteUrl = "http://dalion.eu/";
      urlInfo.appUrl = "/RingorApi";
      const url = urlService.getAbsoluteUrl("test");
      expect(url).toBe("http://dalion.eu/RingorApi/test");
    });

    test("should not generate URLs with double forward slashes between the site url and the base url", () => {
      urlInfo.appUrl = "/RingorApi/";
      const url = urlService.getAbsoluteUrl("/test");
      expect(url).toBe("http://dalion.eu/RingorApi/test");
    });

    test("should not mess with URLs that are already absolute", () => {
      var url = urlService.getAbsoluteUrl("http://dalion.eu/RingorApi/test");
      expect(url).toBe("http://dalion.eu/RingorApi/test");
    });

    test("should be able to deal with no app URL", () => {
      urlInfo = {siteUrl: "http://dalion.eu", appUrl: undefined};
      urlService = new UrlService(urlInfo);
      var url = urlService.getAbsoluteUrl("test");
      expect(url).toBe("http://dalion.eu/test");
    });
  });

  describe('getApplicationUrl', () => {
    beforeEach(() => {
      urlInfo = {siteUrl: "http://dalion.eu", appUrl: "/RingorApi"};
      urlService = new UrlService(urlInfo);
    });

    test("should build the correct absolute URL", () => {
      const url = urlService.getApplicationUrl();
      expect(url).toBe("http://dalion.eu/RingorApi");
    });

    test("should build the correct absolute URL when there is no application path", () => {
      urlInfo.appUrl = '';
      urlService = new UrlService(urlInfo);
      const url = urlService.getApplicationUrl();
      expect(url).toBe("http://dalion.eu");
    });
  });
});