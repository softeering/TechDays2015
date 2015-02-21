// Intellisense
/// <reference path="typings/requirejs/require.d.ts" />
/// <reference path="typings/knockout/knockout.d.ts" />
/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="statisticsViewModel.ts" />
define(["require", "exports", 'jquery', 'knockout', 'statisticsViewModel', "bootstrap", "moment", "numeral"], function (require, exports, $, ko, vm) {
    // document ready
    $(function () {
        // instanciate our view model
        var viewModel = new vm.viewModels.statisticsViewModel();
        viewModel.init(); // load data
        ko.applyBindings(viewModel); // apply bindings
    });
});
//# sourceMappingURL=main.js.map