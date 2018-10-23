import { Inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observer, Observable, Subject } from 'rxjs';
import { IAction } from './Interfaces';
import { AppState } from './AppStore';
import { APP_STATE, DISPATCHER } from './Interfaces';

@Injectable()
export class StateAccessService {

    /** the current application state - do not modify the state directly */
    get currentState() { return this._currentState; }

    private _currentState: AppState;

    /** property paths whose parts have been converted into an array of strings */
    private parsedPaths: { [path: string]: string[] };

    /** as paths to variables are checked the change detection results */
    private pathResults: { [path: string]: boolean };

    /** the appState from the previous onStateUpdated execution */
    private priorState: AppState;

    /** Obserable for when updates occur. */
    // if this isn't behavior subject, subscriber would have to wait for change to get the initialState
    private stateDetectionStateUpdateSubject: BehaviorSubject<any>;

    /**
     * Creates a new instance of the service
     * @param appState Observable containing application state
     * @param dispatcher Dispatcher that sends actions to the store
     */
    constructor(
        @Inject(APP_STATE) private appStateStore: BehaviorSubject<AppState>,
        @Inject(DISPATCHER) private dispatcher: Observer<IAction>) {

        this._currentState = this.appStateStore.value;
        this.parsedPaths = {};
        this.stateDetectionStateUpdateSubject = new BehaviorSubject<any>(undefined);
        this.appStateStore.subscribe(s => this.onStateUpdated(s));
    }

    /**
     * Dispatches a new action
     * @param action the IAction to be dispatched
     */
    next(action: IAction) {
        this.dispatcher.next(action);
    }

    /**
     * overrides the app state with the passed state.  Good for reinitialize the state, won't trigger any events
     */
    initState(state: AppState) {
        this.appStateStore.next(state);
    }
    /**
     * subscribe to get notified when there are any state changes along the path or paths
     * @param pathOrPaths either a string to one path or an array to multiple paths to check.
     * If nothing is provided then the appState is returned directly.
     */
    watch(pathOrPaths?: string | string[]): Observable<AppState> {
        if (!pathOrPaths) return this.appStateStore;

        let paths = this.processWatchPaths(pathOrPaths);

        // map func will always return the first set value, after that it will return values only if changes occur
        // var is used on purpose here
        var mapFunc = () => {
            if (this._currentState !== undefined) {
                mapFunc = () => {
                    if (this.detectChanges(paths)) {
                        return this._currentState;
                    }
                }
                return this._currentState;
            }
        };

        let subj = this.stateDetectionStateUpdateSubject
            .map(() => mapFunc())
            .filter(x => x !== undefined);

        return subj;
    }

    /**
     * Resets global processing variables and calls next on stateDetectionStateUpdateSubject
     * @param state
     */
    private onStateUpdated(state: AppState) {
        this.priorState = this._currentState;
        this._currentState = state;
        this.pathResults = {};

        this.stateDetectionStateUpdateSubject.next(undefined);

    }

    /**
     * finds any changes along tracked paths
     */
    private detectChanges(paths: string[]) {

        for (let path of paths) {

            if (this.pathResults[path] == null) {
                let priorValue = this.getObjValue(this.priorState, path);
                let currentValue = this.getObjValue(this._currentState, path);
                let result = this.pathResults[path] = (priorValue !== currentValue);
                return result;
            }
            else if (this.pathResults[path]) {
                return true;
            }
        }
        return false;
    }

    /**
     * gets an object value from a path string.
     */
    private getObjValue(obj: any, path: string) {
        if (obj == null) return obj;
        let pathParts = this.parsedPaths[path];
        for (let part of pathParts) {
            obj = obj[part];
            if (obj == null) break;
        }
        return obj;
    }

    /**
     * Adds any new paths to parsedPaths and returns pathOrPaths as an array of individual paths.
     */
    private processWatchPaths(pathOrPaths: string | string[]) {

        let paths = Array.isArray(pathOrPaths) ? pathOrPaths : [pathOrPaths];

        for (let path of paths) {
            if (!this.parsedPaths[path]) {
                this.parsedPaths[path] = path.split('.');
            }
        }

        return paths;
    }
}