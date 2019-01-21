require("colors");
const webpack = require("webpack");
const logger = require("../logger");
const createWebpackOptions = require("./createWebpackOptions");

module.exports = bundleArguments => {
  logger.logStart("Creating webpack compiler...");
  const webpackOptions = createWebpackOptions(bundleArguments);
  const compiler = webpack(webpackOptions);
  logger.logFinish("OK Finished creating webpack compiler");
  return compiler;
};
