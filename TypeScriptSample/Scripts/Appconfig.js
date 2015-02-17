// RequireJS configuration

require.config({
	baseUrl: "/Scripts/",
	paths: {
		jquery: "libs/jquery-2.1.3.min",
		bootstrap: "libs/bootstrap.min",
		knockout: "libs/knockout-3.2.0",
		moment: "libs/moment.min",
		numeral: "libs/numeral.min"
	},
	shim: {
		"bootstrap": ["jquery"]
	}
});
