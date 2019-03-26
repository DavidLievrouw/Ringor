import * as React from "react";
import { IApplicationInfo } from '../facades/applicationInfo';
import { CompanyLogo } from './CompanyLogo';
import { ISecuredApiClient } from '../services/SecuredApiClient';
import { IUrlService } from '../services/UrlService';

export interface ILogoutProps {
  applicationInfo: IApplicationInfo;
  urlService: IUrlService;
  securedApiClient: ISecuredApiClient;
}

export interface ILogoutState { 
  isLoggedIn: boolean;
}

export class Logout extends React.Component<ILogoutProps, ILogoutState> {
  constructor(props: ILogoutProps) {
    super(props);
    this.state = { 
      isLoggedIn: true
    };
  }

  componentWillMount() {
    const isLoggedIn = this.props.securedApiClient.isLoggedIn();
    this.setState({
      isLoggedIn: isLoggedIn
    });
    if (!isLoggedIn) {
      const url = this.props.urlService.getApplicationUrl();
      window.location.href = url;
    }
  }

  componentDidMount() {
    if (this.state.isLoggedIn) {
      this.props.securedApiClient.logout();
    }
  }

  render() {
    return (
      <div className="ui middle aligned center aligned full-height padded padded-content grid">
        <div className="five wide input column">
          <CompanyLogo applicationInfo={this.props.applicationInfo} />
          <h2 className="content">
            Logging out...
          </h2>
        </div>
      </div>
    );
  }
}
