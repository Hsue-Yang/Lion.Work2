var _formElement;

function PushTopicNotificationDetailForm_onLoad(formElement) {
    _formElement = formElement;
    ImmediatelyPush_onChange();
    $('#ImmediatelyPush').closest('tr').hide();
}

function ImmediatelyPush_onChange(srcElement) {
    if ($('#ImmediatelyPush').val() === 'N') {
        $('#PushDate').closest('tr').show();
    } else {
        $('#PushDate').closest('tr').hide();
    }
}

function CancelButton_onClick(srcElement) {
    window.location.href = _enumERPAP + "/Pub/PushTopicNotification"
}

function ConfirmButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeAdd);
    if (result) {
        $('#ImmediatelyPush').removeAttr('disabled');
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

//- 時間格式設定(暫不用) -
function PushTime_onBlur(srcElement) {
    if (srcElement.value.length === 4) {
        srcElement.value = srcElement.value.substr(0, 2) + ':' + srcElement.value.substr(2, 2);
    }
}