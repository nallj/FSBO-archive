import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { AlertService, AuthenticationService } from '../Services/index';
import { RouteKeys } from '../AppRoutes.config';

@Component({
	templateUrl: 'Login.component.html',
	styles: [
		'.row { margin-top:30px }'
	]
})

export class LoginComponent implements OnInit {

	model: any;
	routeKeys: any;
	loading: boolean;
	returnUrl: string;

	constructor(
		private route: ActivatedRoute,
		private router: Router,
		private authenticationService: AuthenticationService,
		private alertService: AlertService) { }

	ngOnInit() {
		// reset login status
		//this.authenticationService.logout();

		// if the user is already logged in, redirect to the home page.
		if (localStorage.getItem('currentUser')) {
			this.router.navigate([RouteKeys.home]);
		}

		this.model = {};
		this.routeKeys = RouteKeys;
		this.loading = false;

		// get return url from route parameters or default to '/'
		this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
	}

	login() {
		this.loading = true;
		this.authenticationService.login(this.model.username, this.model.password)
			.subscribe(
			data => {
				this.router.navigate([this.returnUrl]);
			},
			error => {
				this.alertService.error('Username or password is incorrect');
				this.loading = false;
			});
	}
}
