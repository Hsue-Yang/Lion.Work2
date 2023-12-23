var _formElement;
function LinkLeaveDetail_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    $('#ppm96_id').val(keys[1]);
    $('#ExecAction').val(_ActionTypeSelect);
    return true;
}

function AddButton_onClick(srcElement) {
    $.blockUI({ message: '' });
    $('#ExecAction').val(_ActionTypeAdd);
    return true;
}

function EditButton_onClick(srcElement) {
    var selectedRadioValue = $("input[name='ppm96_id_Radio']:checked").val();
    if (selectedRadioValue !== undefined) {
        $.blockUI({ message: '' });
        $('#ppm96_id').val(selectedRadioValue);
        $('#ExecAction').val(_ActionTypeUpdate);
        return true;
    } else {
        alert("請選擇一個選項");
        return false;
    }
}