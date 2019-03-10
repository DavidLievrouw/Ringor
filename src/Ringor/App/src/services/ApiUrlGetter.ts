import { IApiClient } from './ApiClient';

export interface IApiUrlGetter {
  get(url: string): Promise<Object>;
}

class ApiUrlGetter implements IApiUrlGetter {
  private apiClient: IApiClient;

  constructor(apiClient: IApiClient) {
    this.apiClient = apiClient;
  }

  get(url: string): Promise<Object> {
    const paramsValidation = new Promise<string>(function (resolve, reject) {
      let sanitizedUrl = url && url.trim();
      if (!sanitizedUrl) reject(new Error('No Api URL is specified.'));

      if (sanitizedUrl.toLowerCase().indexOf("http") === 0) reject(new Error('Absolute URLs are not allowed. URLs should start with \'/api\'.'));
      if (sanitizedUrl.toLowerCase().indexOf("/api") !== 0) reject(new Error('Only relative URLs that start with \'/api\' are allowed.'));

      sanitizedUrl = '/api' + sanitizedUrl.substring(4);

      resolve(sanitizedUrl);
    });

    return paramsValidation
      .then(sanitized => this.apiClient.get(sanitized))
      .then(response => {
        if (!response.ok) throw new Error(`Call to get url '${url}' failed: ${response.status} - ${response.statusText}.`);
        return response.json();
      });
  }
}

export default ApiUrlGetter;