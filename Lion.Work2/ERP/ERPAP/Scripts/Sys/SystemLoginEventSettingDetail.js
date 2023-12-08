var _formElement;

function SystemLoginEventSettingDetailForm_onLoad(formElement) {
    _formElement = formElement;

    $('#SubSysID', _formElement).combobox({
        width: 180,
        isRemoveButton: true
    });
}

function AddButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeAdd);

    if (result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function CancelButton_onClick(srcElement) {
    $('#ExecAction').val(_ActionTypeSelect);
    _formElement.submit();
}

function UpdateButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);

    if (result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function DeleteButton_onClick(srcElement) {
    $('#ExecAction').val(_ActionTypeDelete);
    return true;
}

function StartExecTime_onBlur(srcElement) {
    _FormatTime(srcElement);
}

function EndExecTime_onBlur(srcElement) {
    _FormatTime(srcElement);
}

function _FormatTime(srcElement) {
    if (srcElement.value.length === 4) {
        srcElement.value = srcElement.value.substr(0, 2) + ':' + srcElement.value.substr(2, 2);
    }
}