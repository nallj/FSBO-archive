import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';

import { AppConfig } from '../App.config';
import { RegistrationData, User } from '../Models';

@Injectable()
export class UserService {
    constructor(private http: Http, private config: AppConfig) { }

    getAll() {
        return this.http.get(this.config.apiUrl + '/user', this.jwt()).map((response: Response) => response.json());
    }

	getById(id: number) {
        return this.http.get(this.config.apiUrl + '/user/' + id, this.jwt()).map((response: Response) => response.json());
    }

	create(user: RegistrationData) {
		user.sanitizeModelForSubmission();
        return this.http.post(this.config.apiUrl + '/user', user, this.jwt());
    }

    update(user: User) {
        return this.http.put(this.config.apiUrl + '/user/' + user.id, user, this.jwt());
    }

    delete(id: number) {
        return this.http.delete(this.config.apiUrl + '/user/' + id, this.jwt());
    }

    // private helper methods

    private jwt() {
        // create authorization header with jwt token
        let currentUser = JSON.parse(localStorage.getItem('currentUser'));
        if (currentUser && currentUser.token) {
            let headers = new Headers({ 'Authorization': 'Bearer ' + currentUser.token });
            return new RequestOptions({ headers: headers });
        }
    }
}