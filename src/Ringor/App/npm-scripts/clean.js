const commandLineArgs = require('command-line-args');
const removeFolders = require("./FileSystem/removeFolders");
const removeFiles = require("./FileSystem/removeFiles");
const logger = require("./logger");
const nodeModulesFolder = "./node_modules";
const buildOutputFolder = "../wwwroot/App";
const sourceFolder = "./src";
const testsFolder = "./_tests";

const options = commandLineArgs([
  { name: 'target', alias: 't', type: String, multiple: true }
]);

const cleanArguments = {
  targets: options.target || ["build-output", "tests", "js"]  // by default, clean everything except node_modules
};

const getAction = target => {
  switch (target) {
    case "build-output":
      return removeFolders([buildOutputFolder]);
    case "tests":
      return removeFolders([testsFolder]);
    case "tests-contents":
      return removeFiles(testsFolder + '/**/*');
    case "sourcemaps":
      return removeFiles(buildOutputFolder + "/**/*.map");
    case "node-modules":
      return removeFolders([nodeModulesFolder]);
    case "js":
      return removeFiles(sourceFolder + "/**/*.js{x,\.map,x\.map,}");
  }
};

cleanArguments.targets
  .map(getAction)
  .reduce(
    (promise, nextAction) => promise.then(nextAction, logger.logError),
    Promise.resolve(true)
  );

