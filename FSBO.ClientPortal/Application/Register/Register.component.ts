import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { RegistrationData } from '../Models/index';
import { AlertService, UserService } from '../Services/index';
import { RouteKeys } from '../AppRoutes.config';

@Component({
	templateUrl: 'Register.component.html'
})

export class RegisterComponent {

	model: RegistrationData;
	routeKeys: any;
	loading: boolean;

	constructor(
		private router: Router,
		private userSvc: UserService,
		private alertSvc: AlertService
	) {

		// if the user is already logged in, redirect to the home page.
		if (localStorage.getItem('currentUser')) {
			this.router.navigate([RouteKeys.home]);
		}

		this.model = new RegistrationData();
		this.routeKeys = RouteKeys;
		this.loading = false;
	}

	register() {
		this.loading = true;

		this.userSvc.create(this.model)
			.subscribe(
				data => {
					this.alertSvc.success('Registration successful', true);
					this.router.navigate([RouteKeys.login]);
				},
				error => {
					this.alertSvc.error(error._body);
					this.loading = false;
				}
			);
	}
}
