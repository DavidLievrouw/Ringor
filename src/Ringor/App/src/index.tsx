import * as React from "react";
import {render} from 'react-dom';
import {App} from './components/App';

global.Promise = require('es6-promise').Promise; // For IE11 compatibility

var appElem = document.getElementById('App');
appElem.setAttribute("style", 'height:100%;margin:0px;');

render(<App />, appElem);

