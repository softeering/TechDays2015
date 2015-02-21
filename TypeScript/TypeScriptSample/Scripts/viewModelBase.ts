/// <reference path="typings/knockout/knockout.d.ts" />

import ko = require('knockout')

export class viewModelBase {

	// different accessors private, public, protected
	private _clearErrorMessageTimeOut: number = null;
	public errorMessage: KnockoutObservable<string> = ko.observable(null);
	public loading: KnockoutObservable<boolean> = ko.observable(false);

	constructor() {

	}

	protected logError = (message: string): void => {
		if (!message)
			return;

		if (this._clearErrorMessageTimeOut)
			clearTimeout(this._clearErrorMessageTimeOut);

		this.errorMessage(null);
		this.errorMessage(message);

		this._clearErrorMessageTimeOut = setTimeout(() => { this.errorMessage(null); }, 5000);
	}

	protected baseInit() { }
}
