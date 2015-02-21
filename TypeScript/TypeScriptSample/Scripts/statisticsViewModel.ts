/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="typings/knockout/knockout.d.ts" />
/// <reference path="typings/knockout.mapping/knockout.mapping.d.ts" />
/// <amd-dependency path="/Scripts/libs/collections.js" />
/// <reference path="typings/numeraljs/numeraljs.d.ts" />
/// <amd-dependency path="numeral" />
/// <reference path="helpers.ts" />

import ko = require('knockout')
import $ = require('jquery')
import helpers = require('helpers')
import mapping = require('knockout.mapping')
import vm = require('viewModelBase')

export module viewModels {
									 // Class inheritance
	export class statisticsViewModel extends vm.viewModelBase {
		
		//#region Properties

		public statistics: KnockoutObservableArray<any> = ko.observableArray();

		public numberOfDataSources: KnockoutComputed<number> = ko.computed(() => {
			return this.statistics().length;
		}, this); // if computed are defined as properties, the context (this) needs to be passed

		public numberOfCalls: KnockoutComputed<string> = ko.computed(() => {
			return numeral(this.statistics().reduce((prev, current, index) => {
				return prev += current.NumberOfCalls || 0;
			}, 0)).format("0,0"); //format can be done in the view, just for testing purpose
		}, this); 

		//#endregion

		//#region Ctor

		constructor() {
			super();
		}

		//#endregion

		//#region Private Methods

		private loadData = () => {
			var client = new helpers.helpers.DynamoClient();
			client.addParameter("$orderby", "NumberOfCalls desc");

			this.loading(true);

			client.loadDataSource("DynamoStatistic")
				.done((result) => {
					ko.utils.arrayPushAll<any>(this.statistics, result);
				})
				.fail(() => {
					//base method
					this.logError("An error occurred while loading the data, maybe you don't have access to Dynamo (are you connected to Expedia's network ?)");
				})
				.always(() => {
					this.loading(false);
				});
		}

		//#endregion

		//#region Public Methods

		public init = () => {
			super.baseInit();
			this.loadData();
		}

		//#endregion
	}
}