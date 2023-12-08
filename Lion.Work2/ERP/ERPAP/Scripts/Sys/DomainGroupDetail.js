var _formElement;

function DomainGroupDetailForm_onLoad(formElement) {
    _formElement = formElement;

    $('#DomainNameReadOnlyText').val($('#DomainName').find("option:selected").text());

    return true;
}

function DomainGroupID_onBlur(srcElement) {

    if ($('#DomainGroupID').val().length != 0) {
        IsDomainGroupIDExist();
    }
    return true;
}

function IsDomainGroupIDExist() {
    var isExist = false;

    $.ajax({
        url: '/Sys/SelectDomainGroupID',
        type: 'POST',
        data: { domainGroupID: $('#DomainGroupID').val(), domainName: $('#DomainName').val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $('#DomainGroupID').val('');
                _AddJsErrMessage(JsMsg_SelectDomainGroupID);
                _ShowJsErrMessageBox();

            } else {
                isExist = true;
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AlertJsErrorMsg('SelectDomainGroupID', textStatus, errorThrown);
        }
    });
    return isExist;
}

function AddButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeAdd);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function UpdateButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function DeleteButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    _alert('dialog_Confirm');
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    $('#ExecAction').val(_ActionTypeSelect);
    _formElement.submit();
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