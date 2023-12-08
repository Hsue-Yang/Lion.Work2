var _formElement;

function SystemRecordUserPurviewDetailForm_onLoad(formElement) {
    _formElement = formElement;

    var filterIcon = $('div.advancedFilter .ico-href');

    filterIcon.click(function () {
        var self = $(this);
        self.find('i').toggleClass('icon-angle-down');
        self.parents('div.advancedFilter').find('div.filterCont').toggleClass('clickShow');
    });
}

function CloseButton_onClick(parameters) {
    _windowClose();
}