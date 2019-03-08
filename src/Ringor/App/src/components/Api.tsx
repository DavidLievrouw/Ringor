import * as React from "react";
import IFrame from 'react-iframe';
const styles = require('./styles/site.less');

export interface IApiProps { }

export interface IApiState {
}

export class Api extends React.Component<IApiProps, IApiState> {
  constructor(props: IApiProps) {
    super(props);
  }

  render() {
    return (
      <div className={`${styles['fillbox']}`}>
        <IFrame
          className={`${styles['row']} ${styles['content']}`}
          position="relative"
          url="api" />
      </div>
    );
  }
}
