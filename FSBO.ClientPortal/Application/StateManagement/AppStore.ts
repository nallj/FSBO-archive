import { FactoryProvider, OpaqueToken, ValueProvider } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { APP_INIT_STATE, APP_STATE, DISPATCHER, IAction } from './Interfaces';

//import * as notification from './NotificationReducer';
//import * as other from './OtherReducer';


// sources
// https://dzone.com/articles/managing-state-in-angular-2-applications-1
// http://blog.angular-university.io/angular-2-application-architecture-building-applications-using-rxjs-and-functional-reactive-programming-vs-redux/

/** our application state */
export interface AppState {

	//notifications: notification.NotificationState;
	//other: other.OtherState;
}

/**
 * This never changes.  Needs to be added to a module
 */
export const DispatcherProvider: FactoryProvider = {
	provide: DISPATCHER,
	useFactory: () => new Subject<IAction>()
};
/**
 * The initial state for the app - needs to be added to a module for AppStateProvider
 */
export const AppInitialStateProvider: ValueProvider = {
	provide: APP_INIT_STATE,
	useValue: <AppState>{
		notifications: {
			notifications: []
		},
		other: {
			activeBlades: []
		}
	}
};
/**
 * Provider for AppState - needs to be added to a module
 */
export const AppStateProvider: FactoryProvider = {
	provide: APP_STATE,
	useFactory: appStateFactory,
	deps: [APP_INIT_STATE, DISPATCHER]
}

/**
 * return an observable that is called every time the dispatcher adds an action and returns a new state
 */
function appStateFactory(initalState: AppState, dispatcher: Observable<IAction>) {

	let appStateObservable = dispatcher.scan((state: AppState, action: IAction) => {
		let newState: AppState = {
			//notifications: notification.notificationReducer(state.notifications, action),
			//other: other.otherReducer(state.other, action)
		};
		console.info('%c State updated by %c%s%c: %o %o', 'color:green', 'color:#052;font-weight:bold', action.key, 'color:green', action, newState);
		return newState;
	}, initalState).share();

	return wrapIntoBehaviorSubject(initalState, appStateObservable);
}

function wrapIntoBehaviorSubject<T>(init: T, obs: Observable<T>) {
	const res = new BehaviorSubject<T>(init);
	obs.subscribe(s => res.next(s));
	return res;
}