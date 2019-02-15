const commandLineArgs = require('command-line-args');
const removeFolders = require("./FileSystem/removeFolders");
const logger = require("./logger");
const nodeModulesFolder = "./node_modules";
const buildOutputFolder = "../wwwroot/App";

const options = commandLineArgs([
  { name: 'target', alias: 't', type: String, multiple: true }
]);

const cleanArguments = {
  targets: options.target || ["build-output"]  // by default, clean build output
};

const getAction = target => {
  switch (target) {
    case "build-output":
      return removeFolders([buildOutputFolder]);
    case "node-modules":
      return removeFolders([nodeModulesFolder]);
  }
};

cleanArguments.targets
  .map(getAction)
  .reduce(
    (promise, nextAction) => promise.then(nextAction, logger.logError),
    Promise.resolve(true)
  );

