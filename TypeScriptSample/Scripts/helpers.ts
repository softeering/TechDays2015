/// <reference path="configuration.ts" />

import config = require('configuration')

export module helpers {

	export class stringHelper{
		// static method !
		static endsWith = (str:string, suffix:string) : boolean => {
			if (!str || !suffix)
				return false;

			return str.indexOf(suffix, str.length - suffix.length) !== -1;
		};
	}

	export class DynamoClient {

		private _parameters = new collections.Dictionary<string, string>(); //Generics !
		private _config = new config.configuration.configValues();

		constructor() {

		}

		private createUrl = (dataSourceCode: string) : string => {
			if (!dataSourceCode)
				throw "Invalid data source code";

			var baseUrl = config.configuration.configValues.getDynamoBaseURL();
			if (!baseUrl)
				throw "Invalid configuration";

			var url = baseUrl + (!helpers.stringHelper.endsWith(baseUrl, "/") ? "/" : "") + "odata/" + dataSourceCode;

			if (!this._parameters.containsKey("$format"))
				this._parameters.setValue("$format", "json");

			if (this._parameters.size() > 0)
				url += "?";

			this._parameters.forEach((key, value) => {
				url += key + "=" + value + "&";
			});

			return url;
		}

		loadDataSource = (code: string) : JQueryPromise<any> => {
			if (code) {
				return $.ajax({
					url: this.createUrl(code),
					accepts: 'json',
					type: 'get',
					async: true
				}).then((result) => {
						var data = [];
						if (result && result.value)
							data = result.value;

						return data;
					});
			}
			else
				return null;
		}

		addParameter = (key: string, value: string) : void => {
			if (key != undefined && value != undefined)
				this._parameters.setValue(key, value);
		}
	}
} 