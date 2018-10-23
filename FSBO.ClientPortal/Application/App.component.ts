import { Component, Inject, OnInit } from '@angular/core';
//import { AppInsightsService } from './shared/AppInsights.service';
//import * as stateMgmt from './StateManagement/';

@Component({
	selector: 'app',
	template: `

<top-navigation *ngIf="isUserLoggedIn()"></top-navigation>

<div class="container">
	
    <alert></alert>
	<main-navigation *ngIf="isUserLoggedIn()"></main-navigation>
    <router-outlet></router-outlet>
</div>
`
})
export class AppComponent { //implements OnInit {

	constructor() { //private aiSvc: AppInsightsService, private stateAccessSvc: stateMgmt.StateAccessService) {

		//this.aiSvc.startAi();
	}

	/*ngOnInit() {
		this.stateAccessSvc.next(new stateMgmt.NotificationAddedAction({ message: 'Welcome to Workflow Master.' }));
	}*/


	isUserLoggedIn(): boolean {
		let userCredentials = localStorage.getItem('currentUser');
		return (userCredentials != null);
	}

}