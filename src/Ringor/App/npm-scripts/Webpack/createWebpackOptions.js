const path = require('path');
const webpack = require('webpack');
const CleanWebpackPlugin = require('clean-webpack-plugin');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const ProgressBarPlugin = require('progress-bar-webpack-plugin');
const OptimizeCSSAssetsPlugin = require("optimize-css-assets-webpack-plugin");
const TerserPlugin = require('terser-webpack-plugin');
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;

const bundleName = 'ringor-bundle';

module.exports = bundleArguments => {
  const workingDirectory = path.resolve(__dirname, '../../');
  const targetDirectory = path.join(workingDirectory, bundleArguments.target || '');
  const sourceDirectory = path.join(workingDirectory, 'src');

  let commonOptions = {
    entry: path.join(sourceDirectory, 'index.tsx'),
    output: {
      filename: bundleName + '.js',
      path: targetDirectory
    },
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
      new CleanWebpackPlugin(),
      new MiniCssExtractPlugin({ filename: bundleName + '.css' })
    ],
    module: {
      rules: [
        {
          test: /\.less$/,
          use: [
            MiniCssExtractPlugin.loader,
            'css-loader',
            "less-loader"
          ]
        },
        {
          test: /.\.css$/,
          use: [
            MiniCssExtractPlugin.loader,
            'css-loader'
          ]
        }
      ]
    }
  };

  switch (bundleArguments.configuration) {
    case "debug":
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
      break;
    case "release":
      commonOptions.mode = 'production';
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
            }
          }),
        ]
      };
      commonOptions.module.rules.push({
        test: /\.(ts|tsx)?$/,
        loader: 'ts-loader',
        options: { configFile: 'tsconfig.release.json' },
        exclude: [/node_modules/, /\.test\.(ts|tsx)?$/]
      });
      commonOptions.plugins.push(new OptimizeCSSAssetsPlugin({}));
      break;
  }

  return commonOptions;
};