import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Http, HttpModule } from '@angular/http';
import { AuthHttp } from 'angular2-jwt';
import { RouterModule } from '@angular/router';

//import { AppInsightsService, EnvironmentSettings, ErrorService, LoadingOverlayComponent } from './shared';
//import { TccUiModule } from '@tcc/ui';

import * as app from './';
import * as stateMgmt from './StateManagement/';
//import { API_BASE_URL } from './ApiClient.service';

@NgModule({
	imports: [
		BrowserAnimationsModule,
		BrowserModule,
		HttpModule,
		ReactiveFormsModule,
		FormsModule, // for ngModels everywhere.
		RouterModule.forRoot(app.AppRoutesConfig.getConfig())
	],
	providers: [
		app.AlertService,
		app.AppConfig,
		app.AuthenticationService,
		app.AuthGuard,
		app.UserService
		/*app.AppAuthService,
		AppInsightsService,
		{
			provide: AuthHttp,
			useFactory: app.AppAuthHttpFactories.createBearerAuthHttpService,
			deps: [Http, app.AppAuthService] // dependencies required by the factor
		},
		{
			// the api root is provided by EnvironmentSettings and then returned as API_BASE_URL which
			// is in turn used by the ClientApi service
			provide: API_BASE_URL,
			useValue: EnvironmentSettings.apiPath
		},
		stateMgmt.DispatcherProvider,
		stateMgmt.AppInitialStateProvider,
		stateMgmt.AppStateProvider,
		stateMgmt.StateAccessService,
		app.ApiClient,
		ErrorService,
		{ provide: app.IWorkflowService, useClass: app.WorkflowService },
		{ provide: app.IFormService, useClass: app.FormService }*/
	],
	declarations: [
		app.AccountComponent,
		app.AlertComponent,
		app.AppComponent,
		app.FeedbackComponent,
		app.HomeComponent,
		app.ListingsComponent,
		app.LoginComponent,
		app.MainNavigationComponent,
		app.PrivacyPolicyComponent,
		app.RegisterComponent,
		app.TermsOfServiceComponent,
		app.TopNavigationComponent,
		app.WalletComponent
	],
	bootstrap: [app.AppComponent]
})
export class AppModule { }