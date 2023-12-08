var _formElement;

function SystemWorkFlowNextForm_onLoad(formElement) {
    _formElement = formElement;

    return true;
}

function SystemWorkFlowNodeDetail_onClick(srcElement) {
    $(srcElement).attr('con', 'Sys');
    $(srcElement).attr('act', 'SystemWorkFlowNodeDetail');
    $('#ExecAction').val(_ActionTypeUpdate);
    _submit($(srcElement));

    return false;
}

function SystemWorkFlowSignature_onClick(srcElement) {
    $(srcElement).attr('con', 'Sys');
    $(srcElement).attr('act', 'SystemWorkFlowSignature');
    $('#ExecAction').val(_ActionTypeUpdate);
    _submit($(srcElement));

    return false;
}

function SystemWorkFlowDocument_onClick(srcElement) {
    $(srcElement).attr('con', 'Sys');
    $(srcElement).attr('act', 'SystemWorkFlowDocument');
    $('#ExecAction').val(_ActionTypeUpdate);
    _submit($(srcElement));

    return false;
}

function LinkFunKeyDetail_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#NextWFNodeID').val(keys[1]);
    
    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

//----Button----//
function AddButton_onClick(srcElement) {
    var result = _FormValidation();
    if (result) {
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
    $('#NextWFNodeID').val('');
}
