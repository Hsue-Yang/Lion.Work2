var _formElement;

function TrustIPDetailForm_onLoad(formElement) {
    _formElement = formElement;

    $('#IPBeginReadOnlyText').val($('#IPBegin').val());
    $('#IPEndReadOnlyText').val($('#IPEnd').val());

    $('#ComID, #TrustType, #SourceType', _formElement).combobox({
        width: 180,
        isRemoveButton: false
    });

    return true;
}

//----Button----//
function AddButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeAdd);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function UpdateButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function DeleteButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    _alert('dialog_Confirm');
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });
    
    $('#ExecAction').val(_ActionTypeSelect);
    _formElement.submit();
}

function ConfirmOKButton_onClick(srcElement) {
    var Result = _FormValidation();

    $('#ExecAction').val(_ActionTypeDelete);
    if (Result) {
        _formElement.submit();
    }

    _btnUnblockUI(this, false);
}

function ConfirmNOButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}