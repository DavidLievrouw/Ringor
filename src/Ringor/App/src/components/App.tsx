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
    this.showCookiePolicy = this.showCookiePolicy.bind(this);
  }

  showCookiePolicy() {
    const modal : any = $('.ui.modal');
    modal.modal('show');
  }

  consentToCookieTracking() {
    const trackingConsent = this.props.composition.trackingConsent;
    document.cookie = trackingConsent.cookieString;
    this.setState({ hasConsent: true });
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
            <div className="ui floating message">
              <i className="close icon" onClick={this.consentToCookieTracking}></i>
              <div className="header">
                We use cookies
              </div>
              <p>We use cookies to give you a better service. By using this website or closing this message you agree to our <a href="#" onClick={this.showCookiePolicy}>use of cookies</a>.</p>
              <div className="ui primary mini basic button" onClick={this.showCookiePolicy}>Learn more</div>
            </div>
          </div>
          <div className="ui modal">
            <div className="header">Cookie Policy for {applicationInfo.company} {applicationInfo.product}</div>
            <div className="content">
              <p><strong>What Are Cookies</strong></p>
              <p>As is common practice with almost all professional websites this site uses cookies, which are tiny files that are downloaded to your computer, to improve your experience. This page describes what information they gather, how we use it and why we sometimes need to store these cookies. We will also share how you can prevent these cookies from being stored however this may downgrade or 'break' certain elements of the sites functionality.</p>
              <p>For more general information on cookies see the Wikipedia article on HTTP Cookies.</p>
              <p><strong>How We Use Cookies</strong></p>
              <p>We use cookies for a variety of reasons detailed below. Unfortunately in most cases there are no industry standard options for disabling cookies without completely disabling the functionality and features they add to this site. It is recommended that you leave on all cookies if you are not sure whether you need them or not in case they are used to provide a service that you use.</p>
              <p><strong>Disabling Cookies</strong></p>
              <p>You can prevent the setting of cookies by adjusting the settings on your browser (see your browser Help for how to do this). Be aware that disabling cookies will affect the functionality of this and many other websites that you visit. Disabling cookies will usually result in also disabling certain functionality and features of the this site. Therefore it is recommended that you do not disable cookies.</p>
              <p><strong>The Cookies We Set</strong></p>
              <ul>
                <li>
                  <p>Login related cookies</p>
                  <p>We use cookies when you are logged in so that we can remember this fact. This prevents you from having to log in every single time you visit a new page. These cookies are typically removed or cleared when you log out to ensure that you can only access restricted features and areas when logged in.</p>
                </li>
              </ul>
              <p><strong>Third Party Cookies</strong></p>
              <p>In some special cases we also use cookies provided by trusted third parties. The following section details which third party cookies you might encounter through this site.</p>
              <ul>
                <li>
                  <p>From time to time we test new features and make subtle changes to the way that the site is delivered. When we are still testing new features these cookies may be used to ensure that you receive a consistent experience whilst on the site whilst ensuring we understand which optimisations our users appreciate the most.</p>
                </li>
              </ul>
              <p><strong>More Information</strong></p>
              <p>Hopefully that has clarified things for you and as was previously mentioned if there is something that you aren't sure whether you need or not it's usually safer to leave cookies enabled in case it does interact with one of the features you use on our site.</p>
              <p>However, if you are still looking for more information, then you can contact us through one of our preferred contact methods:</p>
              <ul>
                <li>Email: info@dalion.eu</li>
              </ul>
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
