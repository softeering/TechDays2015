/// <reference path="typings/jquery/jquery.d.ts" />
var UserAssignmentSettings = (function () {
    function UserAssignmentSettings(name) {
        var _this = this;
        this.greet = function () {
            return '<h1>Hello ' + _this.name + '!!!</h1>';
        };
        this.name = name;
    }
    return UserAssignmentSettings;
})();
var uas = new UserAssignmentSettings('Expedient');
var message = uas.greet();
$(function () {
    $('body').append(message);
});
//# sourceMappingURL=test.js.map