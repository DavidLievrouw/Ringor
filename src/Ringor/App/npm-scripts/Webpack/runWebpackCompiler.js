const logger = require("../logger");

module.exports = (webpackCompiler, bundleArguments) => {
  logger.logStart("Running webpack compiler...");

  const compilerCallback = (errors, statistics) => {
    if (errors) {
      logger.logError(errors);
      return;
    }

    logger.logFinish("OK Finished running webpack compiler");
    console.info(statistics.toString({
      version: true,
      colors: true,
      assets: true,
      chunks: false,
      chunkModules: false,
      modules: false,
      children: false,
      cached: false
    }));
  };

  switch(bundleArguments.action) {
    case "watch":
      logger.logFinish("OK Webpack compiler is watching for changes ...");
      webpackCompiler.watch({ aggregateTimeout: 200, poll: true }, compilerCallback);
      break;
    case "bundle":
      logger.logStart("Webpack compiler is running ...");
      webpackCompiler.run(compilerCallback);
      break;
  }
};