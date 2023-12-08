var _formElement;

function SystemTeamsForm_onLoad(formElement) {
    _formElement = formElement;

    $('table.tblsearch #SysID', _formElement).combobox({
        width: 180,
        isRemoveButton: false
    });

    return true;
}

function DetailLinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });

    $('#SysID').val(keys[1]);
    $('#TeamsChannelID').val(keys[2]);
    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function PageSize_onEnter(srcElement) {
    SelectButton_onClick();
    return true;
}

function SelectButton_onClick(srcElement) {
    $('#ExecAction').val(_ActionTypeSelect);
    if (_FormValidation()) {
        $.blockUI({ message: '' });
        $('#PageIndex').val(1);
        _formElement.submit();
    }
}

function AddButton_onClick(srcElement) {
    $.blockUI({ message: '' });
    $('#ExecAction').val(_ActionTypeAdd);
    return true;
}