var _formElement;

function LeaveForm_onLoad(formElement) {
    _formElement = formElement;
}

function AddButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeAdd);
    if (result) {
        return true;
    }
}
function LeaveDataDetail_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    $('#ppm96_stfn').val(keys[1]);
    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}
function EditButton_onClick(srcElement) {
    let selectedVal = $("input[name='ppm96_stfn_Radio']:checked").val();
    if (selectedVal !== undefined) {
        $('#ppm96_id').val(selectedVal);
        $('#ExecAction').val(_ActionTypeUpdate);
        return true;
    } else {
        alert("需選擇一個項目");
        return false;
    }
}

