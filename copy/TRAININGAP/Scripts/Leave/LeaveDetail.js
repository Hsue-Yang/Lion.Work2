var _formElement;

document.addEventListener("DOMContentLoaded", function (event) {
    if (execAction == "Select") {
        $(".ui-datepicker-trigger").hide(); // 隱藏小日曆
    }
});

function LeaveDetailForm_onLoad(formElement) {
    _formElement = formElement;
    let srcElement = document.querySelector("#ppm95_id");
    ppm95_id_onChange(srcElement);
    return true;
}

function ppm95_id_onChange(srcElement) {
    let Value = srcElement.value;
    $.ajax({
        url: "/Leave/GetLeaveCategoryChildByIdList",
        type: 'POST',
        data: { 'ppm95_id': Value },
        dataType: 'json',
        success: function (result) {
            $('#ppd95_id > option', _formElement).remove();
            if (result.length > 1) {
                $('#ppd95_id').show();
                for (let i = 0; i < result.length; i++) {
                    $('#ppd95_id').append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                }
                ppd95_id.value = $('#ppd95_id').attr('originalvalue');
            }
            else {
                $('#ppd95_id').hide()
            }
        }
    })
}

function isNumberValid(input) {
    var pattern = /^\d{8}$/;
    return pattern.test(input);
}

function isTimeValid(input) {
    var pattern = /^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$/;
    return pattern.test(input);
}

function UpdateButton_onClick(srcElement) {
    var Result = _FormValidation();
    var ppm96_beginDateValue = $('#ppm96_beginDate').val();
    var ppm96_EndDateValue = $('#ppm96_EndDate').val();
    var ppm96_beginTimeValue = $('#ppm96_beginTime').val();
    var ppm96_EndTimeValue = $('#ppm96_EndTime').val();
    if (ppm96_beginDateValue > ppm96_EndDateValue) {
        alert('開始日期不能大於结束日期。');
        return false;
    }
    if (ppm96_beginTimeValue > ppm96_EndTimeValue) {
        alert('開始時間不能大於结束時間。');
        return false;
    }
    if (!isNumberValid(ppm96_beginDateValue)) {
        alert('請點選開始時間小日曆輸入正確的時間格式 (yyyymmdd)');
        return false;
    }
    if (!isNumberValid(ppm96_EndDateValue)) {
        alert('請點選結束時間小日曆輸入正確的時間格式 (yyyymmdd)');
        return false;
    }
    if (!isTimeValid(ppm96_beginTimeValue)) {
        alert('請在開始時間第2個輸入框輸入正確的時間格式 (HH:MM)');
        return false;
    }
    if (!isTimeValid(ppm96_EndTimeValue)) {
        alert('請在結束時間第2個輸入框輸入正確的時間格式 (HH:MM)');
        return false;
    }
    if ($('#ppd95_id > option').length > 1 && $('#ppd95_id').val() == '') {
        alert('請選擇假別細項');
        Result = false;
    }
    $('#ExecAction').val(_ActionTypeUpdate);
    if (Result) {
        if ($('input[name="options"]:checked').val() == null) {
            alert("送審主管請至少勾選一個選項。");
            return;
        }
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });
    $('#ExecAction').val(_ActionTypeSelect);
    _formElement.submit();
}

function DeleteButton_onClick(srcElement) {
    $.blockUI({ message: '' });
    _alert('dialog_Confirm');
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

function AddButton_onClick(srcElement) {
    var Result = _FormValidation();
    var ppm96_beginDateValue = $('#ppm96_beginDate').val();
    var ppm96_EndDateValue = $('#ppm96_EndDate').val();
    var ppm96_beginTimeValue = $('#ppm96_beginTime').val();
    var ppm96_EndTimeValue = $('#ppm96_EndTime').val();
    if (ppm96_beginDateValue > ppm96_EndDateValue) {
        alert('開始日期不能大於结束日期。');
        return false;
    }
    if (ppm96_beginTimeValue > ppm96_EndTimeValue) {
        alert('開始時間不能大於结束時間。');
        return false;
    }
    if (!isNumberValid(ppm96_beginDateValue)) {
        alert('請點選開始時間小日曆輸入正確的時間格式 (yyyymmdd)');
        return false;
    }
    if (!isNumberValid(ppm96_EndDateValue)) {
        alert('請點選結束時間小日曆輸入正確的時間格式 (yyyymmdd)');
        return false;
    }
    if (!isTimeValid(ppm96_beginTimeValue)) {
        alert('請在開始時間第2個輸入框輸入正確的時間格式 (HH:MM)');
        return false;
    }
    if (!isTimeValid(ppm96_EndTimeValue)) {
        alert('請在結束時間第2個輸入框輸入正確的時間格式 (HH:MM)');
        return false;
    }
    if ($('#ppd95_id > option').length > 1 && $('#ppd95_id').val() == '') {
        alert('請選擇假別細項');
        Result = false;
    }
    $('#ExecAction').val(_ActionTypeAdd);
    if (Result) {
        if ($('input[name="options"]:checked').val() == null) {
            alert("送審主管請至少勾選一個選項。");
            return;
        }
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}