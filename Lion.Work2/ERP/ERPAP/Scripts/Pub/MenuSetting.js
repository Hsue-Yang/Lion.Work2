var _formElement;

function MenuSettingForm_onLoad(formElement) {
    _formElement = formElement;
    var table = $('#MenuSettingTable', _formElement);

    if (table.length > 0) {
        var width = $(window).width();
        table.show();
        var height = $(window).height() - table.offset().top - $('#UpdateButton').closest('div').height();
        table.freezePanes({ width: width + 'px', height: height + 'px', fixedCols: 0 });
    }

    return true;
}

//----Button----//
function UpdateButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
    if (result) {
        _formElement.submit();
    }
}