var _formElement;

function SystemEDIJobLogForm_onLoad(formElement) {
    _formElement = formElement;
    var table = $('#SystemEDIJobLogTable', _formElement);

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
        isRemoveButton: false,
        onChange: QueryEDIFlowID_onChange
    });

    $('table.tblsearch #QueryEDIJobID', _formElement).combobox({
        width: 180,
        isRemoveButton: true
    });
   
    return true;
}

//連動
function QuerySysID_onChange(event) {
    if (event.select.val() === '') {
        $('#QueryEDIFlowID > option', _formElement).remove();
        $('#QueryEDIFlowID', _formElement).combobox('SetSelected', '');
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
                for (var i = 1; i < result.length; i++) {
                    $('#QueryEDIFlowID').append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            _AddJsErrMessage(JsMsg_GetSystemFunControllerIDList);
            _ShowJsErrMessageBox();
        }
    });
    QueryEDIFlowID_onChange();
}
function QueryEDIFlowID_onChange(event) {
    if (event == undefined) {
        $('#QueryEDIJobID > option', _formElement).remove();
        $('#QueryEDIJobID', _formElement).combobox('SetSelected', '');
        return false;
    }

    $.ajax({
        url: '/Sys/GetSysSystemEDIJobList',
        type: 'POST',
        data: { SysID: $('#QuerySysID').val(), EDIFlowID: event.select.val() },
        dataType: 'json',
        async: false,
        success: function (result) {
            if (result != null) {
                $('#QueryEDIJobID > option').remove();
                $('#QueryEDIJobID', _formElement).combobox('SetSelected', '');
                for (var i = 0; i < result.length; i++) {
                    $('#QueryEDIJobID').append('<option value="' + result[i].Value + '">' + result[i].Text + '</option>');
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
function SearchButton_onClick(srcElement) {
    var result = _FormValidation();
    $('#ExecAction').val(_ActionTypeSelect);
    if (result) {
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