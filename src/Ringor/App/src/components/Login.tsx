import * as React from "react";

export interface ILoginProps {}

export interface ILoginState {
}

export class Login extends React.Component<ILoginProps, ILoginState> {
  constructor(props: ILoginProps) {
    super(props);
  }

  render() {
    return (
      <div>Login</div>
    );
  }
}
