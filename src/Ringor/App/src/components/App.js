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
          <div className="two wide column">Hi!</div>
        </h1>
        <div className="ui stacked segment">
          <div className="ui middle aligned center aligned grid">
            <div className="four wide column">Swagger UI</div>
            <div className="four wide column">Log in</div>
            <div className="four wide column">Navigate the API</div>
          </div>
        </div>
      </div>
    );
  }
}
