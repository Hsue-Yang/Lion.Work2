var _formElement;

function SystemTokenClientLogForm_onLoad(formElement) {
    _formElement = formElement;

    if ($(formElement).find('#SystemTokenClientLogTable')[0] != null) {
        _TableHover('SystemTokenClientLogTable', formElement);
    }

    $('#GenTimeBegin, #GenTimeEnd', $('table.tblsearch', _formElement)).combobox({
        isRemoveButton: false
    });

    return true;
}
function SelectButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);
    if (Result) {
        _formElement.submit();
    }
}