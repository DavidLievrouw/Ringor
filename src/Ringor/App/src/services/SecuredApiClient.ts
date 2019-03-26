import { IDictionary } from "../facades/IDictionary";
import { IRequestSender } from '../facades/RequestSender';
import { IUrlService } from './UrlService';
import ApiClient from './ApiClient';
import { IMsalConfig } from '../facades/applicationInfo';
import * as Msal from 'msal';
import { IApiClient } from './IApiClient';

export interface ISecuredApiClient extends IApiClient {
  logout(): void;
  getUser(): Msal.User;
}

class SecuredApiClient extends ApiClient implements ISecuredApiClient {
  private msalConfig: IMsalConfig;
  private userAgentApplication: Msal.UserAgentApplication;

  constructor(urlService: IUrlService, requestSender: IRequestSender, msalConfig: IMsalConfig) {
    super(urlService, requestSender);
    this.msalConfig = msalConfig;
    this.userAgentApplication = new Msal.UserAgentApplication(
      msalConfig.clientId,
      msalConfig.authority,
      () => { },
      { 
        cacheLocation: 'localStorage',
        navigateToLoginRequestUrl: false
      });
  }

  getUser() {
    return this.userAgentApplication.getUser();
  }
  
  logout() {
    return this.userAgentApplication.logout();
  }

  sendRequest(method: string, url: string, queryParams: IDictionary<string> = undefined, data: any = undefined, headers: IDictionary<string> = undefined, mode: RequestMode = 'same-origin'): Promise<Response> {
    const tokenAcquisitionMethod = this.userAgentApplication.acquireTokenSilent.bind(this.userAgentApplication);
    return tokenAcquisitionMethod(
      this.msalConfig.scopes,
      this.msalConfig.authority,
      this.userAgentApplication.getUser(),
      undefined,
      this.msalConfig.clientId
    )
    .then((token: string) => {
      if (!headers) headers = {};
      headers['authorization'] = "Bearer " + token;
      return super.sendRequest(method, url, queryParams, data, headers, mode);
    })
    .catch((failure: any) => {
      const userIsNotLoggedIn = failure === 'user_login_error|User login is required';
      const invalidToken = failure.status && failure.status === 401;
      if (userIsNotLoggedIn || invalidToken) this.userAgentApplication.loginRedirect(this.msalConfig.scopes);
      throw failure;
    });
  }
}

export default SecuredApiClient;