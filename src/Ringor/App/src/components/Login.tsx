import * as React from "react";
const styles = require('./styles/site.less');

export interface ILoginProps { }

export interface ILoginState {
}

export class Login extends React.Component<ILoginProps, ILoginState> {
  constructor(props: ILoginProps) {
    super(props);
  }

  render() {
    return (
      <div className={`${styles.fill}`}>
        <div className="ui form">
          <div className="ui message">
            Login
          </div>
        </div>
      </div>
    );
  }
}
