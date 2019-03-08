import * as React from "react";
import IFrame from 'react-iframe';
const styles = require('./styles/site.less');

export interface ISwaggerProps { }

export interface ISwaggerState {
}

export class Swagger extends React.Component<ISwaggerProps, ISwaggerState> {
  constructor(props: ISwaggerProps) {
    super(props);
  }

  render() {
    return (
      <div className={`${styles['fillbox']}`}>
        <IFrame
          className={`${styles['row']} ${styles['content']}`}
          position="relative"
          url="swagger" />
      </div>
    );
  }
}
