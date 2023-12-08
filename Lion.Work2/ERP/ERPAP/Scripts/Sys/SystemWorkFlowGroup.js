var _formElement;

function SystemWorkFlowGroupForm_onLoad(formElement) {
    _formElement = formElement;

    $('table.tblsearch #SysID', _formElement).combobox({
        width: 200,
        isRemoveButton: false
    });

    return true;
}

function LinkFunKeyDetail_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#WFFlowGroupID').val(keys[2]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

//----Button----//
function SearchButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);
    if (result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function AddButton_onClick(srcElement) {
    var result = _FormValidation();
    if (result) {
        $.blockUI({ message: '' });
        $('#ExecAction').val(_ActionTypeAdd);
        return true;
    }
}

//----Private Function----//
function Clean_HiddenValue() {
    $('#SysID').val('');
    $('#WFFlowGroupID').val('');
}