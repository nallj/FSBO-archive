export class RegistrationData {
	
	username: string;
	password: string;

	email: string;
	phone: string;
	
	firstZip: number;
	dayToCharge: number;

	paymentName: string;
	ccName: string;
	ccNumber: number;
	expMonth: number
	expYear: number;

	readTermsOfService: boolean;
	readPrivacyPolicy: boolean;

	constructor() {

		this.username = this.password = this.email = this.phone = this.paymentName = this.ccName = '';
		this.firstZip = this.ccNumber = null;
		this.dayToCharge = 1;
		this.readPrivacyPolicy = this.readTermsOfService = false;
	}

	sanitizeModelForSubmission() {

		this.phone = this.removeAllNonNumericCharacters(this.phone);
		this.firstZip = parseInt(this.removeAllNonNumericCharacters(this.firstZip));
		this.ccNumber = parseInt(this.removeAllNonNumericCharacters(this.ccNumber));

		if (!this.paymentName) {
			this.paymentName = null;
		}
	}

	private removeAllNonNumericCharacters(str: any): string {
		console.log('replacing', str);
		return str.toString().replace(/\D/g, '');
	}

}