/// <reference path="../_reference.js" />

function MenuViewModel() {
	var self = this;
	//var currentPageHash;

	//self.currentPageHash = ko.observable('about');

	self.menuitems = ko.observableArray(menuModel.items.filter(function (item) { return item.showInMenu; }));

	self.menuClick = function (data, event) {
		console.log('clicked menu option', data);
		
		console.log(self);
		self.currentPageHash(data.hash);

		$(pageContainerId).load(data.viewRoute, function () {
			ko.cleanNode(pageContainer);
			ko.applyBindings(data.viewModel, pageContainer);

			console.log(window.location.pathname + '#' + data.hash);
			history.pushState('', document.title, window.location.pathname + '#' + data.hash);
		});
	};

}