const commandLineArgs = require('command-line-args');
const createWebpackCompiler = require("./Webpack/createWebpackCompiler");
const runWebpackCompiler = require("./Webpack/runWebpackCompiler");
const logger = require("./logger");

const options = commandLineArgs([
  { name: 'target', alias: 't', type: String },
  { name: 'configuration', alias: 'c', type: String },
  { name: 'action', alias: 'a', type: String }
]);

const bundleArguments = {
  'target': options.target,
  'configuration': options.configuration,
  'action': options.action
};
logger.logStart('Bundling with following arguments:');
Object.keys(bundleArguments).forEach(function(key) {
  logger.logFinish(' - ' + key + ': ' + bundleArguments[key]);
});

const webpackCompiler = createWebpackCompiler(bundleArguments);
runWebpackCompiler(webpackCompiler, bundleArguments);
