var _formElement, _originalWFSigMemo;

function SystemWorkFlowSignatureForm_onLoad(formElement) {
    _formElement = formElement;
    _advCKEditor();
    CKEDITOR.instances.WFSigMemo.setData($('#WFSigMemoZHTW').val());
    _originalWFSigMemo = 'WFSigMemoZHTW';

    $('#SigAPISysID', _formElement).combobox({
        width: 180,
        isRemoveButton: false,
        onChange: SigAPISysID_onChange
    });

    $('#SigAPIControllerID', _formElement).combobox({
        width: 180,
        isRemoveButton: false,
        onChange: SigAPIControllerID_onChange
    });

    $('#SigAPIActionName, #ValidAPIActionName', _formElement).combobox({
        width: 180,
        isRemoveButton: false
    });

    $('#ValidAPISysID', _formElement).combobox({
        width: 180,
        isRemoveButton: false,
        onChange: ValidAPISysID_onChange
    });

    $('#ValidAPIControllerID', _formElement).combobox({
        width: 180,
        isRemoveButton: false,
        onChange: ValidAPIControllerID_onChange
    });
    
    return true;
}

function SystemWorkFlowNodeDetail_onClick(srcElement) {
    $(srcElement).attr('con', 'Sys');
    $(srcElement).attr('act', 'SystemWorkFlowNodeDetail');
    $('#ExecAction').val(_ActionTypeUpdate);
    _submit($(srcElement));

    return false;
}

function SystemWorkFlowNext_onClick(srcElement) {
    $(srcElement).attr('con', 'Sys');
    $(srcElement).attr('act', 'SystemWorkFlowNext');
    $('#ExecAction').val(_ActionTypeUpdate);
    _submit($(srcElement));

    return false;
}

function SystemWorkFlowDocument_onClick(srcElement) {
    $(srcElement).attr('con', 'Sys');
    $(srcElement).attr('act', 'SystemWorkFlowDocument');
    $('#ExecAction').val(_ActionTypeUpdate);
    _submit($(srcElement));

    return false;
}

function LinkFunKeyDetail_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();
    
    $('#WFSigSeq').val(keys[1]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

function SigAPISysID_onChange(event) {
    $('#SigAPIControllerID > option', _formElement).remove();
    $('#SigAPIActionName > option', _formElement).remove();
    $('#SigAPIControllerID', _formElement).combobox('SetSelected', '');
    $('#SigAPIActionName', _formElement).combobox('SetSelected', '');

    if (event.select.val() === '')
        return false;

    $.ajax({
        url: '/Sys/GetSysSystemAPIGroupList',
        type: 'POST',
        data: { SysID: event.select.val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                for (var i = 0; i < result.length; i++) {
                    $('#SigAPIControllerID', _formElement).append('<option value=\'' + result[i].Value + '\'>' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXhr, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSysSystemAPIGroupList);
            _ShowJsErrMessageBox();
        }
    });
}

function SigAPIControllerID_onChange(event) {
    $('#SigAPIActionName > option', _formElement).remove();
    $('#SigAPIActionName', _formElement).combobox('SetSelected', '');

    if (event.select.val() === '')
        return false;

    $.ajax({
        url: '/Sys/GetSysSystemAPIFunList',
        type: 'POST',
        data: { SysID: $('#SigAPISysID').val(), APIGroup: event.select.val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                for (var i = 0; i < result.length; i++) {
                    $('#SigAPIActionName', _formElement).append('<option value=\'' + result[i].Value + '\'>' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXhr, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSysSystemAPIFunList);
            _ShowJsErrMessageBox();
        }
    });
}

function ValidAPISysID_onChange(event) {
    $('#ValidAPIControllerID > option', _formElement).remove();
    $('#ValidAPIActionName > option', _formElement).remove();
    $('#ValidAPIControllerID', _formElement).combobox('SetSelected', '');
    $('#ValidAPIActionName', _formElement).combobox('SetSelected', '');

    if (event.select.val() === '')
        return false;

    $.ajax({
        url: '/Sys/GetSysSystemAPIGroupList',
        type: 'POST',
        data: { SysID: event.select.val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                for (var i = 0; i < result.length; i++) {
                    $('#ValidAPIControllerID', _formElement).append('<option value=\'' + result[i].Value + '\'>' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXhr, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSysSystemAPIGroupList);
            _ShowJsErrMessageBox();
        }
    });

}

function ValidAPIControllerID_onChange(event) {
    $('#ValidAPIActionName > option', _formElement).remove();
    $('#ValidAPIActionName', _formElement).combobox('SetSelected', '');

    if (event.select.val() === '')
        return false;

    $.ajax({
        url: '/Sys/GetSysSystemAPIFunList',
        type: 'POST',
        data: { SysID: $('#ValidAPISysID').val(), APIGroup: event.select.val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                for (var i = 0; i < result.length; i++) {
                    $('#ValidAPIActionName', _formElement).append('<option value=\'' + result[i].Value + '\'>' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXhr, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSysSystemAPIFunList);
            _ShowJsErrMessageBox();
        }
    });
}

//----Button----//
function UpdateButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeUpdate);
    if (result) {
        $('#' + _originalWFSigMemo).val(CKEDITOR.instances.WFSigMemo.getData());
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function AddButton_onClick(srcElement) {
    $.blockUI({ message: '' }); //按下按鈕跳轉前，畫面會鎖住
    Clean_HiddenValue();
    $('#ExecAction').val(_ActionTypeAdd);
    return true;
}

function CloseButton_onClick(srcElement) {
    _windowClose();
}

function WFSigMemoLangLink_onClick(srcElement, keys) {
    $('#' + _originalWFSigMemo).val(CKEDITOR.instances.WFSigMemo.getData());
    CKEDITOR.instances.WFSigMemo.setData($('#' + keys[1]).val());
    _originalWFSigMemo = keys[1];
    $('div.md_cont li').removeClass('active');
    $(srcElement).closest('li').addClass('active');
}

//----Private Function----//
function Clean_HiddenValue() { //清空 hidden 內值
    $('#WFSigSeq').val('');
}

//#region 加掛編輯器
function _advCKEditor() {
    CKEDITOR.replace('WFSigMemo', {
        toolbar:
        [
            ['Source', '-', 'Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', 'SelectAll'],
            ['Bold', 'Italic', 'Underline', 'Link', 'Unlink', 'Maximize'],
            ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'Table', 'TextColor'],
            ['FontSize']
        ]
    });
}
//#endregion