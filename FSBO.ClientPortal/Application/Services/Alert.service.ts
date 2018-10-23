import { Injectable } from '@angular/core';
import { Router, NavigationStart } from '@angular/router';
import { Observable } from 'rxjs';
import { Subject } from 'rxjs/Subject';

@Injectable()
export class AlertService {
    private subject = new Subject<any>();
    private keepAfterNavigationChange = false;

    constructor(private router: Router) {
        // clear alert message on route change
        router.events.subscribe(event => {
            if (event instanceof NavigationStart) {
                if (this.keepAfterNavigationChange) {
                    // only keep for a single location change
                    this.keepAfterNavigationChange = false;
                } else {
                    // clear alert
                    this.subject.next();
                }
            }
        });
    }

	success(message: string, keepAfterNavigationChange = false) {
		this.messageOfType('success', message, keepAfterNavigationChange);
	}

	info(message: string, keepAfterNavigationChange = false) {
		this.messageOfType('info', message, keepAfterNavigationChange);
	}

    error(message: string, keepAfterNavigationChange = false) {
		this.messageOfType('error', message, keepAfterNavigationChange);
	}

	private messageOfType(type: string, message: string, keepAfterNavigationChange: boolean) {

		this.keepAfterNavigationChange = keepAfterNavigationChange;
		this.subject.next({ type: type, text: message });
	}

    getMessage(): Observable<any> {
        return this.subject.asObservable();
    }
}