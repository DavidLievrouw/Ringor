import * as React from "react";
import { IApplicationInfo } from '../facades/applicationInfo';
import { ISecuredApiClient } from '../services/SecuredApiClient';
import { Redirect } from 'react-router';

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
      form = (
        <form className="ui form">
          <div className="field">
            <label>Account</label>
            <input placeholder={user.userIdentifier} type="text" readOnly />
          </div>
          <div className="field">
            <label>Name</label>
            <input placeholder={user.name} type="text" readOnly />
          </div>
        </form>
      );
    }

    let logOutButton = null;
    if (user) {
      logOutButton = <button className="ui button" onClick={this.logOut}>Log out</button>;
    }

    return (
      <div className="ui middle aligned center aligned full-height padded padded-content grid">
        <div className="five wide input column">
          {form}
          {logOutButton}
          {redirector}
        </div>
      </div>
    );
  }
}
