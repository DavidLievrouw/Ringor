import UrlService from "./services/UrlService";
import ApiClient from "./services/ApiClient";

import applicationInfo from "./facades/applicationInfo";

const urlService = new UrlService(applicationInfo.urlInfo);
const apiClient = new ApiClient(urlService);

const composition = {
  applicationInfo: applicationInfo,
  services: {
    urlService: urlService,
    apiClient: apiClient
  }
};
    
export default composition;