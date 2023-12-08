var _formElement;
var _execEDIEventNo;

function SystemEventEDIForm_onLoad(formElement) {
    _formElement = formElement;
    var table = $('#SystemEventEDITable', _formElement);

    if (table.length > 0) {
        table.hide();
        var width = $(window).width();
        table.show();
        var height = $(window).height() - table.offset().top;
        $('table[id=Pager]').each(function () { height -= $(this).height(); });
        table.freezePanes({ width: width + 'px', height: height + 'px', fixedCols: 0 });
    }

    $('.BaseContainer').css('z-index', 0);

    $('table.tblsearch #QuerySysID', _formElement).combobox({
        width: 180,
        isRemoveButton: true,
        onChange: QuerySysID_onChange
    });

    $('table.tblsearch #QueryEventGroupID', _formElement).combobox({
        width: 180,
        isRemoveButton: true,
        onChange: QueryEventGroupID_onChange
    });

    $('#QueryEventID, #QueryTargetSysID', $('table.tblsearch', _formElement)).combobox({
        width: 180,
        isRemoveButton: true
    });

    return true;
}

function QuerySysID_onChange(event) {
    $('#QueryEventID > option', _formElement).remove();
    $('#QueryEventID', _formElement).combobox('SetSelected', '');
    if (event.select.val() === '') {
        $('#QueryEventGroupID > option', _formElement).remove();
        $('#QueryEventGroupID', _formElement).combobox('SetSelected', '');
        return false;
    }

    $.ajax({
        url: '/Sys/GetSysSystemEventGroupByIdList',
        type: 'POST',
        data: { sysID: event.select.val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            $('#QueryEventGroupID > option', _formElement).remove();
            $('#QueryEventGroupID', _formElement).combobox('SetSelected', '');
            if (result != null) {
                for (var i = 0; i < result.length; i++) {
                    $('#QueryEventGroupID', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSystemEventGroupByIdList);
            _ShowJsErrMessageBox();
        }
    });
}

function QueryEventGroupID_onChange(event) {
    if (event.select.val() === '') {
        $('#QueryEventID > option', _formElement).remove();
        $('#QueryEventID', _formElement).combobox('SetSelected', '');
        return false;
    }

    $.ajax({
        url: '/Sys/GetSysSystemEventByIdList',
        type: 'POST',
        data: { sysID: $('#QuerySysID').find('option:selected').val(), eventGroupID: event.select.val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $('#QueryEventID > option', _formElement).remove();
                $('#QueryEventID', _formElement).combobox('SetSelected', '');
                for (var i = 0; i < result.length; i++) {
                    $('#QueryEventID', _formElement).append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSystemEventByIdList);
            _ShowJsErrMessageBox();
        }
    });
}

function LinkFunKeyTargetEDI_onClick(srcElement, keys) {
    Clean_HiddenValue();

    $('#SysID').val(keys[1]);
    $('#EventGroupID').val(keys[2]);
    $('#EventID').val(keys[3]);

    $('#ExecAction').val(_ActionTypeQuery);
    return true;
}

function LinkFunKeyTargetLog_onClick(srcElement, keys) {
    var para = 'EDIDate=' + keys[1] + '&EDITime=' + keys[2];

    var objfeatures = { width: 450, height: 500 };

    _openWin('newwindow', '/Sys/SystemEventTargetLog', para, objfeatures);
    return false;
}

function PageSize_onEnter(srcElement) {
    SelectButton_onClick();
    return true;
}

//----Button----//
function SelectButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);
    if (result) {
        $.blockUI({ message: '' });
        $('#PageIndex').val(1);
        _formElement.submit();
    }
}

//----Private Function----//
function Clean_HiddenValue() {
    $('#SysID').val('');
    $('#EventGroupID').val('');
    $('#EventID').val('');
}