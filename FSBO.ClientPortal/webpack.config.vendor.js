var isDevBuild = process.argv.indexOf('--env.prod') < 0;
var path = require('path');
var webpack = require('webpack');
var ExtractTextPlugin = require('extract-text-webpack-plugin');

module.exports = {
	resolve: {
		//extensions: ['', '.js']
		extensions: ['.js']
	},
	module: {
		rules: [
			{
				test: /\.(png|woff|woff2|eot|ttf|svg)(\?|$)/,
				use: [{
					loader: 'url-loader?limit=100000'
				}]
			},
			{
				test: /\.css(\?|$)/,
				use: ExtractTextPlugin.extract({ use: 'css-loader' })
			}
		]
	},
	entry: {
		vendor: [
			'@angular/common',
			'@angular/compiler',
			'@angular/core',
			'@angular/forms',
			'@angular/http',
			'@angular/platform-browser',
			'@angular/platform-browser-dynamic',
			'@angular/router',
			'angular2-jwt',
			'applicationinsights-js',
			'bootstrap',
			'bootstrap/dist/css/bootstrap.css',
			'core-js',
			'font-awesome/css/font-awesome.css',
			'es6-promise',
			'jquery',
			'rxjs',
			//'@tcc/ui',
			'zone.js'
		]
	},
	output: {
		path: path.join(__dirname, 'wwwroot', 'dist'),
		filename: '[name].js',
		library: '[name]_[hash]',
	},
	plugins: [
		new ExtractTextPlugin({ filename: "vendor.css", disable: false, allChunks: true }),
		new webpack.ProvidePlugin({ $: 'jquery', jQuery: 'jquery' }), // Maps these identifiers to the jQuery package (because Bootstrap expects it to be a global variable)
		new webpack.ProvidePlugin({ _: 'underscore', 'underscore': 'underscore' }),
		new webpack.optimize.OccurrenceOrderPlugin(),
		new webpack.DllPlugin({
			path: path.join(__dirname, 'wwwroot', 'dist', '[name]-manifest.json'),
			name: '[name]_[hash]'
		})
	].concat(isDevBuild ? [] : [
		new webpack.optimize.OccurrenceOrderPlugin(),
		new webpack.optimize.UglifyJsPlugin({ compress: true, sourceMap: true, comments: false }),
		new webpack.optimize.DedupePlugin()
	])
};