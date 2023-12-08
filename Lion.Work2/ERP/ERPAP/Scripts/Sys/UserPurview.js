var _formElement;

function UserPurviewForm_onLoad(formElement) {
    _formElement = formElement;

    _RegisterBoxToFixedBottom($('#ButtonBox'));
}

function UpdateButton_onClick(srcElement, key) {
    var para = 'ExecAction=' + _ActionTypeUpdate + '&SysID=' + key[1] + '&SysNM=' + encodeURI(key[2]) + '&UserID=' + key[3] + '&UserNM=' + encodeURI(key[4]);
    _openWin('UserPurviewDetail', '/Sys/UserPurviewDetail', para, { width: 800, height: 600 });
    return false;
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    $('#ExecAction').val(_ActionTypeSelect);
    _formElement.submit();
}

//----Tab----//
function UserRoleFunDetail_onClick(srcElement) {
    $(srcElement).attr('con', 'Sys');
    $(srcElement).attr('act', 'UserRoleFunDetail');
    $('#ExecAction').val(_ActionTypeUpdate);
    _submit($(srcElement));

    return false;
}

function UserFunction_onClick(srcElement) {
    $(srcElement).attr('con', 'Sys');
    $(srcElement).attr('act', 'UserFunction');
    $('#ExecAction').val(_ActionTypeUpdate);
    _submit($(srcElement));

    return false;
}