import * as React from "react";
import { IComposition } from '../composition';
import { BrowserRouter as Router, Route, Link, Switch } from "react-router-dom";
import { Landing } from './Landing';
import { Login } from './Login';
import { Swagger } from './Swagger';
import { Api } from './Api';
import { NotFound } from './NotFound';
import { Logout } from './Logout';
import { Profile } from './Profile';

export interface IAppProps {
  composition: IComposition
}

export interface IAppState { 
  hasConsent: boolean;
}

export class App extends React.Component<IAppProps, IAppState> {
  constructor(props: IAppProps) {
    super(props);

    this.state = {
      hasConsent: !props.composition.trackingConsent.showNag
    }

    this.consentToCookieTracking = this.consentToCookieTracking.bind(this);
  }

  consentToCookieTracking() {
    const trackingConsent = this.props.composition.trackingConsent;
    document.cookie = trackingConsent.cookieString;
    this.setState({hasConsent: true});
  }

  render() {
    const applicationInfo = this.props.composition.applicationInfo;
    const isLoggedIn = this.props.composition.services.securedApiClient.isLoggedIn();

    let logoutLink;
    if (isLoggedIn) {
      logoutLink = <Link to="/logout" className="item">
        <i className="sign-out icon"></i>
        Log out
      </Link>;
    }

    return (
      <Router>
        <div className="app">
          <div className={`cookie nag ${this.state.hasConsent ? "hidden" : ""}`}>
            <div className="ui floating brown message">
              <i className="close icon" onClick={this.consentToCookieTracking}></i>
              <div className="header">
                We use cookies
              </div>
              <p>We use cookies to ensure you get the best experience on our website</p>
            </div>
          </div>
          <div className="ui left fixed vertical labeled icon menu fixed-width-menu">
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
            <Link to="/profile" className="item">
              <i className="id card icon"></i>
              Your profile
            </Link>
            {logoutLink}
          </div>
          <div className="app-content">
            <Switch>
              <Route exact path="/" render={(routeProps) => <Landing applicationInfo={this.props.composition.applicationInfo} />} />
              <Route path="/swaggerui" render={(routeProps) => <Swagger />} />
              <Route path="/apinav" render={(routeProps) => <Api
                apiUrlGetter={this.props.composition.services.apiUrlGetter}
                urlService={this.props.composition.services.urlService}
                apiUrlPasteHandler={this.props.composition.services.apiUrlPasteHandler} />}
              />
              <Route path="/login" render={(routeProps) => <Login applicationInfo={this.props.composition.applicationInfo} />} />
              <Route path="/profile" render={(routeProps) => <Profile
                applicationInfo={this.props.composition.applicationInfo}
                securedApiClient={this.props.composition.services.securedApiClient} />} />
              <Route path="/logout" render={(routeProps) => <Logout
                applicationInfo={this.props.composition.applicationInfo}
                urlService={this.props.composition.services.urlService}
                securedApiClient={this.props.composition.services.securedApiClient} />} />
              <Route render={(routeProps) => <NotFound urlService={this.props.composition.services.urlService} path={routeProps.location.pathname} query={routeProps.location.search} />} />
            </Switch>
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
