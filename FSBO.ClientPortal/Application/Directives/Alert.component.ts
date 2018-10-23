import { Component, OnInit } from '@angular/core';

import { AlertService } from '../Services';

@Component({
	selector: 'alert',
	templateUrl: 'Alert.component.html'
})

export class AlertComponent {

	message: any;

	constructor(private alertSvc: AlertService) { }

	ngOnInit() {
		this.alertSvc.getMessage()
			.subscribe(message => { this.message = message; });
	}
}