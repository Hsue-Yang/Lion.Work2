var _formElement;

function UserBasicInfoDetailForm_onLoad(formElement) {
    _formElement = formElement;

    return true;
}

//----Tab----//
function UserSystemRole_onClick(srcElement) {
    $(srcElement).attr('con', 'Sys');
    $(srcElement).attr('act', 'UserSystemRole');
    $('#ExecAction').val(_ActionTypeSelect);

    _submit($(srcElement));

    return false;
}

//----Button----//
function CloseButton_onClick(srcElement) {
    _windowClose();
}