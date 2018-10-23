import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { AlertService, AuthenticationService } from '../Services';
import { RouteKeys } from '../AppRoutes.config';
import { MainNavigationLink } from '../Models';

@Component({
	selector: 'main-navigation',
	templateUrl: 'MainNavigation.component.html',
	styles: [
		'@media (max-width: 767px) { #main-navigation { background:#ccc } }',
		
		'ul { display:flex; flex-direction:row; height:50px; margin:0; padding:15px 0 0 0; list-style-type:none; background:#ccc }',
		'li { flex-grow:1; flex- basis:0; padding:2px 0; text-align:center }' // float:left;
	]
})
export class MainNavigationComponent {
	
	mainNavigationLinks: MainNavigationLink[];

	constructor(
		private router: Router,
		private authSvc: AuthenticationService,
		private alertSvc: AlertService
	) {

		this.mainNavigationLinks = [
			new MainNavigationLink(RouteKeys.home, 'Overview'),
			new MainNavigationLink(RouteKeys.listings, 'Listings'),
			new MainNavigationLink(RouteKeys.wallet, 'Wallet'),
			new MainNavigationLink(RouteKeys.account, 'Account')
		];
	}

}
