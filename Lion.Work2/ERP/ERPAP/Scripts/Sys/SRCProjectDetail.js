var _formElement;

function SRCProjectDetailForm_onLoad(formElement) {
    _formElement = formElement;

    $('#DomainNameReadOnlyText').val($('#DomainName').find("option:selected").text());
    $('#DomainGroupIDReadOnlyText').val($('#DomainGroupID').find("option:selected").text());

    return true;
}

function AddProjectParent_onBlur(srcElement) {
    if ($('#AddProjectParent').val() != "")
    $('#ProjectParent').append("<option value='" + $('#AddProjectParent').val() + "' selected >" + $('#AddProjectParent').val() + "</option>");

}

function ProjectID_onBlur(srcElement) {

    if ($('#ProjectID').val().length != 0) {
        IsProjectIDExist();
    }
    return true;
}

function IsProjectIDExist() {
    var isExist = false;

    $.ajax({
        url: '/Sys/SelectProjectID',
        type: 'POST',
        data: { projectID: $('#ProjectID').val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $('#ProjectID').val('');
                _AddJsErrMessage(JsMsg_SelectProjectID);
                _ShowJsErrMessageBox();

            } else {
                isExist = true;
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AlertJsErrorMsg('SelectProjectID', textStatus, errorThrown);
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
