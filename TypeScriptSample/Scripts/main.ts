// Intellisense
/// <reference path="typings/requirejs/require.d.ts" />
/// <reference path="typings/knockout/knockout.d.ts" />
/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="statisticsViewModel.ts" />

// External scripts (non-module) dependencies : loaded using requireJS, we can use names defined in RequireJS configuration or absolute paths
/// <amd-dependency path="bootstrap" />
/// <amd-dependency path="moment" />
/// <amd-dependency path="numeral" />

// Import modules
import $ = require('jquery')
import ko = require('knockout')
import vm = require('statisticsViewModel')

// document ready
$(() => {
	// instanciate our view model
	var viewModel = new vm.viewModels.statisticsViewModel();
	viewModel.init(); // load data
	ko.applyBindings(viewModel); // apply bindings
});


