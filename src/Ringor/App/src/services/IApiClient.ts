import { IDictionary } from "../facades/IDictionary";

export interface IApiClient {
  get(url: string, queryParams?: IDictionary<string>, headers?: IDictionary<string>, mode?: RequestMode): Promise<Response>;
  post(url: string, queryParams?: IDictionary<string>, data?: any, headers?: IDictionary<string>, mode?: RequestMode): Promise<Response>;
  put(url: string, queryParams?: IDictionary<string>, data?: any, headers?: IDictionary<string>, mode?: RequestMode): Promise<Response>;
  delete(url: string, queryParams?: IDictionary<string>, data?: any, headers?: IDictionary<string>, mode?: RequestMode): Promise<Response>;
  patch(url: string, queryParams?: IDictionary<string>, data?: any, headers?: IDictionary<string>, mode?: RequestMode): Promise<Response>;
}
