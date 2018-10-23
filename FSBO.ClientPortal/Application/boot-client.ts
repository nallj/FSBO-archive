
// required imports for Angular (must be imported first).
import 'core-js/client/shim';
import 'reflect-metadata';
import 'zone.js';

import { DebugElement, ReflectiveInjector, enableProdMode, NgModuleRef, PlatformRef } from '@angular/core';
import { ValueProvider } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from './App.module';
//import * as stateMgmt from './StateManagement/';
//import { APP_INIT_STATE } from './StateManagement/Interfaces';


let platform: PlatformRef = platformBrowserDynamic();
let bootApplication: () => Promise<NgModuleRef<AppModule>>;

// Enable either Hot Module Reloading or production mode
if (module['hot']) {
	const stateCacheKey = '__HMR_SAVED_STATE';
	var appModuleInjector: NgModuleRef<AppModule>;

	bootApplication = () => {
		//let providers = [];
		//let savedStateRaw = sessionStorage.getItem(stateCacheKey);
		//if (savedStateRaw) {
		//    sessionStorage.removeItem(stateCacheKey);
		//    let savedState: stateMgmt.AppState = JSON.parse(savedStateRaw);
		//    providers.push(<ValueProvider>{
		//        provide: APP_STATE,
		//        useValue: savedState
		//    });
		//}
		//return platform.bootstrapModule(AppModule, { providers: providers });
		return platform.bootstrapModule(AppModule);
	};

	let parentElem = document.getElementsByTagName("app")[0].parentElement;
	let hmr = module['hot'];
	hmr.accept();

	// NOTE: in the webpack documentation, it is stronly insinuated that disposing of the module is unnecessary unless the module produces "side-effects."  it provides no definition or explanation for what those are.
	// explicitly disposing causes the 'my-app' node to disappear from the DOM before it can be swapped.
	hmr.dispose(() => {
		if (appModuleInjector) {
			//let appElem: DebugElement = (<any>window).ng.probe(document.getElementsByTagName("my-app")[0]);
			//// if in an error occurred in the app didn't boot properly appModuleInjector will be undefined
			//let stateAccessSvc = appModuleInjector.injector.get(stateMgmt.StateAccessService);
			//sessionStorage.setItem(stateCacheKey, JSON.stringify(stateAccessSvc.currentState));
		}

		platform.destroy();

		// to fix this, insert a new 'my-app' node into the DOM so the application will be recreated without issue.
		let appRootElem = document.createElement('app');
		parentElem.appendChild(appRootElem);
	});
} else {
	enableProdMode();
	bootApplication = () => platform.bootstrapModule(AppModule);
}

if (document.readyState === 'complete') {
	bootApplication();
} else {
	document.addEventListener('DOMContentLoaded', bootApplication);
}
