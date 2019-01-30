import { IDictionary } from "../facades/IDictionary";

export interface IApiClient {
  get(url: string, data: IDictionary<string>, headers: IDictionary<string>, mode: RequestMode): Promise<Response>;
  post(url: string, data: IDictionary<string>, headers: IDictionary<string>, mode : RequestMode): Promise<Response>;
}

class ApiClient {
  get(url: string, data: IDictionary<string>, headers: IDictionary<string>, mode : RequestMode = 'same-origin'): Promise<Response> {
    var query = [];
    for (var key in data) {
      query.push(encodeURIComponent(key) + '=' + encodeURIComponent(data[key]));
    }
    return fetch(url, {
      method: 'GET',
      mode: mode as RequestMode,
      headers: headers && new Headers(headers)
    });
  }

  post(url: string, data: IDictionary<string>, headers: IDictionary<string>, mode : RequestMode = 'same-origin'): Promise<Response> {
    var query = [];
    for (var key in data) {
      query.push(encodeURIComponent(key) + '=' + encodeURIComponent(data[key]));
    }
    return fetch(url, {
      method: 'POST',
      mode: mode as RequestMode,
      headers: headers && new Headers(headers),
      body: data && JSON.stringify(data)
    });
  }
}

export default ApiClient;