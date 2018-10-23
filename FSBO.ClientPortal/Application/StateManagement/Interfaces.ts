import { OpaqueToken } from '@angular/core';
import { BehaviorSubject, Observer, Observable, Subject } from 'rxjs';

export interface IAction {
    /** The key of an action should equal a static key variable set on the action.
        This how the reducer detects the right action.
        Make sure all keys in app are distict per action.
        DO NOT USE class.name or override it with a static variable.  Otherwise minification will mess things up.
    */
    key: string;
}

/** Token for injecting initial app state */
export const APP_INIT_STATE = new OpaqueToken("appInitState");

/** Token for refrencing dispatcher */
export const DISPATCHER = new OpaqueToken("dispatcher");

/** Token for appState */
export const APP_STATE = new OpaqueToken("appState");