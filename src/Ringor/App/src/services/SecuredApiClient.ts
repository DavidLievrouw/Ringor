import { IDictionary } from "../facades/IDictionary";
import { IRequestSender } from '../facades/RequestSender';
import { IUrlService } from './UrlService';
import ApiClient from './ApiClient';
import { IMsalConfig } from '../facades/applicationInfo';
import * as Msal from 'msal';
import { IApiClient } from './IApiClient';

export interface ISecuredApiClient extends IApiClient {
  logout(): void;
  isLoggedIn(): boolean;
  getAccount(): Msal.Account;
}

class SecuredApiClient extends ApiClient implements ISecuredApiClient {
  private msalConfig: IMsalConfig;
  private userAgentApplication: Msal.UserAgentApplication;

  constructor(urlService: IUrlService, requestSender: IRequestSender, msalConfig: IMsalConfig) {
    super(urlService, requestSender);
    this.msalConfig = msalConfig;
    this.userAgentApplication = new Msal.UserAgentApplication({
      auth: {
        clientId: this.msalConfig.clientId,
        authority: this.msalConfig.authority,
        postLogoutRedirectUri: urlService.getApplicationUrl(),
        navigateToLoginRequestUrl: true
      },
      cache: {
        cacheLocation: "localStorage",
        storeAuthStateInCookie: false
      }
    });
    this.userAgentApplication.handleRedirectCallback(
      tokenResponse => {
        console.log('Token acquisition succeeded: ' + tokenResponse.account.userName + '.');
      },
      errorResponse => {
        const errorMessage = errorResponse.errorMessage;
        console.error('Access token acquisition for web extension failed: ' + (errorMessage || errorResponse ));
        throw errorResponse;
      });
  }

  logout() {
    return this.userAgentApplication.logout();
  }

  getAccount() {
    return this.userAgentApplication.getAccount();
  }

  isLoggedIn() {
    return !!this.userAgentApplication.getAccount();
  }

  sendRequest(method: string, url: string, queryParams: IDictionary<string> = undefined, data: any = undefined, headers: IDictionary<string> = undefined, mode: RequestMode = 'same-origin'): Promise<Response> {
    const tokenAcquisitionMethod = this.userAgentApplication.acquireTokenSilent.bind(this.userAgentApplication);

    const authParameters: any = {
      scopes: this.msalConfig.scopes,
      authority: this.msalConfig.authority,
      account: this.userAgentApplication.getAccount()
    };

    return tokenAcquisitionMethod(authParameters)
      .then((tokenResponse: Msal.AuthResponse) => {
        if (!headers) headers = {};
        if (!tokenResponse.accessToken) throw new Error('No access token was received from the identity provider.');
        headers['authorization'] = "Bearer " + tokenResponse.accessToken;
        return super.sendRequest(method, url, queryParams, data, headers, mode);
      })
      .catch((failure: any) => {
        const errorMessage = failure.errorMessage;
        const errorCode = failure.errorCode;

        const userInteractionRequired = (
          errorMessage && (
            errorMessage.indexOf("login_required") !== -1 ||
            errorMessage.indexOf("consent_required") !== -1 ||
            errorMessage.indexOf("interaction_required") !== -1 ||
            errorMessage.lastIndexOf('AADSTS50058', 0) === 0 ||
            errorMessage.lastIndexOf('AADSTS16000', 0) === 0) ||
          errorCode && (
            errorCode === 'interaction_required')
        );
        const invalidToken = failure.status && failure.status === 401;

        if (userInteractionRequired || invalidToken) {
          this.userAgentApplication.loginRedirect(authParameters);
        } else {
          throw failure;
        }
      });
  }
}

export default SecuredApiClient;