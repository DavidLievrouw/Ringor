import UrlService, { IUrlService } from "./services/UrlService";
import ApiClient, { IApiClient } from "./services/ApiClient";

import applicationInfo, { IApplicationInfo } from "./facades/applicationInfo";

const urlService = new UrlService(applicationInfo.urlInfo);
const apiClient = new ApiClient();

export interface IServices {
  urlService: IUrlService;
  apiClient: IApiClient;
}

export interface IComposition {
  applicationInfo: IApplicationInfo;
  services: IServices;
}

const composition = {
  applicationInfo: applicationInfo,
  services: {
    urlService: urlService,
    apiClient: apiClient
  }
};
    
export default composition;