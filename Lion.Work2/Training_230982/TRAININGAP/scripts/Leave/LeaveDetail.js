var _formElement;

function LeaveDetailForm_onLoad(formElement) {
    _formElement = formElement;
}

function CancelButton_onClick() {  //Problem Can't find URL Leave/Leave
    $.blockUI({ message: '' });
    $('#ExecAction').val(_ActionTypeSelect);
    return true;
}

function UpdateButton_onClick() {
    let result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
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

            console.log(res)
        },
        error: function (err) {
            console.log('ajax error:')
            console.log(err)
        }
    })
}
