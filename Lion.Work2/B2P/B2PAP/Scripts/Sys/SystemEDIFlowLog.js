var _formElement;

function SystemEDIFlowLogForm_onLoad(formElement) {
    _formElement = formElement;

    $('#SysIDReadOnlyText').val($('#QuerySysID').find("option:selected").text());

    return true;
}
//新開"錯誤檔案內容"視窗
function LinkEDIFlowError_onClick(srcElement, keys) {
    var para = "EDIFlowID=" + keys[1] + "&EDIDate=" + keys[2] + "&EDITime=" + keys[3] + "&QuerySysID="+keys[4];

    var objfeatures = { width: 450, height: 500 };

    _openWin("newwindow", "/Sys/SystemEDIFlowError", para, objfeatures);
    return false;
}
function LinkFunKeyDetail_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();
    $('#QuerySysID').val(keys[1]);
    $('#QueryEDIFlowID').val(keys[2]);
    $('#EDINO').val(keys[3]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

//下拉選單連動
function QuerySysID_onChange(srcElement) {
    if ($(srcElement).val() == '') {
        $('#QueryFunControllerID > option').remove();
        $('#QueryFunMenu > option').remove();
        return false;
    }

    $.ajax({
        url: '/Sys/GetSysSystemEDIFlowList',
        type: 'POST',
        data: { sysID: $(srcElement).val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $('#QueryEDIFlowID > option').remove();
                for (var i = 0; i < result.length; i++) {
                    $('#QueryEDIFlowID').append("<option value='" + result[i].Value + "'>" + result[i].Text + "</option>");
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSystemFunControllerIDList);
            _ShowJsErrMessageBox();
        }
    });
}

function PageSize_onEnter(srcElement) {
    SearchButton_onClick();
    return true;
}

//----Button----//
function AddButton_onClick(srcElement, keys) {
    var para = "QuerySysID=" + $('#QuerySysID').val() + "&"
             + "QueryEDIFlowID=" + $('#QueryEDIFlowID').val();

    var objfeatures = { width: 450, height: 500 };

    _openWin("newwindow", "/Sys/SystemEDIFlowLogSetting", para, objfeatures);
    return false;
}

function SearchButton_onClick(srcElement) {
    var Result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);
    if (Result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
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
}