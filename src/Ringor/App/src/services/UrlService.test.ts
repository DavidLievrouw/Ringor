import UrlService from './UrlService';

let urlService;
let urlInfo;

describe('UrlService', () => {
  describe('getAbsoluteUrl', () => {
    beforeEach(function () {
      urlInfo = {siteUrl: "http://dalion.eu", appUrl: "/RingorApi", path: "/RingorApi/files"};
      urlService = new UrlService(urlInfo);
    });

    test("should build the correct absolute URL for a simple resource", function () {
      const url = urlService.getAbsoluteUrl("test");
      expect(url).toBe("http://dalion.eu/RingorApi/test");
    });

    test("should build the correct absolute URL with a deeply nested resource", function () {
      const url = urlService.getAbsoluteUrl("test/1/2/3/four");
      expect(url).toBe("http://dalion.eu/RingorApi/test/1/2/3/four");
    });

    test("should build the correct absolute URL with a deeply nested resource and query parameters", function () {
      const url = urlService.getAbsoluteUrl("test/1/2/3/four?what=goingon");
      expect(url).toBe("http://dalion.eu/RingorApi/test/1/2/3/four?what=goingon");
    });

    test("should not generate URLs with double forward slashes between the site url and the base url", function () {
      urlInfo.siteUrl = "http://dalion.eu/";
      urlInfo.appUrl = "/RingorApi";
      const url = urlService.getAbsoluteUrl("test");
      expect(url).toBe("http://dalion.eu/RingorApi/test");
    });

    test("should not generate URLs with double forward slashes between the base url and the relative url", function () {
      urlInfo.appUrl = "/RingorApi/";
      const url = urlService.getAbsoluteUrl("/test");
      expect(url).toBe("http://dalion.eu/RingorApi/test");
    });

    test("should not mess with URLs that are already absolute", function () {
      var url = urlService.getAbsoluteUrl("http://dalion.eu/RingorApi/test");
      expect(url).toBe("http://dalion.eu/RingorApi/test");
    });

    test("should be able to deal with no base URL", function() {
      urlInfo = {siteUrl: "http://dalion.eu"};
      urlService = new UrlService(urlInfo);
      var url = urlService.getAbsoluteUrl("test");
      expect(url).toBe("http://dalion.eu/test");
    });
  });
});