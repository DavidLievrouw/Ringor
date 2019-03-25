import { IDictionary } from "../facades/IDictionary";
import { IRequestSender } from '../facades/RequestSender';
import { IUrlService } from './UrlService';
import { IApiClient } from './IApiClient';

class ApiClient implements IApiClient {
  protected requestSender: IRequestSender;
  protected urlService: IUrlService;

  constructor(urlService: IUrlService, requestSender: IRequestSender) {
    this.requestSender = requestSender;
    this.urlService = urlService;
  }

  get(url: string, queryParams: IDictionary<string> = undefined, headers: IDictionary<string> = undefined, mode: RequestMode = 'same-origin'): Promise<Response> {
    const absoluteUrl = this.urlService.getAbsoluteUrl(url);
    return this.sendRequest('GET', absoluteUrl, queryParams, undefined, headers, mode);
  }

  post(url: string, queryParams: IDictionary<string> = undefined, data: any = undefined, headers: IDictionary<string> = undefined, mode: RequestMode = 'same-origin'): Promise<Response> {
    const absoluteUrl = this.urlService.getAbsoluteUrl(url);
    return this.sendRequest('POST', absoluteUrl, queryParams, data, headers, mode);
  }

  put(url: string, queryParams: IDictionary<string> = undefined, data: any = undefined, headers: IDictionary<string> = undefined, mode: RequestMode = 'same-origin'): Promise<Response> {
    const absoluteUrl = this.urlService.getAbsoluteUrl(url);
    return this.sendRequest('PUT', absoluteUrl, queryParams, data, headers, mode);
  }

  delete(url: string, queryParams: IDictionary<string> = undefined, data: any = undefined, headers: IDictionary<string> = undefined, mode: RequestMode = 'same-origin'): Promise<Response> {
    const absoluteUrl = this.urlService.getAbsoluteUrl(url);
    return this.sendRequest('DELETE', absoluteUrl, queryParams, data, headers, mode);
  }

  patch(url: string, queryParams: IDictionary<string> = undefined, data: any = undefined, headers: IDictionary<string> = undefined, mode: RequestMode = 'same-origin'): Promise<Response> {
    const absoluteUrl = this.urlService.getAbsoluteUrl(url);
    return this.sendRequest('PATCH', absoluteUrl, queryParams, data, headers, mode);
  }
  
  sendRequest(method: string, url: string, queryParams: IDictionary<string> = undefined, data: any = undefined, headers: IDictionary<string> = undefined, mode: RequestMode = 'same-origin'): Promise<Response> {
    if (queryParams) {
      url += (url.indexOf('?') === -1 ? '?' : '&') + ApiClient.queryParams(queryParams);
    }
    return this.requestSender.send(url, {
      method: method,
      mode: mode as RequestMode,
      headers: headers,
      body: data && JSON.stringify(data, (key, value) => {
        return value && typeof value === 'object'
          ? ApiClient.toCamelCasedProps(value)
          : value;
      })
    });
  }

  static toCamelCasedProps(o: any): any {
    let newO: any, origKey: string, newKey: string, value: any;
    if (o instanceof Array) {
      return o.map((value) => {
        return typeof value === "object"
          ? ApiClient.toCamelCasedProps(value)
          : value;
      })
    } else {
      newO = {};
      for (origKey in o) {
        if (o.hasOwnProperty(origKey)) {
          newKey = (origKey.charAt(0).toLowerCase() + origKey.slice(1) || origKey).toString();
          value = o[origKey];
          if (value instanceof Array || (value !== null && value.constructor === Object)) {
            value = ApiClient.toCamelCasedProps(value);
          }
          newO[newKey] = value;
        }
      }
    }
    return newO;
  }

  static queryParams(params: IDictionary<string>) {
    return Object.keys(params)
      .map(key => {
        const val = params[key];
        return val ? encodeURIComponent(key) + '=' + encodeURIComponent(val) : encodeURIComponent(key);
      })
      .join('&');
  }
}

export default ApiClient;