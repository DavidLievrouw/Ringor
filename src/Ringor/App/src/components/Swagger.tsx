import * as React from "react";
import IFrame from 'react-iframe';

export interface ISwaggerProps {}

export interface ISwaggerState {}

export class Swagger extends React.Component<ISwaggerProps, ISwaggerState> {
  constructor(props: ISwaggerProps) {
    super(props);
  }

  render() {
    return (
      <div className="fillbox">
        <IFrame
          className="row content"
          position="relative"
          url="swagger" />
      </div>
    );
  }
}
