import React from 'react';
import "./styles/site.less";

export class App extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <div className="column">
        <h1 className="ui image header middle aligned center aligned grid">
          <div className="four wide column"><img src="dalilogo.png" /></div>
          <div className="eight wide column">Up-and-running!</div>
        </h1>
        <div className="ui divider"></div>
        <div className="ui middle aligned center aligned grid">
          <div className="four wide column">
            <div className="ui segment">
              <a className="ui big image label" href="swagger">
                <img src="/swagger.png" />
                Swagger UI
              </a>
            </div>
          </div>
          <div className="four wide column">
            <div className="ui segment">
              <a className="ui big image label" href="login">
                <i class="sign-in icon"></i>
                Log in
              </a>
            </div>
          </div>
          <div className="four wide column">
            <div className="ui segment">
              <a className="ui big image label" href="api">
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
