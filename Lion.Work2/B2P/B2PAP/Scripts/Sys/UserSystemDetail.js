var _formElement;

function UserSystemDetailForm_onLoad(formElement) {
    _formElement = formElement;

    return true;
}

//----Button----//
function UpdateButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    $('#ExecAction').val(_ActionTypeSelect);
    _formElement.submit();
}