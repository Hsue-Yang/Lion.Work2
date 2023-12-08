var _formElement;

function FunScheduleForm_onLoad(formElement) {
    _formElement = formElement;

    if ($(formElement).find('#FunScheduleTable')[0] != null) {
        _TableHover('FunScheduleTable', formElement);
    }

    return true;
}

function LinkFunKey_onClick(srcElement, keys) {
    var para = "SysID=" + keys[1] + "&"
             + "FunControllerID=" + keys[2] + "&"
             + "FunActionName=" + keys[3] + "&"
             + "DevPhase=" + keys[4] + "&"
             + "IsFun=" + $('#IsFun').val();

    _openWin("newwindow", "/Dev/FunIssue", para);
    return false;
}

//----Button----//
function UpdateButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });
    
    location.href = '/Dev/SystemFun';
}

function Help03Button_onClick(srcElement) {
    vMapFields = new Array(3);
    vMapFields[1] = "DevOwner";
    _hISearch(vMapFields, "newwindow", _enumPUBAP + "/Help/Ishlp03", "Name=" + encodeURI($.trim($("#DevOwner").val())));
}