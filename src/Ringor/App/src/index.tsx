import * as React from "react";
import {render} from 'react-dom';
import {App} from './components/App';
const styles = require('./components/styles/site.less');

global.Promise = require('es6-promise').Promise; // For IE11 compatibility

var appElem = document.getElementById('App');
appElem.setAttribute("class", styles.app);

render(<App />, appElem);

