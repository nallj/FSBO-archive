import { Routes } from '@angular/router';
import * as app from './index';

export const RouteKeys = {
	account: 'account',
	feedback: 'feedback',
	home: '',
	listings: 'listings',
	login: 'login',
	privacyPolicy: 'privacy',
	register: 'register',
	termsOfService: 'tos',
	wallet: 'wallet'
};

export class AppRoutesConfig {

	// return route config. 
	static getConfig(): Routes {

		return <Routes>[
			{ path: RouteKeys.account, component: app.AccountComponent, canActivate: [app.AuthGuard] },
			{ path: RouteKeys.feedback, component: app.FeedbackComponent },
			{ path: RouteKeys.home, component: app.HomeComponent, canActivate: [app.AuthGuard] },
			{ path: RouteKeys.listings, component: app.ListingsComponent, canActivate: [app.AuthGuard] },
			{ path: RouteKeys.login, component: app.LoginComponent },
			{ path: RouteKeys.privacyPolicy, component: app.PrivacyPolicyComponent },
			{ path: RouteKeys.register, component: app.RegisterComponent },
			{ path: RouteKeys.termsOfService, component: app.TermsOfServiceComponent },
			{ path: RouteKeys.wallet, component: app.WalletComponent, canActivate: [app.AuthGuard] },
			{ path: '**', redirectTo: RouteKeys.home }
		];
	}
}