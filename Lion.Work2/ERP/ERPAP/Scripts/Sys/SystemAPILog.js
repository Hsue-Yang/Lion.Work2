var _formElement;

function SystemAPILogForm_onLoad(formElement) {
    _formElement = formElement;
    var table = $('#SystemAPILogTable', _formElement);

    if (table.length > 0) {
        table.hide();
        var width = $(window).width();
        table.show();
        var height = $(window).height() - table.offset().top;
        $('table[id=Pager]:eq(1)').each(function () { height -= $(this).height(); });
        table.freezePanes({ width: width + 'px', height: height + 'px', fixedCols: 0 });
    }

    $('.BaseContainer').css('z-index', 0);

    var sysWidth = $('table.tblsearch #QuerySysID', _formElement).width();
    $('table.tblsearch #QuerySysID', _formElement).combobox({
        width: sysWidth,
        isRemoveButton: true,
        onChange: QuerySysID_onChange
    });

    $('table.tblsearch #QueryAPIGroupID', _formElement).combobox({
        width: 180,
        isRemoveButton: true,
        onChange: QueryAPIGroup_onChange
    });

    $('table.tblsearch #QueryAPIFunID', _formElement).combobox({
        width: 180,
        isRemoveButton: true
    });

    $('table.tblsearch #QueryClientSysID', _formElement).combobox({
        width: sysWidth,
        isRemoveButton: true
    });

    return true;
}

function LinkFunKeyDetail_onClick(srcElement, keys) {
    var para = 'APINo=' + keys[1];

    var objfeatures = { width: 800, height: 600 };

    _openWin('SystemAPIClientDetail_' + keys[1], '/Sys/SystemAPIClientDetail', para, objfeatures);
    return false;
}

function QuerySysID_onChange(event) {
    $('#QueryAPIFunID > option', _formElement).remove();
    $('#QueryAPIFunID', _formElement).combobox('SetSelected', '');
    $('#QueryAPIGroupID > option', _formElement).remove();
    $('#QueryAPIGroupID', _formElement).combobox('SetSelected', '');

    $.ajax({
        url: '/Sys/GetSysSystemAPIGroupList',
        type: 'POST',
        data: { sysID: event.select.val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                for (var i = 0; i < result.length; i++) {
                    $('#QueryAPIGroupID', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_UnGetSysSystemAPIGroupByIdList);
            _ShowJsErrMessageBox();
        }
    });
}

function QueryAPIGroup_onChange(event) {
    $('#QueryAPIFunID > option', _formElement).remove();
    $('#QueryAPIFunID', _formElement).combobox('SetSelected', '');

    $.ajax({
        url: '/Sys/GetSysSystemAPIByIdList',
        type: 'POST',
        data: { sysID: $('#QuerySysID').find('option:selected').val(), apiGroup: event.select.val() },
        dataType: 'json',
        async: true,
        success: function (result) {
            if (result != null) {
                for (var i = 0; i < result.length; i++) {
                    $('#QueryAPIFunID', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_UnGetSysSystemAPIByIdList);
            _ShowJsErrMessageBox();
        }
    });
}

function PageSize_onEnter(srcElement) {
    SelectButton_onClick();
    return true;
}

//----Button----//
function SelectButton_onClick(srcElement) {
    var result = _FormValidation() && _DateValidation();
    $('#ExecAction').val(_ActionTypeSelect);
    if (result) {
        $.blockUI({ message: '' });
        $('#PageIndex').val(1);
        _formElement.submit();
    }
}

//----Private Function----//
function _DateValidation() {
    var timeInterval = 1; //時間間隔(時)
    var beginDate = $('#BeginDate').val();
    var endDate = $('#EndDate').val();
    var beginTime = $('#BeginTime').val();
    var endTime = $('#EndTime').val();

    if ((beginDate.length == 8 && beginTime.match('^(([0-1]?[0-9])|([2][0-3])):([0-5]?[0-9])$')) && (endDate.length == 8 && endTime.match('^(([0-1]?[0-9])|([2][0-3])):([0-5]?[0-9])$'))) {
        beginDate = new Date(beginDate.substr(0, 4) + '-' + beginDate.substr(4, 2) + '-' + beginDate.substr(6, 2) + ' ' + beginTime);
        endDate = new Date(endDate.substr(0, 4) + '-' + endDate.substr(4, 2) + '-' + endDate.substr(6, 2) + ' ' + endTime);
    }
    else {
        _AddJsErrMessage(JsMsg_HHmm_Error);
        _ShowJsErrMessageBox();
        return false;
    }

    if ((beginDate <= endDate) == false) {
        _AddJsErrMessage(JsMsg_Date_Error);
        _ShowJsErrMessageBox();
        return false;
    }
    else {
        if (((endDate - beginDate) <= timeInterval * 60 * 60 * 1000) == false) {
            _AddJsErrMessage(JsMsg_TimeInterval_Error);
            _ShowJsErrMessageBox();
            return false;
        }
    }

    return true;
}

function BeginTime_onBlur(srcElement) {
    Time_format(srcElement);
    return true;
}

function EndTime_onBlur(srcElement) {
    Time_format(srcElement);
    return true;
}

function Time_format(srcElement) {
    if (srcElement.value.length == 4 && srcElement.value.match('^(([0-1]?[0-9])|([2][0-3]))([0-5]?[0-9])$')) {
        srcElement.value = srcElement.value.substr(0, 2) + ':' + srcElement.value.substr(2, 2);
        return true;
    } else {
        _AddJsErrMessage(JsMsg_HHmm_Error);
        _ShowJsErrMessageBox();
    }
}

function Clean_HiddenValue() {
    $('#SysID').val('');
    $('#APIGroupID').val('');
    $('#APIFunID').val('');
}