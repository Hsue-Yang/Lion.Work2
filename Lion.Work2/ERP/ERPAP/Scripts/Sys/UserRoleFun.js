var _formElement;

function UserRoleFunForm_onLoad(formElement) {
    _formElement = formElement;

    if ($(formElement).find('#UserRoleFunTable')[0] != null) {
        _TableHover('UserRoleFunTable', formElement);
    }

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
        $('#PageIndex').val(1);
        _formElement.submit();
    }
}

function Help03Button_onClick(srcElement) {
    vMapFields = new Array(3);
    vMapFields[1] = "QueryUserID";
    _hISearch(vMapFields, "newwindow", _enumPUBAP + "/Help/Ishlp03", "Name=" + encodeURI($.trim($("#QueryUserID").val())));
}
