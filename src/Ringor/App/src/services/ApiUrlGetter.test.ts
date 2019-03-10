import 'isomorphic-fetch';
import ApiUrlGetter, { IApiUrlGetter } from './ApiUrlGetter';
import { IApiClient } from './ApiClient';
import { IDictionary } from '../facades/IDictionary';

let apiUrlGetter: IApiUrlGetter;
let apiClient: IApiClient;

describe('ApiUrlGetter', () => {
  describe('load', () => {
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
      return expect(apiUrlGetter.load(null)).rejects
        .toThrow("No URL is specified");
    });

    test("should not allow an empty URL", () => {
      return expect(apiUrlGetter.load("")).rejects
        .toThrow("No URL is specified");
    });

    test("should not allow an whitespace URL", () => {
      return expect(apiUrlGetter.load(" ")).rejects
        .toThrow("No URL is specified");
    });

    test("should query the correct URL", async () => {
      await apiUrlGetter.load(url);
      const expectedUrl = url;
      expect(apiClient.get)
        .toHaveBeenCalledWith(expectedUrl);
    });

    test("should return the JSON data from the response", async () => {
      const actual = await apiUrlGetter.load(url);
      expect(actual).toMatchObject({ trace: { id: 42 } });
    });

    test("should throw when the request failed", async () => {
      createSut(true);
      return expect(apiUrlGetter.load(url)).rejects
        .toThrow("404");
    });
  });
});