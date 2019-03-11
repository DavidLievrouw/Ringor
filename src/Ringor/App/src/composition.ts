import UrlService, { IUrlService } from "./services/UrlService";
import ApiClient, { IApiClient } from "./services/ApiClient";

import applicationInfo, { IApplicationInfo } from "./facades/applicationInfo";
import RequestSender, { IRequestSender } from './facades/RequestSender';
import ApiUrlGetter, { IApiUrlGetter } from './services/ApiUrlGetter';
import ApiUrlPasteHandler, { IApiUrlPasteHandler } from './services/ApiUrlPasteHandler';

const urlService = new UrlService(applicationInfo.urlInfo);
const requestSender = new RequestSender();
const apiClient = new ApiClient(urlService, requestSender);
const apiUrlGetter = new ApiUrlGetter(apiClient);
const apiUrlPasteHandler = new ApiUrlPasteHandler(urlService);

export interface IServices {
  urlService: IUrlService;
  requestSender: IRequestSender;
  apiClient: IApiClient;
  apiUrlGetter: IApiUrlGetter;
  apiUrlPasteHandler: IApiUrlPasteHandler;
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
    apiUrlGetter: apiUrlGetter,
    apiUrlPasteHandler: apiUrlPasteHandler
  }
};
    
export default composition;