const styles = require('./styles/site.less');

import * as React from "react";
import { IApplicationInfo } from '../facades/applicationInfo';

export interface ILoginProps { 
  applicationInfo: IApplicationInfo;
}

export interface ILoginState {}

export class Login extends React.Component<ILoginProps, ILoginState> {
  constructor(props: ILoginProps) {
    super(props);
  }

  render() {
    return (
      <div className={`ui middle aligned center aligned full-height padded ${styles['padded-content']} grid`}>
        <div className="five wide input column">
          <h2 className="ui image header">
            <div className={`ui huge ${styles.header}`}>
              <div className={`ui massive ${styles.company}`}>{this.props.applicationInfo.company}</div>
            </div>
            <div className="content">
              Log-in to your account
            </div>
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
