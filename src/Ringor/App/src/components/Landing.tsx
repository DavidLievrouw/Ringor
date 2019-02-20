import * as React from "react";
import { Link } from "react-router-dom";
const styles = require('./styles/site.less');
import composition from '../composition';
import { IApplicationInfo } from '../facades/applicationInfo';

export interface ILandingProps { }

export interface ILandingState {
  applicationInfo: IApplicationInfo;
}

export class Landing extends React.Component<ILandingProps, ILandingState> {
  private applicationInfo: IApplicationInfo;

  constructor(props: ILandingProps) {
    super(props);
    this.applicationInfo = composition.applicationInfo;
  }

  render() {
    return (
      <div className="column">
        <div className={`ui huge ${styles.header}`}>
          <div className={`ui massive ${styles.company}`}>{this.applicationInfo.company}</div>
        </div>
        <div className={`ui huge ${styles.header} ${styles.status}`}>
          <div className="ui large basic label">{this.applicationInfo.product}</div>
          <div className="ui large green label">
            <i className="check circle icon"></i>
            up and running
          </div>
        </div>
        <div className="ui divider"></div>
        <div className="ui one column stackable center aligned grid">
          <div className={`ui three item secondary menu ${styles.menu} ${styles.landing}`}>
            <div className="item">
              <a className="ui olive huge basic image label" href="swagger">
                <img src="/swagger.png" />
                Swagger UI
              </a>
            </div>
            <div className="item">
              <Link to="/login" className="ui violet huge basic label">
                <i className="sign-in icon"></i>
                Browser sign in
              </Link>
            </div>
            <div className="item">
              <a className="ui black huge basic image label" href="api">
                <img src="/api.png" />
                Navigate API
              </a>
            </div>
          </div>
        </div>
      </div>
    );
  }
}
