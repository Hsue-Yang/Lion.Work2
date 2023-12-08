var _formElement;

function SystemFunMenuForm_onLoad(formElement) {
    _formElement = formElement;

    if ($(formElement).find('#SystemFunMenuTable')[0] !== null) {
        _TableHover('SystemFunMenuTable', formElement);
    }

    $('table.tblsearch #QuerySysID', _formElement).combobox({
        width: 180,
        isRemoveButton: false
    });

    return true;
}

function LinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#FunMenu').val(keys[2]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function PageSize_onEnter(srcElement) {
    SelectButton_onClick();
    return true;
}

//----Button----//
function SelectButton_onClick(srcElement) {
    $('#ExecAction').val(_ActionTypeSelect);
    if (_FormValidation()) {
        $.blockUI({ message: '' });
        $('#PageIndex').val(1);
        _formElement.submit();
    }
}

function AddButton_onClick(srcElement) {
    if (_FormValidation()) {
        $.blockUI({ message: '' });
        Clean_HiddenValue();

        $('#SysID').val($('#QuerySysID').val());

        $('#ExecAction').val(_ActionTypeAdd);
        return true;
    }
}

//----Private Function----//
function Clean_HiddenValue() {
    $('#SysID').val('');
    $('#FunMenu').val('');
}