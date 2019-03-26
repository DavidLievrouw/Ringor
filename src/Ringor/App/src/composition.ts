import UrlService, { IUrlService } from "./services/UrlService";
import ApiClient from "./services/ApiClient";
import SecuredApiClient, { ISecuredApiClient } from "./services/SecuredApiClient";
import { IApiClient } from "./services/IApiClient";

import applicationInfo, { IApplicationInfo } from "./facades/applicationInfo";
import RequestSender, { IRequestSender } from './facades/RequestSender';
import ApiUrlGetter, { IApiUrlGetter } from './services/ApiUrlGetter';
import ApiUrlPasteHandler, { IApiUrlPasteHandler } from './services/ApiUrlPasteHandler';

const urlService = new UrlService(applicationInfo.urlInfo);
const requestSender = new RequestSender();
const anonymousApiClient = new ApiClient(urlService, requestSender);
const securedApiClient = new SecuredApiClient(urlService, requestSender, applicationInfo.msalConfig);
const apiUrlGetter = new ApiUrlGetter(securedApiClient);
const apiUrlPasteHandler = new ApiUrlPasteHandler(urlService);

export interface IServices {
  urlService: IUrlService;
  requestSender: IRequestSender;
  anonymousApiClient: IApiClient;
  securedApiClient: ISecuredApiClient;
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
    anonymousApiClient: anonymousApiClient,
    securedApiClient: securedApiClient,
    apiUrlGetter: apiUrlGetter,
    apiUrlPasteHandler: apiUrlPasteHandler
  }
};
    
export default composition;