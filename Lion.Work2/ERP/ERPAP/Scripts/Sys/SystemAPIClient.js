var _formElement;

function SystemAPIClientForm_onLoad(formElement) {
    _formElement = formElement;
    var table = $('#SystemAPIClientTable', _formElement);

    if (table.length > 0) {
        var width = $(window).width();
        table.show();
        var height = $(window).height() - table.offset().top;
        $('.tblsearch ,table[id=Pager]').each(function () { height -= $(this).height(); });
        table.freezePanes({ width: width + 'px', height: height + 'px', fixedCols: 0 });
    }

    return true;
}

function LinkFunKeyDetail_onClick(srcElement, keys) {
    var para = 'APINo=' + keys[1];

    var objfeatures = { width: 800, height: 600 };

    _openWin('SystemAPIClientDetail_' + keys[1], '/Sys/SystemAPIClientDetail', para, objfeatures);
    return false;
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

function APIParaButton_onClick(srcElement) {
    var para = 'SysID=' + $('#SysID').val() + '&'
        + 'APIGroupID=' + $('#APIGroupID').val() + '&'
        + 'APIFunID=' + $('#APIFunID').val();

    var objfeatures = { width: 450, height: 500 };

    _openWin('newwindow', '/Sys/SystemAPIPara', para, objfeatures);
    return false;
}

function CancelButton_onClick(srcElement) {
    $.blockUI({ message: '' });

    location.href = '/Sys/SystemAPI';
}

//----Privte Function----//
function _DateValidation() {
    var timeInterval = 1; //時間間隔(時)
    var beginDate = $('#BeginDate').val();
    var endDate = $('#EndDate').val();
    var beginTime = $('#BeginTime').val();
    var endTime = $('#EndTime').val();

    //var starDt = new Date(Common.FormatLongDateTimeString(startDate.val() + startTime.val().replace(':', '')));
    //var endDt = new Date(Common.FormatLongDateTimeString(endDate.val() + endTime.val().replace(':', '')));

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