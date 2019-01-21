require("colors");

module.exports = {
  logError : error => console.error(">> " + error),
  logStart : message => console.log(">> " + message.cyan),
  logFinish : message => console.log(">> " + message.green)
};