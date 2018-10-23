function CountryViewModel() {
	var self = this;

	var saveState = {};

	self.NewCountry = {
		id: ko.observable(0),
		name: ko.observable(''),
		isEdit: ko.observable(false)
	};

	self.countries = ko.observableArray([]);

	self.GetAllCountry = function () {
		$.getJSON("/api/Country", function (data) {
			var jsonData = eval(data.content);
			for (var i = 0; i < jsonData.length; i++) {
				var countryObj = new country(jsonData[i].id, jsonData[i].name);
				$.each(jsonData[i].players, function (index, pl) {
					countryObj.players.push(new player(pl.id, pl.name));
				});
				self.countries.push(countryObj);
			}
		})
        .done(function () {
        	console.log("second success");
        })
        .fail(function (jqxhr, textStatus, error) {
        	console.log("Request Failed: " + error);
        });
	};

	self.AddCountry = function (model, event) {
		$.post("/api/Country", model.NewCountry, function (data) {
			model.NewCountry.id(data.id);
			model.countries.push(model.NewCountry);
		});
	};

	self.EditCountry = function (model, event) {
		saveState.name = model.name();
		model.isEdit(true);
	};

	self.UpdateCountry = function (model, event) {
		$.ajax({
			url: "api/Country/" + model.id(),
			type: "PUT",
			dataType: "json",
			contentType: "application/json",
			data: ko.toJSON(model),
			success: function (data) {
				model.isEdit(false);
			},
			error: function (err) {
				alert('Error');
			}
		});
	};

	self.CancelCountry = function (model, event) {
		model.name(saveState.name);
		model.isEdit(false);
	};

	self.DeleteCountry = function (model, event) {
		$.ajax({
			url: "api/Country/" + model.id(),
			type: "DELETE",
			dataType: "json",
			contentType: "application/json",
			success: function (data) {
				self.countries.remove(model);
			},
			error: function (err) {
				alert('Error');
			}
		});
	};
}