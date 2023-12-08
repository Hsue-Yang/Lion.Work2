var _formElement;

function UserBasicInfoForm_onLoad(formElement) {
    _formElement = formElement;

    if ($(formElement).find('#UserBasicInfoTable')[0] != null) {
        _TableHover('UserBasicInfoTable', formElement);
    }

    $('#TimeBegin, #TimeEnd', $('table.tblsearch', _formElement)).combobox({
        isRemoveButton: false
    });

    return true;
}

function LinkFunKeyDetail_onClick(srcElement, keys) {
    var para = 'UserID=' + keys[1] +
               '&ExecAction=' + _ActionTypeSelect;
    var objfeatures = { width: window.screen.availWidth - 100, height: window.screen.availheight, top: 0, left: 0 };

    _openWin('newwindow', '/Sys/UserBasicInfoDetail', para, objfeatures);
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

function Help03Button_onClick(srcElement) {
    vMapFields = new Array(3);
    vMapFields[1] = "QueryUserID";
    _hISearch(vMapFields, "newwindow", _enumPUBAP + "/Help/Ishlp03", "Name=" + encodeURI($.trim($("#QueryUserID").val())));
}