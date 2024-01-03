var _formElement;

function LeaveForm_onLoad(formElement) {
    _formElement = formElement;
}

function AddButton_onClick(srcElement) {
    var result = _FormValidation();
    if (result) {
        return true;
    }
}
function LeaveDataDetail_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    $('#ppm96_id').val(keys[1]);
    $('#ExecAction').val(_ActionTypeQuery);
    return true;
}
function EditButton_onClick(srcElement) {
    let selectedVal = $("input[name='ppm96_stfn_Radio']:checked").val();
    if (selectedVal !== undefined) {
        $('#ppm96_id').val(selectedVal);
        $('#ExecAction').val(_ActionTypeQuery);
        return true;
    } else {
        _AddJsErrMessage(JsMsg_EditOption_Error);
        _ShowJsErrMessageBox();
        return false;
    }
}
function DeleteButton_onClick(srcElement) {
    let selectedVal = $("input[name='ppm96_stfn_Radio']:checked").val();
    if (selectedVal !== undefined) {
        $('#ppm96_id').val(selectedVal);
        $('#ExecAction').val(_ActionTypeDelete);
        return true;
    } else {
        _AddJsErrMessage(JsMsg_DeleteOption_Error);
        _ShowJsErrMessageBox();
        return false;
    }
}

