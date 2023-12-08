var _formElement;

function SystemRoleGroupForm_onLoad(formElement) {
    _formElement = formElement;

    return true;
}

function LinkFunKeyDetail_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#RoleGroupID').val(keys[1]);
    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function LinkFunKeyCollect_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#RoleGroupID').val(keys[1]);
    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

//----Button----//
function SearchButton_onClick(srcElement) { //按下搜尋，驗證combobox有無值
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function AddButton_onClick(srcElement, keys) {
    var Result = _FormValidation(); //form驗證
    if (Result) {
        $.blockUI({ message: '' }); //按下按鈕跳轉前，畫面會鎖住
        Clean_HiddenValue();

        $('#RoleGroupID').val($('#QueryRoleGroupID').val());

        $('#ExecAction').val(_ActionTypeAdd); //jQuery套件執行Action (有底線為套件)
        return true;
    }
}

//----Private Function----//
function Clean_HiddenValue() { //清空 hidden 內值
    $('#RoleGroupID').val('');
}

function ConfirmOKButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}