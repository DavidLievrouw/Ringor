import * as React from "react";
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
      <div className={`ui middle aligned center aligned full-height bounds-adhering ${styles['padded']} grid`}>
        <div className="column">
          <div className={`ui huge ${styles.header}`}>
            <div className={`ui massive ${styles.company}`}>{this.applicationInfo.company}</div>
          </div>
          <div className={`ui huge ${styles.header}`}>
            <div className="ui centered card">
              <div className="content">
                <div className="header">{this.applicationInfo.product}</div>
              </div>
              <div className="extra content">
                <i className="check circle green icon"></i>
                up and running
            </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}
