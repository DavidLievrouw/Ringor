const path = require('path');
const glob = require("glob");
const webpack = require('webpack');
const CleanWebpackPlugin = require('clean-webpack-plugin');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const ProgressBarPlugin = require('progress-bar-webpack-plugin');
const EncodingPlugin = require('webpack-encoding-plugin');
const OptimizeCSSAssetsPlugin = require("optimize-css-assets-webpack-plugin");
const TerserPlugin = require('terser-webpack-plugin');
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;

const bundleName = 'ringor-bundle';

module.exports = bundleArguments => {
  const workingDirectory = path.resolve(__dirname, '../../');
  let commonOptions = {
    resolve: {
      modules: [
        path.resolve(workingDirectory, 'node_modules'),
        workingDirectory
      ],
      extensions: [ '.tsx', '.ts', '.js' ]
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
      commonOptions.entry = { "tests": glob.sync("./src/**/*.test.ts{x,}") };
      commonOptions.module.rules.push({
        test: /\.(ts|tsx)?$/,
        loader: 'ts-loader',
        options: { configFile: 'tsconfig.json' },
        exclude: [/node_modules/, /\.test\.(ts|tsx)?$/]
      });
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
      commonOptions.module.rules.push({
        test: /\.(ts|tsx)?$/,
        loader: 'ts-loader',
        options: { configFile: 'tsconfig.json' },
        exclude: [/node_modules/, /\.test\.(ts|tsx)?$/]
      });
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
          new TerserPlugin({
            terserOptions: {
              compress: {
                drop_console: true
              },
              output: {
                comments: false
              },
              mangle: {
                toplevel: true,
                eval: true
              }
            },
          }),
        ]
      };
      commonOptions.module.rules.push({
        test: /\.(ts|tsx)?$/,
        loader: 'ts-loader',
        options: { configFile: 'tsconfig.release.json' },
        exclude: [/node_modules/, /\.test\.(ts|tsx)?$/]
      });
      commonOptions.plugins.push(new CleanWebpackPlugin([targetDirectory], { root: path.join(targetDirectory, '..') }));
      commonOptions.plugins.push(new MiniCssExtractPlugin({ filename: bundleName + '.css' }));
      commonOptions.plugins.push(new OptimizeCSSAssetsPlugin({}));
      break;
  }

  return commonOptions;
};