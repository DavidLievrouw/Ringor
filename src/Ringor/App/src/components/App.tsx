import * as React from "react";
import { BrowserRouter as Router, Route, Link } from "react-router-dom";
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
        <div className={`${styles.app}`}>
          <div className={`ui left fixed vertical labeled icon menu ${styles['vertical']} ${styles['icon']} ${styles['menu']} ${styles['fixed-width-menu']}`}>
            <Link to="/" className="item">
              <i className="home icon"></i>
              Home
            </Link>
            <a className="item" href="swagger">
              <img src="/swagger.png" />
              Swagger UI
            </a>
            <a className="item" href="api">
              <img src="/api.png" />
              Navigate API
            </a>
            <Link to="/login" className="item">
              <i className="sign-in icon"></i>
              Browser login
            </Link>
          </div>
          <div className={`${styles['app-content']}`}>
            <Route exact path="/" component={Landing} />
            <Route path="/login" component={Login} />
          </div>
        </div>
      </Router>
    );
  }
}
