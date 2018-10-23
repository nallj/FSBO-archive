import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { AlertService, AuthenticationService } from '../Services';
import { RouteKeys } from '../AppRoutes.config';

@Component({
	selector: 'top-navigation',
	templateUrl: 'TopNavigation.component.html',
	styles: [
		'#top-navigation { background:#333; margin-bottom:30px }',
		'#company-logo { width:100px; height:50px; background:url(/images/logo-placeholder.png) no-repeat; background-size:contain }',
		'@media (max-width: 767px) { #company-logo { margin-left:20px } }',
		'ul { padding-top:15px; list-style-type:none }',
		'@media (max-width: 767px) { ul { margin-right:20px } }',
		'li { float:right; margin-left:15px }',
		'li:last-child { margin-left:0 }',
		'li.feedback-link a { color:#fed80b }',
		'li.logout-link a { cursor:pointer }'
	]
})
export class TopNavigationComponent {

	routeKeys: any;

	constructor(
		private router: Router,
		private authSvc: AuthenticationService,
		private alertSvc: AlertService
	) {

		this.routeKeys = RouteKeys;
	}

	
	logOutAndRedirectToLoginPage() {

		// log out the user.
		this.authSvc.logout();

		// alert user that they have just logged out.
		this.alertSvc.info('You have logged out successfully.', true);

		// go to login page.
		this.router.navigate([RouteKeys.login]);
	}

}
