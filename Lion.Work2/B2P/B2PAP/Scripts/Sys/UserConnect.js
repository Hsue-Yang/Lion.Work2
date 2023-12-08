var _formElement;

function UserConnectForm_onLoad(formElement) {
    _formElement = formElement;

    return true;
}

function PageSize_onEnter(srcElement) {
    SelectButton_onClick();
    return true;
}

function SelectButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);
    if (Result) {
        _formElement.submit();
    }
}