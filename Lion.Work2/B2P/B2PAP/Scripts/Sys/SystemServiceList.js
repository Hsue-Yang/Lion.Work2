var _formElement;

function SystemServiceListForm_onLoad(formElement) {
    _formElement = formElement;

    return true;
}

function UpdataLinkFunKey_onClick(srcElement, keys) {
    $('#ServiceID').attr('value', keys[1]);
    $('#Remark').attr('value', keys[2]);
    $('#AddButton').attr('value', keys[3]);
    $('#AddButton').attr('name', 'SaveButton');
    $('#AddButton').attr('id', 'SaveButton');
}

function DeleteLinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });

    $('#ServiceID').attr('value', keys[1]);

    _alert('dialog_Confirm');
}

function PageSize_onEnter(srcElement) {
    SelectButton_onClick();
    return true;
}

//----Button----//
function SelectButton_onClick(srcElement) {
    var Result = _FormValidation();
    _formElement.submit();
}

function AddButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeAdd);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function SaveButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    location.href = '/Sys/SystemSetting';
}

function ConfirmOKButton_onClick(srcElement) {
    $('#ExecAction').val(_ActionTypeDelete);
    _formElement.submit();
}

function ConfirmNOButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}