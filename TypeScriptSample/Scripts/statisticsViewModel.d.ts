/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="typings/knockout/knockout.d.ts" />
/// <reference path="typings/knockout.mapping/knockout.mapping.d.ts" />
/// <reference path="typings/numeraljs/numeraljs.d.ts" />
/// <reference path="helpers.d.ts" />
import vm = require('viewModelBase');
export declare module viewModels {
    class statisticsViewModel extends vm.viewModelBase {
        statistics: KnockoutObservableArray<any>;
        numberOfDataSources: KnockoutComputed<number>;
        numberOfCalls: KnockoutComputed<string>;
        constructor();
        private loadData;
        init: () => void;
    }
}
