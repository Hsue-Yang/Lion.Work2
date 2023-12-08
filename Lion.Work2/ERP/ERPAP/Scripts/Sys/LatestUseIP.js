var _formElement;

function LatestUseIPForm_onLoad(formElement) {
    _formElement = formElement;
}

function SelectButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);
    if (result) {
        $('#IPAddresss').val($('#IPAddresss').val().trim());
        $.blockUI({ message: '' });
        return true;
    }

    _ShowJsErrMessageBox();
    return false;
}

function PageSize_onEnter(srcElement) {
    if (SelectButton_onClick()) {
        _formElement.submit();
        return true;
    }
    return false;
}