var _formElement;

function SystemRoleCategoryForm_onLoad(formElement) {
    _formElement = formElement;

    if ($(formElement).find('#SystemRoleCategoryTable')[0] != null) {
        _TableHover('SystemRoleCategoryTable', formElement);
    }

    $('table.tblsearch #SysID', _formElement).combobox({
        width: 180,
        isRemoveButton: false
    });

    return true;
}

function LinkFunKeyDetail_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#RoleCategoryID').val(keys[1]);
    
    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

//----Button----//
function SelectButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function AddButton_onClick(srcElement) {
    var Result = _FormValidation();
    if (Result) {
        $.blockUI({ message: '' });
        Clean_HiddenValue();
        $('#ExecAction').val(_ActionTypeAdd);
        return true;
    }
}

function CloseButton_onClick(srcElement) {
    _windowClose();
}

//----Private Function----//
function Clean_HiddenValue() {
    $('#RoleCategoryID').val('');
}