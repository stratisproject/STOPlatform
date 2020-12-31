function Model() {
    var self = this;
    self.periods = ko.observableArray();
    data.periods.forEach(item => new SalePeriod(item.duration, item.tokenPrice));

    self.add = function () {
        self.periods.push(new SalePeriod(1000, 0.001));
    };

    self.add();
    self.remove = function (period) {
        self.periods.remove(period);
    };

    function SalePeriod(duration, tokenPrice) {
        this.duration = ko.observable(duration);
        this.tokenPrice = ko.observable(tokenPrice);
    };

    self.wallet = ko.observable(data.walletName);
    self.sender = ko.observable(data.sender);
    self.addresses = ko.computed(function () {
        return data.addresses.filter(function (a) { return a.wallet == self.wallet() })
            .map(function (a,i) { return { title: "["+ i + "] - "+ a.address + " - (" + a.amount + " CRS)", address: a.address, amount: a.amount } });
    });

    self.setToBold = function (option, item) {
        if (!item)
            return;

        if (item.amount > 0)
            ko.applyBindingsToNode(option, { css: 'semi-bold' }, item);
    }
};

var model = new Model();
ko.applyBindings(model);
