const path = require('path');
const glob = require("glob");
const webpack = require('webpack');
const CleanWebpackPlugin = require('clean-webpack-plugin');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const ProgressBarPlugin = require('progress-bar-webpack-plugin');
const EncodingPlugin = require('webpack-encoding-plugin');
const OptimizeCSSAssetsPlugin = require("optimize-css-assets-webpack-plugin");
const UglifyJsPlugin = require('uglifyjs-webpack-plugin');
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;

const bundleName = 'ringor-bundle';

module.exports = bundleArguments => {
  const workingDirectory = path.resolve(__dirname, '../../');
  let commonOptions = {
    resolve: {
      modules: [
        path.resolve(workingDirectory, 'node_modules'),
        workingDirectory
      ]
    },
    plugins: [
      //new BundleAnalyzerPlugin(),
      new webpack.HashedModuleIdsPlugin(),
      new ProgressBarPlugin(),
      new EncodingPlugin({ encoding: 'utf8' })
    ],
    module: {
      rules: [
        {
          test: /\.less$/,
          use: [
            MiniCssExtractPlugin.loader,
            "css-loader",
            "less-loader"
          ]
        },
        {
          test: /.\.css$/,
          use: [
            MiniCssExtractPlugin.loader,
            "css-loader"
          ]
        },
        {
          test: /\.(js|jsx)$/,
          exclude: /node_modules/,
          use: {
            loader: "babel-loader",
            options: {
              presets: ["@babel/preset-env", "@babel/preset-react"]
            }
          }
        }
      ]
    }
  };

  let targetDirectory;
  switch (bundleArguments.configuration) {
    case "test":
      targetDirectory = path.join(workingDirectory, bundleArguments.target || '');
      commonOptions.output = {
        filename: 'tests.js',
        path: targetDirectory
      },
      commonOptions.mode = 'development';
      commonOptions.devtool = 'source-map';
      commonOptions.optimization = {
        minimize: false
      };
      commonOptions.entry = { "tests": glob.sync("./src/**/*.test.js") };
      break;
    case "debug":
      targetDirectory = path.join(workingDirectory, bundleArguments.target || '');
      commonOptions.output = {
        filename: bundleName + '.js',
        path: targetDirectory
      };
      commonOptions.mode = 'development';
      commonOptions.devtool = 'source-map';
      commonOptions.optimization = {
        minimize: false
      };
      commonOptions.plugins.push(new CleanWebpackPlugin([targetDirectory], { root: path.join(targetDirectory, '..') }));
      commonOptions.plugins.push(new MiniCssExtractPlugin({ filename: bundleName + '.css' }));
      break;
    case "release":
      targetDirectory = path.join(workingDirectory, bundleArguments.target || '');
      commonOptions.output = {
        filename: bundleName + '.js',
        path: targetDirectory
      };
      commonOptions.mode = 'production';
      commonOptions.devtool = 'nosources-source-map';
      commonOptions.optimization = {
        minimize: true,
        minimizer: [
          // we specify a custom UglifyJsPlugin here to get source maps in production
          new UglifyJsPlugin({
            cache: true,
            parallel: true,
            uglifyOptions: {
              compress: {
                properties: true,
                drop_debugger: true,
                drop_console: true,
                sequences: true,
                dead_code: true,
                conditionals: true,
                comparisons: true,
                evaluate: true,
                booleans: true,
                unused: true,
                loops: true,
                if_return: true,
                negate_iife: true,
                hoist_funs: true,
                hoist_vars: false,
                join_vars: true,
                global_defs: {}
              },
              output: {
                beautify: false,
                comments: false
              },
              beautify: false,
              mangle: {
                toplevel: true,
                eval: true
              }
            },
            sourceMap: false
          })
        ]
      };
      commonOptions.plugins.push(new CleanWebpackPlugin([targetDirectory], { root: path.join(targetDirectory, '..') }));
      commonOptions.plugins.push(new MiniCssExtractPlugin({ filename: bundleName + '.css' }));
      commonOptions.plugins.push(new OptimizeCSSAssetsPlugin({}));
      break;
  }

  return commonOptions;
};