import * as React from "react";
import { Landing } from './Landing';

export interface IAppProps {}

export interface IAppState {}

export class App extends React.Component<IAppProps, IAppState> {
  constructor(props: IAppProps) {
    super(props);
  }

  render() {
    return (
      <Landing />
    );
  }
}
