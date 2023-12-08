var _formElement;

function MenuSettingForm_onLoad(formElement) {
    _formElement = formElement;

    return true;
}

//----Button----//
function UpdateButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
    if (Result) {
        _formElement.submit();
    }
}