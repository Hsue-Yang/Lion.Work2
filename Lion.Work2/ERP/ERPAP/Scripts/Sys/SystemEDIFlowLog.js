var _formElement;

function SystemEDIFlowLogForm_onLoad(formElement) {
    _formElement = formElement;

    $('#SysIDReadOnlyText').val($('#QuerySysID').find('option:selected').text());

    var table = $('#SystemEDIFlowLogTable', _formElement);

    if (table.length > 0) {
        table.hide();
        var width = $(window).width();
        table.show();
        var height = $(window).height() - table.offset().top;
        $('.tblsearch').each(function () { height -= $(this).height(); });
        table.freezePanes({ width: width + 'px', height: height + 'px', fixedCols: 4 });
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

    return true;
}
//新開"錯誤檔案內容"視窗
function LinkEDIFlowError_onClick(srcElement, keys) {
    var para = 'EDIFlowID=' + keys[1] + '&EDIDate=' + keys[2] + '&EDITime=' + keys[3] + '&QuerySysID='+keys[4];

    var objfeatures = { width: 450, height: 500 };

    _openWin('newwindow', '/Sys/SystemEDIFlowError', para, objfeatures);
    return false;
}

function LinkFunKeyDetail_onClick(srcElement, keys) {
    $.blockUI({ message: '' });
    Clean_HiddenValue();
    $('#QuerySysID').val(keys[1]);
    $('#QueryEDIFlowID').val(keys[2]);
    $('#EDINO').val(keys[3]);
    $('#EDIDate').val(keys[4]);

    $('#ExecAction').val(_ActionTypeUpdate);
    return true;
}

//下拉選單連動
function QuerySysID_onChange(event) {
    if (event.select.val() === '') {
        $('#QueryFunControllerID > option').remove();
        $('#QueryFunMenu > option').remove();
        $('#QueryFunControllerID', _formElement).combobox('SetSelected', '');
        $('#QueryFunMenu', _formElement).combobox('SetSelected', '');
        return false;
    }

    $.ajax({
        url: '/Sys/GetSysSystemEDIFlowList',
        type: 'POST',
        data: { sysID: event.select.val(), useNullFlow: true },
        dataType: 'json',
        async: false,
        success: function (result) {
            $('#QueryEDIFlowID > option').remove();
            $('#QueryEDIFlowID', _formElement).combobox('SetSelected', '');
            
            if (result != null) {
                for (var i = 0; i < result.length; i++) {
                    $('#QueryEDIFlowID').append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
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
    var para = 'QuerySysID=' + $('#QuerySysID').val() + '&'
             + 'QueryEDIFlowID=' + $('#QueryEDIFlowID').val();

    var objfeatures = { width: 450, height: 500 };

    _openWin('newwindow', '/Sys/SystemEDIFlowLogSetting', para, objfeatures);
    return false;
}

function SearchButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);
    if (result) {
        $.blockUI({ message: '' });
        _formElement.submit();
    }
}

function RemoveWaitButton_onClick(srcElement) {
    var result = _FormValidation();

    if (result) {
        $.blockUI({ message: '' });
        var sysNm = $('option:selected', $('table.tblsearch #QuerySysID', _formElement).combobox('Selected').select).text();
        var ediFlowNm = $('option:selected', $('table.tblsearch #QueryEDIFlowID', _formElement).combobox('Selected').select).text();

        var message = sysNm;
        if (ediFlowNm > '') {
            message += '<br/>' + ediFlowNm;
        }
        message += '<br/>' + $('th>label', $('#DataDate').closest('tr')).html() + ':' + $('#DataDate').val();

        $('#dialog_RemoveWait #message').html(message);
        _alert('dialog_RemoveWait');
    }
}

function RemoveWaitConfirmOKButton_onClick(srcElement) {
    $('#ExecAction').val(_ActionTypeDelete);
    _formElement.submit();
    _btnUnblockUI(this, false);
}

function RemoveWaitConfirmNOButton_onClick(srcElement) {
    _btnUnblockUI(this, false);
}

//----Private Function----//
function Clean_HiddenValue() { //清空 hidden 內值
    $('#SysID').val('');
    $('#EDIFlowID').val('');
}