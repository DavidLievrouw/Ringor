import UrlService, { IUrlService } from "./services/UrlService";
import ApiClient, { IApiClient } from "./services/ApiClient";

import applicationInfo, { IApplicationInfo } from "./facades/applicationInfo";
import RequestSender, { IRequestSender } from './facades/RequestSender';
import ApiUrlGetter, { IApiUrlGetter } from './services/ApiUrlGetter';

const urlService = new UrlService(applicationInfo.urlInfo);
const requestSender = new RequestSender();
const apiClient = new ApiClient(urlService, requestSender);
const apiUrlGetter = new ApiUrlGetter(apiClient);

export interface IServices {
  urlService: IUrlService;
  requestSender: IRequestSender;
  apiClient: IApiClient;
  apiUrlGetter: IApiUrlGetter;
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
    apiClient: apiClient,
    apiUrlGetter: apiUrlGetter
  }
};
    
export default composition;