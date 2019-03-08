const styles = require('./styles/site.less');

import * as React from "react";
import { IApplicationInfo } from '../facades/applicationInfo';

export interface ILandingProps {
  applicationInfo: IApplicationInfo;
}

export interface ILandingState {}

export class Landing extends React.Component<ILandingProps, ILandingState> {
  constructor(props: ILandingProps) {
    super(props);
  }

  render() {
    return (
      <div className={`ui middle aligned center aligned full-height bounds-adhering ${styles['padded']} grid`}>
        <div className="column">
          <div className={`ui huge ${styles.header}`}>
            <div className={`ui massive ${styles.company}`}>{this.props.applicationInfo.company}</div>
          </div>
          <div className={`ui huge ${styles.header}`}>
            <div className="ui centered card">
              <div className="content">
                <div className="header">{this.props.applicationInfo.product}</div>
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
