function Model() {
    self.submitAttempted = ko.observable(false);
    self.save = function () {
        self.submitAttempted(true);
        if (self.disclaimerValid())
            return true;
    };

    self.disclaimerRead = ko.observable(false);
    self.disclaimerAgreement = ko.observable(false);
    self.disclaimerValid = ko.computed(function () {
        return self.disclaimerAgreement() || self.disclaimerRead();
    });
    self.scrollDisclaimer = function (m, e) {
        var el = $(e.currentTarget);
        if (el.scrollTop() + el.innerHeight() + 1 >= e.currentTarget.scrollHeight) {
            self.disclaimerRead(true);
        }
    }; 
}

var model = new Model();
ko.applyBindings(model);
