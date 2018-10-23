/// <reference path="../_reference.js" />

var pageContainerId = '#page-content';
var pageContainer = $(pageContainerId)[0];
var requestedPageHash = window.location.hash;

var countryViewModel = new CountryViewModel();
//countryViewModel.GetAllCountry();

// Create menu model and view model.
var menuModel = new Menu();
var menuViewModel = new MenuViewModel();

// moved lower: Bind menu DOM element to the menu view model.
//ko.applyBindings(menuViewModel, document.getElementById('menu'));

var startingPageHash = 'home';

// If the requested hash is a valid one, use it as the starting point.
if (requestedPageHash && menuModel.isValidHash(requestedPageHash.substr(1)))
	startingPageHash = requestedPageHash.substr(1);

// If the requested hash is not valid, remove it from the URL to prevent confusion.
else
	history.pushState('', document.title, window.location.pathname + window.location.search);

// Get starting page route depending on the starting page hash selected.
var startingPage = menuModel.getItemByHash(startingPageHash);
menuViewModel.currentPageHash = ko.observable(startingPageHash);

// Bind menu DOM element to the menu view model.  Must be after the currentPageHash observable is created.
ko.applyBindings(menuViewModel, document.getElementById('brand'));

$('#brand').click(function () {
	var startRoute = menuModel.getDefaultStart();

	menuViewModel.currentPageHash(startRoute.hash);

	$(pageContainerId).load(startRoute.viewRoute, function () {
		ko.cleanNode(pageContainer);
		ko.applyBindings(startRoute.viewModel, pageContainer);

		console.log(window.location.pathname + '#' + startRoute.hash);
		history.pushState('', document.title, window.location.pathname + '#' + startRoute.hash);
	});
});

// Load starting page content.
$(pageContainerId).load(startingPage.viewRoute, function () {
	ko.applyBindings(startingPage.viewModel, pageContainer);
});

// Handle user attempt at backward history traversal.
$(window).on('popstate', function () {
	var previousPageHash = 'home';
	var requestedPageHash = window.location.hash;

	// If the previous hash is a valid one, go to that page.
	if (requestedPageHash && menuModel.isValidHash(requestedPageHash.substr(1)))
		previousPageHash = requestedPageHash.substr(1);

	// Get previous page route depending on the chosen previous page hash.
	var previousPage = menuModel.getItemByHash(previousPageHash);

	$(pageContainerId).load(previousPage.viewRoute, function () {
		ko.cleanNode(pageContainer);
		ko.applyBindings(previousPage.viewModel, pageContainer);
	});
});

console.log(menuViewModel.menuitems());