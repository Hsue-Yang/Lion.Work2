var _formElement;

function SystemSubsysListForm_onLoad(formElement) {
    _formElement = formElement;

    return true;
}

function DeleteLinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });

    $('#SubSysID').attr('value', keys[1]);
    $('#SubSysID').attr('ReadOnly', 'true');
    $('#SubSysID').attr('style', 'background-color : #E7E8ED');

    _alert('dialog_Confirm');
}

function UpdateLinkFunKey_onClick(srcElement, keys) {
    $('#SubSysID').attr('value', keys[1]);
    $('#SubSysID').attr('ReadOnly', 'true');
    $('#SubSysID').attr('style', 'background-color : #E7E8ED');

    $('#SysNMZHTW').attr('value', keys[2]);
    $('#SysNMZHCN').attr('value', keys[3]);
    $('#SysNMENUS').attr('value', keys[4]);
    $('#SysNMTHTH').attr('value', keys[5]);
    $('#SysNMJAJP').attr('value', keys[6]);
    $('#SortOrder').attr('value', keys[7]);

    $('#AddButton').attr('value', keys[8]);
    $('#AddButton').attr('name', 'SaveButton');
    $('#AddButton').attr('id', 'SaveButton');
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