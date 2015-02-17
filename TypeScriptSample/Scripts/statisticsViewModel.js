/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="typings/knockout/knockout.d.ts" />
/// <reference path="typings/knockout.mapping/knockout.mapping.d.ts" />
/// <amd-dependency path="/Scripts/libs/collections.js" />
/// <reference path="typings/numeraljs/numeraljs.d.ts" />
/// <amd-dependency path="numeral" />
/// <reference path="helpers.ts" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
define(["require", "exports", 'knockout', 'helpers', 'viewModelBase', "/Scripts/libs/collections.js", "numeral"], function (require, exports, ko, helpers, vm) {
    var viewModels;
    (function (viewModels) {
        // Class inheritance
        var statisticsViewModel = (function (_super) {
            __extends(statisticsViewModel, _super);
            //#endregion
            //#region Ctor
            function statisticsViewModel() {
                var _this = this;
                _super.call(this);
                //#region Properties
                this.statistics = ko.observableArray();
                this.numberOfDataSources = ko.computed(function () {
                    return _this.statistics().length;
                }, this); // if computed are defined as properties, the context (this) needs to be passed
                this.numberOfCalls = ko.computed(function () {
                    return numeral(_this.statistics().reduce(function (prev, current, index) {
                        return prev += current.NumberOfCalls || 0;
                    }, 0)).format("0,0"); //format can be done in the view, just for testing purpose
                }, this);
                //#endregion
                //#region Private Methods
                this.loadData = function () {
                    var client = new helpers.helpers.DynamoClient();
                    client.addParameter("$orderby", "NumberOfCalls desc");
                    _this.loading(true);
                    client.loadDataSource("DynamoStatistic").done(function (result) {
                        ko.utils.arrayPushAll(_this.statistics, result);
                    }).fail(function () {
                        //base method
                        _this.logError("An error occurred while loading the data, maybe you don't have access to Dynamo (are you connected to Expedia's network ?)");
                    }).always(function () {
                        _this.loading(false);
                    });
                };
                //#endregion
                //#region Public Methods
                this.init = function () {
                    _super.prototype.baseInit.call(_this);
                    _this.loadData();
                };
            }
            return statisticsViewModel;
        })(vm.viewModelBase);
        viewModels.statisticsViewModel = statisticsViewModel;
    })(viewModels = exports.viewModels || (exports.viewModels = {}));
});
//# sourceMappingURL=statisticsViewModel.js.map