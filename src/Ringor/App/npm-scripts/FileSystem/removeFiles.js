const fs = require("fs");
const logger = require("../logger");
const glob = require("glob");

const removeSingleFile = file => new Promise((resolve, reject) => {
  fs.unlink(file, error => {
    if (error) {
      logger.logError(error);
      reject(error);
    }
    resolve();
  });
});

module.exports = filePattern => {
  logger.logStart("Removing " + filePattern);
  return new Promise((resolve, reject) => {
    glob(filePattern, (findFilesError, files) => {
      if (findFilesError) {
        logger.logError(findFilesError);
        reject(findFilesError);
      }
      const removeFilePromises = files.map(removeSingleFile);
      Promise.all(removeFilePromises).then(() => {
          logger.logFinish("OK Finished removing every file that matched " + filePattern);
          resolve();
        },
        reject);
    });
  });
};
