import * as React from "react";
import {render} from 'react-dom';
import {App} from './components/App';

import composition from './composition';

import "./components/styles/site.less";

global.Promise = require('es6-promise').Promise; // For IE11 compatibility

var appElem = document.getElementById('App');
appElem.setAttribute("class", "app");

render(<App composition={composition} />, appElem);

