import ApiClient, { IApiClient } from './ApiClient';
import { IRequestSender } from '../facades/RequestSender';
import { IDictionary } from '../facades/IDictionary';

let apiClient: IApiClient;
let requestSender: IRequestSender;

describe('ApiClient', () => {
  describe('get', () => {
    let url: string;
    let queryParams: IDictionary<string>;
    let headers: IDictionary<string>;
    let interceptedUrl: string;
    let interceptedRequest: Request;

    beforeEach(() => {
      const RequestSenderMock = jest.fn<IRequestSender>(() => ({
        send: jest.fn().mockImplementation((url, request) => {
          interceptedUrl = url;
          interceptedRequest = request;
        })
      }));
      requestSender = new RequestSenderMock();
      apiClient = new ApiClient(requestSender);
      url = 'https://dalion.eu/ringortests/item';
      queryParams = {};
      headers = {};
    });

    test("should get the correct url", async () => {
      await apiClient.get(url);
      expect(requestSender.send)
        .toHaveBeenCalledWith(url, expect.objectContaining({ method: 'GET' }));
    });

    test("should send request with default headers and mode, if none is specified", async () => {
      await apiClient.get(url);
      expect(requestSender.send)
        .toHaveBeenCalledWith(url, expect.objectContaining({ mode: 'same-origin', headers: undefined }));
    });

    test("should send specified mode, if overridden", async () => {
      await apiClient.get(url, undefined, undefined, 'no-cors');
      expect(requestSender.send)
        .toHaveBeenCalledWith(url, expect.objectContaining({ mode: 'no-cors' }));
    });

    test("should send specified headers, if specified", async () => {
      headers['h1'] = 'v1';
      headers['h2'] = 'v2';
      await apiClient.get(url, undefined, headers);
      expect(interceptedRequest.headers).toEqual({ "h1": "v1", "h2": "v2" });
    });

    test("should send specified data in the query string, if specified", async () => {
      queryParams['q1'] = 'v1';
      queryParams['q2'] = 'v2';
      await apiClient.get(url, queryParams);
      expect(interceptedUrl).toEqual('https://dalion.eu/ringortests/item?q1=v1&q2=v2');
    });

    test("should send uri encoded data in the query string, if specified", async () => {
      queryParams['q1'] = 'https://dalion.eu/';
      queryParams['https://dalion.eu/'] = 'v2';
      await apiClient.get(url, queryParams);
      expect(interceptedUrl).toEqual('https://dalion.eu/ringortests/item?q1=https%3A%2F%2Fdalion.eu%2F&https%3A%2F%2Fdalion.eu%2F=v2');
    });

    test("should send specified data in the query string, even if there is no value", async () => {
      queryParams['q1'] = undefined;
      queryParams['q2'] = undefined;
      await apiClient.get(url, queryParams);
      expect(interceptedUrl).toEqual('https://dalion.eu/ringortests/item?q1&q2');
    });
  });

  describe('post', () => {
    let url: string;    
    let queryParams: IDictionary<string>;
    let headers: IDictionary<string>;
    let interceptedUrl: string;
    let interceptedRequest: Request;

    beforeEach(() => {
      const RequestSenderMock = jest.fn<IRequestSender>(() => ({
        send: jest.fn().mockImplementation((url, request) => {
          interceptedUrl = url;
          interceptedRequest = request;
        })
      }));
      requestSender = new RequestSenderMock();
      apiClient = new ApiClient(requestSender);
      url = 'https://dalion.eu/ringortests/item';
      queryParams = {};
      headers = {};
    });

    test("should post the correct url", async () => {
      await apiClient.post(url);
      expect(requestSender.send)
        .toHaveBeenCalledWith(url, expect.objectContaining({ method: 'POST' }));
    });

    test("should send request with default headers and mode, if none is specified", async () => {
      await apiClient.post(url);
      expect(requestSender.send)
        .toHaveBeenCalledWith(url, expect.objectContaining({ mode: 'same-origin', headers: undefined }));
    });

    test("should send specified mode, if overridden", async () => {
      await apiClient.post(url, undefined, undefined, undefined, 'no-cors');
      expect(requestSender.send)
        .toHaveBeenCalledWith(url, expect.objectContaining({ mode: 'no-cors' }));
    });

    test("should send specified headers, if specified", async () => {
      headers['h1'] = 'v1';
      headers['h2'] = 'v2';
      await apiClient.post(url, undefined, undefined, headers);
      expect(interceptedRequest.headers).toEqual({ "h1": "v1", "h2": "v2" });
    });

    test("should send undefined body as default", async () => {
      await apiClient.post(url);
      expect(interceptedRequest.body).toBeUndefined();
    });

    test("should send camel-cased json representation of the body, if specified", async () => {
      const data: any = {
        'q1': 'v1',
        'q2': {'complex': 42, 'CasING': 'aBc'},
        'CasedImproperly': 'ABC'
      };
      await apiClient.post(url, undefined, data);
      expect(interceptedRequest.body).toEqual('{\"q1\":\"v1\",\"q2\":{\"complex\":42,\"casING\":\"aBc\"},\"casedImproperly\":\"ABC\"}');
    });
    
    test("should send specified data in the query string, if specified", async () => {
      queryParams['q1'] = 'v1';
      queryParams['q2'] = 'v2';
      await apiClient.post(url, queryParams);
      expect(interceptedUrl).toEqual('https://dalion.eu/ringortests/item?q1=v1&q2=v2');
    });

    test("should send uri encoded data in the query string, if specified", async () => {
      queryParams['q1'] = 'https://dalion.eu/';
      queryParams['https://dalion.eu/'] = 'v2';
      await apiClient.post(url, queryParams);
      expect(interceptedUrl).toEqual('https://dalion.eu/ringortests/item?q1=https%3A%2F%2Fdalion.eu%2F&https%3A%2F%2Fdalion.eu%2F=v2');
    });

    test("should send specified data in the query string, even if there is no value", async () => {
      queryParams['q1'] = undefined;
      queryParams['q2'] = undefined;
      await apiClient.post(url, queryParams);
      expect(interceptedUrl).toEqual('https://dalion.eu/ringortests/item?q1&q2');
    });
  });

  describe('put', () => {
    let url: string;    
    let queryParams: IDictionary<string>;
    let headers: IDictionary<string>;
    let interceptedUrl: string;
    let interceptedRequest: Request;

    beforeEach(() => {
      const RequestSenderMock = jest.fn<IRequestSender>(() => ({
        send: jest.fn().mockImplementation((url, request) => {
          interceptedUrl = url;
          interceptedRequest = request;
        })
      }));
      requestSender = new RequestSenderMock();
      apiClient = new ApiClient(requestSender);
      url = 'https://dalion.eu/ringortests/item';
      queryParams = {};
      headers = {};
    });

    test("should put the correct url", async () => {
      await apiClient.put(url);
      expect(requestSender.send)
        .toHaveBeenCalledWith(url, expect.objectContaining({ method: 'PUT' }));
    });

    test("should send request with default headers and mode, if none is specified", async () => {
      await apiClient.put(url);
      expect(requestSender.send)
        .toHaveBeenCalledWith(url, expect.objectContaining({ mode: 'same-origin', headers: undefined }));
    });

    test("should send specified mode, if overridden", async () => {
      await apiClient.put(url, undefined, undefined, undefined, 'no-cors');
      expect(requestSender.send)
        .toHaveBeenCalledWith(url, expect.objectContaining({ mode: 'no-cors' }));
    });

    test("should send specified headers, if specified", async () => {
      headers['h1'] = 'v1';
      headers['h2'] = 'v2';
      await apiClient.put(url, undefined, undefined, headers);
      expect(interceptedRequest.headers).toEqual({ "h1": "v1", "h2": "v2" });
    });

    test("should send undefined body as default", async () => {
      await apiClient.put(url);
      expect(interceptedRequest.body).toBeUndefined();
    });

    test("should send camel-cased json representation of the body, if specified", async () => {
      const data: any = {
        'q1': 'v1',
        'q2': {'complex': 42, 'CasING': 'aBc'},
        'CasedImproperly': 'ABC'
      };
      await apiClient.put(url, undefined, data);
      expect(interceptedRequest.body).toEqual('{\"q1\":\"v1\",\"q2\":{\"complex\":42,\"casING\":\"aBc\"},\"casedImproperly\":\"ABC\"}');
    });
    
    test("should send specified data in the query string, if specified", async () => {
      queryParams['q1'] = 'v1';
      queryParams['q2'] = 'v2';
      await apiClient.put(url, queryParams);
      expect(interceptedUrl).toEqual('https://dalion.eu/ringortests/item?q1=v1&q2=v2');
    });

    test("should send uri encoded data in the query string, if specified", async () => {
      queryParams['q1'] = 'https://dalion.eu/';
      queryParams['https://dalion.eu/'] = 'v2';
      await apiClient.put(url, queryParams);
      expect(interceptedUrl).toEqual('https://dalion.eu/ringortests/item?q1=https%3A%2F%2Fdalion.eu%2F&https%3A%2F%2Fdalion.eu%2F=v2');
    });

    test("should send specified data in the query string, even if there is no value", async () => {
      queryParams['q1'] = undefined;
      queryParams['q2'] = undefined;
      await apiClient.put(url, queryParams);
      expect(interceptedUrl).toEqual('https://dalion.eu/ringortests/item?q1&q2');
    });
  });

  describe('delete', () => {
    let url: string;    
    let queryParams: IDictionary<string>;
    let headers: IDictionary<string>;
    let interceptedUrl: string;
    let interceptedRequest: Request;

    beforeEach(() => {
      const RequestSenderMock = jest.fn<IRequestSender>(() => ({
        send: jest.fn().mockImplementation((url, request) => {
          interceptedUrl = url;
          interceptedRequest = request;
        })
      }));
      requestSender = new RequestSenderMock();
      apiClient = new ApiClient(requestSender);
      url = 'https://dalion.eu/ringortests/item';
      queryParams = {};
      headers = {};
    });

    test("should delete the correct url", async () => {
      await apiClient.delete(url);
      expect(requestSender.send)
        .toHaveBeenCalledWith(url, expect.objectContaining({ method: 'DELETE' }));
    });

    test("should send request with default headers and mode, if none is specified", async () => {
      await apiClient.delete(url);
      expect(requestSender.send)
        .toHaveBeenCalledWith(url, expect.objectContaining({ mode: 'same-origin', headers: undefined }));
    });

    test("should send specified mode, if overridden", async () => {
      await apiClient.delete(url, undefined, undefined, undefined, 'no-cors');
      expect(requestSender.send)
        .toHaveBeenCalledWith(url, expect.objectContaining({ mode: 'no-cors' }));
    });

    test("should send specified headers, if specified", async () => {
      headers['h1'] = 'v1';
      headers['h2'] = 'v2';
      await apiClient.delete(url, undefined, undefined, headers);
      expect(interceptedRequest.headers).toEqual({ "h1": "v1", "h2": "v2" });
    });

    test("should send undefined body as default", async () => {
      await apiClient.delete(url);
      expect(interceptedRequest.body).toBeUndefined();
    });

    test("should send camel-cased json representation of the body, if specified", async () => {
      const data: any = {
        'q1': 'v1',
        'q2': {'complex': 42, 'CasING': 'aBc'},
        'CasedImproperly': 'ABC'
      };
      await apiClient.delete(url, undefined, data);
      expect(interceptedRequest.body).toEqual('{\"q1\":\"v1\",\"q2\":{\"complex\":42,\"casING\":\"aBc\"},\"casedImproperly\":\"ABC\"}');
    });
    
    test("should send specified data in the query string, if specified", async () => {
      queryParams['q1'] = 'v1';
      queryParams['q2'] = 'v2';
      await apiClient.delete(url, queryParams);
      expect(interceptedUrl).toEqual('https://dalion.eu/ringortests/item?q1=v1&q2=v2');
    });

    test("should send uri encoded data in the query string, if specified", async () => {
      queryParams['q1'] = 'https://dalion.eu/';
      queryParams['https://dalion.eu/'] = 'v2';
      await apiClient.delete(url, queryParams);
      expect(interceptedUrl).toEqual('https://dalion.eu/ringortests/item?q1=https%3A%2F%2Fdalion.eu%2F&https%3A%2F%2Fdalion.eu%2F=v2');
    });

    test("should send specified data in the query string, even if there is no value", async () => {
      queryParams['q1'] = undefined;
      queryParams['q2'] = undefined;
      await apiClient.delete(url, queryParams);
      expect(interceptedUrl).toEqual('https://dalion.eu/ringortests/item?q1&q2');
    });
  });

  describe('patch', () => {
    let url: string;    
    let queryParams: IDictionary<string>;
    let headers: IDictionary<string>;
    let interceptedUrl: string;
    let interceptedRequest: Request;

    beforeEach(() => {
      const RequestSenderMock = jest.fn<IRequestSender>(() => ({
        send: jest.fn().mockImplementation((url, request) => {
          interceptedUrl = url;
          interceptedRequest = request;
        })
      }));
      requestSender = new RequestSenderMock();
      apiClient = new ApiClient(requestSender);
      url = 'https://dalion.eu/ringortests/item';
      queryParams = {};
      headers = {};
    });

    test("should patch the correct url", async () => {
      await apiClient.patch(url);
      expect(requestSender.send)
        .toHaveBeenCalledWith(url, expect.objectContaining({ method: 'PATCH' }));
    });

    test("should send request with default headers and mode, if none is specified", async () => {
      await apiClient.patch(url);
      expect(requestSender.send)
        .toHaveBeenCalledWith(url, expect.objectContaining({ mode: 'same-origin', headers: undefined }));
    });

    test("should send specified mode, if overridden", async () => {
      await apiClient.patch(url, undefined, undefined, undefined, 'no-cors');
      expect(requestSender.send)
        .toHaveBeenCalledWith(url, expect.objectContaining({ mode: 'no-cors' }));
    });

    test("should send specified headers, if specified", async () => {
      headers['h1'] = 'v1';
      headers['h2'] = 'v2';
      await apiClient.patch(url, undefined, undefined, headers);
      expect(interceptedRequest.headers).toEqual({ "h1": "v1", "h2": "v2" });
    });

    test("should send undefined body as default", async () => {
      await apiClient.patch(url);
      expect(interceptedRequest.body).toBeUndefined();
    });

    test("should send camel-cased json representation of the body, if specified", async () => {
      const data: any = {
        'q1': 'v1',
        'q2': {'complex': 42, 'CasING': 'aBc'},
        'CasedImproperly': 'ABC'
      };
      await apiClient.patch(url, undefined, data);
      expect(interceptedRequest.body).toEqual('{\"q1\":\"v1\",\"q2\":{\"complex\":42,\"casING\":\"aBc\"},\"casedImproperly\":\"ABC\"}');
    });
    
    test("should send specified data in the query string, if specified", async () => {
      queryParams['q1'] = 'v1';
      queryParams['q2'] = 'v2';
      await apiClient.patch(url, queryParams);
      expect(interceptedUrl).toEqual('https://dalion.eu/ringortests/item?q1=v1&q2=v2');
    });

    test("should send uri encoded data in the query string, if specified", async () => {
      queryParams['q1'] = 'https://dalion.eu/';
      queryParams['https://dalion.eu/'] = 'v2';
      await apiClient.patch(url, queryParams);
      expect(interceptedUrl).toEqual('https://dalion.eu/ringortests/item?q1=https%3A%2F%2Fdalion.eu%2F&https%3A%2F%2Fdalion.eu%2F=v2');
    });

    test("should send specified data in the query string, even if there is no value", async () => {
      queryParams['q1'] = undefined;
      queryParams['q2'] = undefined;
      await apiClient.patch(url, queryParams);
      expect(interceptedUrl).toEqual('https://dalion.eu/ringortests/item?q1&q2');
    });
  });
});