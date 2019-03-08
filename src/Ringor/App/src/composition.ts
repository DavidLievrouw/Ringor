import UrlService, { IUrlService } from "./services/UrlService";
import ApiClient, { IApiClient } from "./services/ApiClient";

import applicationInfo, { IApplicationInfo } from "./facades/applicationInfo";
import RequestSender, { IRequestSender } from './facades/RequestSender';

const urlService = new UrlService(applicationInfo.urlInfo);
const requestSender = new RequestSender();
const apiClient = new ApiClient(urlService, requestSender);

export interface IServices {
  urlService: IUrlService;
  requestSender: IRequestSender;
  apiClient: IApiClient;
}

export interface IComposition {
  applicationInfo: IApplicationInfo;
  services: IServices;
}

const composition : IComposition = {
  applicationInfo: applicationInfo,
  services: {
    urlService: urlService,
    requestSender: requestSender,
    apiClient: apiClient
  }
};
    
export default composition;