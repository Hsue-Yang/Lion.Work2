﻿var _formElement;

function SystemEDIJobForm_onLoad(formElement) {
    _formElement = formElement;
    var table = $('#SystemEDIJobTable', _formElement);

    if (table.length > 0) {
        var width = $(window).width();
        table.show();
        var height = $(window).height() - table.offset().top;
        $('.tblsearch').each(function () { height -= $(this).height(); });
        table.freezePanes({ width: width + 'px', height: height + 'px', fixedCols: 0 });
    }

    $('.BaseContainer').css('z-index', 0);

    $('table.tblsearch #QuerySysID', _formElement).combobox({
        width: 180,
        isRemoveButton: false,
        onChange: QuerySysID_onChange
    });

    $('table.tblsearch #QueryEDIFlowID', _formElement).combobox({
        width: 180,
        isRemoveButton: true
    });

    $('table.tblsearch #QueryEDIJobType', _formElement).combobox({
        width: 180,
        isRemoveButton: true
    });

    return true;
}

function QuerySysID_onChange(event) {
    if (event.select.val() === '') {
        $('#EDIFlowID > option', _formElement).remove();
        $('#EDIFlowID', _formElement).combobox('SetSelected', '');
        return false;
    }

    $.ajax({
        url: '/Sys/GetSysSystemEDIFlowList',
        type: 'POST',
        data: { SysID: event.select.val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            $('#QueryEDIFlowID > option', _formElement).remove();
            $('#QueryEDIFlowID', _formElement).combobox('SetSelected', '');
            if (result != null) {
                for (var i = 0; i < result.length; i++) {
                    $('#QueryEDIFlowID', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetFunMenuList);
            _ShowJsErrMessageBox();
        }
    });
}
//單一勾選
//function IsMoved_onClick(srcElement) {
//    var obj = document.getElementsByName("IsMoved");
//    for (i = 0; i < obj.length; i++) {
//        if (obj[i] != srcElement) obj[i].checked = false;

//        else obj[i].checked = srcElement.checked;

//    }
//}

function MoveUp_onClick(srcElement) {
    var after;
    var current = $('input[name="IsMoved"]:checked').parent().parent();
    var prev = current.prev();
    if (prev[0] != undefined) {
        current.insertBefore(prev);
        after = prev.find('input[name$="AfterSortOrder"]').val();
        if (after != null) {
            prev.find('input[name$="AfterSortOrder"]').val(current.find('input[name$="AfterSortOrder"]').val());
            current.find('input[name$="AfterSortOrder"]').val(after);
        }
    }
}

function MoveDown_onClick(srcElement) {
    var after;
    var current = $('input[name="IsMoved"]:checked').parent().parent();
    var next = current.next();
    if (next[0] != undefined) {
        current.insertAfter(next);
        after = next.find('input[name$="AfterSortOrder"]').val();
        if (after != null) {
            next.find('input[name$="AfterSortOrder"]').val(current.find('input[name$="AfterSortOrder"]').val());
            current.find('input[name$="AfterSortOrder"]').val(after);
        }
    }
}

function LinkFunKeyDetail_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#EDIFlowID').val(keys[2]);
    $('#EDIJobID').val(keys[3]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function LinkFunKeyJob_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#EDIFlowID').val(keys[2]);
    $('#EDIJobID').val(keys[3]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function LinkFunKeyPara_onClick(srcElement, keys) {
    var para = 'QuerySysID=' + keys[1] + '&'
             + 'QueryEDIFlowID=' + keys[2] + '&'
             + 'QueryEDIJobID=' + keys[3];
    _openWin('newwindow' + keys[3], '/Sys/SystemEDIPara', para); //新視窗可多開
    return false;
}

function LinkFunKeyJobLog_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();

    $('#QuerySysID').val(keys[1]);
    $('#QueryEDIFlowID').val(keys[2]);
    $('#QueryEDIJobID').val(keys[3]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

//----Button----//
function SearchButton_onClick(srcElement) { //按下搜尋，驗證combobox有無值
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);
    if (result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function AddButton_onClick(srcElement, keys) {
    var result = _FormValidation(); //form驗證
    if (result) {
        $.blockUI({ message: '' }); //按下按鈕跳轉前，畫面會鎖住
        Clean_HiddenValue();

        $('#SysID').val($('#QuerySysID').val());
        $('#EDIFlowID').val($('#QueryEDIFlowID').val());
        
        $('#ExecAction').val(_ActionTypeAdd); //jQuery套件執行Action (有底線為套件)
        return true;
    }
}
function SaveButton_onClick(srcElement) {
    $('#ExecAction').val(_ActionTypeUpdate);
    _formElement.submit();

}

function ConveyButton_onClick(srcElement) {
    if ($('#QueryEDIFlowID').val() === '') {
        _alert('dialog_Confirm');
        return false;
    }
    $('#SysID').val($('#QuerySysID').val());
    $('#EDIFlowID').val($('#QueryEDIFlowID').val());
    return true;
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    $('#ExecAction').val(_ActionTypeQuery);
    _formElement.submit();
}

//----Private Function----//
function Clean_HiddenValue() { //清空 hidden 內值
    $('#SysID').val('');
    $('#EDIFlowID').val('');
    $('#EDIJobID').val('');
}

function ConfirmOKButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}