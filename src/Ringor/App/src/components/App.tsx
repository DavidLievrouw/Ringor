const styles = require('./styles/site.less');

import * as React from "react";
import { IComposition } from '../composition';
import { BrowserRouter as Router, Route, Link } from "react-router-dom";
import { Landing } from './Landing';
import { Login } from './Login';
import { Swagger } from './Swagger';
import { Api } from './Api';

export interface IAppProps {
  composition: IComposition
}

export interface IAppState { }

export class App extends React.Component<IAppProps, IAppState> {
  constructor(props: IAppProps) {
    super(props);
  }

  render() {
    const applicationInfo = this.props.composition.applicationInfo;

    return (
      <Router>
        <div className={`${styles.app}`}>
          <div className={`ui left fixed vertical labeled icon menu ${styles['fixed-width-menu']}`}>
            <Link to="/" className="item">
              <i className="home icon"></i>
              Home
            </Link>
            <Link to="/swaggerui" className="item">
              <img src="/swagger.png" />
              Swagger UI
            </Link>
            <Link to="/apinav" className="item">
              <img src="/api.png" />
              Navigate the API
            </Link>
            <Link to="/login" className="item">
              <i className="sign-in icon"></i>
              Browser login
            </Link>
          </div>
          <div className={`${styles['app-content']}`}>
            <Route exact path="/" render={(routeProps) => <Landing applicationInfo={this.props.composition.applicationInfo} />} />
            <Route path="/login" render={(routeProps) => <Login applicationInfo={this.props.composition.applicationInfo} />} />
            <Route path="/apinav" render={(routeProps) => <Api
              apiUrlGetter={this.props.composition.services.apiUrlGetter}
              urlService={this.props.composition.services.urlService}
              apiUrlPasteHandler={this.props.composition.services.apiUrlPasteHandler} />}
            />
            <Route path="/swaggerui" render={(routeProps) => <Swagger />} />
          </div>
          <footer>
            <div className="ui label">{applicationInfo.company} {applicationInfo.product} v{applicationInfo.version}</div>
            <div className="ui label">{applicationInfo.environment}</div>
          </footer>
        </div>
      </Router>
    );
  }
}
