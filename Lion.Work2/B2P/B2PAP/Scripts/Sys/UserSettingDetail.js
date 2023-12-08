var _formElement;

function UserSettingDetailForm_onLoad(formElement) {
    _formElement = formElement;

    return true;
}

//----Button----//
function AddButton_onClick(srcElement) {
    SetValidate();
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeAdd);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function UpdateButton_onClick(srcElement) {
    SetValidate();
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    $('#ExecAction').val(_ActionTypeSelect);
    _formElement.submit();
}

//----Private Function----//
function SetValidate() {
    var gender = $("[name=UserGender]:checked").val();
    if (gender == undefined) {
        _AddJsErrMessage(JsMsg_UserGender);
    }
}