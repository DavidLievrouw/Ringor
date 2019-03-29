import * as React from "react";
import { IApplicationInfo } from '../facades/applicationInfo';
import { ISecuredApiClient } from '../services/SecuredApiClient';
import { Redirect } from 'react-router';
import { IDictionary } from '../facades/IDictionary';
import { CompanyLogo } from './CompanyLogo';
import { BigHeader } from './BigHeader';

export interface IProfileProps {
  applicationInfo: IApplicationInfo;
  securedApiClient: ISecuredApiClient;
}

export interface IProfileState {
  requestsLogOut: boolean;
}

export class Profile extends React.Component<IProfileProps, IProfileState> {
  constructor(props: IProfileProps) {
    super(props);

    this.state = {
      requestsLogOut: false
    };

    this.logOut = this.logOut.bind(this);
  }

  componentDidMount() {
    // Make an API call to force authentication
    this.props.securedApiClient.get('api/claims')
      .then(response => {
        if (!response.ok) throw new Error(`Call to get profile information failed: ${response.status} - ${response.statusText}.`);
        return response.json();
      });
  }

  logOut() {
    this.setState({ requestsLogOut: true });
  }

  render() {
    const user = this.props.securedApiClient.getUser();

    let redirector = null;
    if (this.state.requestsLogOut) {
      redirector = <Redirect to="/logout" />;
    }

    let form = null;
    if (user) {
      const account = user.name;
      const idTokenDictionary = user.idToken as IDictionary<string>;
      const preferredUserName = idTokenDictionary["preferred_username"];

      form = (
        <form className="ui form">
          <div className="field">
            <label>Account</label>
            <input placeholder={account} type="text" readOnly />
          </div>
          <div className="field">
            <label>Preferred user name</label>
            <input placeholder={preferredUserName} type="text" readOnly />
          </div>
          <div className="ui submit button" onClick={this.logOut}>Log out</div>
        </form>
      );
    }

    return (
      <div className="padded-content">
        <BigHeader icon="id card" header="Profile" />
        <div className="ui middle aligned center aligned full-height padded padded-content grid">
          <div className="five wide input column">
            <div className="ui left aligned padded grid">
              <div className="sixteen wide column">
                {form}
              </div>
            </div>
            {redirector}
          </div>
        </div>
      </div>
    );
  }
}
