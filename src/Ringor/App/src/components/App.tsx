import * as React from "react";
import { BrowserRouter as Router, Route } from "react-router-dom";
import { Landing } from './Landing';
import { Login } from './Login';
const styles = require('./styles/site.less');

export interface IAppProps { }

export interface IAppState { }

export class App extends React.Component<IAppProps, IAppState> {
  constructor(props: IAppProps) {
    super(props);
  }

  render() {
    return (
      <Router>
        <div className={`ui middle aligned center aligned grid ${styles.ringorApp}`} style={{ margin: "0px" }}>
          <Route exact path="/" component={Landing} />
          <Route path="/login" component={Login} />
        </div>
      </Router>
    );
  }
}
