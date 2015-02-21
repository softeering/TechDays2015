/// <reference path="configuration.ts" />
define(["require", "exports", 'configuration'], function (require, exports, config) {
    var helpers;
    (function (helpers) {
        var stringHelper = (function () {
            function stringHelper() {
            }
            // static method !
            stringHelper.endsWith = function (str, suffix) {
                if (!str || !suffix)
                    return false;
                return str.indexOf(suffix, str.length - suffix.length) !== -1;
            };
            return stringHelper;
        })();
        helpers.stringHelper = stringHelper;
        var DynamoClient = (function () {
            function DynamoClient() {
                var _this = this;
                this._parameters = new collections.Dictionary(); //Generics !
                this._config = new config.configuration.configValues();
                this.createUrl = function (dataSourceCode) {
                    if (!dataSourceCode)
                        throw "Invalid data source code";
                    var baseUrl = config.configuration.configValues.getDynamoBaseURL();
                    if (!baseUrl)
                        throw "Invalid configuration";
                    var url = baseUrl + (!helpers.stringHelper.endsWith(baseUrl, "/") ? "/" : "") + "odata/" + dataSourceCode;
                    if (!_this._parameters.containsKey("$format"))
                        _this._parameters.setValue("$format", "json");
                    if (_this._parameters.size() > 0)
                        url += "?";
                    _this._parameters.forEach(function (key, value) {
                        url += key + "=" + value + "&";
                    });
                    return url;
                };
                this.loadDataSource = function (code) {
                    if (code) {
                        return $.ajax({
                            url: _this.createUrl(code),
                            accepts: 'json',
                            type: 'get',
                            async: true
                        }).then(function (result) {
                            var data = [];
                            if (result && result.value)
                                data = result.value;
                            return data;
                        });
                    }
                    else
                        return null;
                };
                this.addParameter = function (key, value) {
                    if (key != undefined && value != undefined)
                        _this._parameters.setValue(key, value);
                };
            }
            return DynamoClient;
        })();
        helpers.DynamoClient = DynamoClient;
    })(helpers = exports.helpers || (exports.helpers = {}));
});
//# sourceMappingURL=helpers.js.map