import * as React from "react";
import { IApplicationInfo } from '../facades/applicationInfo';
import { ISecuredApiClient } from '../services/SecuredApiClient';

export interface IProfileProps {
  applicationInfo: IApplicationInfo;
  securedApiClient: ISecuredApiClient;
}

export interface IProfileState { }

export class Profile extends React.Component<IProfileProps, IProfileState> {
  constructor(props: IProfileProps) {
    super(props);
  }

  componentDidMount() {
    // Make an API call to force authentication
    this.props.securedApiClient.get('api/claims')
      .then(response => {
        if (!response.ok) throw new Error(`Call to get profile information failed: ${response.status} - ${response.statusText}.`);
        return response.json();
      });
  }

  render() {
    const user = this.props.securedApiClient.getUser();

    return (
      <div className="ui middle aligned center aligned full-height padded padded-content grid">
        <div className="five wide input column">
          <h2 className="content">
            {user && user.name}
          </h2>
        </div>
      </div>
    );
  }
}
