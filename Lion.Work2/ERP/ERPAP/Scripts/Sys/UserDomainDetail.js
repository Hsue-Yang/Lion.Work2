var _formElement;

function UserDomainDetailForm_onLoad(formElement) {
    _formElement = formElement;

    
    
    return true;
}

function AddButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeAdd);
    if (Result) {
        if (ValidObjectChecked()) {
            $.blockUI({ message: '' });
            _formElement.submit();
        } else {
            _ShowJsErrMessageBox();
        }
    }
}

function UpdateButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
    if (Result) {
        if (ValidObjectChecked()) {
            $.blockUI({ message: '' });
            _formElement.submit();
        } else {
            _ShowJsErrMessageBox();
        }
    }
}

function DeleteButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    _alert('dialog_Confirm');
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    location.href = '/Sys/UserDomain';
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

function Help03Button_onClick(srcElement) {
    vMapFields = new Array(3);
    vMapFields[1] = "QueryUserID";
    _hISearch(vMapFields, "newwindow", _enumPUBAP + "/Help/Ishlp03", "Name=" + encodeURI($.trim($("#QueryUserID").val())));
}

function ValidObjectChecked() {
    var result = true;
    var domainGroupIDIsValid = false;


    $('input[name="DomainGroupID"]').each(function () {
        if (this.checked == true) {
            domainGroupIDIsValid = true;
        }
    });

    if (domainGroupIDIsValid == false) {
        _AddJsErrMessage(JsMsg_DomainGroupIDIsEmpty);
        result = false;
    }

    return result;
}

