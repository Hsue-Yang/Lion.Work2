var _formElement;

function SystemFunToolParaForm_onLoad(formElement) {
    _formElement = formElement;

    if ($(_formElement).find('#SystemFunToolParaTable')[0] != null) {
        _TableHover('SystemFunToolParaTable', _formElement);
    }

    return true;
}

function PageSize_onEnter(srcElement) {
    var Result = _FormValidation();

    $('#ExecAction').val(_ActionTypeSelect);
    $('#IsSelect').val('Y');
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
    return true;
}

//----Button----//
function CloseButton_onClick() {
    _windowClose();
}