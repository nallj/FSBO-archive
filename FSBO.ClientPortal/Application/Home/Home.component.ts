import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { User } from '../Models/index';
import { AlertService, AuthenticationService, UserService } from '../Services';
import { RouteKeys } from '../AppRoutes.config';

@Component({
	templateUrl: 'Home.component.html'
})

export class HomeComponent {

	currentUser: User;

	constructor(
		private router: Router,
		private userSvc: UserService,
		private authSvc: AuthenticationService,
		private alertSvc: AlertService
	) {

		this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
	} 

	//ngOnInit() {
	//	this.loadAllUsers();
	//}

	//deleteUser(id: number) {
	//	this.userSvc.delete(id).subscribe(() => { this.loadAllUsers() });
	//}

	//private loadAllUsers() {
	//	this.userSvc.getAll().subscribe(users => { this.users = users; });
	//}

	// also in TopNavigation
	//logOutAndRedirectToLoginPage() {

	//	// log out the user.
	//	this.authSvc.logout();

	//	// alert user that they have just logged out.
	//	this.alertSvc.info('You have logged out successfully.', true);

	//	// go to login page.
	//	this.router.navigate([RouteKeys.login]);
	//}

}