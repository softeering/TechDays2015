define(["require", "exports"], function (require, exports) {
    var configuration;
    (function (configuration) {
        var configValues = (function () {
            function configValues() {
            }
            configValues._dynamoBaseUrl = "http://dynamo";
            // static method
            configValues.getDynamoBaseURL = function () {
                return configValues._dynamoBaseUrl;
            };
            return configValues;
        })();
        configuration.configValues = configValues;
    })(configuration = exports.configuration || (exports.configuration = {}));
});
//# sourceMappingURL=configuration.js.map