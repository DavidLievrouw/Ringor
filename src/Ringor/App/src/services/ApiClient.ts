import { IDictionary } from "../facades/IDictionary";
import { IRequestSender } from '../facades/RequestSender';

export interface IApiClient {
  get(url: string, data: IDictionary<string>, headers: IDictionary<string>, mode: RequestMode): Promise<Response>;
  post(url: string, data: IDictionary<string>, headers: IDictionary<string>, mode : RequestMode): Promise<Response>;
}

class ApiClient {
  private requestSender: IRequestSender;

  constructor(requestSender: IRequestSender) {
    this.requestSender = requestSender;
  }

  get(url: string, data: IDictionary<string>, headers: IDictionary<string>, mode : RequestMode = 'same-origin'): Promise<Response> {
    var query = [];
    for (var key in data) {
      query.push(encodeURIComponent(key) + '=' + encodeURIComponent(data[key]));
    }
    return this.requestSender.send(url, {
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
    return this.requestSender.send(url, {
      method: 'POST',
      mode: mode as RequestMode,
      headers: headers && new Headers(headers),
      body: data && JSON.stringify(data)
    });
  }
}

export default ApiClient;