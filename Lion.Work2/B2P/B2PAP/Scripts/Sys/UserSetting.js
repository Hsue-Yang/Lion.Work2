var _formElement;

function UserSettingForm_onLoad(formElement) {
    _formElement = formElement;

    return true;
}

function DetailLinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#UserID').val(keys[1]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function PageSize_onEnter(srcElement) {
    SearchButton_onClick();
    return true;
}

//----Button----//
function SearchButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function AddButton_onClick(srcElement) {
    $.blockUI({ message: '' });
    $('#ExecAction').val(_ActionTypeAdd);
    return true;
}

//----Private Function----//
function Clean_HiddenValue() {
    $('#UserID').val('');
}