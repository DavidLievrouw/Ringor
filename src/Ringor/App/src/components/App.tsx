import * as React from "react";
import { IComposition } from '../composition';
import { BrowserRouter as Router, Route, Link, Switch } from "react-router-dom";
import { Landing } from './Landing';
import { Login } from './Login';
import { NotFound } from './NotFound';
import { Logout } from './Logout';
import { Profile } from './Profile';
import { CookiePolicy } from './CookiePolicy';

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
    this.showCookiePolicy = this.showCookiePolicy.bind(this);
    this.toggleNag = this.toggleNag.bind(this);
    this.resetNag = this.resetNag.bind(this);
  }

  componentDidMount() {
    this.resetNag();
    if (!this.state.hasConsent) {
      setTimeout(() => this.toggleNag(), 1000);
    }
  }

  showCookiePolicy() {
    const modal : any = $('.cookie-policy');
    modal.modal('show');
  }

  consentToCookieTracking() {
    const trackingConsent = this.props.composition.trackingConsent;
    document.cookie = trackingConsent.cookieString;

    this.setState({ hasConsent: true });

    this.toggleNag();
  }

  toggleNag() {
    const nag : any = $('.cookie.nag');
    nag.transition('fly down');
  }

  resetNag() {
    const nag : any = $('.cookie.nag');
    nag.transition('hide');
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
          <div className="cookie nag">
            <div className="ui floating message">
              <i className="close icon" onClick={this.consentToCookieTracking}></i>
              <div className="header">
                We use cookies
              </div>
              <p>We use cookies to give you a better service. By using this website or closing this message you agree to our <a href="#" onClick={this.showCookiePolicy}>use of cookies</a>.</p>
              <div className="ui primary mini basic button" onClick={this.showCookiePolicy}>Learn more</div>
            </div>
          </div>
          <CookiePolicy applicationInfo={applicationInfo} />
          <div className="ui left fixed vertical labeled icon menu fixed-width-menu">
            <Link to="/" className="item">
              <i className="home icon"></i>
              Home
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
          </footer>
        </div>
      </Router>
    );
  }
}
