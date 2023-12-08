var _formElement;

function SystemEDIJobConveyForm_onLoad(formElement) {
    _formElement = formElement;
    $(_formElement).attr('enctype', 'multipart/form-data');
    $('#QuerySysID').hide();
    $('#QueryEDIFlowID').hide();
    return true;
}

//----Button----//
function CancelButton_onClick(srcElement) {
    $('#Content').val("");
    $('#ExecAction').val(_ActionTypeQuery);
    _formElement.submit();
}

function UploadButton_onClick(srcElement) {
    var filenameExtension = /(\.xml)$/i;

    if ($('#UploadEdiJobFile').val() === '') {
        _AddJsErrMessage(JsMsg_HasNoUploadFile_Failure);
        _ShowJsErrMessageBox();
        return false;
    }
    if (filenameExtension.test($('#UploadEdiJobFile').val()) === false) {
        _AddJsErrMessage(JsMsg_FileFormat_Failure);
        _ShowJsErrMessageBox();
        return false;
    }
    $('#ExecAction').val(_ActionTypeAdd);
    return true;
}