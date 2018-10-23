function country(id, name) {
	return {
		id: ko.observable(id),
		name: ko.observable(name),
		players: ko.observableArray([]),
		isEdit: ko.observable(false)
	};
}
function player(id, name) {
	return {
		id: ko.observable(id),
		name: ko.observable(name)
	};
}

function Menu() {
	return {
		items: [
			{ name: 'Home Page', hash: 'home', viewRoute: '/Home', viewModel: countryViewModel, defaultStart: true },
			{ name: 'Services', hash: 'services', viewRoute: '/Services', viewModel: countryViewModel, showInMenu: true },
			{ name: 'About', hash: 'about', viewRoute: '/About', viewModel: countryViewModel, showInMenu: true }
		],
		getItemByHash: function (hash) {
			return this.items.find(function (item) { return item.hash == hash; });
		},
		getDefaultStart: function () {
			return this.items.find(function (item) { return item.defaultStart; });
		},
		isValidHash: function (hash) {
			return (this.getItemByHash(hash) !== undefined);
		}
	};
}