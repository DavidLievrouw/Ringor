import * as React from "react";
import "./styles/site.less";
import composition from '../composition';
import { IApplicationInfo } from '../facades/applicationInfo';

export interface IAppProps {
  className: string;
}

export interface IAppState {
  applicationInfo: IApplicationInfo;
}

export class App extends React.Component<IAppProps, IAppState> {
  private applicationInfo: IApplicationInfo;

  constructor(props: IAppProps) {
    super(props);
    this.applicationInfo = composition.applicationInfo;
  }

  render() {
    return (
      <div className="column">
        <div className="ui huge header">
          <div className="ui massive company">{this.applicationInfo.company}</div>
        </div>
        <div className="ui huge header status">
          <div className="ui large basic label">{this.applicationInfo.product}</div>
          <div className="ui large green label">
            <i className="check circle icon"></i>
            up and running
          </div>
        </div>
        <div className="ui divider"></div>
        <div className="ui one column stackable center aligned grid">
          <div className="ui three item secondary menu landing">
            <div className="item">
              <a className="ui olive huge basic image label" href="swagger">
                <img src="/swagger.png" />
                Swagger UI
              </a>
            </div>
            <div className="item">
              <a className="ui violet huge basic label" href="login">
                <i className="sign-in icon"></i>
                Browser sign in
              </a>
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
