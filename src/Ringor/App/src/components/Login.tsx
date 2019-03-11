import * as React from "react";
import { IApplicationInfo } from '../facades/applicationInfo';
import { CompanyLogo } from './CompanyLogo';

export interface ILoginProps {
  applicationInfo: IApplicationInfo;
}

export interface ILoginState { }

export class Login extends React.Component<ILoginProps, ILoginState> {
  constructor(props: ILoginProps) {
    super(props);
  }

  render() {
    return (
      <div className="ui middle aligned center aligned full-height padded padded-content grid">
        <div className="five wide input column">
          <CompanyLogo applicationInfo={this.props.applicationInfo} />
          <h2 className="content">
            Log-in to your account
          </h2>
          <form className="ui large form">
            <div className="ui stacked segment">
              <div className="field">
                <div className="ui left icon input">
                  <i className="user icon"></i>
                  <input type="text" name="email" placeholder="E-mail address" autoComplete="off" />
                </div>
              </div>
              <div className="field">
                <div className="ui left icon input">
                  <i className="lock icon"></i>
                  <input type="password" name="password" placeholder="Password" autoComplete="off" />
                </div>
              </div>
              <div className="ui fluid large primary submit button">Login</div>
            </div>
            <div className="ui error message"></div>
          </form>
        </div>
      </div>
    );
  }
}
