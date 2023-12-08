var _formElement;

function SystemUserListForm_onLoad(formElement) {
    _formElement = formElement;

    return true;
}

function LinkFunKey_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    
    $('#UserID').val(keys[1]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function PageSize_onEnter(srcElement) {
    SelectButton_onClick();
    return true;
}

//----Button----//
function SelectButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    location.href = '/Sys/SystemSetting';
}

function Help03Button_onClick(srcElement) {
    vMapFields = new Array(3)				//參數個數
    vMapFields[1] = "QueryUserID";
    _hISearch(vMapFields, "newwindow", _enumB2PAP + "/Help/Ishlp03", "Name=" + encodeURI($.trim($("#QueryUserID").val())));
}
