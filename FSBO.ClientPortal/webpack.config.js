var isDevBuild = process.argv.indexOf('--env.prod') < 0;
var path = require('path');
var webpack = require('webpack');
var nodeExternals = require('webpack-node-externals');
var merge = require('webpack-merge');
var allFilenamesExceptJavaScript = /\.(?!js(\?|$))([^.]+(\?|$))/;

// Configuration in common to both client-side and server-side bundles
var sharedConfig = {
	//resolve: { extensions: ['', '.js', '.ts'] },
	resolve: { extensions: ['.js', '.ts'] },
	output: {
		filename: '[name].js',
		publicPath: '/dist/' // Webpack dev middleware, if enabled, handles requests for this URL prefix
	},
	module: {
		rules: [
			{
				test: /\.ts$/,
				include: /Application/,
				exclude: /node_modules/,
				use: [
					{ loader: 'awesome-typescript-loader', },//options: { silent: true }  },
					{ loader: 'angular2-template-loader' }
				]
			},
			{
				test: /\.html$/,
				use: { loader: 'raw-loader' }
			},
			{
				test: /\.css$/,
				use: [
					{ loader: 'to-string-loader' },
					{ loader: 'css-loader' }
				]
			},
			{
				test: /\.(png|jpg|jpeg|gif|svg)$/,
				use: { loader: 'url-loader', query: { limit: 25000 } }
			}
		]
	}
};

// Configuration for client-side bundle suitable for running in browsers
var clientBundleConfig = merge(sharedConfig, {
	entry: { 'main-client': './Application/boot-client.ts' },
	output: { path: path.join(__dirname, './wwwroot/dist') },
	//debug: isDevBuild,
	devtool: 'source-map',

	plugins: [
		new webpack.DllReferencePlugin({
			context: __dirname,
			manifest: require('./wwwroot/dist/vendor-manifest.json')
		})
	].concat(isDevBuild ? [] : [
		// Plugins that apply in production builds only
		new webpack.optimize.OccurrenceOrderPlugin(),
		new webpack.optimize.UglifyJsPlugin({ compress: true, sourceMap: true, comments: false }),

		// this was not in the original ng2-ASP.Core template.
		//new webpack.optimize.DedupePlugin() 
	])
});

module.exports = [clientBundleConfig];

// we're not using server-side prerendering so this can be safely left out.
//, serverBundleConfig];

// Configuration for server-side (prerendering) bundle suitable for running in Node
/*var serverBundleConfig = merge(sharedConfig, {
    entry: { 'main-server': './app/boot-server.ts' },
    output: {
        libraryTarget: 'commonjs',
        path: path.join(__dirname, './app/dist')
    },
    target: 'node',
    devtool: 'inline-source-map',
    externals: [nodeExternals({ whitelist: [allFilenamesExceptJavaScript] })] // Don't bundle .js files from node_modules
});*/