// Karma configuration
// Generated on Thu Dec 01 2016 12:24:13 GMT-0500 (Eastern Standard Time)

var webpackConfig = require('./webpack.config');

module.exports = function (config) {
	config.set({

		// base path that will be used to resolve all patterns (eg. files, exclude)
		basePath: '',


		// frameworks to use
		// available frameworks: https://npmjs.org/browse/keyword/karma-adapter
		frameworks: ['mocha', 'chai', 'sinon'], // 'sinon-chai', 'karma-typescript'

		// karma - test runner
		// mocha - test framework
		// chai - bdd/tdd (behavior/test driven development) assertion library (should, expect, and assert)
		// sinon - standalone test spies, stubs, and mocks


		// list of files / patterns to load in the browser
		files: [
			//'node_modules/es6-shim/es6-shim.js', // Need this shim to support Map() function in PhantomJS.
			'node_modules/rxjs/bundles/Rx.js',
			'node_modules/@angular/core/bundles/core.umd.js',
			'node_modules/@angular/common/bundles/common.umd.js',
			'node_modules/@angular/forms/bundles/forms.umd.js',
			'node_modules/@angular/http/bundles/http.umd.js',
			'node_modules/@angular/router/bundles/router.umd.js',
			'node_modules/reflect-metadata/Reflect.js',
			'tests/*.ts'
		],


		// list of files to exclude
		exclude: [
		],


		// preprocess matching files before serving them to the browser
		// available preprocessors: https://npmjs.org/browse/keyword/karma-preprocessor
		preprocessors: {
			'tests/*.ts': ['webpack']
		},


		// needed for Chrome to run the tests.
		mime: {
			'text/x-typescript': ['ts', 'tsx']
		},


		webpack: {
			module: webpackConfig.module,
			resolve: webpackConfig.resolve
		},


		// test results reporter to use
		// possible values: 'dots', 'progress'
		// available reporters: https://npmjs.org/browse/keyword/karma-reporter
		reporters: ['progress'], // 'karma-typescript'


		// web server port
		port: 9876,


		// enable / disable colors in the output (reporters and logs)
		colors: true,


		// level of logging
		// possible values: config.LOG_DISABLE || config.LOG_ERROR || config.LOG_WARN || config.LOG_INFO || config.LOG_DEBUG
		logLevel: config.LOG_INFO,


		// enable / disable watching file and executing tests whenever any file changes
		autoWatch: true,


		// start these browsers
		// available browser launchers: https://npmjs.org/browse/keyword/karma-launcher
		browsers: ['Chrome'],//['PhantomJS', 'IE'],


		// Continuous Integration mode
		// if true, Karma captures browsers, runs the tests and exits
		singleRun: false,

		// Concurrency level
		// how many browser should be started simultaneous
		concurrency: Infinity
	})
}