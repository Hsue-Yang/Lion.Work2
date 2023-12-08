var _formElement;

function UserMainForm_onLoad(formElement) {
    _formElement = formElement;

    return true;
}

//----Button----//
function UpdateButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
    if (PWDCheck() && Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    } 
    else {
        _ShowJsErrMessageBox();
    }
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });
    window.close();
}

function PWDCheck(srcElement) {
    var result = true;
    if ($('#UserPWD').val() != "" || $('#UserNewPWD').val() != "" || $('#UserNewPWDCheck').val() != "") {
        result = false;
        if($('#UserNewPWD').val() != $('#UserNewPWDCheck').val()){
            _AddJsErrMessage(JsMsg_UserNewPWDIsNotConsistency);
        }
        else if ($('#UserPWD').val() != "" && $('#UserNewPWD').val() != "" && $('#UserNewPWDCheck').val() != "") {
            result = true;
        }
        else {
            _AddJsErrMessage(JsMsg_UserPWDColumnIsEmpty);
        }
    }
    return result;
}