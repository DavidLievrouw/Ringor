import 'isomorphic-fetch';
import ApiUrlGetter, { IApiUrlGetter } from './ApiUrlGetter';
import { IApiClient } from './ApiClient';
import { IDictionary } from '../facades/IDictionary';

let apiUrlGetter: IApiUrlGetter;
let apiClient: IApiClient;

describe('ApiUrlGetter', () => {
  describe('get', () => {
    let url: string;

    beforeEach(() => {
      createSut(false);
      url = "/api/user/1";
    });

    function createSut(shouldFail: boolean = false) {
      const ApiClientMock = jest.fn<IApiClient, any[]>(() => ({
        get: jest.fn().mockImplementation((url: string, queryParams?: IDictionary<string>, headers?: IDictionary<string>, mode?: RequestMode): Promise<Response> => {
          return shouldFail
            ? Promise.resolve({ ok: false, status: 404 } as Response)
            : Promise.resolve({ ok: true, json: () => Promise.resolve({ trace: { id: 42 } }) } as Response);
        }),
        post: jest.fn().mockImplementation((url: string, queryParams?: IDictionary<string>, data?: any, headers?: IDictionary<string>, mode?: RequestMode): Promise<Response> => {
          return Promise.resolve(new Response());
        }),
        put: jest.fn().mockImplementation((url: string, queryParams?: IDictionary<string>, data?: any, headers?: IDictionary<string>, mode?: RequestMode): Promise<Response> => {
          return Promise.resolve(new Response());
        }),
        delete: jest.fn().mockImplementation((url: string, queryParams?: IDictionary<string>, data?: any, headers?: IDictionary<string>, mode?: RequestMode): Promise<Response> => {
          return Promise.resolve(new Response());
        }),
        patch: jest.fn().mockImplementation((url: string, queryParams?: IDictionary<string>, data?: any, headers?: IDictionary<string>, mode?: RequestMode): Promise<Response> => {
          return Promise.resolve(new Response());
        })
      }));
      apiClient = new ApiClientMock();
      apiUrlGetter = new ApiUrlGetter(apiClient);
    }

    test("should not allow a null URL", () => {
      return expect(apiUrlGetter.get(null)).rejects
        .toThrow("No Api URL is specified");
    });

    test("should not allow an absolute HTTP URL", () => {
      return expect(apiUrlGetter.get('http://www.dalion.eu/api')).rejects
        .toThrow("Absolute URL");
    });

    test("should not allow an absolute HTTPS URL", () => {
      return expect(apiUrlGetter.get('https://www.dalion.eu/api')).rejects
        .toThrow("Absolute URL");
    });

    test("should not allow an absolute incorrectly cased HTTP URL", () => {
      return expect(apiUrlGetter.get('Http://www.dalion.eu/api')).rejects
        .toThrow("Absolute URL");
    });

    test("should not allow an absolute incorrectly cased HTTPS URL", () => {
      return expect(apiUrlGetter.get('Https://www.dalion.eu/api')).rejects
        .toThrow("Absolute URL");
    });

    test("should not allow URLs that don't start with /api", () => {
      return expect(apiUrlGetter.get('/swagger')).rejects
        .toThrow("/api");
    });

    test("should allow URLs that start with an incorrectly cased /api", async () => {
      const incorrectlyCasedUrl = '/ApI/userInfo';
      await apiUrlGetter.get(incorrectlyCasedUrl);
      const expectedUrl = '/api/userInfo';
      expect(apiClient.get)
        .toHaveBeenCalledWith(expectedUrl);
    });

    test("should not allow an empty URL", () => {
      return expect(apiUrlGetter.get("")).rejects
        .toThrow("No Api URL is specified");
    });

    test("should not allow an whitespace URL", () => {
      return expect(apiUrlGetter.get(" ")).rejects
        .toThrow("No Api URL is specified");
    });

    test("should query the correct URL", async () => {
      await apiUrlGetter.get(url);
      const expectedUrl = url;
      expect(apiClient.get)
        .toHaveBeenCalledWith(expectedUrl);
    });

    test("should sanitize the URL", async () => {
      const unsanitizedUrl = ' /Api/test?debug=true  ';
      await apiUrlGetter.get(unsanitizedUrl);
      const expectedUrl = '/api/test?debug=true';
      expect(apiClient.get)
        .toHaveBeenCalledWith(expectedUrl);
    });

    test("should return the JSON data from the response", async () => {
      const actual = await apiUrlGetter.get(url);
      expect(actual).toMatchObject({ trace: { id: 42 } });
    });

    test("should throw when the request failed", async () => {
      createSut(true);
      return expect(apiUrlGetter.get(url)).rejects
        .toThrow("404");
    });
  });
});