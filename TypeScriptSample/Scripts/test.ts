/// <reference path="typings/jquery/jquery.d.ts" />

class UserAssignmentSettings {
    name: string;

    constructor(name: string) {
        this.name = name;
    }

    greet = () => {
        return '<h1>Hello ' + this.name + '!!!</h1>';
    }
}

var uas = new UserAssignmentSettings('Expedient');
var message = uas.greet();

$(function () {
    $('body').append(message);
});
