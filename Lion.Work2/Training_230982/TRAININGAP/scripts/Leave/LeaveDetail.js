var _formElement;

function LeaveDetailForm_onLoad(formElement) {
    _formElement = formElement;
}

function CancelButton_onClick() {
    $.blockUI({ message: '' });
    _formElement.submit();
}

function UpdateButton_onClick() {
    let result = _FormValidation();
    let beginDate = $('#Ppm96_beginDate').val();
    let beginTime = $('#Ppm96_beginTime').val();
    let endDate = $('#Ppm96_endDate').val();
    let endTime = $('#Ppm96_endTime').val();
    let timeRegex = /^([01]\d|2[0-3])[0-5]\d$/;
    let begin = new Date(`${beginDate.substr(0, 4)}-${beginDate.substr(4, 2)}-${beginDate.substr(6, 2)}T${beginTime.substr(0, 2)}:${beginTime.substr(2, 2)}`);
    let end = new Date(`${endDate.substr(0, 4)}-${endDate.substr(4, 2)}-${endDate.substr(6, 2)}T${endTime.substr(0, 2)}:${endTime.substr(2, 2)}`);
    $('#ExecAction').val(_ActionTypeUpdate);
    if ((timeRegex.test(beginTime) && timeRegex.test(endTime)) == false) {
        _AddJsErrMessage(JsMsg_InputTime_Error);
        _ShowJsErrMessageBox();
        return false;
    }
    if (begin >= end) {
        _AddJsErrMessage(JsMsg_InputDate_Error);
        _ShowJsErrMessageBox();
        return false;
    }
    if ($('input[name="Ppm96_signList"]:checked').val() == null) {
        _AddJsErrMessage(JsMsg_ChooseDirector_Error);
        _ShowJsErrMessageBox();
        return false;
    }
    if (result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function Ppm95_id_onChange(srcElement) {

    $.ajax({
        url: '/Leave/GetPpd95List',
        type: 'POST',
        data: { ppm95_id: srcElement.value },
        dataType: 'json',
        async: false,
        success: function (res) {
            $('#Ppd95_id > option').remove();
            if (res.length > 0) {
                $('#Ppd95_id').show();
                for (let i = 0; i < res.length; i++) {
                    $('#Ppd95_id').append('<option value="' + res[i].Value + '">' + res[i].Text + '</option>');
                }
            } else {
                $('#Ppd95_id').hide();
            }
        },
        error: function (err) {
            _AddJsErrMessage(JsMsg_GetPpd95List_Error);
            _ShowJsErrMessageBox();
        }
    })
}