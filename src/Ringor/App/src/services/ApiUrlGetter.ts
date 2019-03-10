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
    const paramsValidation = new Promise(function (resolve, reject) {
      const sanitizedUrl = url && url.trim();
      if (!sanitizedUrl) throw new Error('No URL is specified when performing a call to get an API endpoint.');
      resolve();
    });

    return paramsValidation
      .then(() => this.apiClient.get(url))
      .then(response => {
        if (!response.ok) throw new Error(`Call to get url '${url}' failed: ${response.status} - ${response.statusText}.`);
        return response.json();
      });
  }
}

export default ApiUrlGetter;