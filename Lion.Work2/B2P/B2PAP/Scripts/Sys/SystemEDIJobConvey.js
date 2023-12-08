var _formElement;

function SystemEDIJobConveyForm_onLoad(formElement) {
    _formElement = formElement;
    $('#QuerySysID').hide();
    $('#QueryEDIFlowID').hide();
    return true;
}

//----Button----//
function SaveButton_onClick(srcElement) {
    var Result = _FormValidation();
    var contentReplace = $('#Content').val();
    if (Result) {
            $('#ExecAction').val(_ActionTypeAdd);
            $('#QuerySysID').val($('#SysID').val());
            $('#QueryEDIFlowID').val($('#EDIFlowID').val());
            contentReplace = contentReplace.replace(/(<)/ig, "&LT;");
            contentReplace = contentReplace.replace(/(>)/ig, "&RT;");
            $('#Content').val(contentReplace);
            _formElement.submit();
    }
}

function CancelButton_onClick(srcElement) {
    $('#Content').val("");
    $('#ExecAction').val(_ActionTypeQuery);
    _formElement.submit();
}

