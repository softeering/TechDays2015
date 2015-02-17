/// <reference path="typings/knockout/knockout.d.ts" />
define(["require", "exports", 'knockout'], function (require, exports, ko) {
    var viewModelBase = (function () {
        function viewModelBase() {
            var _this = this;
            // different accessors private, public, protected
            this._clearErrorMessageTimeOut = null;
            this.errorMessage = ko.observable(null);
            this.loading = ko.observable(false);
            this.logError = function (message) {
                if (!message)
                    return;
                if (_this._clearErrorMessageTimeOut)
                    clearTimeout(_this._clearErrorMessageTimeOut);
                _this.errorMessage(null);
                _this.errorMessage(message);
                _this._clearErrorMessageTimeOut = setTimeout(function () {
                    _this.errorMessage(null);
                }, 5000);
            };
        }
        viewModelBase.prototype.baseInit = function () {
        };
        return viewModelBase;
    })();
    exports.viewModelBase = viewModelBase;
});
//# sourceMappingURL=viewModelBase.js.map