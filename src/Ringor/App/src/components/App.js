import React from 'react';
import "./styles/site.less";
import composition from '../composition';

export class App extends React.Component {
  constructor(props) {
    super(props);
    this.applicationInfo = composition.applicationInfo;
  }

  render() {
    return (
      <div className="column">
        <div class="ui huge header">
          <div class="ui massive company">{this.applicationInfo.company}</div>
        </div>
        <div class="ui huge header status">
          <div className="ui large basic label">{this.applicationInfo.product}</div>
          <div className="ui large basic label">v{this.applicationInfo.version}</div>
          <div className="ui large green label">
            <i class="check circle icon"></i>
            up and running
        </div>
        </div>
        <div className="ui divider"></div>
        <div className="ui three item secondary menu">
          <div className="item">
            <a className="ui olive huge basic image label" href="swagger">
              <img src="/swagger.png" />
              Swagger UI
            </a>
          </div>
          <div className="item">
            <a className="ui violet huge basic label" href="login">
              <i class="sign-in icon"></i>
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
    );
  }
}
