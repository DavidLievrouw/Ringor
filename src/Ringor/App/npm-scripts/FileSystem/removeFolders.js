const rimraf = require("rimraf");
const logger = require("../logger");

const removeSingleFolder = folderName => {
  logger.logStart("Removing " + folderName);
  return new Promise((resolve, reject) => rimraf(folderName, error => {
    if (error) {
      logger.logError(error);
      reject(error);
    } else {
      logger.logFinish("OK Finished removing " + folderName);
      resolve();
    }
  }));
};

module.exports = folderNames => {
  return new Promise(function (resolve, reject) {
    const removeFolderPromises = folderNames.map(removeSingleFolder);
    Promise.all(removeFolderPromises).then(resolve, reject);
  });
};
