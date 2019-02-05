import * as React from "react";
import {render} from 'react-dom';
import {App} from './components/App';

const styles = require('./components/styles/site.less');

var appElem = document.getElementById('App');
appElem.setAttribute("class", `ui middle aligned center aligned grid ${styles.ringorApp}`);
appElem.setAttribute("style", 'height:100%;margin:0px;');

render(<App />, appElem);

